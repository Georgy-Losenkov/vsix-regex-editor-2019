using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    internal sealed class MatchNode : Node
    {
        public IEnumerable<GroupNode> Groups { get; }
        public override Boolean Success { get; }
        public Int32 Index { get; }

        public MatchNode(Match match, Int32 index) : base(match.Value, new Segment(match.Index, match.Length))
        {
            Groups = new List<GroupNode>();
            Success = match.Success;
            Index = index;
        }

        MatchNode(String text, Boolean success, Int32 index, IEnumerable<GroupNode> groups) : base(text, null)
        {
            Groups = new List<GroupNode>();
            Success = success;
            Index = index;

            if (groups != null)
            {
                ((List<GroupNode>)Groups).AddRange(groups);
            }
        }

        public static IEnumerable<MatchNode> GetSamples(Int32 count, Int32 groupCount, Int32 captureCount)
        {
            if (01 <= count)
            {
                yield return new MatchNode("Match # 1", true, 0, GroupNode.GetSamples(groupCount, captureCount));
            }

            if (02 <= count)
            {
                yield return new MatchNode("Match # 2", true, 1, GroupNode.GetSamples(0, 0));
            }

            if (03 <= count)
            {
                yield return new MatchNode("Match # 3", true, 2, GroupNode.GetSamples(0, 0));
            }

            if (04 <= count)
            {
                yield return new MatchNode("Match # 4", false, 3, GroupNode.GetSamples(0, 0));
            }

            if (05 <= count)
            {
                yield return new MatchNode("Match # 5", false, 4, GroupNode.GetSamples(0, 0));
            }

            if (06 <= count)
            {
                yield return new MatchNode("Match # 6", false, 5, GroupNode.GetSamples(0, 0));
            }

            if (07 <= count)
            {
                yield return new MatchNode("", true, 6, GroupNode.GetSamples(0, 0));
            }

            if (08 <= count)
            {
                yield return new MatchNode("", true, 7, GroupNode.GetSamples(0, 0));
            }

            if (09 <= count)
            {
                yield return new MatchNode("", true, 8, GroupNode.GetSamples(0, 0));
            }

            if (10 <= count)
            {
                yield return new MatchNode("", false, 09, GroupNode.GetSamples(0, 0));
            }

            if (11 <= count)
            {
                yield return new MatchNode("", false, 10, GroupNode.GetSamples(0, 0));
            }

            if (12 <= count)
            {
                yield return new MatchNode("", false, 11, GroupNode.GetSamples(0, 0));
            }
        }
    }
}