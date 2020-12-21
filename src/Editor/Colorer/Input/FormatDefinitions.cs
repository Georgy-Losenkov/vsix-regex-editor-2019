using System;
using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Losenkov.RegexEditor.Colorer.Input
{
    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [ClassificationType(ClassificationTypeNames = Name)]
    [Name(Name)]
    [Order(After = Priority.Default, Before = Priority.High)]
    internal class Group01 : ClassificationFormatDefinition
    {
        internal const String Name = "regex-editor.input.group01";

        public Group01() : base()
        {
            base.DisplayName = "Regex Editor - group #01 in input";
            base.BackgroundCustomizable = true;
            base.ForegroundCustomizable = true;
            base.ForegroundColor = Color.FromRgb(0x00, 0x00, 0xFF);
            base.TextDecorations = System.Windows.TextDecorations.Underline;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [ClassificationType(ClassificationTypeNames = Name)]
    [Name(Name)]
    [Order(After = Priority.Default, Before = Priority.High)]
    internal class Group02 : ClassificationFormatDefinition
    {
        internal const String Name = "regex-editor.input.group02";

        public Group02() : base()
        {
            base.DisplayName = "Regex Editor - group #02 in input";
            base.BackgroundCustomizable = true;
            base.ForegroundCustomizable = true;
            base.ForegroundColor = Color.FromRgb(0x00, 0x80, 0x00);
            base.TextDecorations = System.Windows.TextDecorations.Underline;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [ClassificationType(ClassificationTypeNames = Name)]
    [Name(Name)]
    [Order(After = Priority.Default, Before = Priority.High)]
    internal class Group03 : ClassificationFormatDefinition
    {
        internal const String Name = "regex-editor.input.group03";

        public Group03()
        {
            base.DisplayName = "Regex Editor - group #03 in input";
            base.BackgroundCustomizable = true;
            base.ForegroundCustomizable = true;
            base.ForegroundColor = Color.FromRgb(0x99, 0x00, 0xCC);
            base.TextDecorations = System.Windows.TextDecorations.Underline;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [ClassificationType(ClassificationTypeNames = Name)]
    [Name(Name)]
    [Order(After = Priority.Default, Before = Priority.High)]
    internal class Group04 : ClassificationFormatDefinition
    {
        internal const String Name = "regex-editor.input.group04";

        public Group04()
        {
            base.DisplayName = "Regex Editor - group #04 in input";
            base.BackgroundCustomizable = true;
            base.ForegroundCustomizable = true;
            base.ForegroundColor = Color.FromRgb(0x80, 0x00, 0x00);
            base.TextDecorations = System.Windows.TextDecorations.Underline;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [ClassificationType(ClassificationTypeNames = Name)]
    [Name(Name)]
    [Order(After = Priority.Default, Before = Priority.High)]
    internal class Group05 : ClassificationFormatDefinition
    {
        internal const String Name = "regex-editor.input.group05";

        public Group05()
        {
            base.DisplayName = "Regex Editor - group #05 in input";
            base.BackgroundCustomizable = true;
            base.ForegroundCustomizable = true;
            base.ForegroundColor = Color.FromRgb(0x00, 0xCC, 0x33);
            base.TextDecorations = System.Windows.TextDecorations.Underline;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [ClassificationType(ClassificationTypeNames = Name)]
    [Name(Name)]
    [Order(After = Priority.Default, Before = Priority.High)]
    internal class Group06 : ClassificationFormatDefinition
    {
        internal const String Name = "regex-editor.input.group06";

        public Group06()
        {
            base.DisplayName = "Regex Editor - group #06 in input";
            base.BackgroundCustomizable = true;
            base.ForegroundCustomizable = true;
            base.ForegroundColor = Color.FromRgb(0xFF, 0x66, 0x00);
            base.TextDecorations = System.Windows.TextDecorations.Underline;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [ClassificationType(ClassificationTypeNames = Name)]
    [Name(Name)]
    [Order(After = Priority.Default, Before = Priority.High)]
    internal class Group07 : ClassificationFormatDefinition
    {
        internal const String Name = "regex-editor.input.group07";

        public Group07()
        {
            base.DisplayName = "Regex Editor - group #07 in input";
            base.BackgroundCustomizable = true;
            base.ForegroundCustomizable = true;
            base.ForegroundColor = Color.FromRgb(0xCC, 0x00, 0x99);
            base.TextDecorations = System.Windows.TextDecorations.Underline;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [ClassificationType(ClassificationTypeNames = Name)]
    [Name(Name)]
    [Order(After = Priority.Default, Before = Priority.High)]
    internal class EmptyMarkerClassificationFormatDefinition : ClassificationFormatDefinition
    {
        internal const String Name = "regex-editor.input.empty-marker";

        public EmptyMarkerClassificationFormatDefinition()
        {
            base.DisplayName = "Regex Editor - empty marker in input";
            base.BackgroundCustomizable = true;
            base.ForegroundCustomizable = true;
            base.BackgroundColor = Color.FromArgb(0xFF, 0x00, 0xBB, 0x44);
            base.ForegroundColor = Colors.Yellow;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [Name(Name)]
    internal sealed class HighlightMarkerFormatDefinition : MarkerFormatDefinition
    {
        internal const String Name = "regex-editor.input.hightlight";

        public HighlightMarkerFormatDefinition()
        {
            base.DisplayName = "Regex Editor - range highlight in input";
            base.BackgroundCustomizable = true;
            base.ForegroundCustomizable = false;
            base.BackgroundColor = Color.FromArgb(0xFF, 0x00, 0xBB, 0x55);
            base.ZOrder = 2;
        }
    }
}