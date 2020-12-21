using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using IServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace Losenkov.RegexEditor.UI
{
    internal sealed class EditorWrapper
    {
        public IVsTextLines VsTextLines { get; }
        public IVsTextView VsTextView { get; }
        public IWpfTextViewHost WpfTextViewHost { get; }

        public EditorWrapper(String contentType)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var serviceProvider = (IServiceProvider)ServiceProvider.GlobalProvider.GetService(typeof(IServiceProvider));
            var componentModel = (IComponentModel)ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel));
            if (componentModel == null)
            {
                throw new InvalidOperationException("SComponentModel service is not available");
            }
            var service = componentModel.GetService<EditorServices>();

            var message = "";
            var type = service.ContentTypeRegistryService.GetContentType(contentType);

            // var CSharpLanguageServiceId = new Guid("694dd9b6-b865-4c5b-ad85-86356e9c88dc");
            // var VisualBasicLanguageServiceId = new Guid("e34acdc0-baae-11d0-88bf-00a0c9110049");

            VsTextLines = (IVsTextLines)service.VsEditorAdaptersFactoryService.CreateVsTextBufferAdapter(serviceProvider, type);
            VsTextLines.InitializeContent(message, message.Length);
            // vsTextBuffer.SetLanguageServiceID(ref CSharpLanguageServiceId);

            var textRoles = service.TextEditorFactoryService.CreateTextViewRoleSet(
              PredefinedTextViewRoles.Analyzable,
              PredefinedTextViewRoles.Document,
              PredefinedTextViewRoles.Editable,
              PredefinedTextViewRoles.Interactive,
              //PredefinedTextViewRoles.PrimaryDocument,
              PredefinedTextViewRoles.Structured,
              PredefinedTextViewRoles.Zoomable
            );

            VsTextView = service.VsEditorAdaptersFactoryService.CreateVsTextViewAdapter(serviceProvider, textRoles);

            var initFlags =
              (UInt32)TextViewInitFlags.VIF_HSCROLL |
              (UInt32)TextViewInitFlags.VIF_SET_DRAGDROPMOVE |
              // (uint)TextViewInitFlags.VIF_SET_VISIBLE_WHITESPACE |
              (UInt32)TextViewInitFlags.VIF_VSCROLL |
              (UInt32)TextViewInitFlags3.VIF_NO_HWND_SUPPORT
              ;

            var initView = new INITVIEW[] {
                new INITVIEW {
                    fSelectionMargin = 0, // original: 0
                    fWidgetMargin = 0, // original: 0
                    fVirtualSpace = 0,
                    fDragDropMove = 1,
                }
            };

            VsTextView.Initialize(VsTextLines, IntPtr.Zero, initFlags, initView);

            WpfTextViewHost = service.VsEditorAdaptersFactoryService.GetWpfTextViewHost(VsTextView);
            WpfTextViewHost.TextView.Options.SetOptionValue("Appearance/Category", "text");
            WpfTextViewHost.TextView.Options.SetOptionValue("TextView/UseVisibleWhitespace", true);
            WpfTextViewHost.TextView.Options.SetOptionValue("TextViewHost/LineNumberMargin", true);
        }

        public void Select(Int32 startPos, Int32 endPos)
        {
            if (ErrorHandler.Failed(VsTextView.GetLineAndColumn(startPos, out var startLine, out var startColumn)))
            {
                return;
            }

            if (ErrorHandler.Failed(VsTextView.GetLineAndColumn(endPos, out var endLine, out var endColumn)))
            {
                return;
            }

            if (ErrorHandler.Failed(VsTextView.SetSelectionMode(TextSelMode.SM_STREAM)))
            {
                return;
            }

            VsTextView.SetSelection(startLine, startColumn, endLine, endColumn);
        }

        public void Scroll(Int32 startPos)
        {
            if (ErrorHandler.Failed(VsTextView.GetLineAndColumn(startPos, out var startLine, out var startColumn)))
            {
                return;
            }

            var span = new TextSpan() {
                iStartIndex = startColumn,
                iStartLine = startLine,
                iEndIndex = startColumn,
                iEndLine = startLine,
            };
            VsTextView.EnsureSpanVisible(span);

            // VsTextView.SetTopLine(startLine);
        }

        public String Text
        {
            get
            {
                VsTextLines.GetLastLineIndex(out var lastLineIndex, out var lastLineLength);
                VsTextLines.GetLineText(0, 0, lastLineIndex, lastLineLength, out var result);
                return result;
            }
            set
            {
                VsTextLines.GetLastLineIndex(out var lastLineIndex, out var lastLineLength);
                var pszText = Marshal.StringToCoTaskMemAuto(value);
                try
                {
                    VsTextLines.ReplaceLines(0, 0, lastLineIndex, lastLineLength, pszText, value.Length, null);
                }
                finally
                {
                    Marshal.FreeCoTaskMem(pszText);
                }
            }
        }
    }
}