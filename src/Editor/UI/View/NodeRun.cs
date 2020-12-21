using System;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace Losenkov.RegexEditor.UI.View
{
    public enum NodeAspect
    {
        None = 0,
        NodeHeader = 1,
        NodeText = 2,
        TooltipHeader = 3,
        TooltipText = 4,
    }

    public enum NodeMode
    {
        None = 0,
        Empty = 1,
        Failure = 2,
    }

    internal class NodeRun : Run
    {
        static readonly DependencyPropertyKey s_modeKey = DependencyProperty.RegisterReadOnly(
            "Mode",
            typeof(NodeMode),
            typeof(NodeRun),
            new PropertyMetadata(NodeMode.None));
        public static readonly DependencyProperty ModeProperty = s_modeKey.DependencyProperty;

        public static readonly DependencyProperty NodeProperty = DependencyProperty.Register(
            "Node",
            typeof(ViewModel.Node),
            typeof(NodeRun),
            new UIPropertyMetadata(null, new PropertyChangedCallback(NodeChanged)));

        public static readonly DependencyProperty AspectProperty = DependencyProperty.Register(
            "Aspect",
            typeof(NodeAspect),
            typeof(NodeRun),
            new UIPropertyMetadata(NodeAspect.None, new PropertyChangedCallback(AspectChanged)));

        static String PrepareNodeText(String text)
        {
            const Int32 TrimLength = 80;

            if (String.IsNullOrEmpty(text))
            {
                return "--- EMPTY ---";
            }

            var builder = new StringBuilder(TrimLength + 2);
            builder.Append('"');
            if (TrimLength < text.Length)
            {
                builder.Append(text, 0, TrimLength - 3);
                builder.Append("...");
            }
            else
            {
                builder.Append(text);
            }
            builder.Append('"').Replace("\r\n", "\u00B6").Replace('\r', '\u00B6').Replace('\n', '\u00B6');
            return builder.ToString();
        }

        static String PrepareTooltipText(String text)
        {
            const Int32 TrimLength = 80;

            if (String.IsNullOrEmpty(text))
            {
                return "--- EMPTY ---";
            }

            var builder = new StringBuilder(TrimLength + 2);
            builder.Append('"');
            if (TrimLength < text.Length)
            {
                builder.Append(text, 0, TrimLength - 3);
                builder.Append("...");
            }
            else
            {
                builder.Append(text);
            }
            builder.Append('"').Replace("\r\n", "\u00B6").Replace('\r', '\u00B6').Replace('\n', '\u00B6');
            return builder.ToString();
        }

        void UpdateText(ViewModel.Node node, NodeAspect aspect)
        {
            switch (aspect)
            {
                case NodeAspect.NodeHeader:
                    if (node == null)
                    {
                        base.Text = "<<null>>";
                    }
                    else if (node is ViewModel.CaptureNode cn)
                    {
                        base.Text = String.Format("c[{0}]:", cn.Index);
                    }
                    else if (node is ViewModel.GroupNode gn)
                    {
                        base.Text = String.Format("g[{0} or \"{1}\"]:", gn.Number, gn.Name);
                    }
                    else if (node is ViewModel.MatchNode mn)
                    {
                        base.Text = String.Format("m[{0}]:", mn.Index);
                    }
                    else if (node is ViewModel.LineNode ln)
                    {
                        base.Text = String.Format("l[{0}]:", ln.Index);
                    }
                    break;
                case NodeAspect.NodeText:
                    if (node == null)
                    {
                        base.Text = "<<null>>";
                    }
                    else if (!node.Success)
                    {
                        base.Text = "--- FAILURE ---";
                    }
                    else if (String.IsNullOrEmpty(node.Text))
                    {
                        base.Text = "--- EMPTY ---";
                    }
                    else
                    {
                        base.Text = PrepareNodeText(node.Text);
                    }
                    break;
                case NodeAspect.TooltipHeader:
                    if (node == null)
                    {
                        base.Text = "<<null>>";
                    }
                    else if (node is ViewModel.CaptureNode cn)
                    {
                        base.Text = String.Format("group.Captures[{0}]", cn.Index);
                    }
                    else if (node is ViewModel.GroupNode gn)
                    {
                        base.Text = String.Format("match.Groups[{0}] or match.Groups[\"{1}\"]", gn.Number, gn.Name);
                    }
                    else if (node is ViewModel.MatchNode mn)
                    {
                        base.Text = String.Format("matches[{0}]", mn.Index);
                    }
                    else if (node is ViewModel.LineNode ln)
                    {
                        base.Text = String.Format("lines[{0}]", ln.Index);
                    }
                    break;
                case NodeAspect.TooltipText:
                    if (node == null)
                    {
                        base.Text = "<<null>>";
                    }
                    else if (String.IsNullOrEmpty(node.Text))
                    {
                        base.Text = "--- EMPTY ---";
                    }
                    else
                    {
                        base.Text = PrepareTooltipText(node.Text);
                    }
                    break;
            }
        }

        void NodeChanged(ViewModel.Node node)
        {
            if (node == null)
            {
                Mode = NodeMode.None;
            }
            else if (!node.Success)
            {
                Mode = NodeMode.Failure;
            }
            else if (String.IsNullOrEmpty(node.Text))
            {
                Mode = NodeMode.Empty;
            }
            else
            {
                Mode = NodeMode.None;
            }

            UpdateText(node, Aspect);
        }

        void AspectChanged(NodeAspect aspect)
        {
            UpdateText(Node, aspect);
        }

        static void NodeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((NodeRun)o).NodeChanged((ViewModel.Node)e.NewValue);
        }

        static void AspectChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((NodeRun)o).AspectChanged((NodeAspect)e.NewValue);
        }

        public NodeAspect Aspect
        {
            get { return (NodeAspect)base.GetValue(AspectProperty); }
            set { base.SetValue(AspectProperty, value); }
        }

        public ViewModel.Node Node
        {
            get { return (ViewModel.Node)base.GetValue(NodeProperty); }
            set { base.SetValue(NodeProperty, value); }
        }

        public NodeMode Mode
        {
            get { return (NodeMode)base.GetValue(ModeProperty); }
            private set { base.SetValue(s_modeKey, value); }
        }
    }
}