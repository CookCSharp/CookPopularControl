using System.Windows;
using System.Windows.Controls;

namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// ScrollViewerDemo.xaml 的交互逻辑
    /// </summary>
    public partial class ScrollViewerDemo : UserControl
    {
        public ScrollViewerDemo()
        {
            InitializeComponent();

            new Button().Click += ScrollViewerDemo_Click;
        }

        private void ScrollViewerDemo_Click(object sender, RoutedEventArgs e)
        {

            //(sender as Button).Command.Execute()
        }
    }
}
