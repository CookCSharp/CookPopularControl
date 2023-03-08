using CookPopularControl.Expression;
using CookPopularControl.Windows;
using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：PopupMessage
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-21 09:03:29
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 通知消息的弹出动画
    /// </summary>
    public enum PopupAnimationX
    {
        None,
        Fade,
        LeftHorizontalSlide,
        RightHorizontalSlide,
        TopVerticalSlide,
        BottomVerticalSlide,
        HorizontalVerticalSlide,
        Scroll,
    }

    /// <summary>
    /// 含有内容的弹出框
    /// </summary>
    public sealed class PopupMessage : Window
    {
        private const double thickness = 0D;//窗体外边距，为了使窗体阴影显示出来
        private DispatcherTimer _timerClose;
        private PopupAnimationX _animation;
        private static bool _isManualClose;
        private double _waitTime;
        private ScaleTransform _scale;


        static PopupMessage()
        {
            CommandManager.RegisterClassCommandBinding(typeof(PopupMessage), new CommandBinding(SystemCommands.CloseWindowCommand, (s, e) =>
            {
                _isManualClose = true;
                (s as PopupMessage).Close();
            }, (s, e) => e.CanExecute = true));
        }

        private PopupMessage() { }


        /// <summary>
        /// 打开通知消息，5s后自动关闭
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="position">弹出位置</param>
        /// <param name="animation">动画类型</param>
        /// <returns></returns>
        public static PopupMessage ShowClose(object content, PopupPosition position = PopupPosition.AllCenter, PopupAnimationX animation = PopupAnimationX.None)
        {
            return Show(null, content, position, animation, false);
        }

        /// <summary>
        /// 打开通知消息，保持不关闭
        /// </summary>
        /// <param name="content"></param>
        /// <param name="position">弹出位置</param>
        /// <param name="animation"></param>
        /// <returns></returns>
        public static PopupMessage ShowOpen(object content, PopupPosition position = PopupPosition.AllCenter, PopupAnimationX animation = PopupAnimationX.None)
        {
            return Show(null, content, position, animation, true);
        }

        /// <summary>
        /// 打开通知消息
        /// </summary>
        /// <param name="owner">所有者</param>
        /// <param name="content">内容</param>
        /// <param name="position">弹出位置</param>
        /// <param name="animation">动画类型</param>
        /// <param name="isStayOpen">是否一直打开消息</param>
        /// <param name="waitTime">自动关闭消息时保持打开的时间</param>
        /// <returns></returns>
        public static PopupMessage Show(Window owner, object content, PopupPosition position = PopupPosition.AllCenter, PopupAnimationX animation = PopupAnimationX.None, bool isStayOpen = false, double waitTime = 5)
        {
            VerifyHelper.IsNotNull(content, "content");

            var ownerWindow = owner is null ? WindowExtension.GetActiveWindow() : owner;
            var hasWindow = ownerWindow is null;

            _isManualClose = false;
            var popupMessage = new PopupMessage()
            {
                Owner = ownerWindow,
                Content = content,
                Opacity = 0,
                _animation = animation,
                _waitTime = waitTime,
                Topmost = hasWindow,
            };
            popupMessage.Show();

            var workingArea = SystemParameters.WorkArea;
            var leftTop = popupMessage.GetLeftTop(popupMessage, position);
            var left = leftTop.Item1;
            var top = leftTop.Item2;

            switch (animation)
            {
                case PopupAnimationX.None:
                    popupMessage.Opacity = 1;
                    popupMessage.Left = left;
                    popupMessage.Top = top;
                    break;
                case PopupAnimationX.Fade:
                    popupMessage.Left = left;
                    popupMessage.Top = top;
                    popupMessage.BeginAnimation(OpacityProperty, AnimationHelper.CreateDoubleAnimation(1));
                    break;
                case PopupAnimationX.LeftHorizontalSlide:
                    popupMessage.Opacity = 1;
                    popupMessage.Left = -popupMessage.ActualWidth;
                    popupMessage.Top = top;
                    popupMessage.BeginAnimation(LeftProperty, AnimationHelper.CreateDoubleAnimation(left));
                    break;
                case PopupAnimationX.RightHorizontalSlide:
                    popupMessage.Opacity = 1;
                    popupMessage.Left = workingArea.Width;
                    popupMessage.Top = top;
                    popupMessage.BeginAnimation(LeftProperty, AnimationHelper.CreateDoubleAnimation(left));
                    break;
                case PopupAnimationX.TopVerticalSlide:
                    popupMessage.Opacity = 1;
                    popupMessage.Left = left;
                    popupMessage.Top = -popupMessage.ActualHeight;
                    popupMessage.BeginAnimation(TopProperty, AnimationHelper.CreateDoubleAnimation(top));
                    break;
                case PopupAnimationX.BottomVerticalSlide:
                    popupMessage.Opacity = 1;
                    popupMessage.Left = left;
                    popupMessage.Top = workingArea.Height;
                    popupMessage.BeginAnimation(TopProperty, AnimationHelper.CreateDoubleAnimation(top));
                    break;
                case PopupAnimationX.HorizontalVerticalSlide:
                    popupMessage.Opacity = 1;
                    popupMessage.Left = workingArea.Width;
                    popupMessage.Top = workingArea.Height;
                    popupMessage.BeginAnimation(LeftProperty, AnimationHelper.CreateDoubleAnimation(left));
                    popupMessage.BeginAnimation(TopProperty, AnimationHelper.CreateDoubleAnimation(top));
                    break;
                case PopupAnimationX.Scroll:
                    popupMessage.Opacity = 1;
                    popupMessage.Left = left;
                    popupMessage.Top = top;
                    popupMessage.RenderTransformOrigin = new Point(0.5, 0.5);
                    popupMessage._scale = new ScaleTransform();
                    popupMessage.RenderTransform = popupMessage._scale;
                    popupMessage._scale.ScaleX = 0;
                    popupMessage._scale.ScaleY = 0;
                    popupMessage._scale.BeginAnimation(ScaleTransform.ScaleXProperty, AnimationHelper.CreateDoubleAnimation(1));
                    popupMessage._scale.BeginAnimation(ScaleTransform.ScaleYProperty, AnimationHelper.CreateDoubleAnimation(1));
                    break;
                default:
                    popupMessage.Opacity = 1;
                    popupMessage.Left = left;
                    popupMessage.Top = top;
                    break;
            }

            if (!isStayOpen)
                popupMessage.StartTimer();

            return popupMessage;
        }

        private Tuple<double, double> GetLeftTop(Window popupMessage, PopupPosition position)
        {
            double left = 0, top = 0;
            var workingArea = SystemParameters.WorkArea;
            switch (position)
            {
                case PopupPosition.LeftTop:
                    left = 0;
                    top = 0;
                    break;
                case PopupPosition.CenterTop:
                    left = (workingArea.Width - popupMessage.ActualWidth) / 2D;
                    top = 0;
                    break;
                case PopupPosition.RightTop:
                    left = workingArea.Width - popupMessage.ActualWidth;
                    top = 0;
                    break;
                case PopupPosition.LeftCenter:
                    left = 0;
                    top = (workingArea.Height - popupMessage.ActualHeight) / 2D;
                    break;
                case PopupPosition.AllCenter:
                    left = (workingArea.Width - popupMessage.ActualWidth) / 2D;
                    top = (workingArea.Height - popupMessage.ActualHeight) / 2D;
                    break;
                case PopupPosition.RightCenter:
                    left = workingArea.Width - popupMessage.ActualWidth;
                    top = (workingArea.Height - popupMessage.ActualHeight) / 2D;
                    break;
                case PopupPosition.LeftBottom:
                    left = 0;
                    top = workingArea.Height - popupMessage.ActualHeight;
                    break;
                case PopupPosition.CenterBottom:
                    left = (workingArea.Width - popupMessage.ActualWidth) / 2D;
                    top = workingArea.Height - popupMessage.ActualHeight;
                    break;
                case PopupPosition.RightBottom:
                    left = workingArea.Width - popupMessage.ActualWidth;
                    top = workingArea.Height - popupMessage.ActualHeight;
                    break;
                default:
                    break;
            }

            return new Tuple<double, double>(left, top);
        }

        private void StartTimer()
        {
            _timerClose = new DispatcherTimer(DispatcherPriority.Normal, Dispatcher);
            _timerClose.Interval = TimeSpan.FromSeconds(_waitTime);
            _timerClose.Tick += (s, e) =>
            {
                _timerClose.Stop();
                _timerClose = null;
                this.Close();
            };
            _timerClose.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (_isManualClose)
                return;

            var workingArea = SystemParameters.WorkArea;

            switch (_animation)
            {
                case PopupAnimationX.None:
                    break;
                case PopupAnimationX.Fade:
                    {
                        var animation = AnimationHelper.CreateDoubleAnimation(0);
                        animation.Completed += Animation_Completed;
                        BeginAnimation(OpacityProperty, animation);
                        e.Cancel = true;
                    }
                    break;
                case PopupAnimationX.LeftHorizontalSlide:
                    {
                        var animation = AnimationHelper.CreateDoubleAnimation(0);
                        animation.Completed += Animation_Completed;
                        BeginAnimation(LeftProperty, animation);
                        e.Cancel = true;
                    }
                    break;
                case PopupAnimationX.RightHorizontalSlide:
                    {
                        var animation = AnimationHelper.CreateDoubleAnimation(workingArea.Width);
                        animation.Completed += Animation_Completed;
                        BeginAnimation(LeftProperty, animation);
                        e.Cancel = true;
                    }
                    break;
                case PopupAnimationX.TopVerticalSlide:
                    {
                        var animation = AnimationHelper.CreateDoubleAnimation(0);
                        animation.Completed += Animation_Completed;
                        BeginAnimation(TopProperty, animation);
                        e.Cancel = true;
                    }
                    break;
                case PopupAnimationX.BottomVerticalSlide:
                    {
                        var animation = AnimationHelper.CreateDoubleAnimation(workingArea.Height);
                        animation.Completed += Animation_Completed;
                        BeginAnimation(TopProperty, animation);
                        e.Cancel = true;
                    }
                    break;
                case PopupAnimationX.HorizontalVerticalSlide:
                    {
                        var animation1 = AnimationHelper.CreateDoubleAnimation(workingArea.Width);
                        var animation2 = AnimationHelper.CreateDoubleAnimation(workingArea.Width);
                        animation1.Completed += Animation_Completed;
                        animation2.Completed += Animation_Completed;
                        BeginAnimation(LeftProperty, animation1);
                        BeginAnimation(TopProperty, animation2);
                        e.Cancel = true;
                    }
                    break;
                case PopupAnimationX.Scroll:
                    {
                        var animationX = AnimationHelper.CreateDoubleAnimation(0);
                        var animationY = AnimationHelper.CreateDoubleAnimation(0);
                        animationX.Completed += Animation_Completed;
                        animationY.Completed += Animation_Completed;
                        _scale.BeginAnimation(ScaleTransform.ScaleXProperty, animationX);
                        _scale.BeginAnimation(ScaleTransform.ScaleYProperty, animationY);
                        e.Cancel = true;
                    }
                    break;
                default:
                    break;
            }

            _isManualClose = true;
        }

        private void Animation_Completed(object? sender, EventArgs e) => Close();
    }
}
