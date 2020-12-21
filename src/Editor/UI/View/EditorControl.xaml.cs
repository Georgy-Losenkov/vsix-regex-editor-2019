using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using RegexOptions = System.Text.RegularExpressions.RegexOptions;

namespace Losenkov.RegexEditor.UI
{
    public partial class EditorControl : UserControl, IDisposable
    {
        readonly ViewModel.EditorViewModel m_viewModel;
        readonly EditorWrapper m_editorPattern;
        readonly EditorWrapper m_editorReplacement;
        readonly EditorWrapper m_editorInput;
        System.Timers.Timer m_timer;

        public EditorControl()
        {
            InitializeComponent();

            DataContext = m_viewModel = new ViewModel.EditorViewModel();

            m_editorPattern = new EditorWrapper(RegexPatternContentType.ContentTypeName);
            m_editorReplacement = new EditorWrapper(RegexReplacementContentType.ContentTypeName);
            m_editorInput = new EditorWrapper(RegexInputContentType.ContentTypeName);

            phPattern.Content = m_editorPattern.WpfTextViewHost.HostControl;
            phReplacement.Content = m_editorReplacement.WpfTextViewHost.HostControl;
            phInput.Content = m_editorInput.WpfTextViewHost.HostControl;

#if (DEBUG)
            m_editorPattern.Text = @"(\w+)(?:\W+(\w+)(?:\W+(\w+))?)?";
            m_editorReplacement.Text = @"$3 $2 $1";
            m_editorInput.Text =
              "Lorem ipsum dolor sit amet, consectetuer adipiscing elit.\r\n" +
              "Aenean commodo ligula eget dolor.\r\n" +
              "Aenean massa.\r\n" +
              "Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus\r\n" +
              "\r\n" +
              "Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem.\r\n" +
              "Nulla consequat massa quis enim.\r\n" +
              "Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu.\r\n" +
              "\r\n" +
              "In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo.\r\n" +
              "Nullam dictum felis eu pede mollis pretium.\r\n" +
              "Integer tincidunt.\r\n" +
              "Cras dapibus.\r\n" +
              "Vivamus elementum semper nisi.\r\n" +
              "Aenean vulputate eleifend tellus.\r\n" +
              "\r\n" +
              "Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim.\r\n" +
              "Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus.\r\n" +
              "Phasellus viverra nulla ut metus varius laoreet.\r\n" +
              "Quisque rutrum.\r\n" +
              "\r\n" +
              "Aenean imperdiet.\r\n" +
              "Etiam ultricies nisi vel augue.\r\n" +
              "Curabitur ullamcorper ultricies nisi.\r\n" +
              "Nam eget dui.\r\n" +
              "Etiam rhoncus.\r\n" +
              "Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum.\r\n" +
              "\r\n" +
              "Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem.\r\n" +
              "Maecenas nec odio et ante tincidunt tempus.\r\n" +
              "Donec vitae sapien ut libero venenatis faucibus.\r\n" +
              "Nullam quis ante.\r\n" +
              "Etiam sit amet orci eget eros faucibus tincidunt.\r\n" +
              "Duis leo.\r\n" +
              "Sed fringilla mauris sit amet nibh.\r\n" +
              "Donec sodales sagittis magna.\r\n" +
              "Sed consequat, leo eget bibendum sodales, augue velit cursus nunc.";
#endif

            PrepareSplitters();
        }

        #region IOleCommandTarget
        internal IVsTextView GetActiveView()
        {
            if (m_editorPattern != null && m_editorPattern.WpfTextViewHost.HostControl.IsKeyboardFocusWithin)
            {
                return m_editorPattern.VsTextView;
            }
            if (m_editorReplacement != null && m_editorReplacement.WpfTextViewHost.HostControl.IsKeyboardFocusWithin)
            {
                return m_editorReplacement.VsTextView;
            }
            if (m_editorInput != null && m_editorInput.WpfTextViewHost.HostControl.IsKeyboardFocusWithin)
            {
                return m_editorInput.VsTextView;
            }
            return null;
        }
        #endregion

        #region Run button click handler
        class OptionsSnapshot : ViewModel.IOptions
        {
            public String PatternText { get; }
            public RegexOptions Options { get; }
            public String ReplacementText { get; }
            public String InputText { get; }
            public Boolean MultilineInput { get; }

            public OptionsSnapshot(EditorControl editor)
            {
                PatternText = editor.m_editorPattern.Text;
                Options = editor.m_viewModel.RegexOptions;
                ReplacementText = editor.m_editorReplacement.Text;
                InputText = editor.m_editorInput.Text;
                MultilineInput = editor.m_viewModel.MultilineInput;
            }
        }

        public Model.RegexMethod RegexMethod
        {
            get { return m_viewModel.RegexMethod; }
            set { m_viewModel.RegexMethod = value; }
        }

