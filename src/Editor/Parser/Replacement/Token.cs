using System;
using Losenkov.RegexEditor.Colorer.Replacement;

namespace Losenkov.RegexEditor.Parser.Replacement
{
    #region TokenKind
    #endregion

    #region Token
    /// <summary>Unit of parse for a regular expression.</summary>
    internal struct Token
    {
        public Int32 Start { get; }
        public Int32 End { get; }
        public TokenKind Kind { get; }

        public Token(Int32 start, Int32 end, TokenKind kind)
        {
            Start = start;
            End = end;
            Kind = kind;
        }

        public static Boolean TryTranslateEnumToString(TokenKind tokenKindEnum, out String result)
        {
            switch (tokenKindEnum)
            {
                case TokenKind.PlainText:
                case TokenKind.DolarSign:
                    result = String.Empty;
                    return false;
                case TokenKind.NumberedGroup:
                case TokenKind.NamedGroup:
                case TokenKind.DblDolarSign:
                case TokenKind.CopyMatch:
                case TokenKind.TextBefore:
                case TokenKind.TextAfter:
                case TokenKind.LastGroup:
                case TokenKind.EntireInput:
                    result = ReplacementSubstitutionClassificationFormatDefinition.Name;
                    return true;
                default:
                    result = String.Empty;
                    return false;
            }
        }
    }
    #endregion
}