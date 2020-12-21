using System;
using System.Collections.Generic;
using Losenkov.RegexEditor.UI.Model;
using RegexOptions = System.Text.RegularExpressions.RegexOptions;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    public sealed class EditorViewModel : ViewModelBase
    {
        #region Regex Options
        Boolean m_compiled;
        Boolean m_cultureInvariant;
        Boolean m_eCMAScript;
        Boolean m_explicitCapture;
        Boolean m_ignoreCase;
        Boolean m_ignorePatternWhitespace;
        Boolean m_multiline;
        Boolean m_rightToLeft;
        Boolean m_singleline;

        public Boolean Compiled
        {
            get { return m_compiled; }
            set
            {
                if (m_compiled != value)
                {
                    m_compiled = value;
                    RaisePropertyChanged(nameof(Compiled));
                }
            }
        }
        public Boolean CultureInvariant
        {
            get { return m_cultureInvariant; }
            set
            {
                if (m_cultureInvariant != value)
                {
                    m_cultureInvariant = value;
                    RaisePropertyChanged(nameof(CultureInvariant));
                }
            }
        }
        public Boolean ECMAScript
        {
            get { return m_eCMAScript; }
            set
            {
                if (m_eCMAScript != value)
                {
                    m_eCMAScript = value;
                    RaisePropertyChanged(nameof(ECMAScript));
                }
            }
        }
        public Boolean ExplicitCapture
        {
            get { return m_explicitCapture; }
            set
            {
                if (m_explicitCapture != value)
                {
                    m_explicitCapture = value;
                    RaisePropertyChanged(nameof(ExplicitCapture));
                }
            }
        }
        public Boolean IgnoreCase
        {
            get { return m_ignoreCase; }
            set
            {
                if (m_ignoreCase != value)
                {
                    m_ignoreCase = value;
                    RaisePropertyChanged(nameof(IgnoreCase));
                }
            }
        }
        public Boolean IgnorePatternWhitespace
        {
            get { return m_ignorePatternWhitespace; }
            set
            {
                if (m_ignorePatternWhitespace != value)
                {
                    m_ignorePatternWhitespace = value;
                    RaisePropertyChanged(nameof(IgnorePatternWhitespace));
                }
            }
        }
        public Boolean Multiline
        {
            get { return m_multiline; }
            set
            {
                if (m_multiline != value)
                {
                    m_multiline = value;
                    RaisePropertyChanged(nameof(Multiline));
                }
            }
        }
        public Boolean RightToLeft
        {
            get { return m_rightToLeft; }
            set
            {
                if (m_rightToLeft != value)
                {
                    m_rightToLeft = value;
                    RaisePropertyChanged(nameof(RightToLeft));
                }
            }
        }
        public Boolean Singleline
        {
            get { return m_singleline; }
            set
            {
                if (m_singleline != value)
                {
                    m_singleline = value;
                    RaisePropertyChanged(nameof(Singleline));
                }
            }
        }

        public RegexOptions RegexOptions
        {
            get
            {
                return
                  (Compiled ? RegexOptions.Compiled : RegexOptions.None) |
                  (CultureInvariant ? RegexOptions.CultureInvariant : RegexOptions.None) |
                  (ECMAScript ? RegexOptions.ECMAScript : RegexOptions.None) |
                  (ExplicitCapture ? RegexOptions.ExplicitCapture : RegexOptions.None) |
                  (IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None) |
                  (IgnorePatternWhitespace ? RegexOptions.IgnorePatternWhitespace : RegexOptions.None) |
                  (Multiline ? RegexOptions.Multiline : RegexOptions.None) |
                  (RightToLeft ? RegexOptions.RightToLeft : RegexOptions.None) |
                  (Singleline ? RegexOptions.Singleline : RegexOptions.None);
            }
        }
        #endregion

        #region RegexMethod
        RegexMethod m_regexMethod = RegexMethod.Match;
        public RegexMethod RegexMethod
        {
            get { return m_regexMethod; }
            set
            {
                if (m_regexMethod != value)
                {
                    m_regexMethod = value;
                    RaisePropertyChanged(nameof(RegexMethod));
                }
            }
        }
        #endregion

        #region TesterMode
        TesterMode m_testerMode = TesterMode.Invoke;
        public TesterMode TesterMode
        {
            get { return m_testerMode; }
            set
            {
                if (m_testerMode != value)
                {
                    m_testerMode = value;
                    RaisePropertyChanged(nameof(TesterMode));
                }
            }
        }
        #endregion

        Boolean m_resultsVisible = false;
        Boolean m_multilineInput = false;

        public EditorViewModel()
        {
            Results = new ResultsViewModel();
#if (DEBUG)
            m_resultsVisible = true;
#endif
        }

        public ResultsViewModel Results { get; }

        public Boolean ResultsVisible
        {
            get { return m_resultsVisible; }
            set
            {
                if (m_resultsVisible != value)
                {
                    m_resultsVisible = value;
                    RaisePropertyChanged(nameof(ResultsVisible));
                }
            }
        }
        public Boolean MultilineInput
        {
            get { return m_multilineInput; }
            set
            {
                if (m_multilineInput != value)
                {
                    m_multilineInput = value;
                    RaisePropertyChanged(nameof(MultilineInput));
                }
            }
        }

        public static IEnumerable<RegexMethod> RegexMethods
        {
            get
            {
                yield return RegexMethod.Match;
                yield return RegexMethod.Replace;
                yield return RegexMethod.Split;
            }
        }

        public static IEnumerable<TesterMode> TesterModes
        {
            get
            {
                yield return TesterMode.Invoke;
                yield return TesterMode.CsCode;
                yield return TesterMode.VbCode;
            }
        }
    }
}