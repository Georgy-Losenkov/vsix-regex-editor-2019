using System;
using Microsoft.VisualStudio.Shell.Interop;

namespace Losenkov.RegexEditor.Shell
{
    internal class FontAndColorDefaultsResultsText : FontAndColorDefaultsBase
    {
        public const String CategoryGuidString = "16c49e2e-691a-4dbb-9335-5fd06e46aa7e";
        public const String CategoryNameString = "Regex Editor Results Text";

        #region color entries
        internal static class EntryNames
        {
            public const String PlainText = "Plain Text";
            public const String SelectedText = "Selected Text";
        }

#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
        sealed class PlainTextEntry : ColorEntry
        {
            public PlainTextEntry(FontAndColorDefaultsResultsText parent) : base(parent)
            {
                Name = EntryNames.PlainText;
                LocalizedName = "Plain Text";
                Usage = ColorUsage.Background | ColorUsage.Foreground;
                DefaultBackground = new[] { new RgbColor(0xFF, 0xFF, 0xFF) };
                DefaultForeground = new[] { new RgbColor(0x00, 0x00, 0x00) };
            }
        }
        sealed class SelectedTextEntry : ColorEntry
        {
            public SelectedTextEntry(FontAndColorDefaultsResultsText parent) : base(parent)
            {
                Name = EntryNames.SelectedText;
                LocalizedName = "Selected Text";
                Usage = ColorUsage.Background;
                DefaultBackground = new[] { new RgbColor(0x00, 0x00, 0x80) };
                DefaultForeground = new[] { new RgbColor(0x00, 0x00, 0x00) };
            }
        }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        #endregion

#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
        public FontAndColorDefaultsResultsText()
        {
            Instance = this;
            CategoryGuid = new Guid(CategoryGuidString);
            CategoryName = "Regex Editor Results Text";
            Font = CreateFontInfo("Consolas", 9, 1);
            ColorEntries = new ColorEntry[] {
                new PlainTextEntry(this),
                new SelectedTextEntry(this),
            };
        }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread

        public static FontAndColorDefaultsResultsText Instance { get; private set; }
    }
}