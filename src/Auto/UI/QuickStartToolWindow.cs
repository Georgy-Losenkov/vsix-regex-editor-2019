using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace Losenkov.RegexEditor.UI
{
    [Guid("7b3ecaff-e08f-4f3a-b58b-6fcb36bded45")]
    class QuickStartToolWindow : ToolWindowPane
    {
#pragma warning disable IDE0060 // Remove unused parameter
        public QuickStartToolWindow(String message) : base(null)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            Caption = "Quick Start";
            BitmapResourceID = 301;
            BitmapIndex = 0;
        }

        public QuickStartToolWindow() : this(null)
        {
        }

        protected override void OnCreate()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Content = new View.QuickStartControl((IMenuCommandService)GetService(typeof(IMenuCommandService)));
            base.OnCreate();
        }
    }
}