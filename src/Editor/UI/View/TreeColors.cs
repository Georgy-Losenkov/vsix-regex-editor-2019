using System;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EntryNames = Losenkov.RegexEditor.Shell.FontAndColorDefaultsResultsTree.EntryNames;

namespace Losenkov.RegexEditor.UI.View
{
    static class TreeColors
    {
        static TreeColors()
        {
            var category = new Guid(Shell.FontAndColorDefaultsResultsTree.CategoryGuidString);

            FontFamilyKey = new FontResourceKey(category, FontResourceKeyType.FontFamily);
            FontSizeKey = new FontResourceKey(category, FontResourceKeyType.FontSize);
            DocumentBackgroundBrushKey = new ThemeResourceKey(category, EntryNames.Document, ThemeResourceKeyType.BackgroundBrush);
            DocumentForegroundBrushKey = new ThemeResourceKey(category, EntryNames.Document, ThemeResourceKeyType.ForegroundBrush);
            ActiveSelectionBackgroundBrushKey = new ThemeResourceKey(category, EntryNames.ActiveSelection, ThemeResourceKeyType.BackgroundBrush);
            ActiveSelectionForegroundBrushKey = new ThemeResourceKey(category, EntryNames.ActiveSelection, ThemeResourceKeyType.ForegroundBrush);
            InactiveSelectionBackgroundBrushKey = new ThemeResourceKey(category, EntryNames.InactiveSelection, ThemeResourceKeyType.BackgroundBrush);
            InactiveSelectionForegroundBrushKey = new ThemeResourceKey(category, EntryNames.InactiveSelection, ThemeResourceKeyType.ForegroundBrush);

            LineNodeHeaderForegroundBrushKey = new ThemeResourceKey(category, EntryNames.LineNodeHeader, ThemeResourceKeyType.ForegroundBrush);
            MatchNodeHeaderForegroundBrushKey = new ThemeResourceKey(category, EntryNames.MatchNodeHeader, ThemeResourceKeyType.ForegroundBrush);
            GroupNodeHeaderForegroundBrushKey = new ThemeResourceKey(category, EntryNames.GroupNodeHeader, ThemeResourceKeyType.ForegroundBrush);
            CaptureNodeHeaderForegroundBrushKey = new ThemeResourceKey(category, EntryNames.CaptureNodeHeader, ThemeResourceKeyType.ForegroundBrush);
            EmptyMarkerForegroundBrushKey = new ThemeResourceKey(category, EntryNames.EmptyMarker, ThemeResourceKeyType.ForegroundBrush);
            FailureMarkerForegroundBrushKey = new ThemeResourceKey(category, EntryNames.FailureMarker, ThemeResourceKeyType.ForegroundBrush);
        }

        #region font and color properties
        public static FontResourceKey FontFamilyKey { get; }
        public static FontResourceKey FontSizeKey { get; }

        public static ThemeResourceKey DocumentBackgroundBrushKey { get; }
        public static ThemeResourceKey DocumentForegroundBrushKey { get; }
        public static ThemeResourceKey ActiveSelectionBackgroundBrushKey { get; }
        public static ThemeResourceKey ActiveSelectionForegroundBrushKey { get; }
        public static ThemeResourceKey InactiveSelectionBackgroundBrushKey { get; }
        public static ThemeResourceKey InactiveSelectionForegroundBrushKey { get; }

        public static ThemeResourceKey LineNodeHeaderForegroundBrushKey { get; }
        public static ThemeResourceKey MatchNodeHeaderForegroundBrushKey { get; }
        public static ThemeResourceKey GroupNodeHeaderForegroundBrushKey { get; }
        public static ThemeResourceKey CaptureNodeHeaderForegroundBrushKey { get; }
        public static ThemeResourceKey EmptyMarkerForegroundBrushKey { get; }
        public static ThemeResourceKey FailureMarkerForegroundBrushKey { get; }
        #endregion
    }
}