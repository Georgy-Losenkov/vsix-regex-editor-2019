using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Microsoft.VisualStudio.Shell.Interop
{
    abstract class FontAndColorDefaultsBase : IVsFontAndColorDefaults, IVsFontAndColorEvents
    {
        #region ColorUsage
        protected enum ColorUsage
        {
            Background = 1,
            Foreground = 2,
        }
        #endregion
        #region ColorEntry
        protected abstract class ColorEntry
        {
            readonly FontAndColorDefaultsBase m_parent;

            public ColorEntry(FontAndColorDefaultsBase parent)
            {
                m_parent = parent ?? throw new ArgumentNullException(nameof(parent));

#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
                DefaultBackground = Enumerable.Empty<VsColor>();
                DefaultForeground = Enumerable.Empty<VsColor>();
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
            }

            public String Name { get; protected set; }
            public String LocalizedName { get; protected set; }
            public String Description { get; protected set; }
            public ColorUsage Usage { get; protected set; }
            public IEnumerable<VsColor> DefaultBackground { get; protected set; }
            public IEnumerable<VsColor> DefaultForeground { get; protected set; }

            Boolean TryGetDefaultBackground(out UInt32 result)
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                if (Services.TryGetThemeColor(m_parent.CategoryGuid, Name, __THEMEDCOLORTYPE.TCT_Background, out result))
                {
                    return true;
                }

                return VsColor.TryGetValue(DefaultBackground, out result);
            }

            Boolean TryGetDefaultForeground(out UInt32 result)
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                if (Services.TryGetThemeColor(m_parent.CategoryGuid, Name, __THEMEDCOLORTYPE.TCT_Foreground, out result))
                {
                    return true;
                }

                return VsColor.TryGetValue(DefaultForeground, out result);
            }

            public AllColorableItemInfo CreateColorInfo()
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                if (!TryGetDefaultBackground(out var defaultBackgroundColor))
                {
                    defaultBackgroundColor = 0;
                }
                if (!TryGetDefaultForeground(out var defaultForegroundColor))
                {
                    defaultForegroundColor = 0;
                }

                var flags = __FCITEMFLAGS.FCIF_ALLOWCUSTOMCOLORS;
                if ((Usage & ColorUsage.Background) == ColorUsage.Background)
                {
                    flags |= __FCITEMFLAGS.FCIF_ALLOWBGCHANGE;
                }
                if ((Usage & ColorUsage.Foreground) == ColorUsage.Foreground)
                {
                    flags |= __FCITEMFLAGS.FCIF_ALLOWFGCHANGE;
                }

                return new AllColorableItemInfo {
                    Info = new ColorableItemInfo {
                        crBackground = 0x2000000,
                        bBackgroundValid = 1,
                        crForeground = 0x2000000,
                        bForegroundValid = 1,
                        dwFontFlags = 0,
                        bFontFlagsValid = 1
                    },
                    bstrName = Name,
                    bNameValid = (Name != null) ? 1 : 0,
                    bstrLocalizedName = LocalizedName,
                    bLocalizedNameValid = (LocalizedName != null) ? 1 : 0,
                    crAutoBackground = defaultBackgroundColor,
                    bAutoBackgroundValid = 1,
                    crAutoForeground = defaultForegroundColor,
                    bAutoForegroundValid = 1,
                    dwMarkerVisualStyle = 0,
                    bMarkerVisualStyleValid = 1,
                    eLineStyle = LINESTYLE.LI_NONE,
                    bLineStyleValid = 1,
                    fFlags = (UInt32)flags,
                    bFlagsValid = 1,
                    bstrDescription = Description,
                    bDescriptionValid = (Description != null) ? 1 : 0,
                };
            }

            public void UpdateResources(ResourceDictionary resources, Color? backgroundColor, Color? foregroundColor)
            {
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
                if ((Usage & ColorUsage.Background) == ColorUsage.Background)
                {
                    var key = new ThemeResourceKey(m_parent.CategoryGuid, Name, ThemeResourceKeyType.BackgroundBrush);
                    if (backgroundColor.HasValue)
                    {
                        var brush = new SolidColorBrush(backgroundColor.Value);
                        brush.Freeze();
                        resources[key] = brush;
                    }
                    else
                    {
                        resources.Remove(key);
                    }
                }

                if ((Usage & ColorUsage.Foreground) == ColorUsage.Foreground)
                {
                    var key = new ThemeResourceKey(m_parent.CategoryGuid, Name, ThemeResourceKeyType.ForegroundBrush);
                    if (foregroundColor.HasValue)
                    {
                        var brush = new SolidColorBrush(foregroundColor.Value);
                        brush.Freeze();
                        resources[key] = brush;
                    }
                    else
                    {
                        resources.Remove(key);
                    }
                }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
            }
        }
        #endregion

        readonly ResourceDictionary m_resourceDictionary;

        protected FontAndColorDefaultsBase()
        {
            m_resourceDictionary = new ResourceDictionary();
            Application.Current.Resources.MergedDictionaries.Add(m_resourceDictionary);
        }

        Boolean m_fontInfoInitialized;

        public void EnsureFontAndColorsInitialized()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (!m_fontInfoInitialized)
            {
                ReloadFontAndColors();
            }
        }

        public void ReloadFontAndColors()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            m_fontInfoInitialized = true;

            #region fill out temorary resources
            var colorStorage = FontAndColorStorage;
            var categoryGuid = CategoryGuid;
            var fflags =
                (UInt32)__FCSTORAGEFLAGS.FCSF_LOADDEFAULTS |
                (UInt32)__FCSTORAGEFLAGS.FCSF_NOAUTOCOLORS |
                (UInt32)__FCSTORAGEFLAGS.FCSF_READONLY;

            if (ErrorHandler.Succeeded(colorStorage.OpenCategory(ref categoryGuid, fflags)))
            {
                try
                {
                    var pLOGFONT = new LOGFONTW[1];
                    var pFontInfo = new FontInfo[1];
                    if (ErrorHandler.Succeeded(colorStorage.GetFont(pLOGFONT, pFontInfo)) && (pFontInfo[0].bFaceNameValid == 1))
                    {
                        var fontInfoRef = pFontInfo[0];
                        var fontFamily = new FontFamily(fontInfoRef.bstrFaceName);
                        var fontSize = ConvertFromPoint(fontInfoRef.wPointSize);

                        m_resourceDictionary[new FontResourceKey(CategoryGuid, FontResourceKeyType.FontFamily)] = fontFamily;
                        m_resourceDictionary[new FontResourceKey(CategoryGuid, FontResourceKeyType.FontSize)] = fontSize;
                    }
                    else
                    {
                        m_resourceDictionary.Remove(new FontResourceKey(CategoryGuid, FontResourceKeyType.FontFamily));
                        m_resourceDictionary.Remove(new FontResourceKey(CategoryGuid, FontResourceKeyType.FontSize));
                    }

                    foreach (var colorEntry in ColorEntries)
                    {
                        Color? backgroundColor = null;
                        Color? foregroundColor = null;

                        var pColorInfo = new ColorableItemInfo[1];
                        if (ErrorHandler.Succeeded(colorStorage.GetItem(colorEntry.Name, pColorInfo)))
                        {
                            if (pColorInfo[0].bBackgroundValid == 1)
                            {
                                backgroundColor = Services.CreateWpfColor(pColorInfo[0].crBackground);
                            }
                            if (pColorInfo[0].bForegroundValid == 1)
                            {
                                foregroundColor = Services.CreateWpfColor(pColorInfo[0].crForeground);
                            }
                        }

                        colorEntry.UpdateResources(m_resourceDictionary, backgroundColor, foregroundColor);
                    }
                }
                finally
                {
                    colorStorage.CloseCategory();
                }
            }
            #endregion
        }

        protected internal Guid CategoryGuid { get; protected set; }
        protected String CategoryName { private get; set; }
        protected FontInfo Font { private get; set; }
        protected IReadOnlyList<ColorEntry> ColorEntries { private get; set; }

        #region IVsFontAndColorDefaults
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
        Int32 IVsFontAndColorDefaults.GetBaseCategory(out Guid guidBase)
        {
            guidBase = Guid.Empty;
            return 0;
        }

        Int32 IVsFontAndColorDefaults.GetCategoryName(out String stringName)
        {
            stringName = CategoryName;
            return 0;
        }

        Int32 IVsFontAndColorDefaults.GetFlags(out UInt32 flags)
        {
            flags = (Int32)(__FONTCOLORFLAGS.FCF_ONLYTTFONTS | __FONTCOLORFLAGS.FCF_SAVEALL);
            return 0;
        }

        Int32 IVsFontAndColorDefaults.GetFont(FontInfo[] info)
        {
            info[0] = Font;
            return 0;
        }

        Int32 IVsFontAndColorDefaults.GetItem(Int32 index, AllColorableItemInfo[] info)
        {
            if (0 <= index && index < ColorEntries.Count)
            {
                info[0] = ColorEntries[index].CreateColorInfo();
                return 0;
            }
            return -2147467259;
        }

        Int32 IVsFontAndColorDefaults.GetItemByName(String name, AllColorableItemInfo[] info)
        {
            foreach (var entry in ColorEntries)
            {
                if (entry.Name == name)
                {
                    info[0] = entry.CreateColorInfo();
                    return 0;
                }
            }
            return -2147467259;
        }

        Int32 IVsFontAndColorDefaults.GetItemCount(out Int32 items)
        {
            items = ColorEntries.Count;
            return 0;
        }

        Int32 IVsFontAndColorDefaults.GetPriority(out UInt16 priority)
        {
            priority = 0x100;
            return 0;
        }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        #endregion

        #region IVsFontAndColorEvents
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
        Int32 IVsFontAndColorEvents.OnApply()
        {
            ReloadFontAndColors();
            return 0;
        }

        Int32 IVsFontAndColorEvents.OnFontChanged(ref Guid rguidCategory, FontInfo[] pInfo, LOGFONTW[] pLOGFONT, IntPtr HFONT)
        {
            return 0;
        }

        Int32 IVsFontAndColorEvents.OnItemChanged(ref Guid category, String name, Int32 item, ColorableItemInfo[] info, UInt32 forground, UInt32 background)
        {
            return 0;
        }

        Int32 IVsFontAndColorEvents.OnReset(ref Guid category)
        {
            return 0;
        }

        Int32 IVsFontAndColorEvents.OnResetToBaseCategory(ref Guid category)
        {
            return 0;
        }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        #endregion

        #region static helpers
        protected static FontInfo CreateFontInfo(String fontFaceName, UInt16 pointSize, Byte charSet)
        {
            return new FontInfo {
                bstrFaceName = fontFaceName,
                wPointSize = pointSize,
                iCharSet = charSet,
                bFaceNameValid = 1,
                bPointSizeValid = 1,
                bCharSetValid = 1
            };
        }

        internal static Double ConvertFromPoint(UInt16 pointSize)
        {
            return 96.0 * pointSize / 72;
        }
        #endregion

        IVsFontAndColorStorage m_fontAndColorStorage;

        protected IVsFontAndColorStorage FontAndColorStorage
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                if (m_fontAndColorStorage == null)
                {
                    m_fontAndColorStorage = Package.GetGlobalService(typeof(SVsFontAndColorStorage)) as IVsFontAndColorStorage;
                }
                return m_fontAndColorStorage;
            }
        }
    }
}