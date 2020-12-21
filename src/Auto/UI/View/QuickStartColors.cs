namespace Losenkov.RegexEditor.UI.View
{
  using EntryNames = Shell.ThemeColorsQuickStart.EntryNames;
  using Microsoft.VisualStudio.Shell;
  using Microsoft.VisualStudio.Shell.Interop;
  using System;

  static class QuickStartColors
  {
    static QuickStartColors()
    {
      var category = new Guid(Shell.ThemeColorsQuickStart.CategoryGuidString);

      TitleForegroundBrushKey = new ThemeResourceKey(category, EntryNames.Title, ThemeResourceKeyType.ForegroundBrush);
      TextBackgroundBrushKey = new ThemeResourceKey(category, EntryNames.Text, ThemeResourceKeyType.BackgroundBrush);
      TextForegroundBrushKey = new ThemeResourceKey(category, EntryNames.Text, ThemeResourceKeyType.ForegroundBrush);
      HyperlinkForegroundBrushKey = new ThemeResourceKey(category, EntryNames.Hyperlink, ThemeResourceKeyType.ForegroundBrush);
    }

    public static ThemeResourceKey TitleForegroundBrushKey { get; }
    public static ThemeResourceKey TextBackgroundBrushKey { get; }
    public static ThemeResourceKey TextForegroundBrushKey { get; }
    public static ThemeResourceKey HyperlinkForegroundBrushKey { get; }
  }
}