namespace Losenkov.RegexEditor.UI.View
{
  using System;
  using System.ComponentModel.Design;
  using System.Windows;
  using System.Windows.Controls;

  /// <summary>
  /// Interaction logic for QuickStartControl.xaml
  /// </summary>
  partial class QuickStartControl : UserControl
  {
    readonly IMenuCommandService service;

    public QuickStartControl(IMenuCommandService service)
    {
      this.service = service;

      this.InitializeComponent();
    }

    private void Hyperlink_Click(Object sender, RoutedEventArgs e)
    {
      if (this.service != null)
      {
        this.service.GlobalInvoke(new CommandID(new Guid("{6e7a508d-0c48-4703-8e50-8e8eba9508b6}"), 256));
      }
    }
  }
}