using System;

namespace Losenkov.RegexEditor.Colorer.Input
{
    class ObjectInfo
    {

    }

    sealed class MatchInfo : ObjectInfo
    {
        public Int32 Index { get; }

        public MatchInfo(Int32 index)
        {
            Index = index;
        }
    }

    sealed class GroupInfo : ObjectInfo
    {
        public MatchInfo Parent { get; }
        public Int32 Index { get; }
        public Int32 Number { get; }
        public String Name { get; }

        public GroupInfo(MatchInfo parent, Int32 index, Int32 number, String name)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Index = index;
            Number = number;
            Name = name;
        }
    }

    sealed class CaptureInfo : ObjectInfo
    {
        public GroupInfo Parent { get; }
        public Int32 Index { get; }

        public CaptureInfo(GroupInfo parent, Int32 index)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Index = index;
        }
    }
}