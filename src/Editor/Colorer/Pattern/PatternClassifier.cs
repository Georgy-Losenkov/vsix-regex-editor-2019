using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Losenkov.RegexEditor.Parser.Pattern;

namespace Losenkov.RegexEditor.Colorer.Pattern
{
    /// <summary>
    /// Implements the coloring classification.
    /// </summary>
    internal sealed class PatternClassifier : IClassifier
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ITextBuffer m_buffer;
#pragma warning restore IDE0052 // Remove unread private members
        private readonly IClassificationTypeRegistryService m_classificationTypeRegistry;

        internal PatternClassifier(ITextBuffer bufferToClassify, IClassificationTypeRegistryService classificationTypeRegistry)
        {
            m_buffer = bufferToClassify;
            m_classificationTypeRegistry = classificationTypeRegistry;
        }

        //Coloring some expressions may affect other lines. The classifier is called for a specific line, so we
        //need to instruct it to be called again for each line that may be affected. This code calls the colorizer
        //for any span that will be affected.
        private void RecolorizeAffectedLines(Span span, ITextSnapshot snapshot)
        {
            if (span.Start < span.End)
            {
                OnClassificationChanged(new SnapshotSpan(snapshot, span.Start, span.End - span.Start));
            }
        }

        #region IClassifier Members
        // Use this event if a text change causes classifications on a line other the one on which the line occurred.
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

        internal void OnClassificationChanged(SnapshotSpan span)
        {
            if (ClassificationChanged != null)
            {
                ClassificationChanged(this, new ClassificationChangedEventArgs(span));
            }
        }

        //This is the main method of the classifier. It should return one ClassificationSpan per group that needs coloring.
        //It will be called with a span that spans a single line where the edit has been made (or multiple times in paste operations).
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            var classificationSpans = new List<ClassificationSpan>();

            //Create a parser to parse the regular expression, and return the classification spans defined by it.
            foreach (var token in span.Snapshot.TextBuffer.Properties.GetProperty<ParserRunner>(typeof(ParserRunner)).Parser.Tokens)
            {
                var captureClassificationType = m_classificationTypeRegistry.GetClassificationType(Token.TranslateEnumToString(token.Kind));
                if (token.Kind == TokenKind.Capture)
                {
                    classificationSpans.Add(new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.Start, 1), captureClassificationType));
                    classificationSpans.Add(new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.End - 1, 1), captureClassificationType));
                }
                else
                {
                    classificationSpans.Add(new ClassificationSpan(new SnapshotSpan(span.Snapshot, token.Start, token.End - token.Start), captureClassificationType));
                }

                if (span.IntersectsWith(new Span(token.Start, token.End - token.Start))
                    && !(span.Start.Position <= token.Start && span.End.Position >= token.End)
                    && (token.Kind == TokenKind.Capture || token.Kind == TokenKind.CharGroup || token.Kind == TokenKind.Quantifier))
                {
                    RecolorizeAffectedLines(new Span(span.End, token.End - span.End), span.Snapshot);
                }
            }

            return classificationSpans;
        }
        #endregion
    }
}