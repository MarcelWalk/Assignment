using System;
using System.Windows.Input;

namespace Assignment.Model
{
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new NullReferenceException(Constants.ERR_VAL_NULL);
            _canExecute = canExecute;
        }

        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter = null)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter = null)
        {
            _execute.Invoke(parameter);
        }
    }
}