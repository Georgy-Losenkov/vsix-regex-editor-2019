using System;
using System.Collections.Generic;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    internal sealed class LineNode : Node
    {
        public IEnumerable<MatchNode> Matches { get; }
        public Int32 Index { get; }
        public override Boolean Success { get { return true; } }

        public LineNode(Line line, Int32 index) : base(line.Value, new Segment(line.Index, line.Length))
        {
            Matches = new List<MatchNode>();
            Index = index;
        }

        LineNode(String text, Int32 index, IEnumerable<MatchNode> matches) : base(text, null)
        {
            Matches = new List<MatchNode>();
            Index = index;

            if (matches != null)
            {
                ((List<MatchNode>)Matches).AddRange(matches);
            }
        }

        public static IEnumerable<LineNode> GetSamples()
        {
            yield return new LineNode("Line # 1", 0, MatchNode.GetSamples(12, 12, 6));
            yield return new LineNode("Line # 2", 1, MatchNode.GetSamples(0, 0, 0));
            yield return new LineNode("Line # 3", 2, MatchNode.GetSamples(0, 0, 0));
            yield return new LineNode("", 3, MatchNode.GetSamples(0, 0, 0));
            yield return new LineNode("", 4, MatchNode.GetSamples(0, 0, 0));
            yield return new LineNode("", 5, MatchNode.GetSamples(0, 0, 0));
        }
    }
}