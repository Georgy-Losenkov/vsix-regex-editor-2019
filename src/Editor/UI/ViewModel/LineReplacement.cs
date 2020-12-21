using System;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    public class LineReplacement
    {
        public Segment Segment { get; }
        public Int32 LineNum { get; }
        public String Text { get; }

        internal LineReplacement(Line line, Int32 lineNum, String text)
        {
            Segment = new Segment(line.Index, line.Length);
            LineNum = lineNum;
            Text = text;
        }
    }
}