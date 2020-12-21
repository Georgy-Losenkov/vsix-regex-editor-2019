using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;

namespace Losenkov.RegexEditor.UI
{
    internal class RegexPatternContentType
    {
#pragma warning disable 649, 169
        [Export, Name(ContentTypeName), BaseDefinition("code")]
        internal static ContentTypeDefinition ContentTypeDefinition;
#pragma warning restore 649, 169

        internal const String ContentTypeName = "Regex Pattern";
    }

    internal class RegexReplacementContentType
    {
#pragma warning disable 649, 169
        [Export, Name(ContentTypeName), BaseDefinition("code")]
        internal static ContentTypeDefinition ContentTypeDefinition;
#pragma warning restore 649, 169

        internal const String ContentTypeName = "Regex Replacement";
    }

    internal class RegexInputContentType
    {
#pragma warning disable 649, 169
        [Export, Name(ContentTypeName), BaseDefinition("code")]
        internal static ContentTypeDefinition ContentTypeDefinition;
#pragma warning restore 649, 169

        internal const String ContentTypeName = "Regex Input";
    }

    internal class RegexTestContentType
    {
#pragma warning disable 649, 169
        [Export, Name(ContentTypeName), BaseDefinition("code")]
        internal static ContentTypeDefinition ContentTypeDefinition;
#pragma warning restore 649, 169

        internal const String ContentTypeName = "Regex Test";
    }

    internal class RegexCsContentType
    {
#pragma warning disable 649, 169
        [Export, Name(ContentTypeName), BaseDefinition("code")]
        internal static ContentTypeDefinition ContentTypeDefinition;
#pragma warning restore 649, 169

        internal const String ContentTypeName = "Regex Cs";
    }
}