using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：SmoothScrollViewer
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-09 08:57:39
 */
namespace CookPopularControl.Controls.ScrollControls
{
    /// <summary>
    /// 流畅的滚动轮
    /// </summary>
    public class SmoothScrollViewer : ScrollViewer
    {
        public new double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="VerticalOffset"/>的依赖属性
        /// </summary>
        public static new readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.Register("VerticalOffset", typeof(double), typeof(SmoothScrollViewer), new UIPropertyMetadata(ValueBoxes.Double0Box, OnVerticalOffsetChanged));
        private static void OnVerticalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e) => (target as SmoothScrollViewer)?.ScrollToVerticalOffset((double)e.NewValue);

        //对于图像的绘制，可以通过降低其渲染度(可能会损坏图片质量 特别是低清小图 ),对你的图片对象
        //RenderOptions.SetBitmapScalingMode(控件对象,BitmapScalingMode.LowQuality);

        private double lastOffset;

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            double changeValue = e.Delta;
            double newOffset = lastOffset - (changeValue * 2);
            ScrollToVerticalOffset(lastOffset);

            if (newOffset < 0)
                newOffset = 0;
            if (newOffset > ScrollableHeight)
                newOffset = ScrollableHeight;

            ScrollAnimation(newOffset);
            lastOffset = newOffset;
            e.Handled = true;
        }

        private void ScrollAnimation(double offset)
        {
            BeginAnimation(VerticalOffsetProperty, null, HandoffBehavior.SnapshotAndReplace);
            DoubleAnimation Animation = new DoubleAnimation();
            Animation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Animation.From = VerticalOffset;
            Animation.To = offset;
            Animation.Duration = TimeSpan.FromMilliseconds(500);
            //考虑到性能，可以降低动画帧数
            //Timeline.SetDesiredFrameRate(Animation, 40);
            BeginAnimation(VerticalOffsetProperty, Animation);
        }
    }
}
