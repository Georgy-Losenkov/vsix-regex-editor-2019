using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using Losenkov.RegexEditor.UI.ViewModel;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using OleConstants = Microsoft.VisualStudio.OLE.Interop.Constants;

namespace Losenkov.RegexEditor.UI
{
    [Guid("0ee48ac7-d7eb-4653-a54e-3bba557ccc9f")]
    public class EditorToolWindow : ToolWindowPane, IOleCommandTarget
    {
        EditorControl m_editor = null;

#pragma warning disable IDE0060 // Remove unused parameter
        public EditorToolWindow(String message) : base(null)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            Caption = "Regex Editor";
            BitmapResourceID = 301;
            BitmapIndex = 1;
            ToolBar = new CommandID(EditorPackage.Guids.CmdSetGuid, (Int32)EditorPackage.Ids.toolbarEditor);
        }

        public EditorToolWindow() : this(null)
        {
        }

        protected override void OnCreate()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Shell.FontAndColorDefaultsResultsGrid.Instance.EnsureFontAndColorsInitialized();
            Shell.FontAndColorDefaultsResultsText.Instance.EnsureFontAndColorsInitialized();
            Shell.FontAndColorDefaultsResultsTree.Instance.EnsureFontAndColorsInitialized();

            base.Content = m_editor = new EditorControl();
        }

        public override void OnToolWindowCreated()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            base.OnToolWindowCreated();

            // --- Register key bindings to use in the editor
            var windowFrame = (IVsWindowFrame)Frame;
            var cmdUi = VSConstants.GUID_TextEditorFactory;
            windowFrame.SetGuidProperty((Int32)__VSFPROPID.VSFPROPID_InheritKeyBindings, ref cmdUi);
        }

        public Int32 Exec(ref Guid pguidCmdGroup, UInt32 nCmdID, UInt32 nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var editor = m_editor;
            if (editor != null)
            {
                if (pguidCmdGroup == EditorPackage.Guids.CmdSetGuid)
                {
                    switch (nCmdID)
                    {
                        case EditorPackage.Ids.cmdidEditorRunRegexPrimary:
                        case EditorPackage.Ids.cmdidEditorRunRegexSecondary:
                            editor.RunRegex();
                            return VSConstants.S_OK;
                        case EditorPackage.Ids.cmdidEditorToggleResultsPrimary:
                        case EditorPackage.Ids.cmdidEditorToggleResultsSecondary:
                            if (editor.DataContext is EditorViewModel viewModel)
                            {
                                viewModel.ResultsVisible = !viewModel.ResultsVisible;
                            }
                            return VSConstants.S_OK;
                        case EditorPackage.Ids.cmdidEditorRegexMethod:
                        {
                            if (pvaOut != IntPtr.Zero)
                            {
                                // when vOut is non-NULL, the IDE is requesting the current value for the combo
                                Marshal.GetNativeVariantForObject(View.RegexMethodConverter.ToString(editor.RegexMethod), pvaOut);
                            }
                            else if (Marshal.GetObjectForNativeVariant(pvaIn) is String inValue)
                            {
                                if (View.RegexMethodConverter.TryParse(inValue, out var method))
                                {
                                    editor.RegexMethod = method;
                                }
                            }
                            return VSConstants.S_OK;
                        }
                        case EditorPackage.Ids.cmdidEditorRegexMethodItems:
                        {
                            if (pvaOut != IntPtr.Zero)
                            {
                                var array = EditorViewModel.RegexMethods.Select(v => View.RegexMethodConverter.ToString(v)).ToArray();
                                Marshal.GetNativeVariantForObject(array, pvaOut);
                            }
                            return VSConstants.S_OK;
                        }
                        case EditorPackage.Ids.cmdidEditorTesterMode:
                        {
                            if (pvaOut != IntPtr.Zero)
                            {
                                // when vOut is non-NULL, the IDE is requesting the current value for the combo
                                Marshal.GetNativeVariantForObject(View.TesterModeConverter.ToString(editor.TesterMode), pvaOut);
                            }
                            else if (Marshal.GetObjectForNativeVariant(pvaIn) is String inValue)
                            {
                                if (View.TesterModeConverter.TryParse(inValue, out var mode))
                                {
                                    editor.TesterMode = mode;
                                }
                            }
                            return VSConstants.S_OK;
                        }
                        case EditorPackage.Ids.cmdidEditorTesterModeItems:
                        {
                            if (pvaOut != IntPtr.Zero)
                            {
                                var array = EditorViewModel.TesterModes.Select(v => View.TesterModeConverter.ToString(v)).ToArray();
                                Marshal.GetNativeVariantForObject(array, pvaOut);
                            }
                            return VSConstants.S_OK;
                        }
                    }
                }
                else
                {
                    if (editor.GetActiveView() is IOleCommandTarget activeTarget)
                    {
                        return activeTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
                    }
                }
            }

            return (Int32)OleConstants.OLECMDERR_E_NOTSUPPORTED;
        }

        public Int32 QueryStatus(ref Guid pguidCmdGroup, UInt32 cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var editor = m_editor;
            if (editor != null)
            {
                if (pguidCmdGroup == EditorPackage.Guids.CmdSetGuid)
                {
                    switch (prgCmds[0].cmdID)
                    {
                        case EditorPackage.Ids.cmdidEditorRunRegexPrimary:
                        case EditorPackage.Ids.cmdidEditorRunRegexSecondary:
                        case EditorPackage.Ids.cmdidEditorToggleResultsSecondary:
                            return VSConstants.S_OK;
                        case EditorPackage.Ids.cmdidEditorToggleResultsPrimary:
                            prgCmds[0].cmdf = (UInt32)(OLECMDF.OLECMDF_SUPPORTED | OLECMDF.OLECMDF_ENABLED);
                            if (editor.DataContext is EditorViewModel viewModel)
                            {
                                if (viewModel.ResultsVisible)
                                {
                                    prgCmds[0].cmdf |= (UInt32)OLECMDF.OLECMDF_LATCHED;
                                }
                            }
                            return VSConstants.S_OK;
                        case EditorPackage.Ids.cmdidEditorRegexMethod:
                        case EditorPackage.Ids.cmdidEditorRegexMethodItems:
                        case EditorPackage.Ids.cmdidEditorTesterMode:
                        case EditorPackage.Ids.cmdidEditorTesterModeItems:
                            return VSConstants.S_OK;
                    }
                }
                else
                {
                    if (editor.GetActiveView() is IOleCommandTarget activeTarget)
                    {
                        return activeTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
                    }
                }
            }

            return (Int32)OleConstants.OLECMDERR_E_NOTSUPPORTED;
        }
    }
}