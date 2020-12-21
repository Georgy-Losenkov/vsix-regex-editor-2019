using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace Losenkov.RegexEditor.Colorer.Input
{
    [Export(typeof(ITaggerProvider))]
    [ContentType(UI.RegexInputContentType.ContentTypeName)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [TagType(typeof(IClassificationTag))]
    sealed class RegexTaggerProvider : ITaggerProvider
    {
        #region imports
#pragma warning disable 649 // "field never assigned to" -- field is set by MEF.
        [Import] internal IClassificationTypeRegistryService classificationTypeRegistryService;
#pragma warning restore 649
        #endregion

        #region exports
#pragma warning disable 649, 169 // "field never assigned to" -- field is set by MEF.
        [Export, Name(Group01.Name)] internal static ClassificationTypeDefinition InputGroup01ClassificationType;
        [Export, Name(Group02.Name)] internal static ClassificationTypeDefinition InputGroup02ClassificationType;
        [Export, Name(Group03.Name)] internal static ClassificationTypeDefinition InputGroup03ClassificationType;
        [Export, Name(Group04.Name)] internal static ClassificationTypeDefinition InputGroup04ClassificationType;
        [Export, Name(Group05.Name)] internal static ClassificationTypeDefinition InputGroup05ClassificationType;
        [Export, Name(Group06.Name)] internal static ClassificationTypeDefinition InputGroup06ClassificationType;
        [Export, Name(Group07.Name)] internal static ClassificationTypeDefinition InputGroup07ClassificationType;
#pragma warning restore 649, 169
        #endregion

        static RegexTagger CreateTagger(ITextBuffer buffer, IClassificationTypeRegistryService classificationTypeRegistryService)
        {
            return buffer.Properties.GetOrCreateSingletonProperty(() => new RegexTagger(buffer, classificationTypeRegistryService));
        }

        internal static Boolean TryGetTaggerForBuffer(ITextBuffer buffer, out RegexTagger tagger)
        {
            return buffer.Properties.TryGetProperty<RegexTagger>(typeof(RegexTagger), out tagger);
        }

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            if (buffer == null)
            {
                return null;
            }

            return (CreateTagger(buffer, classificationTypeRegistryService) as ITagger<T>);
        }
    }
}