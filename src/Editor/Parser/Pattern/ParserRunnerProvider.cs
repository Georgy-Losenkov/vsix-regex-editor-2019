using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Losenkov.RegexEditor.Parser.Pattern
{
    /// <summary>
    /// Saves an intance of <see cref="ParserRunner"/> in the properties of the text buffer when the text view is created.
    /// </summary>
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType(UI.RegexPatternContentType.ContentTypeName)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class ParserRunnerProvider : IWpfTextViewCreationListener
    {
        public void TextViewCreated(IWpfTextView textView)
        {
            textView.TextBuffer.Properties.GetOrCreateSingletonProperty<ParserRunner>(() => new ParserRunner(textView.TextBuffer));
        }
    }
}