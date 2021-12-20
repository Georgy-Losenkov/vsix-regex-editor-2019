using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Losenkov.RegexEditor.UI
{
    public class EmWidthEvaluator : DependencyObject
    {
        public static readonly DependencyProperty FontFamilyProperty;
        public static readonly DependencyProperty FontStyleProperty;
        public static readonly DependencyProperty FontWeightProperty;
        public static readonly DependencyProperty FontStretchProperty;
        public static readonly DependencyProperty FontSizeProperty;
        public static readonly DependencyProperty EmWidthProperty;
        static readonly DependencyPropertyKey s_emWidthPropertyKey;

        static EmWidthEvaluator()
        {
            var type = typeof(EmWidthEvaluator);
            FontFamilyProperty = DependencyProperty.Register("FontFamily", typeof(FontFamily), type, new PropertyMetadata(SystemFonts.MessageFontFamily, Invalidate));
            FontStyleProperty = DependencyProperty.Register("FontStyle", typeof(FontStyle), type, new PropertyMetadata(SystemFonts.MessageFontStyle, Invalidate));
            FontWeightProperty = DependencyProperty.Register("FontWeight", typeof(FontWeight), type, new PropertyMetadata(SystemFonts.MessageFontWeight, Invalidate));
            FontStretchProperty = DependencyProperty.Register("FontStretch", typeof(FontStretch), type, new PropertyMetadata(FontStretches.Normal, Invalidate));
            FontSizeProperty = DependencyProperty.Register("FontSize", typeof(Double), type, new PropertyMetadata(SystemFonts.MessageFontSize, Invalidate));
            s_emWidthPropertyKey = DependencyProperty.RegisterReadOnly("EmWidth", typeof(Double), type, new PropertyMetadata(5.0, null, CoerceEmWidth));
            EmWidthProperty = s_emWidthPropertyKey.DependencyProperty;
        }

        static Object CoerceEmWidth(DependencyObject obj, Object nValue)
        {
            var self = (EmWidthEvaluator)obj;

            var formattedText = new FormattedText(
              textToFormat: "M",
              culture: CultureInfo.CurrentCulture,
              flowDirection: FlowDirection.LeftToRight,
              typeface: new Typeface(self.FontFamily, self.FontStyle, self.FontWeight, self.FontStretch),
              emSize: self.FontSize,
              foreground: Brushes.Black,
              pixelsPerDip: 1.0);

            return formattedText.Width;
        }

        static void Invalidate(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            obj.CoerceValue(EmWidthProperty);
        }

        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }

        public Double FontSize
        {
            get { return (Double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public Double EmWidth
        {
            get { return (Double)base.GetValue(EmWidthProperty); }
        }
    }
}