using System;
using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Losenkov.RegexEditor.Colorer.Pattern
{
    #region processed
    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [ClassificationType(ClassificationTypeNames = Name)]
    [Order]
    [Name(Name)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class PatternAlternationClassificationFormatDefinition : ClassificationFormatDefinition
    {
        internal const String Name = "regex-editor.pattern.alternation";

        public PatternAlternationClassificationFormatDefinition()
        {
            DisplayName = "Regex Editor - alternation in pattern";

            ForegroundColor = Color.FromRgb(0x00, 0xAA, 0xAA);
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [ClassificationType(ClassificationTypeNames = Name)]
    [Order]
    [Name(Name)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class PatternAnchorClassificationFormatDefinition : ClassificationFormatDefinition
    {
        internal const String Name = "regex-editor.pattern.anchor";

        public PatternAnchorClassificationFormatDefinition()
        {
            DisplayName = "Regex Editor - anchor in pattern";

            ForegroundColor = Color.FromRgb(0xFF, 0x00, 0x55);
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [ClassificationType(ClassificationTypeNames = Name)]
    [Order]
    [Name(Name)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class PatternCommentClassificationFormatDefinition : ClassificationFormatDefinition
    {
        internal const String Name = "regex-editor.pattern.comment";

        public PatternCommentClassificationFormatDefinition()
        {
            DisplayName = "Regex Editor - comment";

            ForegroundColor = Colors.DarkGreen;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [ClassificationType(ClassificationTypeNames = Name)]
    [Order]
    [Name(Name)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class PatternGroupingClassificationFormatDefinition : ClassificationFormatDefinition
    {
        internal const String Name = "regex-editor.pattern.grouping";

        public PatternGroupingClassificationFormatDefinition()
        {
            DisplayName = "Regex Editor - grouping in pattern";

            ForegroundColor = Color.FromRgb(0x00, 0xAA, 0xAA);
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [ClassificationType(ClassificationTypeNames = Name)]
    [Order]
    [Name(Name)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class PatternQuantifierClassificationFormatDefinition : ClassificationFormatDefinition
    {
        internal const String Name = "regex-editor.pattern.quantifier";

        public PatternQuantifierClassificationFormatDefinition()
        {
            DisplayName = "Regex Editor - quantifier in pattern";

            ForegroundColor = Color.FromRgb(0xFF, 0x00, 0x55);
        }
    }
    #endregion

    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [ClassificationType(ClassificationTypeNames = Name)]
    [Order]
    [Name(Name)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class PatternCharGroupClassificationFormatDefinition : ClassificationFormatDefinition
    {
        internal const String Name = "regex-editor.pattern.char-group";

        public PatternCharGroupClassificationFormatDefinition()
        {
            DisplayName = "Regex Editor - character group in pattern";

            ForegroundColor = Color.FromRgb(0x00, 0x55, 0xFF);
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [ClassificationType(ClassificationTypeNames = Name)]
    [Order]
    [Name(Name)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class PatternEscapedExpressionClassificationFormatDefinition : ClassificationFormatDefinition
    {
        internal const String Name = "regex-editor.pattern.escaped-expression";

        public PatternEscapedExpressionClassificationFormatDefinition()
        {
            DisplayName = "Regex Editor - escaped expression in pattern";

            ForegroundColor = Color.FromRgb(0x7E, 0x5B, 0x71);
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [ClassificationType(ClassificationTypeNames = Name)]
    [Order]
    [Name(Name)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class PatternExpressionClassificationFormatDefinition : ClassificationFormatDefinition
    {
        internal const String Name = "regex-editor.pattern.expression";

        public PatternExpressionClassificationFormatDefinition()
        {
            DisplayName = "Regex Editor - expression in pattern";

            ForegroundColor = Color.FromRgb(0x00, 0x55, 0xFF);
        }
    }
}