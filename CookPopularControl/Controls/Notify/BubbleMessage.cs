using CookPopularControl.Communal.Data.Enum;
using CookPopularControl.Communal.Data.Infos;
using CookPopularControl.Tools.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;



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
    /// 注意:
    /// 当<see cref="PopupAnimation"/>的值为<see cref="PopupAnimation.Slide"/>时,
    /// 消息容器的水平和垂直方向如果都是Center或Stretch,
    /// 将直接决定动画为<see cref="PopupAnimation.Scroll"/>，
    /// 另外<see cref="PopupAnimation"/>提供4中动画效果
    /// </remarks> 
    public sealed class BubbleMessage : NotifyMessageBase
    {
        private const string ButtonsClose = "PART_ButtonsClose";
        private static DoubleAnimation showPopupAnimation;
        private static DoubleAnimation closePopupAnimation;
        private static NotifyMessageInfo _info;

        public static readonly ICommand CancelNotifyMessageCommand = new RoutedCommand(nameof(CancelNotifyMessageCommand), typeof(BubbleMessage));
        public static readonly ICommand ConfirmNotifyMessageCommand = new RoutedCommand(nameof(ConfirmNotifyMessageCommand), typeof(BubbleMessage));

        public static Geometry GetBubbleMessageIcon(DependencyObject obj) => (Geometry)obj.GetValue(BubbleMessageIconProperty);
        public static void SetBubbleMessageIcon(DependencyObject obj, Geometry value) => obj.SetValue(BubbleMessageIconProperty, value);
        public static readonly DependencyProperty BubbleMessageIconProperty =
            DependencyProperty.RegisterAttached("BubbleMessageIcon", typeof(Geometry), typeof(BubbleMessage), new PropertyMetadata(default(Geometry)));

        public static Brush GetBubbleMessageIconBrush(DependencyObject obj) => (Brush)obj.GetValue(BubbleMessageIconBrushProperty);
        public static void SetBubbleMessageIconBrush(DependencyObject obj, Brush value) => obj.SetValue(BubbleMessageIconBrushProperty, value);
        public static readonly DependencyProperty BubbleMessageIconBrushProperty =
            DependencyProperty.RegisterAttached("BubbleMessageIconBrush", typeof(Brush), typeof(BubbleMessage), new PropertyMetadata(default(Brush)));


        public static void ShowInfo(object message, string tokenParentPanel = default)
        {
            var info = new NotifyMessageInfo
            {
                Content = message,
                MessageIcon = ResourceHelper.GetResource<Geometry>("InfoGeometry"),
                MessageIconBrush = ResourceHelper.GetResource<Brush>("MessageDialogInfoBrush"),
            };

            Show(info, tokenParentPanel);
        }

        public static void ShowWarning(object message, string tokenParentPanel = default)
        {
            var info = new NotifyMessageInfo
            {
                Content = message,
                MessageIcon = ResourceHelper.GetResource<Geometry>("WarningGeometry"),
                MessageIconBrush = ResourceHelper.GetResource<Brush>("MessageDialogWarningBrush"),
            };

            Show(info, tokenParentPanel);
        }

        public static void ShowError(object message, string tokenParentPanel = default)
        {
            var info = new NotifyMessageInfo
            {
                Content = message,
                MessageIcon = ResourceHelper.GetResource<Geometry>("ErrorGeometry"),
                MessageIconBrush = ResourceHelper.GetResource<Brush>("MessageDialogErrorBrush"),
            };

            Show(info, tokenParentPanel);
        }

        public static void ShowFatal(object message, string tokenParentPanel = default)
        {
            var info = new NotifyMessageInfo
            {
                Content = message,
                MessageIcon = ResourceHelper.GetResource<Geometry>("FatalGeometry"),
                MessageIconBrush = ResourceHelper.GetResource<Brush>("MessageDialogFatalBrush"),
            };

            Show(info, tokenParentPanel);
        }

        public static void ShowQuestion(object message, Func<bool, bool> actionBeforeClose = null, string tokenParentPanel = default)
        {
            var info = new NotifyMessageInfo
            {
                Content = message,
                MessageIcon = ResourceHelper.GetResource<Geometry>("QuestionGeometry"),
                MessageIconBrush = ResourceHelper.GetResource<Brush>("MessageDialogQuestionBrush"),
                IsShowCloseButton = false,
                IsAutoClose = false,
                ActionBeforeClose = actionBeforeClose,
            };

            Show(info, tokenParentPanel, true);
        }

        public static void ShowSuccess(object message, string tokenParentPanel = default)
        {
            var info = new NotifyMessageInfo
            {
                Content = message,
                MessageIcon = ResourceHelper.GetResource<Geometry>("SuccessGeometry"),
                MessageIconBrush = ResourceHelper.GetResource<Brush>("MessageDialogSuccessBrush"),
            };

            Show(info, tokenParentPanel);
        }

        public static void Show(NotifyMessageInfo info, string tokenParentPanel = default, bool isShowQuestion = false)
        {
            Application.Current.Dispatcher?.Invoke(() =>
            {
                Panel? rootMessagePanel;
                if (string.IsNullOrEmpty(tokenParentPanel))
                {
                    rootMessagePanel = DefaultRootMessagePanel;
                }
                else
                {
                    if (PanelDictionary.TryGetValue(tokenParentPanel, out Panel customPanel))
                        rootMessagePanel = customPanel;
                    else
                        throw new AggregateException($"无法找到Token为{tokenParentPanel}的BubbleMessage容器");
                }

                if (rootMessagePanel == null)
                    throw new ArgumentException($"需要一个Panel容器去接收消息并设置{IsParentElementProperty}属性的值或设置{ParentElementTokenProperty}属性的值");

                _info = info;
                var NotifyMessageBaseControl = new ContentControl();
                NotifyMessageBaseControl.Margin = new Thickness(5);
                NotifyMessageBaseControl.Content = info.Content;
                NotifyMessageBaseControl.Style = ResourceHelper.GetResource<Style>("BubbleMessageStyle");
                SetBubbleMessageIcon(NotifyMessageBaseControl, info.MessageIcon);
                SetBubbleMessageIconBrush(NotifyMessageBaseControl, info.MessageIconBrush);
                SetIsShowCloseButton(NotifyMessageBaseControl, info.IsShowCloseButton);

                NotifyMessageBaseControl.Loaded += (s, e) =>
                {
                    var buttonGroup = NotifyMessageBaseControl.Template.FindName(ButtonsClose, NotifyMessageBaseControl) as Panel;
                    if (buttonGroup != null && isShowQuestion)
                        buttonGroup.Visibility = Visibility.Visible;
                };

                NotifyMessageBaseControl.CommandBindings.Add(new CommandBinding(CloseNotifyMessageCommand, (s, e) =>
                {
                    ClosePopupMode(rootMessagePanel, info.PopupAnimation, NotifyMessageBaseControl);
                }));
                NotifyMessageBaseControl.CommandBindings.Add(new CommandBinding(CancelNotifyMessageCommand, (s, e) =>
                {
                    info.ActionBeforeClose?.Invoke(false);
                    ClosePopupMode(rootMessagePanel, info.PopupAnimation, NotifyMessageBaseControl);
                }));
                NotifyMessageBaseControl.CommandBindings.Add(new CommandBinding(ConfirmNotifyMessageCommand, (s, e) =>
                {
                    info.ActionBeforeClose?.Invoke(true);
                    ClosePopupMode(rootMessagePanel, info.PopupAnimation, NotifyMessageBaseControl);
                }));

                rootMessagePanel.Children.Insert(0, NotifyMessageBaseControl);
                SetPopupMode(rootMessagePanel, info.PopupAnimation, NotifyMessageBaseControl);
            });
        }


        private static void SetPopupMode(Panel rootMessagePanel, PopupAnimation mode, ContentControl NotifyMessageBaseControl)
        {
            switch (mode)
            {
                case PopupAnimation.None:
                    break;
                case PopupAnimation.Fade:
                    showPopupAnimation = AnimationHelper.CreateDoubleAnimation(0, 1, AnimationTime);
                    NotifyMessageBaseControl.BeginBeforeAnimation(() => ShowMessageBeforeAction(rootMessagePanel, NotifyMessageBaseControl), UIElement.OpacityProperty, showPopupAnimation);
                    break;
                case PopupAnimation.Slide:
                    TranslateTransform translate = new TranslateTransform();
                    NotifyMessageBaseControl.RenderTransform = translate;
                    if (rootMessagePanel.HorizontalAlignment == HorizontalAlignment.Left)
                    {
                        NotifyMessageBaseControl.Loaded += (s, e) =>
                        {
                            showPopupAnimation = AnimationHelper.CreateDoubleAnimation(-NotifyMessageBaseControl.ActualWidth, 0, AnimationTime);
                            translate.BeginBeforeAnimation(() => ShowMessageBeforeAction(rootMessagePanel, NotifyMessageBaseControl), TranslateTransform.XProperty, showPopupAnimation);
                        };
                    }
                    else if (rootMessagePanel.HorizontalAlignment == HorizontalAlignment.Center || rootMessagePanel.HorizontalAlignment == HorizontalAlignment.Stretch)
                    {
                        if (rootMessagePanel.VerticalAlignment == VerticalAlignment.Top)
                        {
                            showPopupAnimation = AnimationHelper.CreateDoubleAnimation(-NotifyMessageBaseControl.ActualHeight, 0, AnimationTime);
                            translate.BeginBeforeAnimation(() => ShowMessageBeforeAction(rootMessagePanel, NotifyMessageBaseControl), TranslateTransform.YProperty, showPopupAnimation);
                        }
                        else if (rootMessagePanel.VerticalAlignment == VerticalAlignment.Bottom)
                        {
                            showPopupAnimation = AnimationHelper.CreateDoubleAnimation(NotifyMessageBaseControl.ActualHeight, 0, AnimationTime);
                            translate.BeginBeforeAnimation(() => ShowMessageBeforeAction(rootMessagePanel, NotifyMessageBaseControl), TranslateTransform.YProperty, showPopupAnimation);
                        }
                        else
                        {
                            SetScaleTransform(rootMessagePanel, NotifyMessageBaseControl);
                        }
                    }
                    else
                    {
                        NotifyMessageBaseControl.Loaded += (s, e) =>
                        {
                            showPopupAnimation = AnimationHelper.CreateDoubleAnimation(NotifyMessageBaseControl.ActualWidth, 0, AnimationTime);
                            translate.BeginBeforeAnimation(() => ShowMessageBeforeAction(rootMessagePanel, NotifyMessageBaseControl), TranslateTransform.XProperty, showPopupAnimation);
                        };
                    }
                    break;
                case PopupAnimation.Scroll:
                    SetScaleTransform(rootMessagePanel, NotifyMessageBaseControl);
                    break;
                default:
                    break;
            }
        }

        private static void SetScaleTransform(Panel rootMessagePanel, ContentControl NotifyMessageBaseControl)
        {
            ScaleTransform scale = new ScaleTransform();
            NotifyMessageBaseControl.RenderTransform = scale;
            NotifyMessageBaseControl.RenderTransformOrigin = new Point(0.5, 0.5);
            showPopupAnimation = AnimationHelper.CreateDoubleAnimation(0, 1, AnimationTime);
            scale.BeginBeforeAnimation(() => ShowMessageBeforeAction(rootMessagePanel, NotifyMessageBaseControl), ScaleTransform.ScaleXProperty, showPopupAnimation);
            scale.BeginBeforeAnimation(() => ShowMessageBeforeAction(rootMessagePanel, NotifyMessageBaseControl), ScaleTransform.ScaleYProperty, showPopupAnimation);
        }

        private static void ClosePopupMode(Panel rootMessagePanel, PopupAnimation mode, ContentControl NotifyMessageBaseControl)
        {
            switch (mode)
            {
                case PopupAnimation.None:
                    break;
                case PopupAnimation.Fade:
                    closePopupAnimation = AnimationHelper.CreateDoubleAnimation(1, 0, AnimationTime);
                    NotifyMessageBaseControl.BeginBeforeAnimation(() => CloseMessageBeforeAction(rootMessagePanel, NotifyMessageBaseControl), UIElement.OpacityProperty, closePopupAnimation);
                    break;
                case PopupAnimation.Slide:
                    TranslateTransform translate = new TranslateTransform();
                    NotifyMessageBaseControl.RenderTransform = translate;
                    if (rootMessagePanel.HorizontalAlignment == HorizontalAlignment.Left)
                    {
                        closePopupAnimation = AnimationHelper.CreateDoubleAnimation(0, -NotifyMessageBaseControl.ActualWidth, AnimationTime);
                        translate.BeginBeforeAnimation(() => CloseMessageBeforeAction(rootMessagePanel, NotifyMessageBaseControl), TranslateTransform.XProperty, closePopupAnimation);
                    }
                    else if (rootMessagePanel.HorizontalAlignment == HorizontalAlignment.Center || rootMessagePanel.HorizontalAlignment == HorizontalAlignment.Stretch)
                    {
                        if (rootMessagePanel.VerticalAlignment == VerticalAlignment.Top)
                        {
                            closePopupAnimation = AnimationHelper.CreateDoubleAnimation(0, -NotifyMessageBaseControl.ActualHeight, AnimationTime);
                            translate.BeginBeforeAnimation(() => CloseMessageBeforeAction(rootMessagePanel, NotifyMessageBaseControl), TranslateTransform.YProperty, closePopupAnimation);
                        }
                        else if (rootMessagePanel.VerticalAlignment == VerticalAlignment.Bottom)
                        {
                            closePopupAnimation = AnimationHelper.CreateDoubleAnimation(0, NotifyMessageBaseControl.ActualHeight, AnimationTime);
                            translate.BeginBeforeAnimation(() => CloseMessageBeforeAction(rootMessagePanel, NotifyMessageBaseControl), TranslateTransform.YProperty, closePopupAnimation);
                        }
                        else
                        {
                            CloseScaleTransform(rootMessagePanel, NotifyMessageBaseControl);
                        }
                    }
                    else
                    {
                        closePopupAnimation = AnimationHelper.CreateDoubleAnimation(0, NotifyMessageBaseControl.ActualWidth, AnimationTime);
                        translate.BeginBeforeAnimation(() => CloseMessageBeforeAction(rootMessagePanel, NotifyMessageBaseControl), TranslateTransform.XProperty, closePopupAnimation);
                    }
                    break;
                case PopupAnimation.Scroll:
                    CloseScaleTransform(rootMessagePanel, NotifyMessageBaseControl);
                    break;
                default:
                    break;
            }
        }

        private static void CloseScaleTransform(Panel rootMessagePanel, ContentControl NotifyMessageBaseControl)
        {
            ScaleTransform scale = new ScaleTransform();
            NotifyMessageBaseControl.RenderTransform = scale;
            NotifyMessageBaseControl.RenderTransformOrigin = new Point(0.5, 0.5);
            closePopupAnimation = AnimationHelper.CreateDoubleAnimation(1, 0, AnimationTime);
            scale.BeginBeforeAnimation(() => CloseMessageBeforeAction(rootMessagePanel, NotifyMessageBaseControl), ScaleTransform.ScaleXProperty, closePopupAnimation);
            scale.BeginBeforeAnimation(() => CloseMessageBeforeAction(rootMessagePanel, NotifyMessageBaseControl), ScaleTransform.ScaleYProperty, closePopupAnimation);
        }

        private static void ShowMessageBeforeAction(Panel rootMessagePanel, ContentControl NotifyMessageBaseControl)
        {
            showPopupAnimation!.Completed += (s, e) =>
            {
                if (_info.IsAutoClose)
                {
                    DispatcherTimer? CloseMessageTimer = null;
                    CloseMessageTimer = IntervalMultiSeconds(ref CloseMessageTimer, _info.Duration, () =>
                    {
                        ClosePopupMode(rootMessagePanel, _info.PopupAnimation, NotifyMessageBaseControl);
                        CloseMessageTimer.Stop();
                        CloseMessageTimer = null;
                    });
                    CloseMessageTimer.Start();
                }
            };
        }

        private static void CloseMessageBeforeAction(Panel rootMessagePanel, ContentControl NotifyMessageBaseControl)
        {
            closePopupAnimation.Completed += (s, e) =>
            {
                rootMessagePanel.Children.Remove(NotifyMessageBaseControl);
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
