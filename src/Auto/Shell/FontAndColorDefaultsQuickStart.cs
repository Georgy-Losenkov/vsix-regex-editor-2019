using System;
using Microsoft.VisualStudio.Shell.Interop;

namespace Losenkov.RegexEditor.Shell
{
    internal class ThemeColorsQuickStart : ThemeColorsBase
    {
        public const String CategoryGuidString = "6dee35b4-2d10-4284-8587-61eedb86ec1e";
        public const String CategoryNameString = "Regex Quick Start Window";

        #region color entries
        internal static class EntryNames
        {
            public const String Title = "Title";
            public const String Text = "Text";
            public const String Hyperlink = "Hyperlink";
        }

#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
        sealed class TitleEntry : ColorEntry
        {
            public TitleEntry(ThemeColorsQuickStart parent) : base(parent)
            {
                Name = EntryNames.Title;
                Usage = ColorUsage.Foreground;
                FallbackForeground = new[] { new VsSysColor(__VSSYSCOLOREX.VSCOLOR_TOOLWINDOW_BUTTON_DOWN) };
            }
        }
        sealed class TextEntry : ColorEntry
        {
            public TextEntry(ThemeColorsQuickStart parent) : base(parent)
            {
                Name = EntryNames.Text;
                Usage = ColorUsage.Background | ColorUsage.Foreground;
                FallbackBackground = new[] { new VsSysColor(__VSSYSCOLOREX.VSCOLOR_TOOLWINDOW_BACKGROUND) };
                FallbackForeground = new[] { new VsSysColor(__VSSYSCOLOREX.VSCOLOR_TOOLWINDOW_TEXT) };
            }
        }
        sealed class HyperlinkEntry : ColorEntry
        {
            public HyperlinkEntry(ThemeColorsQuickStart parent) : base(parent)
            {
                Name = EntryNames.Hyperlink;
                Usage = ColorUsage.Foreground;
                FallbackForeground = new[] { new VsSysColor(__VSSYSCOLOREX.VSCOLOR_TOOLWINDOW_BUTTON_HOVER_ACTIVE) };
            }
        }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        #endregion

#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
        public ThemeColorsQuickStart()
        {
            Instance = this;
            CategoryGuid = new Guid(CategoryGuidString);
            ColorEntries = new ColorEntry[] {
                new TitleEntry(this),
                new TextEntry(this),
                new HyperlinkEntry(this)
            };
        }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread

        public static ThemeColorsQuickStart Instance { get; private set; }
    }
}