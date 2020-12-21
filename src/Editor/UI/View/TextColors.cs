using System;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EntryNames = Losenkov.RegexEditor.Shell.FontAndColorDefaultsResultsText.EntryNames;

namespace Losenkov.RegexEditor.UI.View
{
    static class TextColors
    {
        static TextColors()
        {
            var category = new Guid(Shell.FontAndColorDefaultsResultsText.CategoryGuidString);

            FontFamilyKey = new FontResourceKey(category, FontResourceKeyType.FontFamily);
            FontSizeKey = new FontResourceKey(category, FontResourceKeyType.FontSize);
            PlainTextBackgroundBrushKey = new ThemeResourceKey(category, EntryNames.PlainText, ThemeResourceKeyType.BackgroundBrush);
            PlainTextForegroundBrushKey = new ThemeResourceKey(category, EntryNames.PlainText, ThemeResourceKeyType.ForegroundBrush);
            SelectedTextBackgroundBrushKey = new ThemeResourceKey(category, EntryNames.SelectedText, ThemeResourceKeyType.BackgroundBrush);
        }

        public static FontResourceKey FontFamilyKey { get; }
        public static FontResourceKey FontSizeKey { get; }
        public static ThemeResourceKey PlainTextBackgroundBrushKey { get; }
        public static ThemeResourceKey PlainTextForegroundBrushKey { get; }
        public static ThemeResourceKey SelectedTextBackgroundBrushKey { get; }
    }
}