using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace Losenkov.RegexEditor.Colorer.Input
{
    [Export(typeof(IViewTaggerProvider))]
    [ContentType(UI.RegexInputContentType.ContentTypeName)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [TagType(typeof(IntraTextAdornmentTag))]
    [TagType(typeof(ITextMarkerTag))]
    sealed class HighlightTaggerProvider : IViewTaggerProvider
    {
        #region imports
#pragma warning disable 649 // "field never assigned to" -- field is set by MEF.
        [Import] internal IClassificationTypeRegistryService classificationTypeRegistryService;
        [Import] internal IClassificationFormatMapService classificationFormatMapService;
#pragma warning restore 649
        #endregion

        #region exports
#pragma warning disable 649, 169 // "field never assigned to" -- field is set by MEF.
        [Export, Name(EmptyMarkerClassificationFormatDefinition.Name)] internal static ClassificationTypeDefinition EmptyMarkerClassificationType;
#pragma warning restore 649, 169
        #endregion

        HighlightTagger CreateTagger(ITextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty(
              () => new HighlightTagger(
                (IWpfTextView)view,
                classificationFormatMapService.GetClassificationFormatMap(view),
                classificationTypeRegistryService));
        }

        internal static Boolean TryGetTaggerForView(ITextView view, out HighlightTagger tagger)
        {
            return view.Properties.TryGetProperty<HighlightTagger>(typeof(HighlightTagger), out tagger);
        }

        public ITagger<T> CreateTagger<T>(ITextView view, ITextBuffer buffer) where T : ITag
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (view.TextBuffer != buffer)
            {
                return null;
            }

            return (CreateTagger(view) as ITagger<T>);
        }
    }
}