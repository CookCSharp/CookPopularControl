using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using CookPopularControl.Communal.Data.Enum;
using CookPopularControl.Tools.Boxes;
using CookPopularControl.Tools.Helpers;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Documents;
using Microsoft.Xaml.Behaviors.Layout;
using CookPopularControl.Tools.Extensions;
using CookPopularControl.Controls.Panels;
using CookPopularControl.Communal.Data.Infos;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：BubbleMessage
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-21 09:01:36
 */
namespace CookPopularControl.Controls.Notify
{
    /// <summary>
    /// 气泡消息
    /// </summary>
    /// <remarks>
    /// <see cref="NotifyPopupPosition"/>提供9个位置方向;
    /// <see cref="PopupAnimation"/>提供4中动画效果
    /// </remarks> 
    public class BubbleMessage : NotifyMessageBase
    {
        private DoubleAnimation showPopupAnimation;
        private DoubleAnimation closePopupAnimation;
        private static ScrollViewer RootScrollViewer;
        private static Panel RootBubbleMessagePanel; //承载所有BubbleMessage消息容器
        private NotifyMessageInfo _info;


        static BubbleMessage()
        {
            RootScrollViewer = new ScrollViewer();
            RootBubbleMessagePanel = new StackPanel();
            RootScrollViewer.Content = RootBubbleMessagePanel;
            RootScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;

            SetNotifyMessageContainer(RootScrollViewer);
        }

        protected override void RemoveMessage() => ClosePopupMode(_info.PopupAnimation);

        public void Show(object message)
        {
            var info = new NotifyMessageInfo
            {
                Content = message,
                PopupPosition = NotifyPopupPosition.CenterTop,
                PopupAnimation = PopupAnimation.Slide,
                IsShowCloseButton = true,
                IsAutoClose = true,
                Duration = 3,
            };

            Show(info);
        }

        public void Show(NotifyMessageInfo info)
        {
            _info = info;
            NotifyMessageBaseControl.Margin = new Thickness(5);
            NotifyMessageBaseControl.Content = info.Content;
            SetIsShowCloseButton(NotifyMessageBaseControl, info.IsShowCloseButton);
            NotifyMessageBaseControl.Style = ResourceHelper.GetResource<Style>("BubbleMessageStyle");

            RootBubbleMessagePanel.Children.Add(NotifyMessageBaseControl);
            SetPopupPosition(info.PopupPosition);
            SetPopupMode(info.PopupAnimation);
            RootScrollViewer.ScrollToEnd();
        }

        private void SetPopupPosition(NotifyPopupPosition position)
        {
            switch (position)
            {
                case NotifyPopupPosition.LeftTop:
                    RootScrollViewer.HorizontalAlignment = HorizontalAlignment.Left;
                    RootScrollViewer.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case NotifyPopupPosition.CenterTop:
                    RootScrollViewer.HorizontalAlignment = HorizontalAlignment.Center;
                    RootScrollViewer.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case NotifyPopupPosition.RightTop:
                    RootScrollViewer.HorizontalAlignment = HorizontalAlignment.Right;
                    RootScrollViewer.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case NotifyPopupPosition.LeftCenter:
                    RootScrollViewer.HorizontalAlignment = HorizontalAlignment.Left;
                    RootScrollViewer.VerticalAlignment = VerticalAlignment.Center;
                    break;
                case NotifyPopupPosition.AllCenter:
                    RootScrollViewer.HorizontalAlignment = HorizontalAlignment.Center;
                    RootScrollViewer.VerticalAlignment = VerticalAlignment.Center;
                    break;
                case NotifyPopupPosition.RightCenter:
                    RootScrollViewer.HorizontalAlignment = HorizontalAlignment.Right;
                    RootScrollViewer.VerticalAlignment = VerticalAlignment.Center;
                    break;
                case NotifyPopupPosition.LeftBottom:
                    RootScrollViewer.HorizontalAlignment = HorizontalAlignment.Left;
                    RootScrollViewer.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                case NotifyPopupPosition.CenterBottom:
                    RootScrollViewer.HorizontalAlignment = HorizontalAlignment.Center;
                    RootScrollViewer.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                case NotifyPopupPosition.RightBottom:
                    RootScrollViewer.HorizontalAlignment = HorizontalAlignment.Right;
                    RootScrollViewer.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                default:
                    break;
            }
        }

