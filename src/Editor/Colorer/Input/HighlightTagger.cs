using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Text.Tagging;

namespace Losenkov.RegexEditor.Colorer.Input
{
    class HighlightTagger : ITagger<IntraTextAdornmentTag>, ITagger<ITextMarkerTag>
    {
        readonly IWpfTextView m_view;
        readonly VersionTrackingTagger<ITag> m_storage;
        readonly IClassificationFormatMap m_classificationFormatMap;
        readonly IClassificationTypeRegistryService m_classificationTypeRegistryService;

        public HighlightTagger(
          IWpfTextView view,
          IClassificationFormatMap classificationFormatMap,
          IClassificationTypeRegistryService classificationTypeRegistryService)
        {
            m_view = view ?? throw new ArgumentNullException(nameof(view));
            m_classificationFormatMap = classificationFormatMap ?? throw new ArgumentNullException(nameof(classificationFormatMap));
            m_classificationTypeRegistryService = classificationTypeRegistryService ?? throw new ArgumentNullException(nameof(classificationTypeRegistryService));
            m_storage = new VersionTrackingTagger<ITag>(m_view.TextBuffer);

            m_view.LayoutChanged += HandleLayoutChanged;
            m_view.Closed += HandleViewClosed;
            m_classificationFormatMap.ClassificationFormatMappingChanged += ClassificationFormatMap_ClassificationFormatMappingChanged;

            UpdateViewLayoutOptionsCache(m_view.VisualElement);
        }

        void ClassificationFormatMap_ClassificationFormatMappingChanged(Object sender, EventArgs e)
        {
            SetFontFromClassification();
        }

        void HandleViewClosed(Object sender, EventArgs e)
        {
            m_view.Closed -= HandleViewClosed;
            m_view.LayoutChanged -= HandleLayoutChanged;
            m_classificationFormatMap.ClassificationFormatMappingChanged -= ClassificationFormatMap_ClassificationFormatMappingChanged;
        }

        void HandleLayoutChanged(Object sender, TextViewLayoutChangedEventArgs args)
        {
            if (AreViewLayoutOptionsChanged(m_view.VisualElement))
            {
                UpdateViewLayoutOptionsCache(m_view.VisualElement);
                if (m_adornment != null)
                {
                    SetLayoutOptions(m_adornment);
                }
            }
        }

        #region ITagger
        IEnumerable<ITagSpan<ITextMarkerTag>> ITagger<ITextMarkerTag>.GetTags(NormalizedSnapshotSpanCollection spans)
        {
            return m_storage.GetTags<ITextMarkerTag>(spans);
        }

        IEnumerable<ITagSpan<IntraTextAdornmentTag>> ITagger<IntraTextAdornmentTag>.GetTags(NormalizedSnapshotSpanCollection spans)
        {
            return m_storage.GetTags<IntraTextAdornmentTag>(spans);
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { m_storage.TagsChanged += value; }
            remove { m_storage.TagsChanged -= value; }
        }
        #endregion

        #region layout options
        TextFormattingMode m_textFormattingMode;
        TextHintingMode m_textHintingMode;
        TextRenderingMode m_textRenderingMode;

        Boolean AreViewLayoutOptionsChanged(FrameworkElement view)
        {
            return (TextOptions.GetTextRenderingMode(view) != m_textRenderingMode)
              || (TextOptions.GetTextHintingMode(view) != m_textHintingMode)
              || (TextOptions.GetTextFormattingMode(view) != m_textFormattingMode);
        }

        void UpdateViewLayoutOptionsCache(FrameworkElement view)
        {
            m_textRenderingMode = TextOptions.GetTextRenderingMode(view);
            m_textHintingMode = TextOptions.GetTextHintingMode(view);
            m_textFormattingMode = TextOptions.GetTextFormattingMode(view);
        }

        void SetLayoutOptions(FrameworkElement adornment)
        {
            TextOptions.SetTextFormattingMode(adornment, m_textFormattingMode);
            TextOptions.SetTextHintingMode(adornment, m_textHintingMode);
            TextOptions.SetTextRenderingMode(adornment, m_textRenderingMode);
        }
        #endregion

        void SetFontFromClassification()
        {
            if (m_adornment != null)
            {
                var classificationType = m_classificationTypeRegistryService.GetClassificationType(EmptyMarkerClassificationFormatDefinition.Name);
                var textProperties = m_classificationFormatMap.GetTextProperties(classificationType);

                m_adornment.SetFont(textProperties);
            }
        }

        internal sealed class EmptyAdornment : Border
        {
            readonly TextBlock m_child;

            internal EmptyAdornment()
            {
                Height = 17;
                Width = 7;
                Child = m_child = new TextBlock(new Run("\u03b5"));
            }

            public void SetFont(TextFormattingRunProperties formatting)
            {
                var backgroundBrush = formatting.BackgroundBrush;
                if (backgroundBrush.Opacity != 1.0)
                {
                    backgroundBrush = backgroundBrush.Clone();
                    backgroundBrush.Opacity = 1.0;
                    backgroundBrush.Freeze();
                }
                Background = backgroundBrush;

                m_child.Foreground = formatting.ForegroundBrush;
                m_child.FontSize = formatting.FontRenderingEmSize;
                m_child.FontFamily = formatting.Typeface.FontFamily;
                m_child.FontStretch = formatting.Typeface.Stretch;
                m_child.FontStyle = formatting.Typeface.Style;
                m_child.FontWeight = formatting.Typeface.Weight;
            }
        }

        EmptyAdornment m_adornment = null;

        EmptyAdornment CreateAdornment(Double columnWidth, Double lineHeight)
        {
            if (m_adornment == null)
            {
                m_adornment = new EmptyAdornment();

                SetFontFromClassification();
            }

            m_adornment.Width = columnWidth;
            m_adornment.Height = lineHeight;

            return m_adornment;
        }

        class HightlightTag : ITag, ITextMarkerTag
        {
            public String Type { get { return HighlightMarkerFormatDefinition.Name; } }
        }

        public void Hightlight(Int32 start, Int32 length)
        {
            using (m_storage.Update())
            {
                m_storage.RemoveTagSpans(s => true);

                if (m_view.TextBuffer.CurrentSnapshot.TryCreateTrackingSpan(start, length, out var span))
                {
                    if (length == 0)
                    {
                        var textHeightAbove = m_view.FormattedLineSource.TextHeightAboveBaseline;
                        var textHeightBelow = m_view.FormattedLineSource.TextHeightBelowBaseline;
                        var columnWidth = m_view.FormattedLineSource.ColumnWidth;
                        var lineHeight = m_view.FormattedLineSource.LineHeight;

                        var adornment = CreateAdornment(columnWidth, lineHeight);

                        SetLayoutOptions(adornment);
                        adornment.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));

                        m_storage.CreateTagSpan(span,
                          new IntraTextAdornmentTag(
                            adornment,
                            null,
                            0,
                            textHeightAbove,
                            0, //textHeightAbove + textHeightBelow,
                            0,
                            PositionAffinity.Predecessor));
                    }
                    else
                    {
                        m_storage.CreateTagSpan(span, new HightlightTag());
                    }
                }
            }
        }

        public void Unhighlight()
        {
            m_storage.RemoveTagSpans(s => true);
        }

        public void PinSnapshot()
        {
            m_storage.PinSnapshot(m_view.TextBuffer.CurrentSnapshot);
        }
    }
}