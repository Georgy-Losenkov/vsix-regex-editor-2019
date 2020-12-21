using System;

namespace Losenkov.RegexEditor.Parser.Replacement
{
    #region TokenKind
    /// <summary>King of tokens</summary>
    internal enum TokenKind : Byte
    {
        NamedGroup = 01,
        NumberedGroup = 02,
        DblDolarSign = 03,
        CopyMatch = 04,
        TextBefore = 05,
        TextAfter = 06,
        LastGroup = 07,
        EntireInput = 08,
        DolarSign = 09,
        PlainText = 10,
    }
    #endregion
}