﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;

namespace WpfApp2.BaseCommands
{
    public class StatusInternetCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public StatusInternetCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
