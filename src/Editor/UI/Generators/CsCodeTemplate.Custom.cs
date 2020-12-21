using System;
using System.Globalization;
using System.Text;
using RegexOptions = System.Text.RegularExpressions.RegexOptions;

namespace Losenkov.RegexEditor.UI.Generators
{
  partial class CsCodeTemplate
  {
    internal CsCodeTemplate(ViewModel.IOptions snapshot, Model.RegexMethod regexMethod) : base(snapshot, regexMethod) 
    {
    }

    private const int MaxLineLength = 80;

    string QuoteSnippetStringCStyle(String value, String indentationString)
    {
      StringBuilder b = new StringBuilder(value.Length + 5);

      b.Append("\"");

      int i = 0;
      while (i < value.Length)
      {
        switch (value[i])
        {
          case '\r':
            b.Append("\\r");
            break;
          case '\t':
            b.Append("\\t");
            break;
          case '\"':
            b.Append("\\\"");
            break;
          case '\'':
            b.Append("\\\'");
            break;
          case '\\':
            b.Append("\\\\");
            break;
          case '\0':
            b.Append("\\0");
            break;
          case '\n':
            b.Append("\\n");
            break;
          case '\u2028':
          case '\u2029':
            b.Append("\\u");
            b.Append(((int)value[i]).ToString("X4", CultureInfo.InvariantCulture));
            break;

          default:
            b.Append(value[i]);
            break;
        }

        if (i > 0 && i % MaxLineLength == 0)
        {
          //
          // If current character is a high surrogate and the following 
          // character is a low surrogate, don't break them. 
          // Otherwise when we write the string to a file, we might lose 
          // the characters.
          // 
          if (Char.IsHighSurrogate(value[i])
              && (i < value.Length - 1)
              && Char.IsLowSurrogate(value[i + 1]))
          {
            b.Append(value[++i]);
          }

          b.Append("\" +");
          b.Append(Environment.NewLine);
          b.Append(indentationString);
          b.Append('\"');
        }
        ++i;
      }

      b.Append("\"");

      return b.ToString();
    }

    string QuoteSnippetStringVerbatimStyle(string value)
    {
      StringBuilder b = new StringBuilder(value.Length + 5);

      b.Append("@\"");

      for (int i = 0; i < value.Length; i++)
      {
        if (value[i] == '\"')
          b.Append("\"\"");
        else
          b.Append(value[i]);
      }

      b.Append("\"");

      return b.ToString();
    }

    string QuoteSnippetString(String value, String indentationString)
    {
      // If the string is short, use C style quoting (e.g "\r\n")
      // Also do it if it is too long to fit in one line
      // If the string contains '\0', verbatim style won't work.
      if (value.Length < 256 || value.Length > 1500 || (value.IndexOf('\0') != -1))
        return QuoteSnippetStringCStyle(value, indentationString);

      // Otherwise, use 'verbatim' style quoting (e.g. @"foo")
      return QuoteSnippetStringVerbatimStyle(value);
    }
  }
}