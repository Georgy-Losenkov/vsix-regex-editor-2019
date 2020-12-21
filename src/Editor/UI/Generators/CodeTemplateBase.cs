using System;
using System.Collections.Generic;
using System.Linq;
using RegexOptions = System.Text.RegularExpressions.RegexOptions;

namespace Losenkov.RegexEditor.UI.Generators
{
    public class CodeTemplateBase : TextTemplateBase
    {
        protected class MethodOptions
        {
            public Boolean MultilineInput { get; }
            public Model.RegexMethod RegexMethod { get; }

            public MethodOptions(Boolean multilineInput, Model.RegexMethod regexMethod)
            {
                this.MultilineInput = multilineInput;
                this.RegexMethod = regexMethod;
            }
        }

        protected readonly String PatternText;
        protected readonly String[] Options;
        protected readonly String ReplacementText;
        protected readonly String InputText;
        protected readonly MethodOptions[] Methods;

        internal CodeTemplateBase(
            String patternText,
            RegexOptions options,
            String replacementText,
            String inputText,
            Boolean? multilineInput,
            Model.RegexMethod? regexMethod)
        {
            this.PatternText = patternText;
            this.ReplacementText = replacementText;
            this.InputText = inputText;

            var list1 = Enum.GetValues(typeof(RegexOptions))
                           .Cast<RegexOptions>()
                           .Where(o => (options & o) != RegexOptions.None)
                           .Select(o => "RegexOptions." + o.ToString("g"))
                           .ToList();
            if (list1.Count == 0)
            {
                list1.Add("RegexOptions.None");
            }

            this.Options = list1.ToArray();

            var list2 = new List<MethodOptions>();
            foreach (var mi in new Boolean[] { true, false })
            {
                if (multilineInput.HasValue && multilineInput.Value != mi)
                {
                    continue;
                }

                foreach (Model.RegexMethod rm in Enum.GetValues(typeof(Model.RegexMethod)))
                {
                    if (regexMethod.HasValue && regexMethod.Value != rm)
                    {
                        continue;
                    }

                    list2.Add(new MethodOptions(mi, rm));
                }
            }

            this.Methods = list2.ToArray();
        }

        internal CodeTemplateBase(ViewModel.IOptions snapshot, Model.RegexMethod regexMethod)
#if (DEBUG)
            : this(snapshot.PatternText, snapshot.Options, snapshot.ReplacementText, snapshot.InputText, null, null)
#else
            : this(snapshot.PatternText, snapshot.Options, snapshot.ReplacementText, snapshot.InputText, snapshot.MultilineInput, regexMethod)
#endif
        {
        }
    }
}