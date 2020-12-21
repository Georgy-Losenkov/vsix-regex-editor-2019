using System;

namespace Microsoft.VisualStudio.Shell.Interop
{
    enum FontResourceKeyType
    {
        FontFamily,
        FontSize,
    }

#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
    sealed class FontResourceKey
    {
        public FontResourceKey(Guid category, FontResourceKeyType keyType)
        {
            Category = category;
            KeyType = keyType;
        }

        public override Boolean Equals(Object obj)
        {
            if (obj is FontResourceKey key)
            {
                return Category == key.Category && KeyType == key.KeyType;
            }
            else
            {
                return false;
            }
        }

        public override Int32 GetHashCode()
        {
            return Category.GetHashCode() ^ (Int32)KeyType;
        }

        public override String ToString()
        {
            return $"{Category}.{KeyType}";
        }

        public Guid Category { get; }
        public FontResourceKeyType KeyType { get; }
    }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
}