        private void SetPopupMode(PopupAnimation mode)
        {
            switch (mode)
            {
                case PopupAnimation.None:
                    break;
                case PopupAnimation.Fade:
                    showPopupAnimation = AnimationHelper.CreateDoubleAnimation(0, 1, AnimationTime);
                    NotifyMessageBaseControl.BeginBeforeAnimation(() => ShowMessageBeforeAction(), UIElement.OpacityProperty, showPopupAnimation);
                    break;
                case PopupAnimation.Slide:
                    TranslateTransform translate = new TranslateTransform();
                    NotifyMessageBaseControl.RenderTransform = translate;
                    if (RootScrollViewer.HorizontalAlignment == HorizontalAlignment.Left)
                    {
                        NotifyMessageBaseControl.Loaded += (s, e) =>
                        {
                            showPopupAnimation = AnimationHelper.CreateDoubleAnimation(-NotifyMessageBaseControl.ActualWidth, 0, AnimationTime);
                            translate.BeginBeforeAnimation(() => ShowMessageBeforeAction(), TranslateTransform.XProperty, showPopupAnimation);
                        };
                    }
                    else if (RootScrollViewer.HorizontalAlignment == HorizontalAlignment.Center || RootScrollViewer.HorizontalAlignment == HorizontalAlignment.Stretch)
                    {
                        if (RootScrollViewer.VerticalAlignment == VerticalAlignment.Top)
                        {
                            showPopupAnimation = AnimationHelper.CreateDoubleAnimation(-NotifyMessageBaseControl.ActualHeight, 0, AnimationTime);
                            translate.BeginBeforeAnimation(() => ShowMessageBeforeAction(), TranslateTransform.YProperty, showPopupAnimation);
                        }
                        else if (RootScrollViewer.VerticalAlignment == VerticalAlignment.Bottom)
                        {
                            showPopupAnimation = AnimationHelper.CreateDoubleAnimation(NotifyMessageBaseControl.ActualHeight, 0, AnimationTime);
                            translate.BeginBeforeAnimation(() => ShowMessageBeforeAction(), TranslateTransform.YProperty, showPopupAnimation);
                        }
                        else
                        {
                            SetScaleTransform();
                        }
                    }
                    else
                    {
                        NotifyMessageBaseControl.Loaded += (s, e) =>
                        {
                            showPopupAnimation = AnimationHelper.CreateDoubleAnimation(NotifyMessageBaseControl.ActualWidth, 0, AnimationTime);
                            translate.BeginBeforeAnimation(() => ShowMessageBeforeAction(), TranslateTransform.XProperty, showPopupAnimation);
                        };
                    }
                    break;
                case PopupAnimation.Scroll:
                    SetScaleTransform();
                    break;
                default:
                    break;
            }
        }

        private void SetScaleTransform()
        {
            ScaleTransform scale = new ScaleTransform();
            NotifyMessageBaseControl.RenderTransform = scale;
            NotifyMessageBaseControl.RenderTransformOrigin = new Point(0.5, 0.5);
            showPopupAnimation = AnimationHelper.CreateDoubleAnimation(0, 1, AnimationTime);
            scale.BeginBeforeAnimation(() => ShowMessageBeforeAction(), ScaleTransform.ScaleXProperty, showPopupAnimation);
            scale.BeginBeforeAnimation(() => ShowMessageBeforeAction(), ScaleTransform.ScaleYProperty, showPopupAnimation);
        }

