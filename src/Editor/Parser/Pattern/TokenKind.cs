namespace Losenkov.RegexEditor.Parser.Pattern
{
    /// <summary>Kind of token</summary>
    internal enum TokenKind
    {
        Alternation,
        Anchor,
        Capture,
        CaptureName,
        Comment,
        Quantifier,

        CharGroup,
        EscapedExpression,
        Expression,
    }
}