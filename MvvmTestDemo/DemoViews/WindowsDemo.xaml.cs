using CookPopularControl.Communal.Data;
using CookPopularControl.Controls;
using CookPopularControl.Controls.Windows.Printers;
using CookPopularCSharpToolkit.Windows;
using MvvmTestDemo.DemoDragables;
using MvvmTestDemo.Windows;
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
                "ToastMessage" => ShowToast(),
                _ => throw new NotImplementedException(),
            };
        }

        private object ShowToast()
        {
            CookPopularControl.Windows.ToastMessage.ShowWarning("123123123");
            return null;
        }



        private object Show<T>() where T : Window, new()
        {
            new T().Show();

            return default;
        }
    }
}
