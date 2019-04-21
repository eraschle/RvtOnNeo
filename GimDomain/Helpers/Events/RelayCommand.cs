namespace Gim.Domain.Helpers.Event
{
    using System;
    using System.Windows.Input;

    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> canExecutePredicate;
        private readonly Action<object> executeAction;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute is null)
            {
                throw new NullReferenceException(nameof(execute));
            }

            executeAction = execute;
            canExecutePredicate = canExecute;
        }

        public RelayCommand(Action<object> execute)
            : this(execute, null) { }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecutePredicate?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            executeAction.Invoke(parameter);
        }
    }
}