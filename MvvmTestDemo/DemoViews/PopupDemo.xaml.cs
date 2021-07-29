using System.Windows;
using System.Windows.Controls;

namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// PopupDemo.xaml 的交互逻辑
    /// </summary>
    public partial class PopupDemo : UserControl
    {
        public PopupDemo()
        {
            InitializeComponent();

            this.Loaded += PopupDemo_Loaded;
        }

        private void PopupDemo_Loaded(object sender, RoutedEventArgs e)
        {
            ExtendPop.IsOpen = true;
            ExtendPop.StaysOpen = true;
        }
    }
}
