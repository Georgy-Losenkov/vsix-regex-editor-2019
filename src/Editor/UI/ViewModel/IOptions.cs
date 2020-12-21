using System;
using System.Text.RegularExpressions;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    internal interface IOptions
    {
        String PatternText { get; }
        RegexOptions Options { get; }
        String ReplacementText { get; }
        String InputText { get; }
        Boolean MultilineInput { get; }
    }
}