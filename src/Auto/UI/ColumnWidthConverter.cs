using System;
using System.Windows;

namespace Losenkov.RegexEditor.UI
{
    public class ColumnWidthConverter : System.Windows.Data.IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Double v)
            {
                if (targetType == typeof(GridLength))
                {
                    var factor = System.Convert.ToDouble(parameter);

                    return new GridLength(v * factor, GridUnitType.Pixel);
                }
            }

            return System.Windows.Data.Binding.DoNothing;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}