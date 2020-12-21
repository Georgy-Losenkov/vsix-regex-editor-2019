using System;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;

namespace Losenkov.RegexEditor
{
    static class UserSettings
    {
        static WritableSettingsStore s_writableSettingsStore;

        static WritableSettingsStore SettingsStore
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                if (s_writableSettingsStore == null)
                {
                    s_writableSettingsStore = new ShellSettingsManager(ServiceProvider.GlobalProvider).GetWritableSettingsStore(SettingsScope.UserSettings);
                }
                return s_writableSettingsStore;
            }
        }

        const String IsHelpForceShownPropertyName = "IsHelpForceShown";
        const String QuickRefFontSizePropertyName = "QuickRefFontSize";
        const String QuickRefFontFamilyPropertyName = "QuickRefFontFamily";
        const String CollectionName = "RegexEditorLite";
        const String DefaultQuickRefFontFamily = "Consolas";
        const Int32 DefaultQuickRefFontSize = 51;

#if (!DEBUG)
        static Boolean? _isHelpForceShown;
#endif

        public static Boolean IsHelpForceShown
        {
            get
            {
#if (!DEBUG)
                if (!_isHelpForceShown.HasValue)
                {
                    _isHelpForceShown = (SettingsStore.GetInt32(CollectionName, IsHelpForceShownPropertyName, 0) != 0);
                }
                return _isHelpForceShown.Value;
#else
                return false;
#endif
            }
            set
            {
#if (!DEBUG)
                if (IsHelpForceShown != value)
                {
                    _isHelpForceShown = value;
                    SettingsStore.CreateCollection(CollectionName);
                    SettingsStore.SetInt32(CollectionName, IsHelpForceShownPropertyName, value ? 1 : 0);
                }
#endif
            }
        }

        public static DateTime GetLastModified()
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                return SettingsStore.GetLastWriteTime(CollectionName);
            }
            catch (ArgumentException)
            {
                return DateTime.MinValue;
            }
        }

        public static Int32 GetQuickRefFontSize()
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                return SettingsStore.GetInt32(CollectionName, QuickRefFontSizePropertyName, DefaultQuickRefFontSize);
            }
            catch (ArgumentException)
            {
                return DefaultQuickRefFontSize;
            }
        }

        public static void SetQuickRefFontSize(Int32 value)
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                SettingsStore.CreateCollection(CollectionName);
                SettingsStore.SetInt32(CollectionName, QuickRefFontSizePropertyName, value);
            }
            catch (ArgumentException) { }
        }

        public static String GetQuickRefFontFamily()
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                return SettingsStore.GetString(CollectionName, QuickRefFontFamilyPropertyName, DefaultQuickRefFontFamily);
            }
            catch (ArgumentException)
            {
                return DefaultQuickRefFontFamily;
            }
        }

        public static void SetQuickRefFontFamily(String value)
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                SettingsStore.CreateCollection(CollectionName);
                SettingsStore.SetString(CollectionName, QuickRefFontFamilyPropertyName, value);
            }
            catch (ArgumentException) { }
        }
    }
}