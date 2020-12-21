using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Text;

namespace Losenkov.RegexEditor.Parser.Pattern
{
    /// <summary>
    /// Executes the parsing when the text buffer changes.
    /// </summary>
    internal sealed class ParserRunner
    {
        private readonly ITextBuffer m_textBuffer;
        internal Parser Parser { get; private set; }

        internal ParserRunner(ITextBuffer textBuffer)
        {
            this.m_textBuffer = textBuffer;
            this.Parser = new Parser();

            m_textBuffer.Changed += new EventHandler<TextContentChangedEventArgs>(TextBuffer_Changed);

            Parse();
        }

        private void TextBuffer_Changed(Object source, TextContentChangedEventArgs e)
        {
            Parse();
        }

        private void Parse()
        {
            this.Parser.Parse(m_textBuffer.CurrentSnapshot.GetText());
        }
    }
}