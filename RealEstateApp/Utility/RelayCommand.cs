using System;
using System.Windows.Input;

namespace RealEstateApp.Utility
{
    class RelayCommand<T> : ICommand
    {
        readonly Action<T> _execute;
        readonly Func<bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new NullReferenceException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action<T> execute) : this(execute, null) { }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
            => _canExecute == null ? true : _canExecute();

        public void Execute(object parameter)
            => _execute.Invoke((T)parameter);
    }
}
