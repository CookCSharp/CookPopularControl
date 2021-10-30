using CookPopularControl.Tools.Boxes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CirclePanel
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-07 16:17:45
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 弧形、圆形布局
    /// </summary>
    public class ArcPanel : Panel
    {
        public static readonly object FullCircleAngle = 360.0;

        /// <summary>
        /// 弧形的角度
        /// </summary>
        public double ArcAngle
        {
            get { return (double)GetValue(ArcAngleProperty); }
            set { SetValue(ArcAngleProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="ArcAngle"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ArcAngleProperty =
            DependencyProperty.Register("ArcAngle", typeof(double), typeof(ArcPanel), new FrameworkPropertyMetadata(FullCircleAngle, FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 半径大小
        /// </summary>
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="Radius"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(ArcPanel), new FrameworkPropertyMetadata(ValueBoxes.Double30Box, FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 间隔角度
        /// </summary>
        public double IntervalAngle
        {
            get { return (double)GetValue(IntervalAngleProperty); }
            set { SetValue(IntervalAngleProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="IntervalAngle"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IntervalAngleProperty =
            DependencyProperty.Register("IntervalAngle", typeof(double), typeof(ArcPanel), new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// 子项是否围绕自身旋转
        /// </summary>
        public bool IsItemRotate
        {
            get => (bool)GetValue(IsItemRotateProperty);
            set => SetValue(IsItemRotateProperty, ValueBoxes.BooleanBox(value));
        }
        /// <summary>
        /// 提供<see cref="IsItemRotate"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsItemRotateProperty =
            DependencyProperty.Register("IsItemRotate", typeof(bool), typeof(ArcPanel), new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.AffectsMeasure));


        protected override Size MeasureOverride(Size availableSize)
        {
            var diamater = 2 * Radius;
            if (Children.Count.Equals(0)) return new Size(diamater, diamater);

            availableSize = new Size(diamater, diamater);
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
            }

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int i = 0;
            var averageAngle = (ArcAngle > 360D ? 360D : ArcAngle) / Children.Count;

            foreach (UIElement child in Children)
            {
                var centerX = child.DesiredSize.Width / 2D;
                var centerY = child.DesiredSize.Height / 2D;
                var angle = averageAngle * i++ + IntervalAngle;

                var rotateTransform = new RotateTransform
                {
                    CenterX = centerX,
                    CenterY = centerY,
                    Angle = !IsItemRotate ? 0 : angle,
                };
                child.RenderTransform = rotateTransform;

                var r = Math.PI * angle / 180D;
                var x = Math.Cos(r) * Radius;
                var y = Math.Sin(r) * Radius;

                var rectX = x + finalSize.Width / 2 - centerX;
                var rectY = y + finalSize.Height / 2 - centerY;

                var finalRect = new Rect(rectX, rectY, child.DesiredSize.Width, child.DesiredSize.Height);
                child.Arrange(finalRect);
            }

            return finalSize;
        }
    }
}
