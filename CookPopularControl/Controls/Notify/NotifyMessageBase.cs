using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：NotifyMessage
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-21 09:16:19
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 消息通知的基类
    /// </summary>
    public abstract class NotifyMessageBase : ContentControl
    {
        public static bool GetIsParentElement(DependencyObject obj) => (bool)obj.GetValue(IsParentElementProperty);
        public static void SetIsParentElement(DependencyObject obj, bool value) => obj.SetValue(IsParentElementProperty, ValueBoxes.BooleanBox(value));
        public static readonly DependencyProperty IsParentElementProperty =
            DependencyProperty.RegisterAttached("IsParentElement", typeof(bool), typeof(NotifyMessageBase), new PropertyMetadata(ValueBoxes.FalseBox, (s, e) =>
            {
                if ((bool)e.NewValue && s is Panel panel)
                {
                    DefaultRootMessagePanel = panel;
                    DefaultRootMessagePanel.Unloaded += (s, e) => Unregister(DefaultRootMessagePanel);
                    Register(panel);
                }
            }));


        public static string GetParentElementToken(DependencyObject obj) => (string)obj.GetValue(ParentElementTokenProperty);
        public static void SetParentElementToken(DependencyObject obj, string value) => obj.SetValue(ParentElementTokenProperty, value);
        public static readonly DependencyProperty ParentElementTokenProperty =
            DependencyProperty.RegisterAttached("ParentElementToken", typeof(string), typeof(NotifyMessageBase),
                new PropertyMetadata(default(string), OnTokenChanged));

        private static void OnTokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Panel panel)
            {
                if (e.NewValue == null)
                {
                    Unregister(panel);
                }
                else
                {
                    panel.Unloaded += (s, e) => Unregister(panel);
                    Register(panel, e.NewValue.ToString());
                }
            }
        }

        private static void Unregister(Panel panel)
        {
            if (panel == null) return;
            var first = PanelDictionary.FirstOrDefault(item => ReferenceEquals(panel, item.Value));
            if (!string.IsNullOrEmpty(first.Key))
            {
                PanelDictionary.Remove(first.Key);
                panel.ContextMenu = null;
                //如果默认容器删除了，从字典表中拿出一个，如果都没有，说明一个消息容器也不存在了
                DefaultRootMessagePanel = DefaultRootMessagePanel ?? PanelDictionary.FirstOrDefault().Value;
            }
        }

        private static void Register(Panel panel, string token = default)
        {
            var menuItem = new MenuItem();
            menuItem.Header = "清空";
            menuItem.Click += (s, e) => panel.Children.Clear();
            panel.ContextMenu = new ContextMenu
            {
                Items = { menuItem },
            };

            if (string.IsNullOrEmpty(token) || panel == null) return;
            PanelDictionary[token] = panel;
        }


        internal static bool GetIsShowCloseButton(DependencyObject obj) => (bool)obj.GetValue(IsShowCloseButtonProperty);
        internal static void SetIsShowCloseButton(DependencyObject obj, bool value) => obj.SetValue(IsShowCloseButtonProperty, value);
        internal static readonly DependencyProperty IsShowCloseButtonProperty =
            DependencyProperty.RegisterAttached("IsShowCloseButton", typeof(bool), typeof(NotifyMessageBase), new PropertyMetadata(ValueBoxes.TrueBox));


        protected const double AnimationTime = 0.5; //动画时间
        protected static Panel DefaultRootMessagePanel; //消息容器
        protected static readonly Dictionary<string, Panel> PanelDictionary = new Dictionary<string, Panel>();
        public static readonly ICommand CloseNotifyMessageCommand = new RoutedCommand(nameof(CloseNotifyMessageCommand), typeof(NotifyMessageBase));

        protected NotifyMessageBase()
        {

        }

        protected static DispatcherTimer IntervalMultiSeconds(ref DispatcherTimer timer, double second, Action action)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(second);
            timer.Tick += delegate { action(); };
            return timer;
        }
    }
}
