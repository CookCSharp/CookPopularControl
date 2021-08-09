using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
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
            BeginAnimation(ScrollViewerAssistant.VerticalOffsetProperty, null);
            DoubleAnimation Animation = new DoubleAnimation();
            Animation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Animation.From = VerticalOffset;
            Animation.To = offset;
            Animation.Duration = TimeSpan.FromMilliseconds(500);
            //考虑到性能，可以降低动画帧数
            //Timeline.SetDesiredFrameRate(Animation, 40);
            BeginAnimation(ScrollViewerAssistant.VerticalOffsetProperty, Animation);
        }
    }
}
