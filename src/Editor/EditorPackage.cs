using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using OleConstants = Microsoft.VisualStudio.OLE.Interop.Constants;

namespace Losenkov.RegexEditor
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid("78e3e2c3-eacf-4bd3-a875-a0db379d8e3b")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(toolType: typeof(UI.EditorToolWindow), DocumentLikeTool = true, MultiInstances = true, Style = VsDockStyle.MDI)]
    [ProvideService(typeof(IFontAndColorDefaultsProvider))]
    [FontAndColorRegistration(typeof(IFontAndColorDefaultsProvider), Shell.FontAndColorDefaultsResultsGrid.CategoryNameString, Shell.FontAndColorDefaultsResultsGrid.CategoryGuidString)]
    [FontAndColorRegistration(typeof(IFontAndColorDefaultsProvider), Shell.FontAndColorDefaultsResultsText.CategoryNameString, Shell.FontAndColorDefaultsResultsText.CategoryGuidString)]
    [FontAndColorRegistration(typeof(IFontAndColorDefaultsProvider), Shell.FontAndColorDefaultsResultsTree.CategoryNameString, Shell.FontAndColorDefaultsResultsTree.CategoryGuidString)]
    sealed class EditorPackage : AsyncPackage, IOleCommandTarget, IVsFontAndColorDefaultsProvider, IFontAndColorDefaultsProvider
    {
        #region Guids and Ids
        internal static class Guids
        {
            // public const string guidToolWindowPersistanceString = "d90e2d21-190d-443c-97db-898be3db59a0";
            public const String CmdSetString = "6e7a508d-0c48-4703-8e50-8e8eba9508b6";

            public static readonly Guid CmdSetGuid = new Guid(CmdSetString);
        }

        internal static class Ids
        {
            public const UInt32 toolbarEditor = 4128;
            public const UInt32 cmdidOpenEditor = 256;
            public const UInt32 cmdidEditorRunRegexPrimary = 257;
            public const UInt32 cmdidEditorRunRegexSecondary = 258;
            public const UInt32 cmdidEditorToggleResultsPrimary = 259;
            public const UInt32 cmdidEditorToggleResultsSecondary = 260;
            public const UInt32 cmdidEditorRegexMethod = 512;
            public const UInt32 cmdidEditorRegexMethodItems = 513;
            public const UInt32 cmdidEditorTesterMode = 514;
            public const UInt32 cmdidEditorTesterModeItems = 515;
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorPackage"/> class.
        /// </summary>
        public EditorPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        #region Package Members
        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override async System.Threading.Tasks.Task InitializeAsync(System.Threading.CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);

            // When initialized asynchronously, we *may* be on a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            // Otherwise, remove the switch to the UI thread if you don't need it.
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            // ensure that we have instance
            new Shell.FontAndColorDefaultsResultsGrid();
            new Shell.FontAndColorDefaultsResultsText();
            new Shell.FontAndColorDefaultsResultsTree();

            ((IServiceContainer)this).AddService(typeof(IFontAndColorDefaultsProvider), this, true);
            SubscribeForColorChangeEvents();
        }

        protected override void Dispose(Boolean disposing)
        {
            UnsubscribeFromColorChangeEvents();

            base.Dispose(disposing);
        }

        #region IOleCommandTarget
        Int32 IOleCommandTarget.Exec(ref Guid pguidCmdGroup, UInt32 nCmdID, UInt32 nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (pguidCmdGroup != Guids.CmdSetGuid)
            {
                return (Int32)OleConstants.OLECMDERR_E_NOTSUPPORTED;
            }

            switch (nCmdID)
            {
                case Ids.cmdidOpenEditor:
                    ThreadHelper.ThrowIfNotOnUIThread();
                    ShowEditorToolWindow();
                    break;
                default:
                    return (Int32)OleConstants.OLECMDERR_E_NOTSUPPORTED;
            }

            return VSConstants.S_OK;
        }

        Int32 IOleCommandTarget.QueryStatus(ref Guid pguidCmdGroup, UInt32 cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            if (pguidCmdGroup != Guids.CmdSetGuid)
            {
                return (Int32)OleConstants.OLECMDERR_E_NOTSUPPORTED;
            }

            return VSConstants.S_OK;
        }
        #endregion

        #region handlers
#if (true)
        void ShowEditorToolWindow()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var windowType = typeof(UI.EditorToolWindow);

            var id = 0;
            for (; ; id++)
            {
                if (FindToolWindow(windowType, id, false) == null)
                {
                    break;
                }
            }

            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            var window = FindToolWindow(windowType, id, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException("Cannot create tool window");
            }

            var windowFrame = (IVsWindowFrame)window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
#else
    async System.Threading.Tasks.Task ShowEditorToolWindowAsync()
    {
      var windowType = typeof(UI.EditorToolWindow);

      var id = 0;
      for (; ; id++)
      {
        if (await this.FindToolWindowAsync(windowType, id, false, this.DisposalToken) == null) break;
      }

      var window = await this.ShowToolWindowAsync(windowType, id, true, this.DisposalToken);
      if ((null == window) || (null == window.Frame))
      {
        throw new NotSupportedException("Cannot create tool window");
      }
    }

    void ShowEditorToolWindow()
    {
      this.JoinableTaskFactory.RunAsync(this.ShowEditorToolWindowAsync);
    }
#endif

        public override IVsAsyncToolWindowFactory GetAsyncToolWindowFactory(Guid toolWindowType)
        {
            if (toolWindowType == typeof(UI.EditorToolWindow).GUID)
            {
                return this;
            }

#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
            return base.GetAsyncToolWindowFactory(toolWindowType);
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        }

        protected override String GetToolWindowTitle(Type toolWindowType, Int32 id)
        {
            if (toolWindowType == typeof(UI.EditorToolWindow))
            {
                return "Regex Editor Loading";
            }

            return base.GetToolWindowTitle(toolWindowType, id);
        }

        protected override System.Threading.Tasks.Task<Object> InitializeToolWindowAsync(Type toolWindowType, Int32 id, System.Threading.CancellationToken cancellationToken)
        {
            if (toolWindowType == typeof(UI.EditorToolWindow))
            {
                return System.Threading.Tasks.Task.FromResult<Object>("Regex Editor Loaded");
            }

            return base.InitializeToolWindowAsync(toolWindowType, id, cancellationToken);
        }
        #endregion

        void FontAndColorsChanged(Object sender, EventArgs args)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Shell.FontAndColorDefaultsResultsGrid.Instance.ReloadFontAndColors();
            Shell.FontAndColorDefaultsResultsText.Instance.ReloadFontAndColors();
            Shell.FontAndColorDefaultsResultsTree.Instance.ReloadFontAndColors();
        }

        void SubscribeForColorChangeEvents()
        {
            SystemEvents.DisplaySettingsChanged += FontAndColorsChanged;
            SystemEvents.PaletteChanged += FontAndColorsChanged;
            SystemEvents.UserPreferenceChanged += FontAndColorsChanged;
        }

        void UnsubscribeFromColorChangeEvents()
        {
            SystemEvents.DisplaySettingsChanged -= FontAndColorsChanged;
            SystemEvents.PaletteChanged -= FontAndColorsChanged;
            SystemEvents.UserPreferenceChanged -= FontAndColorsChanged;
        }

        #region IVsFontAndColorDefaultsProvider
        Int32 IVsFontAndColorDefaultsProvider.GetObject(ref Guid rguidCategory, out Object ppObj)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (rguidCategory == Shell.FontAndColorDefaultsResultsGrid.Instance.CategoryGuid)
            {
                ppObj = Shell.FontAndColorDefaultsResultsGrid.Instance;
            }
            else if (rguidCategory == Shell.FontAndColorDefaultsResultsText.Instance.CategoryGuid)
            {
                ppObj = Shell.FontAndColorDefaultsResultsText.Instance;
            }
            else if (rguidCategory == Shell.FontAndColorDefaultsResultsTree.Instance.CategoryGuid)
            {
                ppObj = Shell.FontAndColorDefaultsResultsTree.Instance;
            }
            else
            {
                ppObj = null;
            }

            return 0;
        }
        #endregion
        #endregion
    }
}