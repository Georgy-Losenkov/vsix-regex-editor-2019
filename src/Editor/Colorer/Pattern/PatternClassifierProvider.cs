using System.ComponentModel.Composition;
using Losenkov.RegexEditor.Parser.Pattern;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Losenkov.RegexEditor.Colorer.Pattern
{
    [Export(typeof(IClassifierProvider))]
    [ContentType(UI.RegexPatternContentType.ContentTypeName)]
    internal sealed class PatternClassifierProvider : IClassifierProvider
    {
        [Import]
        private IClassificationTypeRegistryService ClassificationTypeRegistry { get; set; }

        #region exports
#pragma warning disable 649, 169 // "field never assigned to" -- field is set by MEF.
        [Export, Name(PatternAlternationClassificationFormatDefinition.Name)]
        internal static ClassificationTypeDefinition PatternAlternationClassificationType;

        [Export, Name(PatternAnchorClassificationFormatDefinition.Name)]
        internal static ClassificationTypeDefinition PatternAnchorClassificationType;

        [Export, Name(PatternCommentClassificationFormatDefinition.Name)]
        internal static ClassificationTypeDefinition PatternCommentClassificationType;

        [Export, Name(PatternGroupingClassificationFormatDefinition.Name)]
        internal static ClassificationTypeDefinition PatternGroupingClassificationType;

        [Export, Name(PatternQuantifierClassificationFormatDefinition.Name)]
        internal static ClassificationTypeDefinition PatternQuantifierClassificationType;

        [Export, Name(PatternCharGroupClassificationFormatDefinition.Name)]
        internal static ClassificationTypeDefinition PatternCharGroupClassificationType;

        [Export, Name(PatternEscapedExpressionClassificationFormatDefinition.Name)]
        internal static ClassificationTypeDefinition PatternEscapedExpressionClassificationType;

        [Export, Name(PatternExpressionClassificationFormatDefinition.Name)]
        internal static ClassificationTypeDefinition PatternExpressionClassificationType;
#pragma warning restore 649, 169
        #endregion

        public IClassifier GetClassifier(ITextBuffer buffer)
        {
            buffer.Properties.GetOrCreateSingletonProperty<ParserRunner>(() => new ParserRunner(buffer));

            return new PatternClassifier(buffer, ClassificationTypeRegistry);
        }
    }
}