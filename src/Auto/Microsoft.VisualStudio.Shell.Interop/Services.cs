using System;
using System.ComponentModel;
using System.Windows.Media;
using Microsoft.Internal.VisualStudio.Shell.Interop;

namespace Microsoft.VisualStudio.Shell.Interop
{
    static class Services
    {
        static IVsUIShell2 s_vsUIShell2;
        static IVsUIShell5 s_vsUIShell5;
        static IVsFontAndColorUtilities s_fontAndColorUtilities;
        static IVsColorThemeService s_colorThemeService;

        public static Boolean TryGetThemeColor(Guid colorCategory, String colorName, __THEMEDCOLORTYPE colorType, out UInt32 result)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (colorName == null)
            {
                throw new ArgumentNullException(nameof(colorName));
            }

            var currentTheme = ColorThemeService.CurrentTheme;
            if (currentTheme != null)
            {
                var entry = currentTheme[new ColorName { Category = colorCategory, Name = colorName }];
                if (entry != null)
                {
                    switch (colorType)
                    {
                        case __THEMEDCOLORTYPE.TCT_Background:
                            result = entry.Background;
                            return true;
                        case __THEMEDCOLORTYPE.TCT_Foreground:
                            result = entry.Foreground;
                            return true;
                        default:
                            throw new InvalidEnumArgumentException(nameof(colorType), (Int32)colorType, typeof(__THEMEDCOLORTYPE));
                    }
                }
            }

            result = 0;
            return false;
        }

        public static Color CreateWpfColor(UInt32 dwRGBValue)
        {
            var color = System.Drawing.ColorTranslator.FromWin32((Int32)dwRGBValue);
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static IVsUIShell2 VsUIShell2
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                if (s_vsUIShell2 == null)
                {
                    s_vsUIShell2 = Package.GetGlobalService(typeof(SVsUIShell)) as IVsUIShell2;
                }
                return s_vsUIShell2;
            }
        }

        public static IVsUIShell5 VsUIShell5
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                if (s_vsUIShell5 == null)
                {
                    s_vsUIShell5 = Package.GetGlobalService(typeof(SVsUIShell)) as IVsUIShell5;
                }
                return s_vsUIShell5;
            }
        }

        public static IVsFontAndColorUtilities FontAndColorUtilities
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                if (s_fontAndColorUtilities == null)
                {
                    s_fontAndColorUtilities = Package.GetGlobalService(typeof(SVsFontAndColorStorage)) as IVsFontAndColorUtilities;
                }
                return s_fontAndColorUtilities;
            }
        }

        public static IVsColorThemeService ColorThemeService
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                if (s_colorThemeService == null)
                {
                    s_colorThemeService = Package.GetGlobalService(typeof(SVsColorThemeService)) as IVsColorThemeService;
                }
                return s_colorThemeService;
            }
        }
    }
}