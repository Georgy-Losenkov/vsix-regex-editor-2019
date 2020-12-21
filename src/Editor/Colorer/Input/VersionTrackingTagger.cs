using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;

namespace Losenkov.RegexEditor.Colorer.Input
{
    class VersionTrackingTagger<T> : ITagger<T> where T : ITag
    {
        readonly ITextBuffer m_buffer;
        readonly SimpleTagger<T> m_storage;
        Int32 m_bufferVersionNum;
        Boolean m_bufferIsModified;

        public VersionTrackingTagger(ITextBuffer buffer)
        {
            m_buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
            m_storage = new SimpleTagger<T>(buffer);

            m_buffer.Changed += Buffer_Changed;
            m_storage.TagsChanged += Storage_TagsChanged;
        }

        void Storage_TagsChanged(Object sender, SnapshotSpanEventArgs e)
        {
            if (m_bufferIsModified)
            {
                return;
            }

            var handler = TagsChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        void Buffer_Changed(Object sender, TextContentChangedEventArgs e)
        {
            var proposedVersionNum = e.AfterVersion.ReiteratedVersionNumber;
            var proposedIsModified = (m_bufferVersionNum != proposedVersionNum);

            if (m_bufferIsModified == proposedIsModified)
            {
                return;
            }

            m_bufferIsModified = proposedIsModified;

            var span = new SnapshotSpan(e.After, 0, e.After.Length);

            var handler = TagsChanged;
            if (handler != null)
            {
                handler(this, new SnapshotSpanEventArgs(span));
            }
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public IEnumerable<ITagSpan<T>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (m_bufferIsModified)
            {
                return Enumerable.Empty<ITagSpan<T>>();
            }
            else
            {
                return m_storage.GetTags(spans);
            }
        }

        public IEnumerable<ITagSpan<TOther>> GetTags<TOther>(NormalizedSnapshotSpanCollection spans)
          where TOther : class, ITag
        {
            if (m_bufferIsModified)
            {
                yield break;
            }

            foreach (var ts in m_storage.GetTags(spans))
            {
                if (ts.Tag is TOther tag)
                {
                    yield return new TagSpan<TOther>(ts.Span, tag);
                }
            }
        }

        public TrackingTagSpan<T> CreateTagSpan(ITrackingSpan span, T tag)
        {
            return m_storage.CreateTagSpan(span, tag);
        }

        public Int32 RemoveTagSpans(Predicate<TrackingTagSpan<T>> match)
        {
            return m_storage.RemoveTagSpans(match);
        }

        public IDisposable Update()
        {
            return m_storage.Update();
        }

        public void PinSnapshot(ITextSnapshot snapshot)
        {
            if (snapshot == null)
            {
                return;
            }

            if (snapshot.TextBuffer != m_buffer)
            {
                return;
            }

            m_bufferVersionNum = snapshot.Version.ReiteratedVersionNumber;
            m_bufferIsModified = false;
        }
    }
}