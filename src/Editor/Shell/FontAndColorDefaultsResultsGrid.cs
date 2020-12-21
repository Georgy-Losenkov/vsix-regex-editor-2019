using System;
using Microsoft.VisualStudio.Shell.Interop;

namespace Losenkov.RegexEditor.Shell
{
    internal class FontAndColorDefaultsResultsGrid : FontAndColorDefaultsBase
    {
        public const String CategoryGuidString = "a902e780-3377-4d30-a8d3-a4d715e89c8b";
        public const String CategoryNameString = "Regex Editor Results Grid";

        #region color entries
        internal static class EntryNames
        {
            public const String NormalCell           = "NormalCell";
            public const String SelectedCell         = "SelectedCell";
            public const String InactiveSelectedCell = "InactiveSelectedCell";
            public const String HeaderCell           = "HeaderCell";
            public const String FailureMarker        = "FailureMarker";
            public const String GridLines            = "GridLines";
        }

#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
        sealed class NormalCellEntry : ColorEntry
        {
            public NormalCellEntry(FontAndColorDefaultsResultsGrid parent) : base(parent)
            {
                Name = EntryNames.NormalCell;
                LocalizedName = "Normal cell";
                Usage = ColorUsage.Background | ColorUsage.Foreground;
                DefaultBackground = new[] { new RgbColor(0xFF, 0xFF, 0xFF) };
                DefaultForeground = new[] { new RgbColor(0x00, 0x00, 0x00) };
            }
        }
        sealed class SelectedCellEntry : ColorEntry
        {
            public SelectedCellEntry(FontAndColorDefaultsResultsGrid parent) : base(parent)
            {
                Name = EntryNames.SelectedCell;
                LocalizedName = "Selected cell";
                Usage = ColorUsage.Background | ColorUsage.Foreground;
                DefaultBackground = new[] { new RgbColor(0x00, 0x00, 0xA0) };
                DefaultForeground = new[] { new RgbColor(0xFF, 0xFF, 0xC0) };
            }
        }
        sealed class InactiveSelectedCellEntry : ColorEntry
        {
            public InactiveSelectedCellEntry(FontAndColorDefaultsResultsGrid parent) : base(parent)
            {
                Name = EntryNames.InactiveSelectedCell;
                LocalizedName = "Inactive selected cell";
                Usage = ColorUsage.Background | ColorUsage.Foreground;
                DefaultBackground = new[] { new RgbColor(0xC0, 0xC0, 0xC0) };
                DefaultForeground = new[] { new RgbColor(0x00, 0x00, 0x00) };
            }
        }
        sealed class HeaderCellEntry : ColorEntry
        {
            public HeaderCellEntry(FontAndColorDefaultsResultsGrid parent) : base(parent)
            {
                Name = EntryNames.HeaderCell;
                LocalizedName = "Header cell";
                Usage = ColorUsage.Background | ColorUsage.Foreground;
                DefaultBackground = new[] { new RgbColor(0xE0, 0xE0, 0xE0) };
                DefaultForeground = new[] { new RgbColor(0x00, 0x00, 0x00) };
            }
        }
        sealed class FailureMarkerEntry : ColorEntry
        {
            public FailureMarkerEntry(FontAndColorDefaultsResultsGrid parent) : base(parent)
            {
                Name = EntryNames.FailureMarker;
                LocalizedName = "Failure marker";
                Usage = ColorUsage.Foreground;
                DefaultBackground = new[] { new AutoColor() };
                DefaultForeground = new[] { new RgbColor(0xFF, 0x40, 0x40) };
            }
        }
        sealed class GridLinesEntry : ColorEntry
        {
            public GridLinesEntry(FontAndColorDefaultsResultsGrid parent) : base(parent)
            {
                Name = EntryNames.GridLines;
                LocalizedName = "Grid lines";
                Usage = ColorUsage.Foreground;
                DefaultBackground = new[] { new AutoColor() };
                DefaultForeground = new[] { new RgbColor(0x80, 0x80, 0x80) };
            }
        }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        #endregion

#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
        public FontAndColorDefaultsResultsGrid()
        {
            Instance = this;
            CategoryGuid = new Guid(CategoryGuidString);
            CategoryName = "Regex Editor Results Grid";
            Font = CreateFontInfo("Consolas", 9, 1);
            ColorEntries = new ColorEntry[] {
                new NormalCellEntry(this),
                new SelectedCellEntry(this),
                new InactiveSelectedCellEntry(this),
                new HeaderCellEntry(this),
                new FailureMarkerEntry(this),
                new GridLinesEntry(this),
            };
        }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread

        public static FontAndColorDefaultsResultsGrid Instance { get; private set; }
    }
}