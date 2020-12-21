using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Losenkov.RegexEditor.Parser.Replacement
{
    /// <summary>
    /// Parses the regular expression into tokens.
    /// </summary>
    internal sealed class Parser
    {
        const String ParserPattern =
          /* 01 */ @"(\$\{(?:\p{L}\w+|\d+)\})" + "|" +
          /* 02 */ @"(\$\d+)" + "|" +
          /* 03 */ @"(\$\$)" + "|" +
          /* 04 */ @"(\$\&)" + "|" +
          /* 05 */ @"(\$\`)" + "|" +
          /* 06 */ @"(\$\')" + "|" +
          /* 07 */ @"(\$\+)" + "|" +
          /* 08 */ @"(\$_)" + "|" +
          /* 09 */ @"(\$)" + "|" +
          /* 10 */ @"([^$]+)";
        static readonly Regex s_parserRegex = new Regex(ParserPattern, RegexOptions.Compiled);

        public static List<Token> Parse(String text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var tokens = new List<Token>();

            for (var match = s_parserRegex.Match(text); match.Success; match = match.NextMatch())
            {
                for (Byte i = 1; i <= 10; i++)
                {
                    var group = match.Groups[i];
                    if (group.Success)
                    {
                        tokens.Add(new Token(group.Index, group.Index + group.Length, (TokenKind)i));
                        break;
                    }
                }
            }

            return tokens;
        }
    }
}