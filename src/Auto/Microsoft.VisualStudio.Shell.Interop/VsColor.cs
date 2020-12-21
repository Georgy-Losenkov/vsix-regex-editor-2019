using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Microsoft.VisualStudio.Shell.Interop
{
    internal abstract class VsColor
    {
        public abstract Boolean TryGetValue(out UInt32 result);

        public static Boolean TryGetValue(IEnumerable<VsColor> colors, out UInt32 result)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (colors == null)
            {
                throw new ArgumentNullException(nameof(colors));
            }

            foreach (var color in colors)
            {
                if (color.TryGetValue(out result))
                {
                    return true;
                }
            }

            result = 0;
            return false;
        }
    }

    internal sealed class AutoColor : VsColor
    {
        public override Boolean TryGetValue(out UInt32 result)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return ErrorHandler.Succeeded(Services.FontAndColorUtilities.EncodeAutomaticColor(out result));
        }
    }

    internal sealed class IndexedColor : VsColor
    {
        readonly COLORINDEX m_index;

        public IndexedColor(COLORINDEX index)
        {
            m_index = index;
        }
        public override Boolean TryGetValue(out UInt32 result)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return ErrorHandler.Succeeded(Services.FontAndColorUtilities.GetRGBOfIndex(m_index, out result));
        }
    }

    internal sealed class RgbColor : VsColor
    {
        readonly UInt32 m_value;

        public RgbColor(Byte r, Byte b, Byte g)
        {
            m_value = (UInt32)(r | (g << 8) | (b << 16));
        }
        public override Boolean TryGetValue(out UInt32 result)
        {
            result = m_value;
            return true;
        }
    }

    internal sealed class SysColor : VsColor
    {
        readonly Int32 m_index;

        public SysColor(Int32 index)
        {
            m_index = index;
        }
        public override Boolean TryGetValue(out UInt32 result)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            result = 0xff000000 | (UInt32)Services.GetSysColor(m_index);
            return true;
        }
    }

    internal sealed class VsSysColor : VsColor
    {
        readonly Int32 m_index;

        public VsSysColor(__VSSYSCOLOREX index)
        {
            m_index = (Int32)index;
        }
        public VsSysColor(__VSSYSCOLOREX2 index)
        {
            m_index = (Int32)index;
        }
        public VsSysColor(__VSSYSCOLOREX3 index)
        {
            m_index = (Int32)index;
        }
        public override Boolean TryGetValue(out UInt32 result)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return ErrorHandler.Succeeded(Services.VsUIShell2.GetVSSysColorEx(m_index, out result));
        }
    }

    internal sealed class ThemeColor : VsColor
    {
        readonly Guid m_colorCategory;
        readonly String m_colorName;
        readonly __THEMEDCOLORTYPE m_colorType;

        public ThemeColor(ThemeResourceKey key)
        {
            m_colorCategory = key.Category;
            m_colorName = key.Name;
            if (key.KeyType == ThemeResourceKeyType.BackgroundBrush || key.KeyType == ThemeResourceKeyType.BackgroundColor)
            {
                m_colorType = __THEMEDCOLORTYPE.TCT_Background;
            }
            else
            {
                m_colorType = __THEMEDCOLORTYPE.TCT_Foreground;
            }
        }
        public override Boolean TryGetValue(out UInt32 result)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return Services.TryGetThemeColor(m_colorCategory, m_colorName, m_colorType, out result);
        }
    }
}