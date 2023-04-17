using CookPopularControl.Controls;
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
            //var switchCtl = sender as SwitchButton;
            //if (switchCtl.IsChecked.Value)
            //    DefaultNotifyIcon.IsStartTaskbarFlash = true;
            //else
            //    DefaultNotifyIcon.IsStartTaskbarFlash = false;

            var switchCtl = sender as SwitchButton;
            if (switchCtl.IsChecked!.Value)
                DefaultNotifyIcon.Visibility = Visibility.Visible;
            else
                DefaultNotifyIcon.Visibility = Visibility.Collapsed;

            //(Window.GetWindow(this) as MainWindow).IsOpenNotifyIconSwitch = switchCtl.IsChecked.Value;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var switchCtl = sender as SwitchButton;
            if (switchCtl.IsChecked!.Value)
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
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftTop, PopupAnimationX.None);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftCenter, PopupAnimationX.None); 
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftBottom, PopupAnimationX.None); 
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.CenterTop, PopupAnimationX.None); 
                    PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.AllCenter, PopupAnimationX.None); 
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.CenterBottom, PopupAnimationX.None);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightTop, PopupAnimationX.None);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightCenter, PopupAnimationX.None);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightBottom, PopupAnimationX.None);
                    break;
                case "OpenPopupMessageFade":
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftTop, PopupAnimationX.Fade);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftCenter, PopupAnimationX.Fade);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftBottom, PopupAnimationX.Fade);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.CenterTop, PopupAnimationX.Fade);
                    PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.AllCenter, PopupAnimationX.Fade);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.CenterBottom, PopupAnimationX.Fade);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightTop, PopupAnimationX.Fade);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightCenter, PopupAnimationX.Fade);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightBottom, PopupAnimationX.Fade);
                    break;
                case "OpenPopupMessageLeftHorizontalSlide":
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftTop, PopupAnimationX.LeftHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftCenter, PopupAnimationX.LeftHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftBottom, PopupAnimationX.LeftHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.CenterTop, PopupAnimationX.LeftHorizontalSlide);
                    PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.AllCenter, PopupAnimationX.LeftHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.CenterBottom, PopupAnimationX.LeftHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightTop, PopupAnimationX.LeftHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightCenter, PopupAnimationX.LeftHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightBottom, PopupAnimationX.LeftHorizontalSlide);
                    break;
                case "OpenPopupMessageRightHorizontalSlide":
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftTop, PopupAnimationX.RightHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftCenter, PopupAnimationX.RightHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftBottom, PopupAnimationX.RightHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.CenterTop, PopupAnimationX.RightHorizontalSlide);
                    PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.AllCenter, PopupAnimationX.RightHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.CenterBottom, PopupAnimationX.RightHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightTop, PopupAnimationX.RightHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightCenter, PopupAnimationX.RightHorizontalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightBottom, PopupAnimationX.RightHorizontalSlide);
                    break;
                case "OpenPopupMessageTopVerticalSlide":
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftTop, PopupAnimationX.TopVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftCenter, PopupAnimationX.TopVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftBottom, PopupAnimationX.TopVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.CenterTop, PopupAnimationX.TopVerticalSlide);
                    PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.AllCenter, PopupAnimationX.TopVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.CenterBottom, PopupAnimationX.TopVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightTop, PopupAnimationX.TopVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightCenter, PopupAnimationX.TopVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightBottom, PopupAnimationX.TopVerticalSlide);
                    break;
                case "OpenPopupMessageBottomVerticalSlide":
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftTop, PopupAnimationX.BottomVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftCenter, PopupAnimationX.BottomVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftBottom, PopupAnimationX.BottomVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.CenterTop, PopupAnimationX.BottomVerticalSlide);
                    PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.AllCenter, PopupAnimationX.BottomVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.CenterBottom, PopupAnimationX.BottomVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightTop, PopupAnimationX.BottomVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightCenter, PopupAnimationX.BottomVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightBottom, PopupAnimationX.BottomVerticalSlide);
                    break;
                case "OpenPopupMessageHVSlide":
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftTop, PopupAnimationX.HorizontalVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftCenter, PopupAnimationX.HorizontalVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftBottom, PopupAnimationX.HorizontalVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.CenterTop, PopupAnimationX.HorizontalVerticalSlide);
                    PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.AllCenter, PopupAnimationX.HorizontalVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.CenterBottom, PopupAnimationX.HorizontalVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightTop, PopupAnimationX.HorizontalVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightCenter, PopupAnimationX.HorizontalVerticalSlide);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightBottom, PopupAnimationX.HorizontalVerticalSlide);
                    break;
                case "OpenPopupMessageScroll":
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftTop, PopupAnimationX.Scroll);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftCenter, PopupAnimationX.Scroll);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.LeftBottom, PopupAnimationX.Scroll);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.CenterTop, PopupAnimationX.Scroll);
                    PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.AllCenter, PopupAnimationX.Scroll);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.CenterBottom, PopupAnimationX.Scroll);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightTop, PopupAnimationX.Scroll);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightCenter, PopupAnimationX.Scroll);
                    //PopupMessage.ShowOpen(new AnimationDemo(), PopupPosition.RightBottom, PopupAnimationX.Scroll);
                    break;
                default:
                    break;
            }
        }
    }
}
