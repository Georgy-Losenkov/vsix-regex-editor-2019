using System;
using Losenkov.RegexEditor.UI.Model;

namespace Losenkov.RegexEditor.UI.View
{
    public class RegexMethodConverter : System.Windows.Data.IValueConverter
    {
        const String Text_Match = "Match";
        const String Text_Replace = "Replace";
        const String Text_Split = "Split";

        public static String ToString(RegexMethod value)
        {
            switch (value)
            {
                case RegexMethod.Match:
                    return Text_Match;
                case RegexMethod.Replace:
                    return Text_Replace;
                case RegexMethod.Split:
                    return Text_Split;
            }

            return String.Empty;
        }

        public static Boolean TryParse(String text, out RegexMethod value)
        {
            if (String.Equals(text, Text_Match, StringComparison.InvariantCultureIgnoreCase))
            {
                value = RegexMethod.Match;
                return true;
            }
            if (String.Equals(text, Text_Replace, StringComparison.InvariantCultureIgnoreCase))
            {
                value = RegexMethod.Replace;
                return true;
            }
            if (String.Equals(text, Text_Split, StringComparison.InvariantCultureIgnoreCase))
            {
                value = RegexMethod.Split;
                return true;
            }

            value = default;
            return false;
        }

        public Object Convert(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is RegexMethod method)
            {
                if (targetType == typeof(String))
                {
                    return ToString(method);
                }
            }
            else if (value is String s)
            {
                if (targetType == typeof(RegexMethod))
                {
                    if (TryParse(s, out var result))
                    {
                        return result;
                    }
                }
            }

            return System.Windows.Data.Binding.DoNothing;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}