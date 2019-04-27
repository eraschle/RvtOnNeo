using Gim.Domain.Helpers.Event;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Gim.Revit.Addin.Helper
{
    public class WpfDialog : Window
    {
        private readonly IDialogContentViewModel dialogViewModel;
        public WpfDialog(UserControl view)
        {
            if (!(view.DataContext is IDialogContentViewModel viewModel))
            {
                var message = $"DataContext must implement {nameof(IDialogContentViewModel)}";
                throw new ArgumentException(message);
            }

            dialogViewModel = viewModel;
            var context = new WpfDialogViewModel(dialogViewModel)
            {
                OkayCommand = new RelayCommand(ExecuteOkay, dialogViewModel.CanExecute),
                CancelCommand = new RelayCommand(ExecuteCancel)
            };

            Content = new WpfDialogView(view) { DataContext = context };
            SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void ExecuteOkay(object parameter)
        {
            dialogViewModel.ExecuteOkay(parameter);
            DialogResult = true;
            Close();
        }

        private void ExecuteCancel(object parameter)
        {
            dialogViewModel.ExecuteCancel(parameter);
            DialogResult = false;
            Close();
        }
    }
}