        public Model.TesterMode TesterMode
        {
            get { return m_viewModel.TesterMode; }
            set { m_viewModel.TesterMode = value; }
        }

        public void RunRegex()
        {
            ViewModel.RegexRunner.Execute(
              snapshot: new OptionsSnapshot(this),
              results: m_viewModel.Results,
              regexMethod: m_viewModel.RegexMethod,
              testerMode: m_viewModel.TesterMode);
            m_viewModel.ResultsVisible = true;

            if (Colorer.Input.RegexTaggerProvider.TryGetTaggerForBuffer(m_editorInput.WpfTextViewHost.TextView.TextBuffer, out var regexTagger))
            {
                var tree = m_viewModel.Results.Tree;
                IEnumerable<ViewModel.MatchNode> matches = null;
                if (tree is IEnumerable<ViewModel.LineNode> lineNodes)
                {
                    matches = lineNodes.SelectMany(ln => ln.Matches);
                }
                else if (tree is IEnumerable<ViewModel.MatchNode> matchNodes)
                {
                    matches = matchNodes;
                }

                regexTagger.CreateTags(matches);
            }

            if (Colorer.Input.HighlightTaggerProvider.TryGetTaggerForView(m_editorInput.WpfTextViewHost.TextView, out var highlightTagger))
            {
                highlightTagger.PinSnapshot();
            }
        }
        #endregion

        #region TreeView support
        void HighlightInputSegment(ViewModel.Segment segment)
        {
            if (segment == null)
            {
                if (Colorer.Input.HighlightTaggerProvider.TryGetTaggerForView(m_editorInput.WpfTextViewHost.TextView, out var tagger))
                {
                    tagger.Unhighlight();
                }
            }
            else
            {
                m_editorInput.Scroll(segment.Start);

                if (Colorer.Input.HighlightTaggerProvider.TryGetTaggerForView(m_editorInput.WpfTextViewHost.TextView, out var tagger))
                {
                    tagger.Hightlight(segment.Start, segment.Length);
                }
            }
        }

        void TreeView_SelectedItemChanged(Object sender, RoutedPropertyChangedEventArgs<Object> e)
        {
            if (e == null)
            {
                return;
            }

            ViewModel.Segment segment = null;
            if (e.NewValue is ViewModel.Node node && node.Success)
            {
                segment = node.Segment;
            }

            HighlightInputSegment(segment);

            e.Handled = true;
        }
        #endregion

        #region DataGrid support
        void DataGrid_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            if (0 < e.AddedItems.Count)
            {
                var item = e.AddedItems[0];

                ViewModel.Segment segment = null;

                if (item is ViewModel.LineFragment fragment)
                {
                    segment = fragment.Segment;
                }

                if (item is ViewModel.LineReplacement replacement)
                {
                    segment = replacement.Segment;
                }

                HighlightInputSegment(segment);
            }

            e.Handled = true;
        }
        #endregion

        #region dummy splitter support
        void PrepareSplitters()
        {
            LayoutUpdated += This_LayoutUpdated;

            m_timer = new System.Timers.Timer(100) { AutoReset = true };
            m_timer.Elapsed += Timer_Elapsed;
            m_timer.Enabled = true;
        }

        Boolean m_rowsUpdated = false;
        void UpdateRows()
        {
            if (m_rowsUpdated)
            {
                return;
            }

            var htPattern = rdPattern.ActualHeight;
            var htReplacement = rdReplacement.ActualHeight;
            var htInputResults = rdInputResults.ActualHeight;

            rdPattern.Height = new GridLength(htPattern, GridUnitType.Star);
            rdReplacement.Height = new GridLength(htReplacement, GridUnitType.Star);
            rdInputResults.Height = new GridLength(htInputResults, GridUnitType.Star);

            m_rowsUpdated = true;
        }

        DateTime m_layoutLastUpdated = DateTime.MinValue;
        void Timer_Elapsed(Object sender, System.Timers.ElapsedEventArgs e)
        {
            if (m_layoutLastUpdated < DateTime.Now.AddMilliseconds(-500))
            {
                ThreadHelper.JoinableTaskFactory.Run(async delegate
                {
                    // Switch to main thread  
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    UpdateRows();
                });
                m_timer.Stop();
                LayoutUpdated -= This_LayoutUpdated;
            }
        }

        void This_LayoutUpdated(Object sender, EventArgs e)
        {
            m_layoutLastUpdated = DateTime.Now;
        }

        #region IDisposable Support
        Boolean m_disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(Boolean disposing)
        {
            if (!m_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (m_timer != null)
                    {
                        m_timer.Stop();
                        m_timer.Elapsed -= Timer_Elapsed;
                        m_timer.Dispose();
                        m_timer = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                m_disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~EditorControl() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
        #endregion

        #region IDisposable Support
        #endregion
    }
}