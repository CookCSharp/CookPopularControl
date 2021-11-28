using CookPopularControl.Communal.Data;
using CookPopularCSharpToolkit.Windows;
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
namespace CookPopularControl.Controls
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
        private const string ButtonGroup = "PART_ButtonGroup";
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

        public static void ShowQuestion(object message, Action<bool> actionBeforeClose = null, string tokenParentPanel = default)
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
                var notifyMessageBaseControl = new BubbleMessage();
                notifyMessageBaseControl.Margin = new Thickness(5);
                notifyMessageBaseControl.Content = info.Content;
                notifyMessageBaseControl.Style = ResourceHelper.GetResource<Style>("BubbleMessageStyle");
                SetBubbleMessageIcon(notifyMessageBaseControl, info.MessageIcon);
                SetBubbleMessageIconBrush(notifyMessageBaseControl, info.MessageIconBrush);
                SetIsShowCloseButton(notifyMessageBaseControl, info.IsShowCloseButton);

                notifyMessageBaseControl.Loaded += (s, e) =>
                {
                    var buttonGroup = notifyMessageBaseControl.Template.FindName(ButtonGroup, notifyMessageBaseControl) as Panel;
                    if (buttonGroup != null && isShowQuestion)
                        buttonGroup.Visibility = Visibility.Visible;
                };

                notifyMessageBaseControl.CommandBindings.Add(new CommandBinding(CloseNotifyMessageCommand, (s, e) =>
                {
                    ClosePopupMode(rootMessagePanel, info.PopupAnimation, notifyMessageBaseControl);
                }));
                notifyMessageBaseControl.CommandBindings.Add(new CommandBinding(CancelNotifyMessageCommand, (s, e) =>
                {
                    info.ActionBeforeClose?.Invoke(false);
                    ClosePopupMode(rootMessagePanel, info.PopupAnimation, notifyMessageBaseControl);
                }));
                notifyMessageBaseControl.CommandBindings.Add(new CommandBinding(ConfirmNotifyMessageCommand, (s, e) =>
                {
                    info.ActionBeforeClose?.Invoke(true);
                    ClosePopupMode(rootMessagePanel, info.PopupAnimation, notifyMessageBaseControl);
                }));

                rootMessagePanel.Children.Insert(0, notifyMessageBaseControl);
                SetPopupMode(rootMessagePanel, info.PopupAnimation, notifyMessageBaseControl);
            });
        }


        private static void SetPopupMode(Panel rootMessagePanel, PopupAnimation mode, ContentControl notifyMessageBaseControl)
        {
            switch (mode)
            {
                case PopupAnimation.None:
                    break;
                case PopupAnimation.Fade:
                    showPopupAnimation = AnimationHelper.CreateDoubleAnimation(0, 1, AnimationTime);
                    notifyMessageBaseControl.BeginBeforeAnimation(() => ShowMessageBeforeAction(rootMessagePanel, notifyMessageBaseControl), UIElement.OpacityProperty, showPopupAnimation);
                    break;
                case PopupAnimation.Slide:
                    TranslateTransform translate = new TranslateTransform();
                    notifyMessageBaseControl.RenderTransform = translate;
                    if (rootMessagePanel.HorizontalAlignment == HorizontalAlignment.Left)
                    {
                        notifyMessageBaseControl.Loaded += (s, e) =>
                        {
                            showPopupAnimation = AnimationHelper.CreateDoubleAnimation(-notifyMessageBaseControl.ActualWidth, 0, AnimationTime);
                            translate.BeginBeforeAnimation(() => ShowMessageBeforeAction(rootMessagePanel, notifyMessageBaseControl), TranslateTransform.XProperty, showPopupAnimation);
                        };
                    }
                    else if (rootMessagePanel.HorizontalAlignment == HorizontalAlignment.Center || rootMessagePanel.HorizontalAlignment == HorizontalAlignment.Stretch)
                    {
                        if (rootMessagePanel.VerticalAlignment == VerticalAlignment.Top)
                        {
                            showPopupAnimation = AnimationHelper.CreateDoubleAnimation(-notifyMessageBaseControl.ActualHeight, 0, AnimationTime);
                            translate.BeginBeforeAnimation(() => ShowMessageBeforeAction(rootMessagePanel, notifyMessageBaseControl), TranslateTransform.YProperty, showPopupAnimation);
                        }
                        else if (rootMessagePanel.VerticalAlignment == VerticalAlignment.Bottom)
                        {
                            showPopupAnimation = AnimationHelper.CreateDoubleAnimation(notifyMessageBaseControl.ActualHeight, 0, AnimationTime);
                            translate.BeginBeforeAnimation(() => ShowMessageBeforeAction(rootMessagePanel, notifyMessageBaseControl), TranslateTransform.YProperty, showPopupAnimation);
                        }
                        else
                        {
                            SetScaleTransform(rootMessagePanel, notifyMessageBaseControl);
                        }
                    }
                    else
                    {
                        notifyMessageBaseControl.Loaded += (s, e) =>
                        {
                            showPopupAnimation = AnimationHelper.CreateDoubleAnimation(notifyMessageBaseControl.ActualWidth, 0, AnimationTime);
                            translate.BeginBeforeAnimation(() => ShowMessageBeforeAction(rootMessagePanel, notifyMessageBaseControl), TranslateTransform.XProperty, showPopupAnimation);
                        };
                    }
                    break;
                case PopupAnimation.Scroll:
                    SetScaleTransform(rootMessagePanel, notifyMessageBaseControl);
                    break;
                default:
                    break;
            }
        }

        private static void SetScaleTransform(Panel rootMessagePanel, ContentControl notifyMessageBaseControl)
        {
            ScaleTransform scale = new ScaleTransform();
            notifyMessageBaseControl.RenderTransform = scale;
            notifyMessageBaseControl.RenderTransformOrigin = new Point(0.5, 0.5);
            showPopupAnimation = AnimationHelper.CreateDoubleAnimation(0, 1, AnimationTime);
            scale.BeginBeforeAnimation(() => ShowMessageBeforeAction(rootMessagePanel, notifyMessageBaseControl), ScaleTransform.ScaleXProperty, showPopupAnimation);
            scale.BeginBeforeAnimation(() => ShowMessageBeforeAction(rootMessagePanel, notifyMessageBaseControl), ScaleTransform.ScaleYProperty, showPopupAnimation);
        }

        private static void ClosePopupMode(Panel rootMessagePanel, PopupAnimation mode, ContentControl notifyMessageBaseControl)
        {
            switch (mode)
            {
                case PopupAnimation.None:
                    break;
                case PopupAnimation.Fade:
                    closePopupAnimation = AnimationHelper.CreateDoubleAnimation(1, 0, AnimationTime);
                    notifyMessageBaseControl.BeginBeforeAnimation(() => CloseMessageBeforeAction(rootMessagePanel, notifyMessageBaseControl), UIElement.OpacityProperty, closePopupAnimation);
                    break;
                case PopupAnimation.Slide:
                    TranslateTransform translate = new TranslateTransform();
                    notifyMessageBaseControl.RenderTransform = translate;
                    if (rootMessagePanel.HorizontalAlignment == HorizontalAlignment.Left)
                    {
                        closePopupAnimation = AnimationHelper.CreateDoubleAnimation(0, -notifyMessageBaseControl.ActualWidth, AnimationTime);
                        translate.BeginBeforeAnimation(() => CloseMessageBeforeAction(rootMessagePanel, notifyMessageBaseControl), TranslateTransform.XProperty, closePopupAnimation);
                    }
                    else if (rootMessagePanel.HorizontalAlignment == HorizontalAlignment.Center || rootMessagePanel.HorizontalAlignment == HorizontalAlignment.Stretch)
                    {
                        if (rootMessagePanel.VerticalAlignment == VerticalAlignment.Top)
                        {
                            closePopupAnimation = AnimationHelper.CreateDoubleAnimation(0, -notifyMessageBaseControl.ActualHeight, AnimationTime);
                            translate.BeginBeforeAnimation(() => CloseMessageBeforeAction(rootMessagePanel, notifyMessageBaseControl), TranslateTransform.YProperty, closePopupAnimation);
                        }
                        else if (rootMessagePanel.VerticalAlignment == VerticalAlignment.Bottom)
                        {
                            closePopupAnimation = AnimationHelper.CreateDoubleAnimation(0, notifyMessageBaseControl.ActualHeight, AnimationTime);
                            translate.BeginBeforeAnimation(() => CloseMessageBeforeAction(rootMessagePanel, notifyMessageBaseControl), TranslateTransform.YProperty, closePopupAnimation);
                        }
                        else
                        {
                            CloseScaleTransform(rootMessagePanel, notifyMessageBaseControl);
                        }
                    }
                    else
                    {
                        closePopupAnimation = AnimationHelper.CreateDoubleAnimation(0, notifyMessageBaseControl.ActualWidth, AnimationTime);
                        translate.BeginBeforeAnimation(() => CloseMessageBeforeAction(rootMessagePanel, notifyMessageBaseControl), TranslateTransform.XProperty, closePopupAnimation);
                    }
                    break;
                case PopupAnimation.Scroll:
                    CloseScaleTransform(rootMessagePanel, notifyMessageBaseControl);
                    break;
                default:
                    break;
            }
        }

        private static void CloseScaleTransform(Panel rootMessagePanel, ContentControl notifyMessageBaseControl)
        {
            ScaleTransform scale = new ScaleTransform();
            notifyMessageBaseControl.RenderTransform = scale;
            notifyMessageBaseControl.RenderTransformOrigin = new Point(0.5, 0.5);
            closePopupAnimation = AnimationHelper.CreateDoubleAnimation(1, 0, AnimationTime);
            scale.BeginBeforeAnimation(() => CloseMessageBeforeAction(rootMessagePanel, notifyMessageBaseControl), ScaleTransform.ScaleXProperty, closePopupAnimation);
            scale.BeginBeforeAnimation(() => CloseMessageBeforeAction(rootMessagePanel, notifyMessageBaseControl), ScaleTransform.ScaleYProperty, closePopupAnimation);
        }

        private static void ShowMessageBeforeAction(Panel rootMessagePanel, ContentControl notifyMessageBaseControl)
        {
            showPopupAnimation!.Completed += (s, e) =>
            {
                if (_info.IsAutoClose)
                {
                    DispatcherTimer? CloseMessageTimer = null;
                    CloseMessageTimer = IntervalMultiSeconds(ref CloseMessageTimer, _info.Duration, () =>
                    {
                        ClosePopupMode(rootMessagePanel, _info.PopupAnimation, notifyMessageBaseControl);
                        CloseMessageTimer.Stop();
                        CloseMessageTimer = null;
                    });
                    CloseMessageTimer.Start();
                }
            };
        }

        private static void CloseMessageBeforeAction(Panel rootMessagePanel, ContentControl notifyMessageBaseControl)
        {
            closePopupAnimation.Completed += (s, e) =>
            {
                rootMessagePanel.Children.Remove(notifyMessageBaseControl);
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