        private void ClosePopupMode(PopupAnimation mode)
        {
            switch (mode)
            {
                case PopupAnimation.None:
                    break;
                case PopupAnimation.Fade:
                    closePopupAnimation = AnimationHelper.CreateDoubleAnimation(1, 0, AnimationTime);
                    NotifyMessageBaseControl.BeginBeforeAnimation(() => CloseMessageBeforeAction(), UIElement.OpacityProperty, closePopupAnimation);
                    break;
                case PopupAnimation.Slide:
                    TranslateTransform translate = new TranslateTransform();
                    NotifyMessageBaseControl.RenderTransform = translate;
                    if (RootScrollViewer.HorizontalAlignment == HorizontalAlignment.Left)
                    {
                        closePopupAnimation = AnimationHelper.CreateDoubleAnimation(0, -NotifyMessageBaseControl.ActualWidth, AnimationTime);
                        translate.BeginBeforeAnimation(() => CloseMessageBeforeAction(), TranslateTransform.XProperty, closePopupAnimation);
                    }
                    else if (RootScrollViewer.HorizontalAlignment == HorizontalAlignment.Center || RootScrollViewer.HorizontalAlignment == HorizontalAlignment.Stretch)
                    {
                        if (RootScrollViewer.VerticalAlignment == VerticalAlignment.Top)
                        {
                            closePopupAnimation = AnimationHelper.CreateDoubleAnimation(0, -NotifyMessageBaseControl.ActualHeight, AnimationTime);
                            translate.BeginBeforeAnimation(() => CloseMessageBeforeAction(), TranslateTransform.YProperty, closePopupAnimation);
                        }
                        else if (RootScrollViewer.VerticalAlignment == VerticalAlignment.Bottom)
                        {
                            closePopupAnimation = AnimationHelper.CreateDoubleAnimation(0, NotifyMessageBaseControl.ActualHeight, AnimationTime);
                            translate.BeginBeforeAnimation(() => CloseMessageBeforeAction(), TranslateTransform.YProperty, closePopupAnimation);
                        }
                        else
                        {
                            CloseScaleTransform();
                        }
                    }
                    else
                    {
                        closePopupAnimation = AnimationHelper.CreateDoubleAnimation(0, NotifyMessageBaseControl.ActualWidth, AnimationTime);
                        translate.BeginBeforeAnimation(() => CloseMessageBeforeAction(), TranslateTransform.XProperty, closePopupAnimation);
                    }
                    break;
                case PopupAnimation.Scroll:
                    CloseScaleTransform();
                    break;
                default:
                    break;
            }
        }

        private void CloseScaleTransform()
        {
            ScaleTransform scale = new ScaleTransform();
            NotifyMessageBaseControl.RenderTransform = scale;
            NotifyMessageBaseControl.RenderTransformOrigin = new Point(0.5, 0.5);
            closePopupAnimation = AnimationHelper.CreateDoubleAnimation(1, 0, AnimationTime);
            scale.BeginBeforeAnimation(() => CloseMessageBeforeAction(), ScaleTransform.ScaleXProperty, closePopupAnimation);
            scale.BeginBeforeAnimation(() => CloseMessageBeforeAction(), ScaleTransform.ScaleYProperty, closePopupAnimation);
        }

        private void ShowMessageBeforeAction()
        {
            showPopupAnimation!.Completed += (s, e) =>
            {
                if (_info.IsAutoClose)
                {
                    CloseMessageTimer = IntervalMultiSeconds(CloseMessageTimer, _info.Duration, () => 
                    {
                        ClosePopupMode(_info.PopupAnimation); 
                        CloseMessageTimer.Stop();
                    });
                    CloseMessageTimer.Start();
                }
            };
        }

        private void CloseMessageBeforeAction()
        {          
            closePopupAnimation.Completed += (s, e) =>
            {
                RootBubbleMessagePanel.Children.Remove(NotifyMessageBaseControl);
            };
        }
    }

    public static class AnimatableExtension
    {
        public static void BeginBeforeAnimation(this IAnimatable animatable, Action beforeAction, DependencyProperty dp, AnimationTimeline animation)
        {
            beforeAction?.Invoke();
            animatable.BeginAnimation(dp, animation);
        }
    }
}
