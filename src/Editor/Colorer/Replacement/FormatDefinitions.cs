using System;
using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Losenkov.RegexEditor.Colorer.Replacement
{
    [Export(typeof(EditorFormatDefinition))]
    [UserVisible(true)]
    [ClassificationType(ClassificationTypeNames = Name)]
    [Order(After = Priority.Default, Before = Priority.High)]
    [Name(Name)]
    [ContentType(UI.RegexReplacementContentType.ContentTypeName)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class ReplacementSubstitutionClassificationFormatDefinition : ClassificationFormatDefinition
    {
        internal const String Name = "regex-editor.replacement.substitution";

        public ReplacementSubstitutionClassificationFormatDefinition()
        {
            DisplayName = "Regex Editor - substitution in replacement";
            ForegroundCustomizable = true;
            BackgroundCustomizable = true;

            ForegroundColor = Colors.Blue;
        }
    }
}