namespace Losenkov.RegexEditor.UI.View
{
  using EntryNames = Shell.FontAndColorDefaultsQuickRef.EntryNames;
  using Microsoft.VisualStudio.Shell;
  using Microsoft.VisualStudio.Shell.Interop;
  using System;

  static class QuickRefColors
  {
    static QuickRefColors()
    {
      var category = new Guid(Shell.FontAndColorDefaultsQuickRef.CategoryGuidString);

      FontFamilyKey = new FontResourceKey(category, FontResourceKeyType.FontFamily);
      FontSizeKey = new FontResourceKey(category, FontResourceKeyType.FontSize);
      DocumentBackgroundBrushKey = new ThemeResourceKey(category, EntryNames.Document, ThemeResourceKeyType.BackgroundBrush);
      DocumentForegroundBrushKey = new ThemeResourceKey(category, EntryNames.Document, ThemeResourceKeyType.ForegroundBrush);
      HyperlinkForegroundBrushKey = new ThemeResourceKey(category, EntryNames.Hyperlink, ThemeResourceKeyType.ForegroundBrush);
      HeadRowBackgroundBrushKey = new ThemeResourceKey(category, EntryNames.HeadRow, ThemeResourceKeyType.BackgroundBrush);
      HeadRowForegroundBrushKey = new ThemeResourceKey(category, EntryNames.HeadRow, ThemeResourceKeyType.ForegroundBrush);
      EvenRowBackgroundBrushKey = new ThemeResourceKey(category, EntryNames.EvenRow, ThemeResourceKeyType.BackgroundBrush);
      EvenRowForegroundBrushKey = new ThemeResourceKey(category, EntryNames.EvenRow, ThemeResourceKeyType.ForegroundBrush);
      EvenRowArgBackgroundBrushKey = new ThemeResourceKey(category, EntryNames.EvenRowArg, ThemeResourceKeyType.BackgroundBrush);
      EvenRowArgForegroundBrushKey = new ThemeResourceKey(category, EntryNames.EvenRowArg, ThemeResourceKeyType.ForegroundBrush);
      OddRowBackgroundBrushKey = new ThemeResourceKey(category, EntryNames.OddRow, ThemeResourceKeyType.BackgroundBrush);
      OddRowForegroundBrushKey = new ThemeResourceKey(category, EntryNames.OddRow, ThemeResourceKeyType.ForegroundBrush);
      OddRowArgBackgroundBrushKey = new ThemeResourceKey(category, EntryNames.OddRowArg, ThemeResourceKeyType.BackgroundBrush);
      OddRowArgForegroundBrushKey = new ThemeResourceKey(category, EntryNames.OddRowArg, ThemeResourceKeyType.ForegroundBrush);
    }

    public static FontResourceKey FontFamilyKey { get; }
    public static FontResourceKey FontSizeKey { get; }
    public static ThemeResourceKey DocumentBackgroundBrushKey { get; }
    public static ThemeResourceKey DocumentForegroundBrushKey { get; }
    public static ThemeResourceKey HyperlinkForegroundBrushKey { get; }
    public static ThemeResourceKey HeadRowBackgroundBrushKey { get; }
    public static ThemeResourceKey HeadRowForegroundBrushKey { get; }
    public static ThemeResourceKey EvenRowBackgroundBrushKey { get; }
    public static ThemeResourceKey EvenRowForegroundBrushKey { get; }
    public static ThemeResourceKey EvenRowArgBackgroundBrushKey { get; }
    public static ThemeResourceKey EvenRowArgForegroundBrushKey { get; }
    public static ThemeResourceKey OddRowBackgroundBrushKey { get; }
    public static ThemeResourceKey OddRowForegroundBrushKey { get; }
    public static ThemeResourceKey OddRowArgBackgroundBrushKey { get; }
    public static ThemeResourceKey OddRowArgForegroundBrushKey { get; }
  }
}