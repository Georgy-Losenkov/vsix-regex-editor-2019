using System;
using System.Text.RegularExpressions;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    static class Extensions
    {
        public static Regex CreateRegex(this IOptions options, Int32 milliSeconds)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return new Regex(options.PatternText, options.Options & ~RegexOptions.Compiled, TimeSpan.FromMilliseconds(milliSeconds));
        }
    }
}