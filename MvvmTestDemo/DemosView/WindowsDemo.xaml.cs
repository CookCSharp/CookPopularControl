using CookPopularControl.Controls.Windows.Printers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MvvmTestDemo.DemosView
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
