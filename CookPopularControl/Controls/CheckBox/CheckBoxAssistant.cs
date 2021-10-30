using CookPopularControl.Communal.Data.Enum;
using CookPopularControl.Tools.Boxes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CheckBox
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-16 15:06:11
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// <see cref="System.Windows.Controls.CheckBox"/>的附加基类
    /// </summary>
    [TemplatePart(Name = "PART_Border", Type = typeof(Border))]
    public class CheckBoxAssistant
    {
        public static Brush GetFillBrush(DependencyObject obj) => (Brush)obj.GetValue(FillBrushProperty);
        public static void SetFillBrush(DependencyObject obj, Brush value) => obj.SetValue(FillBrushProperty, value);
        /// <summary>
        /// <see cref="FillBrushProperty"/>提供填充颜色的附加属性
        /// </summary>
        public static readonly DependencyProperty FillBrushProperty =
            DependencyProperty.RegisterAttached("FillBrush", typeof(Brush), typeof(CheckBoxAssistant), new PropertyMetadata(default(Brush)));

        public static double GetFillThickness(DependencyObject obj) => (double)obj.GetValue(FillThicknessProperty);
        public static void SetFillThickness(DependencyObject obj, double value) => obj.SetValue(FillThicknessProperty, value);
        /// <summary>
        /// <see cref="FillThicknessProperty"/>提供填充线条的厚度
        /// </summary>
        public static readonly DependencyProperty FillThicknessProperty =
            DependencyProperty.RegisterAttached("FillThickness", typeof(double), typeof(CheckBoxAssistant), new PropertyMetadata(ValueBoxes.Double1Box));

        public static FillType GetFillType(DependencyObject obj) => (FillType)obj.GetValue(FillTypeProperty);
        public static void SetFillType(DependencyObject obj, FillType value) => obj.SetValue(FillTypeProperty, value);
        /// <summary>
        /// <see cref="FillTypeProperty"/>提供填充类型(■，✔,➖)的附加属性
        /// </summary>
        public static readonly DependencyProperty FillTypeProperty =
            DependencyProperty.RegisterAttached("FillType", typeof(FillType), typeof(CheckBoxAssistant), new PropertyMetadata(default(FillType)));

        public static double GetFillSize(DependencyObject obj) => (double)obj.GetValue(FillSizeProperty);
        public static void SetFillSize(DependencyObject obj, double value) => obj.SetValue(FillSizeProperty, value);
        /// <summary>
        /// <see cref="FillTypeProperty"/>提供填充大小的附加属性
        /// </summary>
        public static readonly DependencyProperty FillSizeProperty =
            DependencyProperty.RegisterAttached("FillSize", typeof(double), typeof(CheckBoxAssistant),
                new FrameworkPropertyMetadata(ValueBoxes.Double30Box, FrameworkPropertyMetadataOptions.Inherits, OnFillSizeChanged));

        private static void OnFillSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element == null) return;
            var maxSize = element.Height;
            SetFillSize(element, (double)e.NewValue > maxSize ? maxSize : (double)e.NewValue);
        }
    }
}
