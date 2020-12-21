using System;
using Losenkov.RegexEditor.UI.View;
using Microsoft.VisualStudio.Shell.Interop;

namespace Losenkov.RegexEditor.Shell
{
    internal class FontAndColorDefaultsResultsTree : FontAndColorDefaultsBase
    {
        public const String CategoryGuidString = "9b06dd5f-dd46-40c2-8dc9-0f534d845e68";
        public const String CategoryNameString = "Regex Editor Results Tree";

        #region color entries
        internal static class EntryNames
        {
            public const String Document = "Document";
            public const String ActiveSelection = "ActiveSelection";
            public const String InactiveSelection = "InactiveSelection";

            public const String LineNodeHeader = "LineNodeHeader";
            public const String MatchNodeHeader = "MatchNodeHeader";
            public const String GroupNodeHeader = "GroupNodeHeader";
            public const String CaptureNodeHeader = "CaptureNodeHeader";
            public const String EmptyMarker = "EmptyMarker";
            public const String FailureMarker = "FailureMarker";
        }

#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
        sealed class DocumentEntry : ColorEntry
        {
            public DocumentEntry(FontAndColorDefaultsResultsTree parent) : base(parent)
            {
                Name = EntryNames.Document;
                LocalizedName = "Document";
                Usage = ColorUsage.Background | ColorUsage.Foreground;
                DefaultBackground = new[] { new RgbColor(0xFF, 0xFF, 0xFF) };
                DefaultForeground = new[] { new RgbColor(0x00, 0x00, 0x00) };
            }
        }
        sealed class ActiveSelectionEntry : ColorEntry
        {
            public ActiveSelectionEntry(FontAndColorDefaultsResultsTree parent) : base(parent)
            {
                Name = EntryNames.ActiveSelection;
                LocalizedName = "Active selection";
                Usage = ColorUsage.Background | ColorUsage.Foreground;
                DefaultBackground = new[] { new RgbColor(0x80, 0x80, 0xFF) };
                DefaultForeground = new[] { new RgbColor(0x00, 0x00, 0x00) };
            }
        }
        sealed class InactiveSelectionEntry : ColorEntry
        {
            public InactiveSelectionEntry(FontAndColorDefaultsResultsTree parent) : base(parent)
            {
                Name = EntryNames.InactiveSelection;
                LocalizedName = "Inactive selection";
                Usage = ColorUsage.Background | ColorUsage.Foreground;
                DefaultBackground = new[] { new RgbColor(0xC0, 0xC0, 0xFF) };
                DefaultForeground = new[] { new RgbColor(0x00, 0x00, 0x00) };
            }
        }
        sealed class LineNodeHeaderEntry : ColorEntry
        {
            public LineNodeHeaderEntry(FontAndColorDefaultsResultsTree parent) : base(parent)
            {
                Name = EntryNames.LineNodeHeader;
                LocalizedName = "Line node";
                Usage = ColorUsage.Foreground;
                DefaultBackground = new VsColor[] { new ThemeColor(TreeColors.DocumentBackgroundBrushKey), new RgbColor(0xFF, 0xFF, 0xFF) };
                DefaultForeground = new VsColor[] { new RgbColor(0xFF, 0x66, 0x00) };
            }
        }
        sealed class MatchNodeHeaderEntry : ColorEntry
        {
            public MatchNodeHeaderEntry(FontAndColorDefaultsResultsTree parent) : base(parent)
            {
                Name = EntryNames.MatchNodeHeader;
                LocalizedName = "Match node";
                Usage = ColorUsage.Foreground;
                DefaultBackground = new VsColor[] { new ThemeColor(TreeColors.DocumentBackgroundBrushKey), new RgbColor(0xFF, 0xFF, 0xFF) };
                DefaultForeground = new VsColor[] { new RgbColor(0x00, 0x00, 0xFF) };
            }
        }
        sealed class GroupNodeHeaderEntry : ColorEntry
        {
            public GroupNodeHeaderEntry(FontAndColorDefaultsResultsTree parent) : base(parent)
            {
                Name = EntryNames.GroupNodeHeader;
                LocalizedName = "Group node";
                Usage = ColorUsage.Foreground;
                DefaultBackground = new VsColor[] { new ThemeColor(TreeColors.DocumentBackgroundBrushKey), new RgbColor(0xFF, 0xFF, 0xFF) };
                DefaultBackground = new VsColor[] { new RgbColor(0x80, 0x00, 0x00) };
            }
        }
        sealed class CaptureNodeHeaderEntry : ColorEntry
        {
            public CaptureNodeHeaderEntry(FontAndColorDefaultsResultsTree parent) : base(parent)
            {
                Name = EntryNames.CaptureNodeHeader;
                LocalizedName = "Capture node";
                Usage = ColorUsage.Foreground;
                DefaultBackground = new VsColor[] { new ThemeColor(TreeColors.DocumentBackgroundBrushKey), new RgbColor(0xFF, 0xFF, 0xFF) };
                DefaultForeground = new VsColor[] { new RgbColor(0x00, 0x80, 0x00) };
            }
        }
        sealed class EmptyMarkerEntry : ColorEntry
        {
            public EmptyMarkerEntry(FontAndColorDefaultsResultsTree parent) : base(parent)
            {
                Name = EntryNames.EmptyMarker;
                LocalizedName = "Empty marker";
                Usage = ColorUsage.Foreground;
                DefaultBackground = new VsColor[] { new ThemeColor(TreeColors.DocumentBackgroundBrushKey), new RgbColor(0xFF, 0xFF, 0xFF) };
                DefaultForeground = new VsColor[] { new RgbColor(0x80, 0x80, 0x80) };
            }
        }
        sealed class FailureMarkerEntry : ColorEntry
        {
            public FailureMarkerEntry(FontAndColorDefaultsResultsTree parent) : base(parent)
            {
                Name = EntryNames.FailureMarker;
                LocalizedName = "Failure marker";
                Usage = ColorUsage.Foreground;
                DefaultBackground = new VsColor[] { new ThemeColor(TreeColors.DocumentBackgroundBrushKey), new RgbColor(0xFF, 0xFF, 0xFF) };
                DefaultForeground = new VsColor[] { new RgbColor(0xFF, 0x40, 0x40) };
            }
        }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        #endregion

#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
        public FontAndColorDefaultsResultsTree()
        {
            Instance = this;
            CategoryGuid = new Guid(CategoryGuidString);
            CategoryName = "Regex Editor Results Tree";
            Font = CreateFontInfo("Consolas", 9, 1);
            ColorEntries = new ColorEntry[] {
                new DocumentEntry(this),
                new ActiveSelectionEntry(this),
                new InactiveSelectionEntry(this),
                new LineNodeHeaderEntry(this),
                new MatchNodeHeaderEntry(this),
                new GroupNodeHeaderEntry(this),
                new CaptureNodeHeaderEntry(this),
                new EmptyMarkerEntry(this),
                new FailureMarkerEntry(this),
            };
        }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread

        public static FontAndColorDefaultsResultsTree Instance { get; private set; }
    }
}