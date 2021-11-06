using CookPopularControl.Communal.Data.Args;
using CookPopularControl.Windows;
using MvvmTestDemo.Commumal;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MvvmTestDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class MainWindow : SideBarWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            //this.Closing += (s, e) =>
            //{
            //    if (!IsOpenNotifyIconSwitch)
            //        Environment.Exit(0);
            //};
        }
    }
}
