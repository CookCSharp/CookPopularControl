using CookPopularControl.Windows;
using PropertyChanged;
using System;

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
        }
    }
}
