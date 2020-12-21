using System;
using System.Collections.Generic;

namespace Losenkov.RegexEditor.Parser.Pattern
{
    /// <summary>
    /// Parses the regular expression into tokens.
    /// </summary>
    internal sealed class Parser
    {
        internal IList<Token> Tokens { get; private set; }
        private int position;

        #region Language Constants
        private const Char StartChar = '$';
        private const Char EndChar = '^';
        private const Char SpaceChar = ' ';
        private const Char NewLineChar = '\n';
        private const Char LineFeedChar = '\r';
        private const Char TabChar = '\t';
        private const Char StartCharGroupChar = '[';
        private const Char EndCharGroupChar = ']';
        private const Char StartRepetitionsChar = '{';
        private const Char EndRepetitionsChar = '}';
        private const Char StartCaptureChar = '(';
        private const Char SecondCaptureChar = '?';
        private const Char EndCaptureChar = ')';
        private const Char EscapeChar = '\\';
        private const Char HexExpressionChar = 'x';
        private const Char UnicodeExpressionChar = 'u';
        private const Char OctalExpressionChar = 'o';
        private const Char OneOrManyChar = '+';
        private const Char ZeroOrManyChar = '*';
        private const Char ZeroOrOneChar = '?';
        private const Char OrChar = '|';
        private const Char NegativeCaptureModifierChar = '-';
        private const Char CaptureSeparatorChar = ':';
        private const Char CommentChar = '#';
        private const Char NegativeChar = '!';
        private const Char LessThanChar = '<';
        private const Char EqualsChar = '=';
        private const Char NamedCaptureEnd = '>';
        #endregion

        internal void Parse(String text)
        {
            this.Tokens = new List<Token>();
            this.position = 0;

            DropWhitespace(text);
            ParseStart(text);
            DropWhitespace(text);
            ParseBody(text, EndChar);
            DropWhitespace(text);
            ParseEnd(text);
        }

        //Whitespace needs not be parsed for coloring. Ignore it when needed.
        private void DropWhitespace(String text)
        {
            while (position < text.Length && (text[position] == SpaceChar || text[position] == NewLineChar || text[position] == LineFeedChar || text[position] == TabChar))
            {
                position++;
            }
        }

        //Parse the body of the expression, looking for different language parts and defaulting to plain text
        internal void ParseBody(String text, char until)
        {
            while (position < text.Length && text[position] != until)
            {
                if (!ParseCharGroup(text)
                    && !ParseRepetition(text)
                    && !ParseExpression(text)
                    && !ParseMultiplier(text)
                    && !ParseCapture(text))
                {
                    position++;
                }
                DropWhitespace(text);
            }
        }

        //Look for capture groups (of the form "(text)" in regular expressions)
        internal Boolean ParseCapture(String text)
        {
            if (position < text.Length && text[position] == StartCaptureChar)
            {
                int firstChar = position;
                position++;
                //When the first character in the group is a "?", there are many options for modifiers
                if (position < text.Length && text[position] == SecondCaptureChar)
                {
                    position++;
                    if (position < text.Length && text[position] == CommentChar)
                    {
                        position++;

                        while (position < text.Length && text[position] != EndCaptureChar)
                        {
                            position++;
                        }

                        if (position < text.Length)
                        {
                            position++;
                            Tokens.Add(new Token(firstChar, position, TokenKind.Comment));
                        }

                        return true;
                    }
                    else
                    {
                        ParseModifiedCapture(text);
                    }
                }

                ParseBody(text, EndCaptureChar);
                if (position < text.Length)
                {
                    if (text[position] == EndCaptureChar)
                    {
                        position++;
                    }
                    Tokens.Add(new Token(firstChar, position, TokenKind.Capture, text[position - 1] == EndCaptureChar));
                }

                return true;
            }
            return false;
        }

        private Boolean IsValidCaptureModifier(Char ch)
        {
            return 0 <= "imnsx".IndexOf(ch);
        }

