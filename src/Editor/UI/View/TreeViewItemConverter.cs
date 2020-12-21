using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Losenkov.RegexEditor.UI.View
{
    class TreeViewItemConverter : System.Windows.Data.IValueConverter
    {
        static Boolean _Equals(Object parameter, String value)
        {
            return String.Equals(parameter as String, value, StringComparison.InvariantCultureIgnoreCase);
        }

        static Int32 GetDepth(TreeViewItem item)
        {
            var result = 0;

            DependencyObject obj = item;
            while (true)
            {
                if (obj == null || obj is TreeView)
                {
                    break;
                }

                if (obj is TreeViewItem)
                {
                    result++;
                }

                obj = VisualTreeHelper.GetParent(obj);
            }

            return result;
        }

        public Object Convert(Object value, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TreeViewItem item)
            {
                if (_Equals(parameter, "margin"))
                {
                    return new Thickness(14 * GetDepth(item) - 14, 0, 0, 0);
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