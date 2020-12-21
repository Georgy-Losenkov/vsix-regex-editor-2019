using System;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    public sealed class Segment
    {
        public Int32 Start { get; }
        public Int32 Length { get; }

        public Segment(Int32 start, Int32 length)
        {
            Start = start;
            Length = length;
        }

        public override String ToString()
        {
            if (Length < 0)
            {
                return "";
            }

            if (Length == 0)
            {
                return "(" + Start + ")";
            }

            return "[" + Start + ".." + (Start + Length - 1) + "]";
        }
    }
}