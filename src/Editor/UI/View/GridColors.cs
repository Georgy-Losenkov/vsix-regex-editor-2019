using System;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EntryNames = Losenkov.RegexEditor.Shell.FontAndColorDefaultsResultsGrid.EntryNames;

namespace Losenkov.RegexEditor.UI.View
{
    static class GridColors
    {
        static GridColors()
        {
            var category = new Guid(Shell.FontAndColorDefaultsResultsGrid.CategoryGuidString);

            FontFamilyKey = new FontResourceKey(category, FontResourceKeyType.FontFamily);
            FontSizeKey = new FontResourceKey(category, FontResourceKeyType.FontSize);
            NormalCellBackgroundBrushKey = new ThemeResourceKey(category, EntryNames.NormalCell, ThemeResourceKeyType.BackgroundBrush);
            NormalCellForegroundBrushKey = new ThemeResourceKey(category, EntryNames.NormalCell, ThemeResourceKeyType.ForegroundBrush);
            SelectedCellBackgroundBrushKey = new ThemeResourceKey(category, EntryNames.SelectedCell, ThemeResourceKeyType.BackgroundBrush);
            SelectedCellForegroundBrushKey = new ThemeResourceKey(category, EntryNames.SelectedCell, ThemeResourceKeyType.ForegroundBrush);
            InactiveSelectedCellBackgroundBrushKey = new ThemeResourceKey(category, EntryNames.InactiveSelectedCell, ThemeResourceKeyType.BackgroundBrush);
            InactiveSelectedCellForegroundBrushKey = new ThemeResourceKey(category, EntryNames.InactiveSelectedCell, ThemeResourceKeyType.ForegroundBrush);
            HeaderCellBackgroundBrushKey = new ThemeResourceKey(category, EntryNames.HeaderCell, ThemeResourceKeyType.BackgroundBrush);
            HeaderCellForegroundBrushKey = new ThemeResourceKey(category, EntryNames.HeaderCell, ThemeResourceKeyType.ForegroundBrush);
            FailureMarkerForegroundBrushKey = new ThemeResourceKey(category, EntryNames.FailureMarker, ThemeResourceKeyType.ForegroundBrush);
            GridLinesBrushKey = new ThemeResourceKey(category, EntryNames.GridLines, ThemeResourceKeyType.ForegroundBrush);
        }

        #region font and color properties
        public static FontResourceKey FontFamilyKey { get; }
        public static FontResourceKey FontSizeKey { get; }
        public static ThemeResourceKey NormalCellBackgroundBrushKey { get; }
        public static ThemeResourceKey NormalCellForegroundBrushKey { get; }
        public static ThemeResourceKey SelectedCellBackgroundBrushKey { get; }
        public static ThemeResourceKey SelectedCellForegroundBrushKey { get; }
        public static ThemeResourceKey InactiveSelectedCellBackgroundBrushKey { get; }
        public static ThemeResourceKey InactiveSelectedCellForegroundBrushKey { get; }
        public static ThemeResourceKey HeaderCellBackgroundBrushKey { get; }
        public static ThemeResourceKey HeaderCellForegroundBrushKey { get; }
        public static ThemeResourceKey FailureMarkerForegroundBrushKey { get; }
        public static ThemeResourceKey GridLinesBrushKey { get; }
        #endregion
    }
}