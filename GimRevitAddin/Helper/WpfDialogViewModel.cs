using Gim.Domain.Helpers.Event;
using System.Windows.Input;

namespace Gim.Revit.Addin.Helper
{
    public class WpfDialogViewModel : ANotifyPropertyChangedModel
    {
        private readonly IDialogContentViewModel contentModel;
        public WpfDialogViewModel(IDialogContentViewModel dialogContent)
        {
            contentModel = dialogContent;
        }

        public object CommandParameter
        {
            get { return contentModel.CommandParameter; }
            set
            {
                contentModel.CommandParameter = value;
                NotifyPropertyChanged();
            }
        }

        private ICommand okayCommand;
        public ICommand OkayCommand
        {
            get { return okayCommand; }
            set
            {
                okayCommand = value;
                NotifyPropertyChanged();
            }
        }

        private ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get { return cancelCommand; }
            set
            {
                cancelCommand = value;
                NotifyPropertyChanged();
            }
        }
    }
}
