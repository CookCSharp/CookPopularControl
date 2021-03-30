using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ScrollViewerAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-26 15:22:43
 */
namespace CookPopularControl.Controls.ScrollControls
{
    /// <summary>
    /// 提供<see cref="ScrollViewer"/>的附加属性基类
    /// </summary>
    public class ScrollViewerAssistant
    {
        public static bool GetIsCornerShow(DependencyObject obj) => (bool)obj.GetValue(IsCornerShowProperty);
        public static void SetIsCornerShow(DependencyObject obj, bool value) => obj.SetValue(IsCornerShowProperty, value);
        /// <summary>
        /// <see cref="IsCornerShowProperty"/>标识右下角的矩形块是否显示
        /// </summary>
        public static readonly DependencyProperty IsCornerShowProperty =
            DependencyProperty.RegisterAttached("IsCornerShow", typeof(bool), typeof(ScrollViewerAssistant), new PropertyMetadata(ValueBoxes.FalseBox));

        public static bool GetIsAutoHideScrollBar(DependencyObject obj) => (bool)obj.GetValue(IsAutoHideScrollBarProperty);
        public static void SetIsAutoHideScrollBar(DependencyObject obj, bool value) => obj.SetValue(IsAutoHideScrollBarProperty, value);
        /// <summary>
        /// <see cref="IsAutoHideScrollBarProperty"/>标识是否自动隐藏<see cref="ScrollBar"/>
        /// </summary>
        public static readonly DependencyProperty IsAutoHideScrollBarProperty =
            DependencyProperty.RegisterAttached("IsAutoHideScrollBar", typeof(bool), typeof(ScrollViewerAssistant), new PropertyMetadata(ValueBoxes.TrueBox));

        public static double GetSyncHorizontalOffset(DependencyObject obj) => (double)obj.GetValue(SyncHorizontalOffsetProperty);
        public static void SetSyncHorizontalOffset(DependencyObject obj, double value) => obj.SetValue(SyncHorizontalOffsetProperty, value);
        public static readonly DependencyProperty SyncHorizontalOffsetProperty =
            DependencyProperty.RegisterAttached("SyncHorizontalOffset", typeof(double), typeof(ScrollViewerAssistant), 
                new PropertyMetadata(ValueBoxes.Double0Box, OnSyncHorizontalOffsetChanged));
        private static void OnSyncHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as ScrollViewer;
            scrollViewer?.ScrollToHorizontalOffset((double)e.NewValue);
        }
    }
}