        //Look for special patterns in a capture group (of the form "(?capture)" in regular expressions).
        private void ParseModifiedCapture(String text)
        {
            //Look for special modifiers to the capture. The modifiers are:
            // i for ignore case
            // m for multiline
            // n for explicit capture
            // s for single line
            // s for ignore whitespace
            if (position < text.Length && IsValidCaptureModifier(text[position]))
            {
                position++;
                if (position < text.Length && text[position] == CaptureSeparatorChar)
                {
                    position++;
                    Tokens.Add(new Token(position - 3, position, TokenKind.CaptureName));
                }
            }
            //Same as before, but prefixed with a '-' to turn off the setting
            else if (position < text.Length && text[position] == NegativeCaptureModifierChar)
            {
                position++;
                if (position < text.Length && IsValidCaptureModifier(text[position]))
                {
                    position++;
                    if (position < text.Length && text[position] == CaptureSeparatorChar)
                    {
                        position++;
                        Tokens.Add(new Token(position - 4, position, TokenKind.CaptureName));
                    }
                }
            }
            //Look for groups that aren't separate matches (of the form "(?:text)" in regular expressions)
            else if (position < text.Length && text[position] == CaptureSeparatorChar)
            {
                position++;
                Tokens.Add(new Token(position - 2, position, TokenKind.CaptureName));
            }
            //Look for lookahead expressions (of the form "(?=group)" in regular expressions)
            else if (position < text.Length && text[position] == EqualsChar)
            {
                position++;
                Tokens.Add(new Token(position - 2, position, TokenKind.CaptureName));
            }
            //Look for negative lookahead expressions (of the form "(?!group)" in regular expressions)
            else if (position < text.Length && text[position] == NegativeChar)
            {
                position++;
                Tokens.Add(new Token(position - 2, position, TokenKind.CaptureName));
            }
            else if (position < text.Length && text[position] == LessThanChar)
            {
                position++;
                ParseNamedGroup(text);
                position++;
            }
            //Look for conditional expressions (of the form "(?(text)yes|no)" in regular expressions)
            else if (position < text.Length && text[position] == StartCaptureChar)
            {
                int firstChar = position - 2;
                while (position < text.Length && text[position] != EndCaptureChar)
                {
                    position++;
                }

                if (position < text.Length)
                {
                    Tokens.Add(new Token(firstChar, position + 1, TokenKind.CaptureName, true));
                }
                else
                {
                    Tokens.Add(new Token(firstChar, position, TokenKind.CaptureName));
                }
                position++;
            }
        }

        //Look for named groups (represented as "(?<name>text)" in regular expressions). Up to this point, the starting "(?<" will be parsed.
        //The remainder might be of the form "(?<=", "(?<!" or an actual named group.
        private void ParseNamedGroup(String text)
        {
            //Look for lookbehind expressions (of the form "(?<=group)" in regular expressions)
            if (position < text.Length && text[position] == EqualsChar)
            {
                Tokens.Add(new Token(position - 2, position + 1, TokenKind.CaptureName));
            }
            //Look for negative lookbehind expressions (of the form "(?<!group)" in regular expressions)
            else if (position < text.Length && text[position] == NegativeChar)
            {
                Tokens.Add(new Token(position - 2, position + 1, TokenKind.CaptureName));
            }
            else
            {
                //Look for named groups
                int firstChar = position - 2;
                while (position < text.Length && text[position] != NamedCaptureEnd)
                {
                    position++;
                }

                if (position < text.Length)
                {
                    Tokens.Add(new Token(firstChar, position + 1, TokenKind.CaptureName, true));
                }
                else
                {
                    Tokens.Add(new Token(firstChar, position, TokenKind.CaptureName));
                }
            }
        }

        //Look for square brackets for character groups (represented as "[abcd]" in regular expressions)
        internal Boolean ParseCharGroup(String text)
        {
            if (position < text.Length && text[position] == StartCharGroupChar)
            {
                int firstChar = position;
                while (position < text.Length && text[position] != EndCharGroupChar)
                {
                    if (text[position] == EscapeChar)
                    {
                        position++;
                    }
                    if (position < text.Length)
                    {
                        position++;
                    }
                }
                if (position < text.Length)
                {
                    Tokens.Add(new Token(firstChar, position + 1, TokenKind.CharGroup, true));
                }
                else
                {
                    Tokens.Add(new Token(firstChar, position, TokenKind.CharGroup));
                }

                position++;
                return true;
            }
            return false;
        }

