using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace Losenkov.RegexEditor.UI
{
    [Guid("01e709bd-92bc-48e7-9cae-524a67db5bd0")]
    class QuickRefToolWindow : ToolWindowPane
    {
#pragma warning disable IDE0060 // Remove unused parameter
        public QuickRefToolWindow(String message) : base(null)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            Caption = "Regex Quick Reference";
            BitmapResourceID = 301;
            BitmapIndex = 0;
        }

        public QuickRefToolWindow() : this(null)
        {
        }

        protected override void OnCreate()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Shell.FontAndColorDefaultsQuickRef.Instance.EnsureFontAndColorsInitialized();
            Content = new View.QuickRefControl();
            base.OnCreate();
        }
    }
}