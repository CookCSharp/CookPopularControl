using CookPopularCSharpToolkit.Communal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：提供WPF控件附加属性的基类
 * Author： Chance_写代码的厨子
 * Create Time：2021-02-18 16:07:05
 */
namespace CookPopularControl.Communal
{
    /// <summary>
    /// 提供WPF控件附加属性的基类
    /// </summary>
    public class FrameworkElementBaseAttached
    {
        public static CornerRadius GetCornerRadius(DependencyObject obj) => (CornerRadius)obj.GetValue(CornerRadiusProperty);
        public static void SetCornerRadius(DependencyObject obj, CornerRadius value) => obj.SetValue(CornerRadiusProperty, value);
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(FrameworkElementBaseAttached), new PropertyMetadata(ValueBoxes.CornerRadius2Box));

        public static Geometry GetIconGeometry(DependencyObject obj) => (Geometry)obj.GetValue(IconGeometryProperty);
        public static void SetIconGeometry(DependencyObject obj, Geometry value) => obj.SetValue(IconGeometryProperty, value);
        public static readonly DependencyProperty IconGeometryProperty =
            DependencyProperty.RegisterAttached("IconGeometry", typeof(Geometry), typeof(FrameworkElementBaseAttached), new PropertyMetadata(default(Geometry)));

        public static double GetIconWidth(DependencyObject obj) => (double)obj.GetValue(IconWidthProperty);
        public static void SetIconWidth(DependencyObject obj, double value) => obj.SetValue(IconWidthProperty, value);
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.RegisterAttached("IconWidth", typeof(double), typeof(FrameworkElementBaseAttached), new PropertyMetadata(ValueBoxes.Double10Box));

        public static double GetIconHeight(DependencyObject obj) => (double)obj.GetValue(IconHeightProperty);
        public static void SetIconHeight(DependencyObject obj, double value) => obj.SetValue(IconHeightProperty, value);
        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.RegisterAttached("IconHeight", typeof(double), typeof(FrameworkElementBaseAttached), new PropertyMetadata(ValueBoxes.Double10Box));

        public static Dock GetIconDirection(DependencyObject obj) => (Dock)obj.GetValue(IconDirectionProperty);
        public static void SetIconDirection(DependencyObject obj, Dock value) => obj.SetValue(IconDirectionProperty, value);
        public static readonly DependencyProperty IconDirectionProperty =
            DependencyProperty.RegisterAttached("IconDirection", typeof(Dock), typeof(FrameworkElementBaseAttached), new PropertyMetadata(Dock.Right));

        public static Thickness GetIconMargin(DependencyObject obj) => (Thickness)obj.GetValue(IconMarginProperty);
        public static void SetIconMargin(DependencyObject obj, Thickness value) => obj.SetValue(IconMarginProperty, value);
        /// <summary>
        /// 提供<see cref="IconMarginProperty"/>的附加属性
        /// </summary>
        /// <remarks>Icon与Content的间距</remarks>
        public static readonly DependencyProperty IconMarginProperty =
            DependencyProperty.RegisterAttached("IconMargin", typeof(Thickness), typeof(FrameworkElementBaseAttached), new PropertyMetadata(ValueBoxes.MarginRight10Box));

        public static DropShadowEffect GetShadowEffect(DependencyObject obj) => (DropShadowEffect)obj.GetValue(ShadowEffectProperty);
        public static void SetShadowEffect(DependencyObject obj, DropShadowEffect value) => obj.SetValue(ShadowEffectProperty, value);
        public static readonly DependencyProperty ShadowEffectProperty =
            DependencyProperty.RegisterAttached("ShadowEffect", typeof(DropShadowEffect), typeof(FrameworkElementBaseAttached), new PropertyMetadata());

        public static int GetStayDecimalPlace(DependencyObject obj) => (int)obj.GetValue(StayDecimalPlaceProperty);
        public static void SetStayDecimalPlace(DependencyObject obj, int value) => obj.SetValue(StayDecimalPlaceProperty, value);
        /// <summary>
        /// <see cref="StayDecimalPlaceProperty"/>标识保留几位小数
        /// </summary>
        public static readonly DependencyProperty StayDecimalPlaceProperty =
            DependencyProperty.RegisterAttached("StayDecimalPlace", typeof(int), typeof(FrameworkElementBaseAttached), new PropertyMetadata(ValueBoxes.Inter0Box));
    }
}
