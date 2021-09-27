using MvvmTestDemo.DemoViewModels;
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
