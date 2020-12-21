using System;
using Microsoft.VisualStudio.Text;

namespace Losenkov.RegexEditor.Colorer.Input
{
    static class Utilities
    {
        public static Boolean TryCreateTrackingSpan(this ITextSnapshot snapshot, Int32 start, Int32 length, out ITrackingSpan span)
        {
            try
            {
                span = snapshot.CreateTrackingSpan(start, length, SpanTrackingMode.EdgeExclusive, TrackingFidelityMode.UndoRedo);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                span = null;
                return false;
            }
        }
    }
}