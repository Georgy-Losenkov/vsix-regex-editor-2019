using System;
using System.Collections.Generic;
using Losenkov.RegexEditor.Parser.Replacement;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace Losenkov.RegexEditor.Colorer.Replacement
{
    /// <summary>Implements the coloring classification.</summary>
    internal sealed class ReplacementClassifier : IClassifier
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ITextBuffer m_buffer;
#pragma warning restore IDE0052 // Remove unread private members
        private readonly IClassificationTypeRegistryService m_classificationTypeRegistryService;

        internal ReplacementClassifier(ITextBuffer bufferToClassify, IClassificationTypeRegistryService classificationTypeRegistryService)
        {
            m_buffer = bufferToClassify;
            m_classificationTypeRegistryService = classificationTypeRegistryService;
        }

        #region IClassifier Members
        // Use this event if a text change causes classifications on a line other the one on which the line occurred.
#pragma warning disable CS0067 // Remove unread private members
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;
#pragma warning restore CS0067 // Remove unread private members

        //This is the main method of the classifier. It should return one ClassificationSpan per group that needs coloring.
        //It will be called with a span that spans a single line where the edit has been made (or multiple times in paste operations).
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            var result = new List<ClassificationSpan>();
            var runner = span.Snapshot.TextBuffer.Properties.GetProperty<ParserRunner>(typeof(ParserRunner));

            //Create a parser to parse the regular expression, and return the classification spans defined by it.
            foreach (var token in runner.Tokens)
            {
                if (Token.TryTranslateEnumToString(token.Kind, out var type))
                {
                    var classificationType = m_classificationTypeRegistryService.GetClassificationType(type);
                    result.Add(
                      new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.Start, token.End - token.Start), classificationType)
                    );
                }

                //if (span.IntersectsWith(new Span(token.Start, token.End)) && !(span.Start.Position <= token.Start && span.End.Position >= token.End) &&
                //    (token.Kind == TokenKind.Capture || token.Kind == TokenKind.CharGroup || token.Kind == TokenKind.Multiplier))
                //{
                //  RecolorizeAffectedLines(new Span(span.End, token.End - span.End), span.Snapshot);
                //}
            }

            return result;
        }
        #endregion
    }
}