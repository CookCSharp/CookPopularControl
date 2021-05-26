using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ViewModelBase
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-27 19:23:10
 */
namespace CookPopularControl.Communal.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T item, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(item, value))
            {
                item = value;
                OnPropertyChanged(propertyName);
            }
        }
    }

    internal class DelegateCommand : ICommand
    {
        Action _executeMethod;
        Func<bool> _canExecuteMethod;

        public DelegateCommand(Action executeMethod) : this(executeMethod, () => true)
        {

        }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException($"{nameof(executeMethod)}or{nameof(canExecuteMethod)}");

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteMethod();
        }

        public void Execute(object parameter)
        {
            _executeMethod();
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Refresh() => CommandManager.InvalidateRequerySuggested();
    }

    internal class DelegateCommand<T> : ICommand
    {
        Action<T> _executeMethod;
        Func<T, bool> _canExecuteMethod;

        public DelegateCommand(Action<T> executeMethod) : this(executeMethod, t => true)
        {

        }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException($"{nameof(executeMethod)}or{nameof(canExecuteMethod)}");

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteMethod((T)parameter);
        }

        public void Execute(object parameter)
        {
            _executeMethod((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Refresh() => CommandManager.InvalidateRequerySuggested();
    }
}
