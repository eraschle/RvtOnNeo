namespace Gim.Revit.Addin.Journal.View
{
    using System.Windows.Controls;

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
