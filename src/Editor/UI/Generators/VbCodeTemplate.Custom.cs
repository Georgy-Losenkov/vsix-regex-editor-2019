using System;
using System.Globalization;
using System.Text;
using RegexOptions = System.Text.RegularExpressions.RegexOptions;

namespace Losenkov.RegexEditor.UI.Generators
{
  partial class VbCodeTemplate
  {
    internal VbCodeTemplate(ViewModel.IOptions snapshot, Model.RegexMethod regexMethod) : base(snapshot, regexMethod) 
    {
    }

    void EnsureInDoubleQuotes(ref bool fInDoubleQuotes, StringBuilder b)
    {
      if (!fInDoubleQuotes)
      {
        b.Append(" & \"");
        fInDoubleQuotes = true;
      }
    }

    void EnsureNotInDoubleQuotes(ref bool fInDoubleQuotes, StringBuilder b)
    {
      if (fInDoubleQuotes)
      {
        b.Append("\"");
        fInDoubleQuotes = false;
      }
    }

    const int MaxLineLength = 80;

    string QuoteSnippetString(String value, String indentationString)
    {
      StringBuilder b = new StringBuilder(value.Length + 5);

      bool fInDoubleQuotes = true;

      b.Append("\"");

      int i = 0;
      while (i < value.Length)
      {
        char ch = value[i];
        switch (ch)
        {
          case '\"':
          // These are the inward sloping quotes used by default in some cultures like CHS. 
          // VBC.EXE does a mapping ANSI that results in it treating these as syntactically equivalent to a
          // regular double quote.
          case '\u201C':
          case '\u201D':
          case '\uFF02':
            EnsureInDoubleQuotes(ref fInDoubleQuotes, b);
            b.Append(ch);
            b.Append(ch);
            break;
          case '\r':
            EnsureNotInDoubleQuotes(ref fInDoubleQuotes, b);
            if (i < value.Length - 1 && value[i + 1] == '\n')
            {
              b.Append(" & VB.vbCrLf");
              i++;
            }
            else
            {
              b.Append(" & VB.vbCr");
            }
            break;
          case '\t':
            EnsureNotInDoubleQuotes(ref fInDoubleQuotes, b);
            b.Append(" & VB.vbTab");
            break;
          case '\0':
            EnsureNotInDoubleQuotes(ref fInDoubleQuotes, b);
            b.Append(" & VB.vbNullChar");
            break;
          case '\n':
            EnsureNotInDoubleQuotes(ref fInDoubleQuotes, b);
            b.Append(" & VB.vbLf");
            break;
          case '\u2028':
          case '\u2029':
            EnsureNotInDoubleQuotes(ref fInDoubleQuotes, b);
            b.Append(" & VB.ChrW(");
            b.Append(((int)ch).ToString(CultureInfo.InvariantCulture));
            b.Append(")");
            break;
          default:
            EnsureInDoubleQuotes(ref fInDoubleQuotes, b);
            b.Append(value[i]);
            break;
        }

        if (0 < i && i % MaxLineLength == 0)
        {
          //
          // If current character is a high surrogate and the following 
          // character is a low surrogate, don't break them. 
          // Otherwise when we write the string to a file, we might lose 
          // the characters.
          // 
          if (Char.IsHighSurrogate(value[i])
              && i < value.Length - 1
              && Char.IsLowSurrogate(value[i + 1]))
          {
            b.Append(value[++i]);
          }

          if (fInDoubleQuotes)
            b.Append("\"");
          fInDoubleQuotes = true;

          b.Append("& _ ");
          b.Append(Environment.NewLine);
          b.Append(indentationString);
          b.Append('\"');
        }
        ++i;
      }

      if (fInDoubleQuotes)
        b.Append("\"");

      return b.ToString();
    }
  }
}