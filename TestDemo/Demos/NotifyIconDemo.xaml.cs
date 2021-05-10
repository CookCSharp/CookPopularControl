using CookPopularControl.Controls.CheckBox;
using CookPopularControl.Tools.Extensions;
using Hardcodet.Wpf.TaskbarNotification;
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

namespace TestDemo.Demos
{
    /// <summary>
    /// NotifyIconDemo.xaml 的交互逻辑
    /// </summary>
    public partial class NotifyIconDemo : UserControl
    {
        public NotifyIconDemo()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //var switchCtl = sender as SwitchControl;
            //if (switchCtl.IsChecked.Value)
            //    DefaultNotifyIcon.IsStartTaskbarFlash = true;
            //else
            //    DefaultNotifyIcon.IsStartTaskbarFlash = false;

            var switchCtl = sender as SwitchControl;
            if (switchCtl.IsChecked.Value)
                DefaultNotifyIcon.Visibility = Visibility.Visible;
            else
                DefaultNotifyIcon.Visibility = Visibility.Collapsed;

            (Window.GetWindow(this) as MainWindow).IsOpenNotifyIconSwitch = switchCtl.IsChecked.Value;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var switchCtl = sender as SwitchControl;
            if (switchCtl.IsChecked.Value)
                DefaultNotifyIcon.IsStartTaskbarIconFlash = true;
            else
                DefaultNotifyIcon.IsStartTaskbarIconFlash = false;
        }
    }
}
