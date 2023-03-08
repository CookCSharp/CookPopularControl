/*
 * Description：CloseWindowCommand 
 * Author： Chance.Zheng
 * Create Time: 2023-03-08 14:53:02
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookPopularControl.Communal
{
    public class CloseWindowCommand : ICommand
    {
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (parameter is DependencyObject dependencyObject)
            {
                if (Window.GetWindow(dependencyObject) is { } window)
                {
                    window.Close();
                }
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
