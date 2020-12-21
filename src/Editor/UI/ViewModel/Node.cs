using System;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    abstract class Node
    {
        public Segment Segment { get; }
        public abstract Boolean Success { get; }
        public String Text { get; }

        protected Node(String text, Segment segment)
        {
            Text = text ?? String.Empty;
            Segment = segment;
        }
    }
}