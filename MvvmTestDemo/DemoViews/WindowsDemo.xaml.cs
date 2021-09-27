using MvvmTestDemo.Windows;
using CookPopularControl.Controls.Windows.Printers;
using MvvmTestDemo.DemoDragables;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// WindowsDemo.xaml 的交互逻辑
    /// </summary>
    public partial class WindowsDemo : UserControl
    {
        public WindowsDemo()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            object p = btn.Name switch
            {
                "PrintPreviewWindow" => Show<PrintPreviewWindow>(),
                "DragableWindows" => Show<QuickStartWindow>(),
                "NoneTitleBarWindow" => Show<NoneWindow>(),
                _ => throw new NotImplementedException(),
            };
        }

        private object Show<T>() where T : Window, new()
        {
            new T().Show();

            return default;
        }
    }
}
