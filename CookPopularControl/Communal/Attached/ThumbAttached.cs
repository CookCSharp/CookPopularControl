using CookPopularCSharpToolkit.Communal;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ThumbAttached
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-27 17:00:36
 */
namespace CookPopularControl.Communal
{
    /// <summary>
    /// 提供<see cref="Thumb"/>的附加属性基类
    /// </summary>
    public class ThumbAttached
    {
        public static double GetThumbWidth(DependencyObject obj) => (double)obj.GetValue(ThumbWidthProperty);
        public static void SetThumbWidth(DependencyObject obj, double value) => obj.SetValue(ThumbWidthProperty, value);
        public static readonly DependencyProperty ThumbWidthProperty =
            DependencyProperty.RegisterAttached("ThumbWidth", typeof(double), typeof(ThumbAttached), new PropertyMetadata(ValueBoxes.Double20Box));

        public static double GetThumbHeight(DependencyObject obj) => (double)obj.GetValue(ThumbHeightProperty);
        public static void SetThumbHeight(DependencyObject obj, double value) => obj.SetValue(ThumbHeightProperty, value);
        public static readonly DependencyProperty ThumbHeightProperty =
            DependencyProperty.RegisterAttached("ThumbHeight", typeof(double), typeof(ThumbAttached), new PropertyMetadata(ValueBoxes.Double20Box));

        public static Geometry GetThumbGeometryData(DependencyObject obj) => (Geometry)obj.GetValue(ThumbGeometryDataProperty);
        public static void SetThumbGeometryData(DependencyObject obj, Geometry value) => obj.SetValue(ThumbGeometryDataProperty, value);
        public static readonly DependencyProperty ThumbGeometryDataProperty =
            DependencyProperty.RegisterAttached("ThumbGeometryData", typeof(Geometry), typeof(ThumbAttached), new PropertyMetadata(Geometry.Empty));

        public static Brush GetThumbBackground(DependencyObject obj) => (Brush)obj.GetValue(ThumbBackgroundProperty);
        public static void SetThumbBackground(DependencyObject obj, Brush value) => obj.SetValue(ThumbBackgroundProperty, value);
        public static readonly DependencyProperty ThumbBackgroundProperty =
            DependencyProperty.RegisterAttached("ThumbBackground", typeof(Brush), typeof(ThumbAttached), new PropertyMetadata(default(Brush)));
    }
}
