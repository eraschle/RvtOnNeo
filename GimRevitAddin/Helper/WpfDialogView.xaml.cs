using System.Windows;
using System.Windows.Controls;

namespace Gim.Revit.Addin.Helper
{
    /// <summary>
    /// Interaction logic for DialogButtonView.xaml
    /// </summary>
    public partial class WpfDialogView : UserControl
    {
        public WpfDialogView(UserControl content)
        {
            InitializeComponent();
            SetContent(content);
        }

        private void SetContent(UserControl content)
        {
            content.HorizontalAlignment = HorizontalAlignment.Stretch;
            content.VerticalAlignment = VerticalAlignment.Stretch;
            DialogView.Children.Add(content);
        }
    }
}
