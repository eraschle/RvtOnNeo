namespace Gim.Revit.Addin.Helper
{
    public interface IDialogContentViewModel
    {
        object CommandParameter { get; set; }
        bool CanExecute(object parameter);
        void ExecuteOkay(object parameter);
        void ExecuteCancel(object parameter);
    }
}
