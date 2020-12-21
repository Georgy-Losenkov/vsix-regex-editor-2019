using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;

namespace Losenkov.RegexEditor.Parser.Replacement
{
    /// <summary>Executes the parsing when the text buffer changes.</summary>
    internal sealed class ParserRunner
    {
        internal ParserRunner(ITextBuffer textBuffer)
        {
            TextBuffer = textBuffer ?? throw new ArgumentNullException(nameof(textBuffer));
            TextBuffer.Changed += new EventHandler<TextContentChangedEventArgs>(this.TextBuffer_Changed);

            Parse();
        }

        void TextBuffer_Changed(Object source, TextContentChangedEventArgs e)
        {
            Parse();
        }

        void Parse()
        {
            Tokens = Parser.Parse(TextBuffer.CurrentSnapshot.GetText());
        }

        ITextBuffer TextBuffer { get; }
        public List<Token> Tokens { get; private set; }
    }
}