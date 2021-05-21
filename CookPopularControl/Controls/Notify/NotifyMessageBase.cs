using CookPopularControl.Communal.Data.Enum;
using CookPopularControl.Tools.Boxes;
using CookPopularControl.Tools.Extensions;
using Microsoft.Xaml.Behaviors.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：NotifyMessage
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-21 09:16:19
 */
namespace CookPopularControl.Controls.Notify
{
    /// <summary>
    /// 消息通知的基类
    /// </summary>
    public abstract class NotifyMessageBase
    {
        internal static bool GetIsShowCloseButton(DependencyObject obj) => (bool)obj.GetValue(IsShowCloseButtonProperty);
        internal static void SetIsShowCloseButton(DependencyObject obj, bool value) => obj.SetValue(IsShowCloseButtonProperty, value);
        internal static readonly DependencyProperty IsShowCloseButtonProperty =
            DependencyProperty.RegisterAttached("IsShowCloseButton", typeof(bool), typeof(NotifyMessageBase), new PropertyMetadata(ValueBoxes.TrueBox));


        protected virtual double AnimationTime => 0.3; //动画时间

        protected DispatcherTimer CloseMessageTimer; //关闭消息时钟

        protected ContentControl NotifyMessageBaseControl; //承载单个消息(指BubbleMessage或着PopupMessage)

        public static readonly ICommand CloseNotifyMessageCommand = new RoutedCommand(nameof(CloseNotifyMessageCommand), typeof(NotifyMessageBase));

        /// <summary>
        /// 设置消息容器
        /// </summary>
        protected static void SetNotifyMessageContainer(UIElement element)
        {
            var win = WindowExtension.GetActiveWindow();
            var adorner = VisualTreeHelperExtension.GetVisualDescendants(win).OfType<AdornerDecorator>().FirstOrDefault();
            if (adorner != null)
            {
                var layer = adorner.AdornerLayer;
                if (layer is not null)
                {
                    //将AdornerLayer作为元素生成一个新的容器
                    var container = new AdornerContainer(adorner.AdornerLayer) { Child = element };
                    layer.Add(container);
                }
            }
        }

        public NotifyMessageBase()
        {
            NotifyMessageBaseControl = new ContentControl();
            NotifyMessageBaseControl.CommandBindings.Add(new CommandBinding(CloseNotifyMessageCommand, (s, e) =>
            {
                RemoveMessage();
            }));
        }

        /// <summary>
        /// 移除消息
        /// </summary>
        protected abstract void RemoveMessage();

        public DispatcherTimer IntervalMultiSeconds(DispatcherTimer timer, double second, Action action)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(second);
            timer.Tick += delegate { action(); };
            return timer;
        }
    }
}
