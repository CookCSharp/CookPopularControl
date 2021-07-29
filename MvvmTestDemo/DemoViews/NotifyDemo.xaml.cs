using CookPopularControl.Communal.Data.Enum;
using CookPopularControl.Controls.CheckBox;
using CookPopularControl.Controls.Notify;
using MvvmTestDemo.UserControls;
using System.Windows;
using System.Windows.Controls;

namespace MvvmTestDemo.DemoViews
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
        private const string token = null;
        private void OpenBubbleMessage_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            switch (btn.Name)
            {
                case "ShowInfoBubbleMessage":
                    BubbleMessage.ShowInfo($"写代码的厨子_{bubbleMessageIndex++}", token);
                    break;
                case "ShowWarningBubbleMessage":
                    BubbleMessage.ShowWarning($"写代码的厨子_{bubbleMessageIndex++}", token);
                    break;
                case "ShowErrorBubbleMessage":
                    BubbleMessage.ShowError($"写代码的厨子_{bubbleMessageIndex++}", token);
                    break;
                case "ShowFatalBubbleMessage":
                    BubbleMessage.ShowFatal($"写代码的厨子_{bubbleMessageIndex++}", token);
                    break;
                case "ShowQuestionBubbleMessage":
                    BubbleMessage.ShowQuestion($"写代码的厨子_{bubbleMessageIndex++}", isSure =>
                    {
                        //MessageDialog.ShowInfo($"Clicked the {isSure.ToString()}");
                        BubbleMessage.ShowInfo($"Clicked the {isSure}");
                        return true;
                    }, token);
                    break;
                case "ShowSuccessBubbleMessage":
                    BubbleMessage.ShowSuccess($"写代码的厨子_{bubbleMessageIndex++}", token);
                    break;
                default:
                    break;
            }
        }

        private void OpenNewWindow_Click(object sender, RoutedEventArgs e)
        {
            new NotifyDemoWindow().Show();
        }

        private void OpenPopupMessage_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            switch (btn.Name)
            {
                case "OpenPopupMessageNone":
                    PopupMessage.Show(new AnimationDemo(), PopupAnimationX.None);
                    break;
                case "OpenPopupMessageFade":
                    PopupMessage.Show(new AnimationDemo(), PopupAnimationX.Fade);
                    break;
                case "OpenPopupMessageHorizontalSlide":
                    PopupMessage.Show(new AnimationDemo(), PopupAnimationX.HorizontalSlide);
                    break;
                case "OpenPopupMessageVerticalSlide":
                    PopupMessage.Show(new AnimationDemo(), PopupAnimationX.VerticalSlide);
                    break;
                case "OpenPopupMessageHVSlide":
                    PopupMessage.Show(new AnimationDemo(), PopupAnimationX.HorizontalVerticalSlide);
                    break;
                case "OpenPopupMessageScroll":
                    PopupMessage.Show(new AnimationDemo(), PopupAnimationX.Scroll);
                    break;
                default:
                    break;
            }
        }
    }
}
