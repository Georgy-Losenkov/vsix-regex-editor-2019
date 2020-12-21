using System;
using System.Windows;
using System.Windows.Controls;

namespace Losenkov.RegexEditor.UI.View
{
    public partial class TreeControl : UserControl
    {
        public TreeControl()
        {
            InitializeComponent();
        }

        #region context menu
        static void CollapseTreeViewItems(ItemCollection items, ItemContainerGenerator generator)
        {
            foreach (var item in items)
            {
                if (generator.ContainerFromItem(item) is TreeViewItem container)
                {
                    container.IsExpanded = false;

                    CollapseTreeViewItems(container.Items, generator);
                }
            }
        }

        static void ExpandTreeViewItems(ItemCollection items, ItemContainerGenerator generator)
        {
            foreach (var item in items)
            {
                if (generator.ContainerFromItem(item) is TreeViewItem container)
                {
                    container.IsExpanded = true;

                    ExpandTreeViewItems(container.Items, generator);
                }
            }
        }

        void CollapseAll_Click(Object sender, RoutedEventArgs e)
        {
            CollapseTreeViewItems(this.treeView.Items, this.treeView.ItemContainerGenerator);
        }

        void ExpandAll_Click(Object sender, RoutedEventArgs e)
        {
            ExpandTreeViewItems(this.treeView.Items, this.treeView.ItemContainerGenerator);
        }
        #endregion
    }
}