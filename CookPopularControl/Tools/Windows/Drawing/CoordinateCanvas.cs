using CookPopularControl.Tools.Boxes;
using System.Windows;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CoordinateCanvas
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-15 10:55:31
 */
namespace CookPopularControl.Tools.Windows.Drawing
{
    /// <summary>
    /// 自定义Canvas坐标系
    /// </summary>
    public class CoordinateCanvas : Canvas
    {
        /// <summary>
        /// 原点X坐标
        /// </summary>
        public double OriginX
        {
            get { return (double)GetValue(OriginXProperty); }
            set { SetValue(OriginXProperty, value); }
        }
        public static readonly DependencyProperty OriginXProperty =
            DependencyProperty.Register("OriginX", typeof(double), typeof(CoordinateCanvas),
                new PropertyMetadata(ValueBoxes.Double0Box, new PropertyChangedCallback(OnPropertiesValueChanged)));

        /// <summary>
        /// 原点Y坐标
        /// </summary>
        public double OriginY
        {
            get { return (double)GetValue(OriginYProperty); }
            set { SetValue(OriginYProperty, value); }
        }
        public static readonly DependencyProperty OriginYProperty =
            DependencyProperty.Register("OriginY", typeof(double), typeof(CoordinateCanvas),
                new PropertyMetadata(ValueBoxes.Double0Box, new PropertyChangedCallback(OnPropertiesValueChanged)));

        private static void OnPropertiesValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as UIElement;
            if (element != null)
            {
                element.InvalidateArrange();
            }
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Point originalPoint = new Point();
            if (OriginX > 0 && OriginX <= arrangeSize.Width)
                originalPoint.X = OriginX;
            if (OriginY > 0 && OriginY <= arrangeSize.Height)
                originalPoint.Y = OriginY;
            foreach (UIElement element in InternalChildren)
            {
                if (element == null) continue;
                double x = 0.0;
                double y = 0.0;
                double left = GetLeft(element);
                if (!double.IsNaN(left))
                {
                    x = left;
                }
                double top = GetTop(element);
                if (!double.IsNaN(top))
                {
                    y = top;
                }
                element.Arrange(new Rect(new Point(originalPoint.X + x, originalPoint.Y + y), element.DesiredSize));
            }
            return arrangeSize;
        }
    }
}
