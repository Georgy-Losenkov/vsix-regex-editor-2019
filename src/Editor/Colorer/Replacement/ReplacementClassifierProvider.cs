using System.ComponentModel.Composition;
using Losenkov.RegexEditor.Parser.Replacement;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Losenkov.RegexEditor.Colorer.Replacement
{
    [Export(typeof(IClassifierProvider))]
    [ContentType(UI.RegexReplacementContentType.ContentTypeName)]
    internal sealed class ReplacementClassifierProvider : IClassifierProvider
    {
        #region imports
#pragma warning disable 649 // "field never assigned to" -- field is set by MEF.
        [Import] internal IClassificationTypeRegistryService classificationTypeRegistryService;
#pragma warning restore 649
        #endregion

        #region exports
#pragma warning disable 649, 169 // "field never assigned to" -- field is set by MEF.
        [Export, Name(ReplacementSubstitutionClassificationFormatDefinition.Name)] internal static ClassificationTypeDefinition ReplacementSubstitutionClassificationType;
#pragma warning restore 649, 169
        #endregion

        public IClassifier GetClassifier(ITextBuffer buffer)
        {
            buffer.Properties.GetOrCreateSingletonProperty<ParserRunner>(() => new ParserRunner(buffer));

            return new ReplacementClassifier(buffer, classificationTypeRegistryService);
        }
    }
}