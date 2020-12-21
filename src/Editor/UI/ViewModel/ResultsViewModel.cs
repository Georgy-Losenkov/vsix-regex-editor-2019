using System;
using System.Collections;
using System.Linq;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    public sealed class ResultsViewModel : ViewModelBase
    {
        #region static members
        public static IEnumerable SampleGrid { get; }
        public static String SampleText { get; }
        public static IEnumerable SampleTree { get; }

        static ResultsViewModel()
        {
            //SampleGrid = new InputFragments(
            //  new InputFragment(null, String.Empty),
            //  new InputFragment(0, "Chunk #1"),
            //  new InputFragment(1, "Chunk #2"),
            //  new InputFragment(2, "Chunk #3"),
            //  new InputFragment(3, "Chunk #4"),
            //  new InputFragment(4, "Chunk #5"),
            //  new InputFragment(5, "Chunk #6"),
            //  new InputFragment(6, "Chunk #7"));
            SampleGrid = new LineFragments(
              new LineFragment(new Line("Line # 1 of text", 0, 10), 0, null, String.Empty),
              new LineFragment(new Line("Line # 1 of text", 0, 10), 0, null, String.Empty),
              new LineFragment(new Line("Line # 1 of text", 0, 10), 0, null, String.Empty),
              new LineFragment(new Line("Line # 2 of text", 0, 10), 1, 0, "Chunk #1"),
              new LineFragment(new Line("Line # 2 of text", 0, 10), 1, 1, "Chunk #2"),
              new LineFragment(new Line("Line # 2 of text", 0, 10), 1, 2, "Chunk #3"),
              new LineFragment(new Line("Line # 6 of text", 0, 10), 2, 0, "Replacement #6"),
              new LineFragment(new Line("Line # 7 of text", 0, 10), 3, 0, "Replacement #7"));
            // SampleGrid = new LineReplacements(
            //   new LineReplacement(new Line("Line # 1 of text", 0, 10), 0, "Replacement #1"),
            //   new LineReplacement(new Line("Line # 2 of text", 0, 10), 1, "Replacement #2"),
            //   new LineReplacement(new Line("Line # 3 of text", 0, 10), 2, "Replacement #3"),
            //   new LineReplacement(new Line("Line # 4 of text", 0, 10), 3, "Replacement #4"),
            //   new LineReplacement(new Line("Line # 5 of text", 0, 10), 4, "Replacement #5"),
            //   new LineReplacement(new Line("Line # 6 of text", 0, 10), 5, "Replacement #6"),
            //   new LineReplacement(new Line("Line # 7 of text", 0, 10), 6, "Replacement #7"));

            SampleText =
              "Line # 1 of text\r\n" +
              "Line # 2 of text\r\n" +
              "Line # 3 of text\r\n" +
              "Line # 4 of text\r\n" +
              "Line # 5 of text\r\n" +
              "Line # 6 of text\r\n";

            SampleTree = LineNode.GetSamples().ToArray();
        }
        #endregion

        private ExecutionState m_state;
        private Boolean m_gridVisible;
        private Boolean m_textVisible;
        private Boolean m_treeVisible;
        private IEnumerable m_grid;
        private String m_text;
        private IEnumerable m_tree;

        public ResultsViewModel()
        {
#if (DEBUG)
            m_state = ExecutionState.Truncated;
            m_gridVisible = true;
            m_textVisible = false;
            m_treeVisible = false;
            m_grid = LineFragment.Sample;
            m_text = String.Empty;
            m_tree = null;
#else
            m_state = ExecutionState.None;
            m_gridVisible = false;
            m_textVisible = false;
            m_treeVisible = false;
            m_grid = null;
            m_text = String.Empty;
            m_tree = null;
#endif
        }

        public ExecutionState State
        {
            get { return m_state; }
            private set
            {
                if (m_state != value)
                {
                    m_state = value;
                    RaisePropertyChanged(nameof(State));
                }
            }
        }
        #region "Visible" properties
        public Boolean GridVisible
        {
            get { return m_gridVisible; }
            private set
            {
                if (m_gridVisible != value)
                {
                    m_gridVisible = value;
                    RaisePropertyChanged(nameof(GridVisible));
                }
            }
        }
        public Boolean TextVisible
        {
            get { return m_textVisible; }
            private set
            {
                if (m_textVisible != value)
                {
                    m_textVisible = value;
                    RaisePropertyChanged(nameof(TextVisible));
                }
            }
        }
        public Boolean TreeVisible
        {
            get { return m_treeVisible; }
            private set
            {
                if (m_treeVisible != value)
                {
                    m_treeVisible = value;
                    RaisePropertyChanged(nameof(TreeVisible));
                }
            }
        }
        #endregion
        #region "value" properties
        public IEnumerable Grid
        {
            get { return m_grid; }
            private set
            {
                if (m_grid != value)
                {
                    m_grid = value;
                    RaisePropertyChanged(nameof(Grid));
                }
            }
        }
        public String Text
        {
            get { return m_text; }
            private set
            {
                if (m_text != value)
                {
                    m_text = value;
                    RaisePropertyChanged(nameof(Text));
                }
            }
        }
        public IEnumerable Tree
        {
            get { return m_tree; }
            private set
            {
                if (m_tree != value)
                {
                    m_tree = value;
                    RaisePropertyChanged(nameof(Tree));
                }
            }
        }
        #endregion

        public void Reset()
        {
            GridVisible = false;
            TextVisible = false;
            TreeVisible = false;

            Grid = null;
            Text = null;
            Tree = null;

            State = ExecutionState.None;
        }

        public void SetGrid(IEnumerable value, ExecutionState state)
        {
            Grid = value;
            GridVisible = true;

            State = state;
        }
        public void SetText(String value)
        {
            Text = value;
            TextVisible = true;

            State = ExecutionState.None;
        }
        public void SetTree(IEnumerable value, ExecutionState state)
        {
            Tree = value;
            TreeVisible = true;

            State = state;
        }
    }
}