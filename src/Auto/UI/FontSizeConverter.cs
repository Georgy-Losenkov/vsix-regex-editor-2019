using System;

namespace Losenkov.RegexEditor.UI
{
    public class FontSizeConverter : System.Windows.Data.IValueConverter
    {
        public static readonly FontSizeConverter Instance = new FontSizeConverter();

        public Object Convert(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Double v)
            {
                if (targetType == typeof(Double))
                {
                    var factor = System.Convert.ToDouble(parameter, culture);

                    return Math.Round(v * factor, 0);
                }
            }

            return System.Windows.Data.Binding.DoNothing;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}