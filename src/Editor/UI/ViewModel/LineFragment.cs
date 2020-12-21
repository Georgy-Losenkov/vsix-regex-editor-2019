using System;
using System.Collections.Generic;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    public class LineFragment
    {
        #region static members
        public static Type EnumerableType { get { return typeof(IEnumerable<LineFragment>); } }
        public static LineFragments Sample { get; }

        static LineFragment()
        {
            Sample = new LineFragments(
              new LineFragment(new Line("Line # 1 of text", 0, 10), 0, null, String.Empty),
              new LineFragment(new Line("Line # 1 of text", 0, 10), 0, null, String.Empty),
              new LineFragment(new Line("Line # 1 of text", 0, 10), 0, null, String.Empty),
              new LineFragment(new Line("Line # 2 of text", 0, 10), 1, 0, "Chunk #1"),
              new LineFragment(new Line("Line # 2 of text", 0, 10), 1, 1, "Chunk #2"),
              new LineFragment(new Line("Line # 2 of text", 0, 10), 1, 2, "Chunk #3"),
              new LineFragment(new Line("Line # 6 of text", 0, 10), 2, 0, "Replacement #6"),
              new LineFragment(new Line("Line # 7 of text", 0, 10), 3, 0, "Replacement #7"));
        }
        #endregion

        public Segment Segment { get; }
        public Int32 LineNum { get; }
        public Int32? ChunkIndex { get; }
        public String Text { get; }

        public LineFragment(Line line, Int32 lineNum, Int32? chunkIndex, String text)
        {
            Segment = new Segment(line.Index, line.Length);
            LineNum = lineNum;
            ChunkIndex = chunkIndex;
            Text = text;
        }
    }
}