using System.Windows.Controls;

namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// DragableControlsDemo.xaml 的交互逻辑
    /// </summary>
    public partial class DragableControlsDemo : UserControl
    {
        public DragableControlsDemo()
        {
            InitializeComponent();
        }

        private void StackPositionMonitor_OnOrderChanged(object sender, CookPopularControl.Communal.Data.OrderChangedEventArgs e)
        {

        }
    }
}
