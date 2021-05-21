using CookPopularControl.Controls.CheckBox;
using CookPopularControl.Controls.Notify;
using CookPopularControl.Tools.Extensions;
using CookPopularControl.Tools.Helpers;
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
    public partial class NotifyDemo : UserControl
    {
        public NotifyDemo()
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

        private int bubbleMessageIndex = 1;
        private void OpenBubbleMessage_Click(object sender, RoutedEventArgs e)
        {
            new BubbleMessage().Show($"写代码的厨子_{bubbleMessageIndex++}");

            //var showPopupAnimation = AnimationHelper.CreateDoubleAnimation(0, 1, 1);
            //showPopupAnimation.Completed += ShowPopupAnimation_Completed;
            //(sender as Button).BeginAnimation(UIElement.OpacityProperty, showPopupAnimation);
        }
    }
}
