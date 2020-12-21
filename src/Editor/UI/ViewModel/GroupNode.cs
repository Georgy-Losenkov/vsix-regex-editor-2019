using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    internal sealed class GroupNode : Node
    {
        public IEnumerable<CaptureNode> Captures { get; }
        public override Boolean Success { get; }
        public Int32 Index { get; }
        public Int32 Number { get; }
        public String Name { get; }

        public GroupNode(Group group, Int32 index, Int32 number, String name)
          : base(group.Value, new Segment(group.Index, group.Length))
        {
            Captures = new List<CaptureNode>();
            Success = group.Success;
            Index = index;
            Number = number;
            Name = name;
        }

        GroupNode(String text, Boolean success, Int32 number, String name, IEnumerable<CaptureNode> captures) : base(text, null)
        {
            Captures = new List<CaptureNode>();
            Success = success;
            Number = number;
            Name = name;

            if (captures != null)
            {
                ((List<CaptureNode>)Captures).AddRange(captures);
            }
        }

        public static IEnumerable<GroupNode> GetSamples(Int32 count, Int32 captureCount)
        {
            if (01 <= count)
            {
                yield return new GroupNode("Group # 1", true, 0, "0", CaptureNode.GetSamples(captureCount));
            }

            if (02 <= count)
            {
                yield return new GroupNode("Group # 2", true, 1, "1", CaptureNode.GetSamples(0));
            }

            if (03 <= count)
            {
                yield return new GroupNode("Group # 3", true, 2, "2", CaptureNode.GetSamples(0));
            }

            if (04 <= count)
            {
                yield return new GroupNode("Group # 4", false, 3, "3", CaptureNode.GetSamples(0));
            }

            if (05 <= count)
            {
                yield return new GroupNode("Group # 5", false, 4, "4", CaptureNode.GetSamples(0));
            }

            if (06 <= count)
            {
                yield return new GroupNode("Group # 6", false, 5, "5", CaptureNode.GetSamples(0));
            }

            if (07 <= count)
            {
                yield return new GroupNode("", true, 6, "6", CaptureNode.GetSamples(0));
            }

            if (08 <= count)
            {
                yield return new GroupNode("", true, 7, "7", CaptureNode.GetSamples(0));
            }

            if (09 <= count)
            {
                yield return new GroupNode("", true, 8, "8", CaptureNode.GetSamples(0));
            }

            if (10 <= count)
            {
                yield return new GroupNode("", false, 09, "9", CaptureNode.GetSamples(0));
            }

            if (11 <= count)
            {
                yield return new GroupNode("", false, 10, "10", CaptureNode.GetSamples(0));
            }

            if (12 <= count)
            {
                yield return new GroupNode("", false, 11, "11", CaptureNode.GetSamples(0));
            }
        }
    }
}