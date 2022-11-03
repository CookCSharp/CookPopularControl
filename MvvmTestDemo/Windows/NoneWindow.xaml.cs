using CookPopularControl.Windows;
using System.Windows;
using System.Windows.Controls;

namespace MvvmTestDemo.Windows
{
    /// <summary>
    /// NoneTitleBarWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NoneWindow : NoneTitleBarWindow
    {
        public NoneWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var bc = win.Template.FindName("RootBorder", win) as Border;
            var ss = bc.Background;
        }
    }
}
