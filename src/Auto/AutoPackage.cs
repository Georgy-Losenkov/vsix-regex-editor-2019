using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
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
    [Guid("f8e44e7a-2db5-4917-8cac-bfab9dfcc5f6")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideService(typeof(IFontAndColorDefaultsProvider))]
    [ProvideToolWindow(typeof(UI.QuickStartToolWindow), DocumentLikeTool = true, Style = VsDockStyle.MDI)]
    [ProvideToolWindow(typeof(UI.QuickRefToolWindow), DocumentLikeTool = false, Orientation = ToolWindowOrientation.Right, DockedWidth = 300, Style = VsDockStyle.Tabbed)]
    [FontAndColorRegistration(typeof(IFontAndColorDefaultsProvider), Shell.FontAndColorDefaultsQuickRef.CategoryNameString, Shell.FontAndColorDefaultsQuickRef.CategoryGuidString)]
    sealed class AutoPackage : AsyncPackage, IOleCommandTarget, IVsFontAndColorDefaultsProvider, IFontAndColorDefaultsProvider
    {
        #region Guids
        internal static class Guids
        {
            public const String CmdSetString = "42187349-3f00-4738-b46a-f2db44fc70e2";

            public static readonly Guid CmdSetGuid = new Guid(CmdSetString);
        }

        internal static class CmdId
        {
            public const UInt32 OpenPage1 = 256;
            public const UInt32 OpenPage2 = 257;
            public const UInt32 OpenPage3 = 258;
            public const UInt32 OpenQuickRef = 259;
            public const UInt32 OpenQuickStart = 260;
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPackage"/> class.
        /// </summary>
        public AutoPackage()
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
        protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);

            // When initialized asynchronously, we *may* be on a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            // Otherwise, remove the switch to the UI thread if you don't need it.
            // await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            await JoinableTaskFactory.StartOnIdle(delegate
            {
                // ensure that we have instance
                _ = new Shell.FontAndColorDefaultsQuickRef();
                _ = new Shell.ThemeColorsQuickStart();
                ((IServiceContainer)this).AddService(typeof(IFontAndColorDefaultsProvider), this, true);
                SubscribeForColorChangeEvents();
            });
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
                case CmdId.OpenPage1:
                    Navigate("https://msdn.microsoft.com/en-us/library/hs600312.aspx");
                    break;
                case CmdId.OpenPage2:
                    Navigate("https://msdn.microsoft.com/en-us/library/az24scfc.aspx");
                    break;
                case CmdId.OpenPage3:
                    Navigate("https://referencesource.microsoft.com/#System/regex/system/text/regularexpressions/Regex.cs");
                    break;
                case CmdId.OpenQuickRef:
                    ShowToolWindow<UI.QuickRefToolWindow>();
                    break;
                case CmdId.OpenQuickStart:
                    ShowToolWindow<UI.QuickStartToolWindow>();
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
#if (false)
    void ShowToolWindow<T>() where T : ToolWindowPane
    {
      ThreadHelper.ThrowIfNotOnUIThread();

      // Get the instance number 0 of this tool window. This window is single instance so this instance
      // is actually the only one.
      // The last flag is set to true so that if the tool window does not exists it will be created.
      ToolWindowPane window = FindToolWindow(typeof(T), 0, true);
      if ((null == window) || (null == window.Frame))
      {
        throw new NotSupportedException("Cannot create tool window");
      }

      var windowFrame = (IVsWindowFrame)window.Frame;
      ErrorHandler.ThrowOnFailure(windowFrame.Show());
    }
#else
        async System.Threading.Tasks.Task ShowToolWindowAsync<T>() where T : ToolWindowPane
        {
            var window = await ShowToolWindowAsync(typeof(T), 0, true, DisposalToken);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException("Cannot create tool window");
            }
        }

        void ShowToolWindow<T>() where T : ToolWindowPane
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            JoinableTaskFactory.Run(ShowToolWindowAsync<T>);
        }
#endif

        void Navigate(String url)
        {
            try
            {
                Process.Start(new ProcessStartInfo { FileName = url, Verb = "open" });
            }
            catch (Exception ex)
            {
                // Show a message box to prove we were here
                ErrorHandler.ThrowOnFailure(
                  VsShellUtilities.ShowMessageBox(
                    this,
                    ex.ToString(),
                    "Open url",
                    OLEMSGICON.OLEMSGICON_CRITICAL,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST));
            }
        }

        public override IVsAsyncToolWindowFactory GetAsyncToolWindowFactory(Guid toolWindowType)
        {
            if (toolWindowType == typeof(UI.QuickRefToolWindow).GUID
              || toolWindowType == typeof(UI.QuickStartToolWindow).GUID)
            {
                return this;
            }

#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
            return base.GetAsyncToolWindowFactory(toolWindowType);
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        }

        protected override String GetToolWindowTitle(Type toolWindowType, Int32 id)
        {
            if (toolWindowType == typeof(UI.QuickRefToolWindow))
            {
                return "Regex Quick Reference Loading";
            }
            else if (toolWindowType == typeof(UI.QuickStartToolWindow))
            {
                return "Quick Start Loading";
            }

            return base.GetToolWindowTitle(toolWindowType, id);
        }

        protected override System.Threading.Tasks.Task<Object> InitializeToolWindowAsync(Type toolWindowType, Int32 id, System.Threading.CancellationToken cancellationToken)
        {
            if (toolWindowType == typeof(UI.QuickRefToolWindow))
            {
                return System.Threading.Tasks.Task.FromResult<Object>("Regex Quick Reference Loaded");
            }
            else if (toolWindowType == typeof(UI.QuickStartToolWindow))
            {
                return System.Threading.Tasks.Task.FromResult<Object>("Quick Start Loaded");
            }

            return base.InitializeToolWindowAsync(toolWindowType, id, cancellationToken);
        }
        #endregion

        void FontAndColorsChanged(Object sender, EventArgs args)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Shell.FontAndColorDefaultsQuickRef.Instance.ReloadFontAndColors();
            Shell.ThemeColorsQuickStart.Instance.ReloadColors();
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

            if (rguidCategory == Shell.FontAndColorDefaultsQuickRef.Instance.CategoryGuid)
            {
                ppObj = Shell.FontAndColorDefaultsQuickRef.Instance;
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