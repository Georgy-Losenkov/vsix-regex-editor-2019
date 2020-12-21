using System;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Projection;
using Microsoft.VisualStudio.Utilities;

namespace Losenkov.RegexEditor.UI
{
    [Export(typeof(EditorServices))]
    [Guid("9434AE0A-2C01-4A61-B2C0-75DD888D5D2E")]
    public class EditorServices
    {
        [Import(typeof(IContentTypeRegistryService))]
        internal IContentTypeRegistryService ContentTypeRegistryService { get; set; }

        [Import(typeof(ITextBufferFactoryService))]
        internal ITextBufferFactoryService TextBufferFactoryService { get; set; }

        [Import(typeof(IProjectionBufferFactoryService))]
        internal IProjectionBufferFactoryService ProjectionBufferFactoryService { get; set; }

        [Import(typeof(ITextEditorFactoryService))]
        internal ITextEditorFactoryService TextEditorFactoryService { get; set; }

        [Import(typeof(IVsEditorAdaptersFactoryService))]
        internal IVsEditorAdaptersFactoryService VsEditorAdaptersFactoryService { get; set; }
    }
}