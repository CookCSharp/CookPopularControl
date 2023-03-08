/*
 * Description：SetMainWindowToTopCommand 
 * Author： Chance.Zheng
 * Create Time: 2023-03-08 14:43:51
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2023 All Rights Reserved.
 */


using CookPopularCSharpToolkit.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookPopularControl.Communal
{
    public class SetMainWindowToTopCommand : ICommand
    {
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (Application.Current.MainWindow != null && Application.Current.MainWindow.Visibility != Visibility.Visible)
            {
                Application.Current.MainWindow.Show();
                Application.Current.MainWindow.SafeActivate();
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
