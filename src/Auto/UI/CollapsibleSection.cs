using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Navigation;
using Losenkov.RegexEditor.UI.View;

namespace Losenkov.RegexEditor.UI
{
    [ContentProperty("CollapsibleBlocks")]
    public class CollapsibleSection : Section
    {
        #region dependency properties
        public static readonly DependencyProperty HeaderTextProperty;
        public static readonly DependencyProperty HeaderUriProperty;
        public static readonly DependencyProperty IsCollapsedProperty;
        public static readonly DependencyProperty ToggleLinkTextProperty;
        public static readonly DependencyProperty ToggleLinkToolTipProperty;

        static readonly DependencyPropertyKey s_toggleLinkTextPropertyKey;
        static readonly DependencyPropertyKey s_toggleLinkToolTipPropertyKey;

        static CollapsibleSection()
        {
            HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(String), typeof(CollapsibleSection), new PropertyMetadata(null));
            HeaderUriProperty = DependencyProperty.Register("HeaderUri", typeof(Uri), typeof(CollapsibleSection), new PropertyMetadata(null));
            IsCollapsedProperty = DependencyProperty.Register("IsCollapsed", typeof(Boolean), typeof(CollapsibleSection), new PropertyMetadata(false, IsCollapsedChanged));

            s_toggleLinkTextPropertyKey = DependencyProperty.RegisterReadOnly("ToggleLinkText", typeof(String), typeof(CollapsibleSection), new PropertyMetadata(null));
            s_toggleLinkToolTipPropertyKey = DependencyProperty.RegisterReadOnly("ToggleLinkToolTip", typeof(String), typeof(CollapsibleSection), new PropertyMetadata(null));
            ToggleLinkTextProperty = s_toggleLinkTextPropertyKey.DependencyProperty;
            ToggleLinkToolTipProperty = s_toggleLinkToolTipPropertyKey.DependencyProperty;
        }

        public Uri HeaderUri
        {
            get { return (Uri)GetValue(HeaderUriProperty); }
            set { SetValue(HeaderUriProperty, value); }
        }

        public String HeaderText
        {
            get { return (String)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        public Boolean IsCollapsed
        {
            get { return (Boolean)GetValue(IsCollapsedProperty); }
            set { SetValue(IsCollapsedProperty, value); }
        }

        public String ToggleLinkText
        {
            get { return (String)GetValue(ToggleLinkTextProperty); }
            private set { SetValue(s_toggleLinkTextPropertyKey, value); }
        }

        public String ToggleLinkToolTip
        {
            get { return (String)GetValue(ToggleLinkToolTipProperty); }
            private set { SetValue(s_toggleLinkToolTipPropertyKey, value); }
        }

        void CoerceToggleLink()
        {
            ToggleLinkText = IsCollapsed ? "▸" : "▾";
            ToggleLinkToolTip = IsCollapsed ? "Expand" : "Collapse";
        }

        static void IsCollapsedChanged(DependencyObject o, DependencyPropertyChangedEventArgs a)
        {
            var sender = (CollapsibleSection)o;

            var collapsed = (Boolean)a.NewValue;
            if (collapsed)
            {
                sender.Blocks.Remove(sender.m_innerSection);
            }
            else
            {
                sender.Blocks.InsertAfter(sender.m_header, sender.m_innerSection);
            }
            sender.CoerceToggleLink();
        }
        #endregion

        readonly Section m_innerSection;
        readonly Paragraph m_header;
        readonly Hyperlink m_navigateLink;

        public CollapsibleSection()
        {
            var fontSizeBinding = new Binding(FontSizeProperty.Name) { Source = this, Converter = new FontSizeConverter(), ConverterParameter = 1.1 };

            var toggleLinkText = new Run { FontWeight = FontWeights.Bold };
            toggleLinkText.SetBinding(Run.TextProperty, new Binding(nameof(ToggleLinkText)) { Source = this, Mode = BindingMode.OneWay });
            toggleLinkText.SetBinding(FontSizeProperty, fontSizeBinding);

            var toggleLink = new Hyperlink(toggleLinkText) { TextDecorations = null };
            toggleLink.SetBinding(ToolTipProperty, new Binding(nameof(ToggleLinkToolTip)) { Source = this, Mode = BindingMode.OneWay });
            toggleLink.SetResourceReference(ForegroundProperty, QuickRefColors.DocumentForegroundBrushKey);
            toggleLink.Click += ToggleLink_Click;

            var space1 = new Run(" ") { FontWeight = FontWeights.Bold };
            space1.SetBinding(FontSizeProperty, fontSizeBinding);

            var headerText = new Run() { FontWeight = FontWeights.Bold };
            headerText.SetBinding(Run.TextProperty, new Binding(nameof(HeaderText)) { Source = this, Mode = BindingMode.OneWay });
            headerText.SetBinding(FontSizeProperty, fontSizeBinding);
            headerText.SetResourceReference(ForegroundProperty, QuickRefColors.DocumentForegroundBrushKey);

            var space2 = new Run("    ") { FontWeight = FontWeights.Bold };
            space1.SetBinding(FontSizeProperty, fontSizeBinding);

            var navigateLinkText = new Run("more \u2192");
            // navigateLinkText.SetBinding(FontSizeProperty, new Binding(FontSizeProperty.Name) { Source = this, Converter = new FontSizeConverter(), ConverterParameter = 0.85 });

            m_navigateLink = new Hyperlink(navigateLinkText) { TextDecorations = null };
            m_navigateLink.SetBinding(Hyperlink.NavigateUriProperty, new Binding(nameof(HeaderUri)) { Source = this });
            m_navigateLink.SetResourceReference(ForegroundProperty, QuickRefColors.HyperlinkForegroundBrushKey);
            m_navigateLink.RequestNavigate += NavigateLink_RequestNavigate;
            m_navigateLink.MouseEnter += NavigateLink_MouseEnter;
            m_navigateLink.MouseLeave += NavigateLink_MouseLeave;

            m_header = new Paragraph() { TextIndent = 0 };
            m_header.Inlines.AddRange(new Inline[] { toggleLink, space1, headerText, space2, m_navigateLink });
            // header.SetBinding(Run.FontSizeProperty, new Binding(FontSizeProperty.Name) { Source = this, Converter = new FontSizeConverter(), ConverterParameter = 1.1 });

            m_innerSection = new Section();

            Blocks.AddRange(new Block[] { m_header, m_innerSection });

            CoerceToggleLink();
        }

        void NavigateLink_MouseLeave(Object sender, System.Windows.Input.MouseEventArgs e)
        {
            m_navigateLink.TextDecorations = null;
        }

        void NavigateLink_MouseEnter(Object sender, System.Windows.Input.MouseEventArgs e)
        {
            m_navigateLink.TextDecorations = TextDecorations.Underline;
        }

        void NavigateLink_RequestNavigate(Object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo { FileName = e.Uri.ToString(), Verb = "open" });
            }
            catch (Exception) { }
        }

        void ToggleLink_Click(Object sender, RoutedEventArgs e)
        {
            IsCollapsed = !IsCollapsed;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Boolean ShouldSerializeCollapsibleBlocks(XamlDesignerSerializationManager manager)
        {
            return m_innerSection.ShouldSerializeBlocks(manager);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BlockCollection CollapsibleBlocks
        {
            get
            {
                return m_innerSection.Blocks;
            }
        }
    }
}