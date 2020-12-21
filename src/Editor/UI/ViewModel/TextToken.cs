using System;
using System.Collections.Generic;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    public struct TextToken
    {
        public TextToken(String text, Byte type)
        {
            Text = text;
            Type = type;
        }

        public String Text { get; }
        public Byte Type { get; }

        public static readonly TextToken Null = new TextToken("-- null --", 1);
        public static readonly TextToken Empty = new TextToken("-- empty --", 1);
        public static readonly TextToken Failure = new TextToken("-- failure --", 1);
        public static readonly TextToken Truncated = new TextToken("-- truncated --", 1);

        private static readonly TextToken[] s_special = new TextToken[33] {
            new TextToken("00", 1),
            new TextToken("01", 1),
            new TextToken("02", 1),
            new TextToken("03", 1),
            new TextToken("04", 1),
            new TextToken("05", 1),
            new TextToken("06", 1),
            new TextToken("07", 1),
            new TextToken("08", 1),
            new TextToken("TAB", 2),
            new TextToken("LF", 2),
            new TextToken("0B", 1),
            new TextToken("0C", 1),
            new TextToken("CR", 2),
            new TextToken("0E", 1),
            new TextToken("0F", 1),
            new TextToken("10", 1),
            new TextToken("11", 1),
            new TextToken("12", 1),
            new TextToken("13", 1),
            new TextToken("14", 1),
            new TextToken("15", 1),
            new TextToken("16", 1),
            new TextToken("17", 1),
            new TextToken("18", 1),
            new TextToken("19", 1),
            new TextToken("1A", 1),
            new TextToken("1B", 1),
            new TextToken("1C", 1),
            new TextToken("1D", 1),
            new TextToken("1E", 1),
            new TextToken("1F", 1),
            new TextToken("\u00B7", 3),
        };

        public static TextToken[] Tokenize(String text)
        {
            if (text == null)
            {
                return new TextToken[] { Null };
            }

            if (text.Length == 0)
            {
                return new TextToken[] { Empty };
            }

            var result = new List<TextToken>();
            var pos = 0;
            var len = text.Length;
            for (var i = 0; i < len; i++)
            {
                var ch = text[i];
                if (ch <= 0x0020)
                {
                    if (pos < i)
                    {
                        result.Add(new TextToken(text.Substring(pos, i - pos), 0));
                    }
                    result.Add(s_special[ch]);
                    pos = i + 1;
                }
            }
            if (pos < len)
            {
                result.Add(new TextToken(text.Substring(pos, len - pos), 0));
            }

            return result.ToArray();
        }
    }
}