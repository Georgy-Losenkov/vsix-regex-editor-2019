using System;

namespace Microsoft.VisualStudio.Shell.Interop
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    internal sealed class FontAndColorRegistrationAttribute : RegistrationAttribute
    {
        public FontAndColorRegistrationAttribute(Type providerType, String name, String category)
        {
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
            Name = name;
            Provider = providerType.GUID;
            Category = new Guid(category);
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        }

        public override void Register(RegistrationContext context)
        {
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
            if (context != null)
            {
                context.Log.WriteLine("FontAndColors:    Name:{0}, Category:{1}, Package:{2}", Name, Category.ToString("B"), Provider.ToString("B"));
                using (var key = context.CreateKey("FontAndColors\\" + Name))
                {
                    key.SetValue("Category", Category.ToString("B"));
                    key.SetValue("Package", Provider.ToString("B"));
                }
            }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        }

        public override void Unregister(RegistrationContext context)
        {
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
            if (context != null)
            {
                context.RemoveKey("FontAndColors\\" + Name);
            }
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        }

        public String Name { get; set; }
        public Guid Category { get; set; }
        public Guid Provider { get; set; }
    }
}