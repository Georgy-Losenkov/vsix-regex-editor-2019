using System;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    public class InputFragment
    {
        public Int32? ChunkIndex { get; }
        public String Text { get; }

        public InputFragment(Int32? chunkIndex, String text)
        {
            ChunkIndex = chunkIndex;
            Text = text;
        }
    }
}