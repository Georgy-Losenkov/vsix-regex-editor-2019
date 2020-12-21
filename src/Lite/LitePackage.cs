using System;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

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
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid("daae5c3e-4e54-4a96-9e3a-1b0247b1f252")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    sealed class LitePackage : AsyncPackage, IOleCommandTarget, IVsShellPropertyEvents
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LitePackage"/> class.
        /// </summary>
        public LitePackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        UInt32 m_shellPropertyChangesCookie;

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
            // await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            await JoinableTaskFactory.StartOnIdle(async delegate
            {
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
                if (!UserSettings.IsHelpForceShown)
                {
                    var shell = (IVsShell)GetGlobalService(typeof(SVsShell));
                    if (ErrorHandler.Succeeded(shell.GetProperty((Int32)__VSSPROPID.VSSPROPID_Zombie, out var obj2))
                      && Microsoft.VisualStudio.PlatformUI.Unbox.AsBoolean(obj2))
                    {
                        shell.AdviseShellPropertyChanges(this, out m_shellPropertyChangesCookie);
                    }
                    else
                    {
                        UserSettings.IsHelpForceShown = true;
                        var menuService = await GetServiceAsync(typeof(IMenuCommandService));
                        if (menuService is IMenuCommandService ms)
                        {
                            ms.GlobalInvoke(new CommandID(new Guid("42187349-3f00-4738-b46a-f2db44fc70e2"), 260));
                        }
                    }
                }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
            });
        }

        #region OnShellPropertyChange
        Int32 IVsShellPropertyEvents.OnShellPropertyChange(Int32 propid, Object var)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (propid == (Int32)__VSSPROPID.VSSPROPID_Zombie)
            {
                if (!UserSettings.IsHelpForceShown)
                {
                    UserSettings.IsHelpForceShown = true;
                    var menuService = GetService(typeof(IMenuCommandService));
                    if (menuService is IMenuCommandService ms)
                    {
                        ms.GlobalInvoke(new CommandID(new Guid("42187349-3f00-4738-b46a-f2db44fc70e2"), 260));
                    }
                }

                var shell = (IVsShell)GetGlobalService(typeof(SVsShell));
                shell.UnadviseShellPropertyChanges(m_shellPropertyChangesCookie);
                m_shellPropertyChangesCookie = 0;
            }
            return 0;
        }
        #endregion
    }
}