using System.Windows;
using System.Windows.Controls;

namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// MenuDemo.xaml 的交互逻辑
    /// </summary>
    public partial class MenuDemo : UserControl
    {
        public MenuDemo()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b1 = circleMenu.IsShowMenu;
            var b2 = popupMenu.IsShowMenu;
        }
    }
}
