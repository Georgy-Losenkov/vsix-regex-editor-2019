using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.VisualStudio.Shell.Interop
{
    [SuppressUnmanagedCodeSecurityAttribute]
    internal static class SafeNativeMethods
    {

        [DllImport("user32.dll")]
        internal static extern Int32 GetSysColor(Int32 nIndex);
    }
}