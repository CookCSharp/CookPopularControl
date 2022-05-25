using CookPopularControl.Communal.Data;
using CookPopularCSharpToolkit.Communal;
using System;
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
    /// <![CDATA[CheckBox.IsChecked=Null]]>时填充类型
    /// </summary>
    public enum NullFillType
    {
        ///// <summary>
        ///// ✔
        ///// </summary>
        //Check,
        /// <summary>
        /// ■
        /// </summary>
        Rectangle,
        /// <summary>
        /// ➖
        /// </summary>
        Line
    }

    /// <summary>
    /// <see cref="System.Windows.Controls.CheckBox"/>的附加基类
    /// </summary>
    [TemplatePart(Name = "PART_Border", Type = typeof(Border))]
    public class CheckBoxAssistant
    {
        public static double GetBoxSize(DependencyObject obj) => (double)obj.GetValue(BoxSizeProperty);
        public static void SetBoxSize(DependencyObject obj, double value) => obj.SetValue(BoxSizeProperty, value);
        /// <summary>
        /// <see cref="BoxSizeProperty"/>提供选中框大小的附加属性
        /// </summary>
        public static readonly DependencyProperty BoxSizeProperty =
            DependencyProperty.RegisterAttached("BoxSize", typeof(double), typeof(CheckBoxAssistant), new PropertyMetadata(OnBoxSizeChanged));

        private static void OnBoxSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element == null) return;
            var maxSize = element.Height;
            SetBoxSize(element, (double)e.NewValue > maxSize ? maxSize : (double)e.NewValue);
        }

        public static Brush GetFillBrush(DependencyObject obj) => (Brush)obj.GetValue(FillBrushProperty);
        public static void SetFillBrush(DependencyObject obj, Brush value) => obj.SetValue(FillBrushProperty, value);
        /// <summary>
        /// <see cref="FillBrushProperty"/>提供填充颜色的附加属性
        /// </summary>
        public static readonly DependencyProperty FillBrushProperty =
            DependencyProperty.RegisterAttached("FillBrush", typeof(Brush), typeof(CheckBoxAssistant), new PropertyMetadata(default(Brush)));

        public static NullFillType GetNullFillType(DependencyObject obj) => (NullFillType)obj.GetValue(NullFillTypeProperty);
        public static void SetNullFillType(DependencyObject obj, NullFillType value) => obj.SetValue(NullFillTypeProperty, value);
        /// <summary>
        /// <see cref="NullFillTypeProperty"/>提供<![CDATA[CheckBox.IsChecked=Null]]>时填充类型(■，✔,➖)的附加属性
        /// </summary>
        public static readonly DependencyProperty NullFillTypeProperty =
            DependencyProperty.RegisterAttached("NullFillType", typeof(NullFillType), typeof(CheckBoxAssistant), new PropertyMetadata(default(NullFillType)));
    }
}
