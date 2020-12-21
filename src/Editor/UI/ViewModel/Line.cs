using System;
using System.Collections.Generic;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    public class Line
    {
        internal Line(String text, Int32 index, Int32 length)
        {
            Text = text;
            Index = index;
            Length = length;
        }

        public override String ToString()
        {
            return Value;
        }

        public Int32 Index { get; }

        public Int32 Length { get; }

        String Text { get; }

        public String Value
        {
            get { return Text.Substring(Index, Length); }
        }

        public static Line[] Split(String str)
        {
            if (str == null)
            {
                return new Line[0];
            }

            var result = new List<Line>();
            var pos = 0;
            var len = str.Length;
            for (var i = 0; i < len;)
            {
                var ch = str[i];
                if (ch == '\r' || ch == '\n')
                {
                    result.Add(new Line(str, pos, i - pos));
                    i++;
                    if (ch == '\r' && i < len && str[i] == '\n')
                    {
                        i++;
                    }
                    pos = i;
                }
                else
                {
                    i++;
                }
            }
            if (pos < len)
            {
                result.Add(new Line(str, pos, len - pos));
            }

            return result.ToArray();
        }
    }
}