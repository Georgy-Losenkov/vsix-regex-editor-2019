using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;

namespace Losenkov.RegexEditor.Colorer.Input
{
    class RegexTagger : ITagger<IClassificationTag>
    {
        readonly String[] m_keys;
        readonly ITextBuffer m_buffer;
        readonly VersionTrackingTagger<CaptureTag> m_storage;
        readonly IClassificationTypeRegistryService m_classificationTypeRegistryService;

        public RegexTagger(ITextBuffer buffer, IClassificationTypeRegistryService classificationTypeRegistryService)
        {
            m_buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
            m_classificationTypeRegistryService = classificationTypeRegistryService ?? throw new ArgumentNullException(nameof(classificationTypeRegistryService));
            m_storage = new VersionTrackingTagger<CaptureTag>(buffer);
            m_keys = new String[] {
                Group01.Name,
                Group02.Name,
                Group03.Name,
                Group04.Name,
                Group05.Name,
                Group06.Name,
                Group07.Name,
            };
        }

        #region ITagger
        IEnumerable<ITagSpan<IClassificationTag>> ITagger<IClassificationTag>.GetTags(NormalizedSnapshotSpanCollection spans)
        {
            return m_storage.GetTags(spans);
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { m_storage.TagsChanged += value; }
            remove { m_storage.TagsChanged -= value; }
        }
        #endregion

        #region properties
        IClassificationType GetGroupClassificationType(Int32 index)
        {
            var count = m_keys.Length;
            index %= count;
            if (index < 0)
            {
                index += count;
            }

            return m_classificationTypeRegistryService.GetClassificationType(m_keys[index]);
        }
        #endregion

        sealed class CaptureTag : ITag, IClassificationTag
        {
            public CaptureInfo Data { get; }
            public IClassificationType ClassificationType { get; }

            public CaptureTag(CaptureInfo data, IClassificationType classificationType)
            {
                Data = data ?? throw new ArgumentNullException(nameof(data));
                ClassificationType = classificationType;
            }
        }

        public void CreateTags(IEnumerable<UI.ViewModel.MatchNode> matches)
        {
            using (m_storage.Update())
            {
                m_storage.RemoveTagSpans(s => true);

                if (matches == null)
                {
                    return;
                }

                var snapshot = m_buffer.CurrentSnapshot;

                foreach (var match in matches)
                {
                    var matchInfo = new MatchInfo(match.Index);

                    foreach (var group in match.Groups)
                    {
                        if (group.Number == 0)
                        {
                            continue;
                        }

                        if (!group.Success)
                        {
                            continue;
                        }

                        var groupInfo = new GroupInfo(matchInfo, group.Index, group.Number, group.Name);

                        foreach (var capture in group.Captures)
                        {
                            var captureInfo = new CaptureInfo(groupInfo, capture.Index);

                            if (capture.Segment != null
                              && 0 < capture.Segment.Length
                              && snapshot.TryCreateTrackingSpan(capture.Segment.Start, capture.Segment.Length, out var span))
                            {
                                // we do not add group with index 0 so it is better to adjust indices
                                var classificationType = GetGroupClassificationType(captureInfo.Parent.Index - 1);
                                m_storage.CreateTagSpan(span, new CaptureTag(captureInfo, classificationType));
                            }
                        }
                    }
                }

                m_storage.PinSnapshot(snapshot);
            }
        }
    }
}