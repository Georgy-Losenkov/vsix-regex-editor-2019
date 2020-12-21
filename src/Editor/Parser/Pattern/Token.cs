using System;
using Losenkov.RegexEditor.Colorer.Pattern;

namespace Losenkov.RegexEditor.Parser.Pattern
{
    /// <summary>
    /// Unit of parse for a regular expression.
    /// </summary>
    internal struct Token
    {
        internal Int32 Start { get; }
        internal Int32 End { get; }
        internal TokenKind Kind { get; }
        internal Boolean MatchBraces { get; }

        internal Token(Int32 start, Int32 end, TokenKind kind)
        {
            Start = start;
            End = end;
            Kind = kind;
            MatchBraces = false;
        }

        internal Token(Int32 start, Int32 end, TokenKind kind, Boolean matchBraces)
        {
            Start = start;
            End = end;
            Kind = kind;
            MatchBraces = matchBraces;
        }

        internal static String TranslateEnumToString(TokenKind tokenKindEnum)
        {
            switch (tokenKindEnum)
            {
                case TokenKind.Alternation:
                    return PatternAlternationClassificationFormatDefinition.Name;
                case TokenKind.Anchor:
                    return PatternAnchorClassificationFormatDefinition.Name;
                case TokenKind.Capture:
                    return PatternGroupingClassificationFormatDefinition.Name;
                case TokenKind.CaptureName:
                    return PatternGroupingClassificationFormatDefinition.Name;
                case TokenKind.Comment:
                    return PatternCommentClassificationFormatDefinition.Name;
                case TokenKind.Quantifier:
                    return PatternQuantifierClassificationFormatDefinition.Name;

                case TokenKind.CharGroup:
                    return PatternCharGroupClassificationFormatDefinition.Name;
                case TokenKind.EscapedExpression:
                    return PatternEscapedExpressionClassificationFormatDefinition.Name;
                case TokenKind.Expression:
                    return PatternExpressionClassificationFormatDefinition.Name;
                default:
                    return "";
            }
        }
    }
}