        //Look for multipliers and separators ("|", "*", "+" and "?" in regular expressions)
        internal Boolean ParseMultiplier(String text)
        {
            if (position < text.Length && text[position] == OneOrManyChar)
            {
                Tokens.Add(new Token(position, position + 1, TokenKind.Quantifier));
                position++;
                return true;
            }
            else if (position < text.Length && text[position] == ZeroOrManyChar)
            {
                Tokens.Add(new Token(position, position + 1, TokenKind.Quantifier));
                position++;
                return true;
            }
            else if (position < text.Length && text[position] == ZeroOrOneChar)
            {
                Tokens.Add(new Token(position, position + 1, TokenKind.Quantifier));
                position++;
                return true;
            }
            else if (position < text.Length && text[position] == OrChar)
            {
                Tokens.Add(new Token(position, position + 1, TokenKind.Alternation));
                position++;
                return true;
            }
            return false;
        }

        private static Boolean IsValidExpressionChar(Char ch)
        {
            return 0 <= "dDwWsSAzZbBvtrnf".IndexOf(ch);
        }

        //Look for escaped expressions (represented as "\" followed by any of this characters: dDwWsSAzZbBuxo)
        internal Boolean ParseExpression(String text)
        {
            if (position < text.Length && text[position] == EscapeChar)
            {
                if (position + 1 < text.Length)
                {
                    if (text[position + 1] == HexExpressionChar)
                    {
                        //Hexadecimal is represented by \x followed by two digits: \x00
                        if (position + 3 < text.Length && Int32.TryParse(text.Substring(position + 2, 2), out _))
                        {
                            Tokens.Add(new Token(position, position + 4, TokenKind.Expression));
                            position += 3;
                            return true;
                        }
                    }
                    else if (text[position + 1] == UnicodeExpressionChar)
                    {
                        //Unicode is represented by \u followed by four digits: \u0000
                        if (position + 5 < text.Length && Int32.TryParse(text.Substring(position + 2, 4), out _))
                        {
                            Tokens.Add(new Token(position, position + 6, TokenKind.Expression));
                            position += 5;
                            return true;
                        }
                    }
                    else if (text[position + 1] == OctalExpressionChar)
                    {
                        //Octal is represented by \o followed by three digits: \o000
                        if (position + 4 < text.Length && Int32.TryParse(text.Substring(position + 2, 3), out _))
                        {
                            Tokens.Add(new Token(position, position + 5, TokenKind.Expression));
                            position += 4;
                            return true;
                        }
                    }
                    else if (IsValidExpressionChar(text[position + 1]))
                    {
                        //All other expressions are of the form \x, where x is one of the valid characters
                        Tokens.Add(new Token(position, position + 2, TokenKind.Expression));
                        position += 2;
                        return true;
                    }
                    else
                    {
                        Tokens.Add(new Token(position, position + 1, TokenKind.EscapedExpression));
                        position += 2;
                        return true;
                    }
                }
                else
                {
                    Tokens.Add(new Token(position, position, TokenKind.EscapedExpression));
                    position += 1;
                    return true;
                }
            }
            return false;
        }

        //Look for curly brackets for repetitions (represented as "{1}" in regular expressions)
        internal Boolean ParseRepetition(String text)
        {
            if (position < text.Length && text[position] == StartRepetitionsChar)
            {
                int firstChar = position;
                while (position < text.Length && text[position] != EndRepetitionsChar)
                {
                    position++;
                }
                if (position < text.Length)
                {
                    Tokens.Add(new Token(firstChar, position + 1, TokenKind.Quantifier, true));
                }
                else
                {
                    Tokens.Add(new Token(firstChar, position, TokenKind.Quantifier));
                }

                position++;
                return true;
            }
            return false;
        }

        //A regular expression may start with a "$" symbol
        internal Boolean ParseStart(String text)
        {
            if (position < text.Length && text[position] == StartChar)
            {
                Tokens.Add(new Token(position, position + 1, TokenKind.Anchor));
                position++;
                return true;
            }
            return false;
        }

        //A regular expression may end with a "^" symbol
        internal Boolean ParseEnd(String text)
        {
            if (position < text.Length && text[position] == EndChar)
            {
                Tokens.Add(new Token(position, position + 1, TokenKind.Anchor));
                position++;
                return true;
            }
            return false;
        }
    }
}