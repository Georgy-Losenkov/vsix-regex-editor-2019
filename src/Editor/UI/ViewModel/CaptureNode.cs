using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    internal sealed class CaptureNode : Node
    {
        public Int32 Index { get; }
        public override Boolean Success { get { return true; } }

        public CaptureNode(Capture capture, Int32 index)
          : base(capture.Value, new Segment(capture.Index, capture.Length))
        {
            Index = index;
        }

        CaptureNode(String text, Int32 index) : base(text, null)
        {
            Index = index;
        }

        public static IEnumerable<CaptureNode> GetSamples(Int32 count)
        {
            if (1 <= count)
            {
                yield return new CaptureNode("Capture # 1", 0);
            }

            if (2 <= count)
            {
                yield return new CaptureNode("Capture # 2", 1);
            }

            if (3 <= count)
            {
                yield return new CaptureNode("Capture # 3", 2);
            }

            if (4 <= count)
            {
                yield return new CaptureNode("", 3);
            }

            if (5 <= count)
            {
                yield return new CaptureNode("", 4);
            }

            if (6 <= count)
            {
                yield return new CaptureNode("", 5);
            }
        }
    }
}