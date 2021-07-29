using System.Windows;
using System.Windows.Controls;

namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// Starred.xaml 的交互逻辑
    /// </summary>
    public partial class StarredDemo : UserControl
    {
        public StarredDemo()
        {
            InitializeComponent();
        }

        private void Star_StarValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
