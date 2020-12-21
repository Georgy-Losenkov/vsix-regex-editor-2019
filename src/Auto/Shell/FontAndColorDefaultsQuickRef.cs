using System;
using Microsoft.VisualStudio.Shell.Interop;

namespace Losenkov.RegexEditor.Shell
{
    internal class FontAndColorDefaultsQuickRef : FontAndColorDefaultsBase
    {
        public const String CategoryGuidString = "fab4f962-52fd-4907-ae92-ea70261b06b6";
        public const String CategoryNameString = "Regex Quick Reference Window";

        #region color entries
        internal static class EntryNames
        {
            public const String Document    = "Document";
            public const String Hyperlink   = "Hyperlink";
            public const String HeadRow     = "HeadRow";
            public const String EvenRow     = "EvenRow";
            public const String EvenRowArg  = "EvenRowArg";
            public const String OddRow      = "OddRow";
            public const String OddRowArg   = "OddRowArg";
        }
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
        sealed class DocumentEntry : ColorEntry
        {
            public DocumentEntry(FontAndColorDefaultsQuickRef parent) : base(parent)
            {
                Name = EntryNames.Document;
                LocalizedName = "Entire document";
                Usage = ColorUsage.Background;
                DefaultBackground = new[] { new RgbColor(0xFF, 0xFF, 0xFF) };
                DefaultForeground = new[] { new RgbColor(0x00, 0x00, 0x00) };
            }
        }
        sealed class HyperlinkEntry : ColorEntry
        {
            public HyperlinkEntry(FontAndColorDefaultsQuickRef parent) : base(parent)
            {
                Name = EntryNames.Hyperlink;
                LocalizedName = "Hyperlink";
                Usage = ColorUsage.Foreground;
                DefaultBackground = new[] { new RgbColor(0xFF, 0xFF, 0xFF) };
                DefaultForeground = new[] { new RgbColor(0x40, 0x40, 0xFF) };
            }
        }
        sealed class HeadRowEntry : ColorEntry
        {
            public HeadRowEntry(FontAndColorDefaultsQuickRef parent) : base(parent)
            {
                Name = EntryNames.HeadRow;
                LocalizedName = "Head row";
                Usage = ColorUsage.Background | ColorUsage.Foreground;
                DefaultBackground = new[] { new RgbColor(0x80, 0xA0, 0x80) };
                DefaultForeground = new[] { new RgbColor(0xFF, 0xFF, 0xFF) };
            }
        }
        sealed class EvenRowEntry : ColorEntry
        {
            public EvenRowEntry(FontAndColorDefaultsQuickRef parent) : base(parent)
            {
                Name = EntryNames.EvenRow;
                LocalizedName = "Even row";
                Usage = ColorUsage.Background | ColorUsage.Foreground;
                DefaultBackground = new[] { new RgbColor(0xFF, 0xFF, 0xFF) };
                DefaultForeground = new[] { new RgbColor(0x00, 0x00, 0x00) };
            }
        }
        sealed class EvenRowArgEntry : ColorEntry
        {
            public EvenRowArgEntry(FontAndColorDefaultsQuickRef parent) : base(parent)
            {
                Name = EntryNames.EvenRowArg;
                LocalizedName = "Argument in even row";
                Usage = ColorUsage.Background | ColorUsage.Foreground;
                DefaultBackground = new[] { new RgbColor(0xC0, 0xC0, 0xC0) };
                DefaultForeground = new[] { new RgbColor(0x00, 0x00, 0x00) };
            }
        }
        sealed class OddRowEntry : ColorEntry
        {
            public OddRowEntry(FontAndColorDefaultsQuickRef parent) : base(parent)
            {
                Name = EntryNames.OddRow;
                LocalizedName = "Odd row";
                Usage = ColorUsage.Background | ColorUsage.Foreground;
                DefaultBackground = new[] { new RgbColor(0xE0, 0xF0, 0xE0) };
                DefaultForeground = new[] { new RgbColor(0x00, 0x00, 0x00) };
            }
        }
        sealed class OddRowArgEntry : ColorEntry
        {
            public OddRowArgEntry(FontAndColorDefaultsQuickRef parent) : base(parent)
            {
                Name = EntryNames.OddRowArg;
                LocalizedName = "Argument in odd row";
                Usage = ColorUsage.Background | ColorUsage.Foreground;
                DefaultBackground = new[] { new RgbColor(0xC0, 0xC0, 0xC0) };
                DefaultForeground = new[] { new RgbColor(0x00, 0x00, 0x00) };
            }
        }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        #endregion

#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
        public FontAndColorDefaultsQuickRef()
        {
            Instance = this;
            CategoryGuid = new Guid(CategoryGuidString);
            CategoryName = "Regex Quick Reference Window";
            Font = CreateFontInfo("Consolas", 9, 1);
            ColorEntries = new ColorEntry[] {
                new DocumentEntry(this),
                new HyperlinkEntry(this),
                new HeadRowEntry(this),
                new EvenRowEntry(this),
                new EvenRowArgEntry(this),
                new OddRowEntry(this),
                new OddRowArgEntry(this),
            };
        }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread

        public static FontAndColorDefaultsQuickRef Instance { get; private set; }
    }
}