using System.Windows.Controls;
using Gim.Revit.Addin.Docs.View;

namespace Gim.Revit.Addin.Journal.View
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CreateJournalView : UserControl
    {
        public CreateJournalView()
        {
            InitializeComponent();
        }

        public DocumentationViewModel DocumentationViewModel
        {
            get { return viewDocumentation.DataContext as DocumentationViewModel; }
        }
    }
}
