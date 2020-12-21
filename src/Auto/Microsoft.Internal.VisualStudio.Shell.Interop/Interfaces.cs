using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Internal.VisualStudio.Shell.Interop
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    [CompilerGenerated]
    [TypeIdentifier("EF2A7BE1-84AF-4E47-A2CF-056DF55F3B7A", "Microsoft.Internal.VisualStudio.Shell.Interop.ColorName")]
    public struct ColorName
    {
        public Guid Category;
        [MarshalAs(UnmanagedType.BStr)]
        public String Name;
    }

    [ComImport]
    [CompilerGenerated]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("BBE70639-7AD9-4365-AE36-9877AF2F973B")]
    [TypeIdentifier]
    public interface IVsColorEntry
    {
        [DispId(0x60010000)]
        ColorName ColorName { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }
        [DispId(0x60010001)]
        Byte BackgroundType { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }
        [DispId(0x60010002)]
        Byte ForegroundType { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }
        [DispId(0x60010003)]
        UInt32 Background { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }
        [DispId(0x60010004)]
        UInt32 Foreground { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }
        [DispId(0x60010005)]
        UInt32 BackgroundSource { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }
        [DispId(0x60010006)]
        UInt32 ForegroundSource { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }
    }

    [ComImport]
    [CompilerGenerated]
    [Guid("413D8344-C0DB-4949-9DBC-69C12BADB6AC")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [TypeIdentifier]
    public interface IVsColorTheme
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Apply();
        [DispId(0)]
        IVsColorEntry this[ColorName Name] { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }
        [DispId(0x60010002)]
        Guid ThemeId { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }
        [DispId(0x60010003)]
        String Name { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }
        [DispId(0x60010004)]
        Boolean IsUserVisible { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }
    }

    [ComImport]
    [CompilerGenerated]
    [Guid("EAB552CF-7858-4F05-8435-62DB6DF60684")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [TypeIdentifier]
    public interface IVsColorThemeService
    {
        [MethodImpl((MethodImplOptions)0, MethodCodeType = MethodCodeType.Runtime)]
#pragma warning disable IDE1006 // Naming Styles
        void _VtblGap1_6();
#pragma warning restore IDE1006 // Naming Styles
        [DispId(0x60010006)]
        IVsColorTheme CurrentTheme { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }
    }

    [ComImport]
    [Guid("0D915B59-2ED7-472A-9DE8-9161737EA1C5")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [TypeIdentifier]
#pragma warning disable IDE1006 // Naming Styles
    public interface SVsColorThemeService
#pragma warning restore IDE1006 // Naming Styles
    {
    }
}