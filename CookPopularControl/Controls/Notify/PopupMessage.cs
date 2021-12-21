using CookPopularControl.Communal.Data;
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
    /// 弹出消息
    /// </summary>
    public sealed class PopupMessage : Window
    {
        private const double thickness = 5D;//窗体外边距，为了使窗体阴影显示出来
        private PopupAnimationX _animation;
        private DispatcherTimer _timerClose;
        private double _waitTime;
        private ScaleTransform _scale;

        static PopupMessage()
        {
            CommandManager.RegisterClassCommandBinding(typeof(PopupMessage), new CommandBinding(SystemCommands.CloseWindowCommand, (s, e) => (s as PopupMessage).Close(), (s, e) => e.CanExecute = true));
        }

        /// <summary>
        /// 打开通知消息
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="animation">动画类型</param>
        /// <param name="isStayOpen">是否一直打开消息</param>
        /// <param name="waitTime">自动关闭消息时保持打开的时间</param>
        /// <returns></returns>
        public static PopupMessage Show(object content, PopupAnimationX animation, bool isStayOpen = false, double waitTime = 5)
        {
            var popupMessage = new PopupMessage()
            {
                Content = content,
                Opacity = 0,
                _animation = animation,
                _waitTime = waitTime,
            };

            popupMessage.Show();

            var workingArea = SystemParameters.WorkArea;
            var left = workingArea.Width - popupMessage.ActualWidth + thickness;
            var top = workingArea.Height - popupMessage.ActualHeight + thickness;

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
                case PopupAnimationX.HorizontalSlide:
                    popupMessage.Opacity = 1;
                    popupMessage.Left = workingArea.Width;
                    popupMessage.Top = top;
                    popupMessage.BeginAnimation(LeftProperty, AnimationHelper.CreateDoubleAnimation(left));
                    break;
                case PopupAnimationX.VerticalSlide:
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

        private void StartTimer()
        {
            _timerClose = new DispatcherTimer(DispatcherPriority.Normal, Dispatcher);
            _timerClose.Interval = TimeSpan.FromSeconds(_waitTime);
            _timerClose.Tick += (s, e) =>
            {
                this.Close();
                _timerClose.Stop();
                _timerClose = null;
            };
            _timerClose.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

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
                case PopupAnimationX.HorizontalSlide:
                    {
                        var animation = AnimationHelper.CreateDoubleAnimation(workingArea.Width);
                        animation.Completed += Animation_Completed;
                        BeginAnimation(LeftProperty, animation);
                        e.Cancel = true;
                    }
                    break;
                case PopupAnimationX.VerticalSlide:
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
        }

        private void Animation_Completed(object sender, EventArgs e) => Close();
    }
}
