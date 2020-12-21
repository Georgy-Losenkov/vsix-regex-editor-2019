using System;
using Losenkov.RegexEditor.UI.Model;

namespace Losenkov.RegexEditor.UI.View
{
    public class TesterModeConverter : System.Windows.Data.IValueConverter
    {
        const String Text_Invoke = "Test";
        const String Text_CsCode = "C# Code";
        const String Text_VbCode = "VB Code";

        public static String ToString(TesterMode value)
        {
            switch (value)
            {
                case TesterMode.Invoke:
                    return Text_Invoke;
                case TesterMode.CsCode:
                    return Text_CsCode;
                case TesterMode.VbCode:
                    return Text_VbCode;
            }

            return String.Empty;
        }

        public static Boolean TryParse(String text, out TesterMode value)
        {
            if (String.Equals(text, Text_Invoke, StringComparison.InvariantCultureIgnoreCase))
            {
                value = TesterMode.Invoke;
                return true;
            }
            if (String.Equals(text, Text_CsCode, StringComparison.InvariantCultureIgnoreCase))
            {
                value = TesterMode.CsCode;
                return true;
            }
            if (String.Equals(text, Text_VbCode, StringComparison.InvariantCultureIgnoreCase))
            {
                value = TesterMode.VbCode;
                return true;
            }

            value = default;
            return false;
        }

        public Object Convert(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TesterMode mode)
            {
                if (targetType == typeof(String))
                {
                    return ToString(mode);
                }
            }
            else if (value is String s)
            {
                if (targetType == typeof(TesterMode))
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