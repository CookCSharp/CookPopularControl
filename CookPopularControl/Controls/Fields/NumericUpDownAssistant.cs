using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：NumericUpDownAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-13 18:22:23
 */
namespace CookPopularControl.Controls.Fields
{
    /// <summary>
    /// 表示<see cref="NumericUpDown"/>的附加属性类
    /// </summary>
    public class NumericUpDownAssistant
    {
        public static Thickness GetUpDownButtonMargin(DependencyObject obj) => (Thickness)obj.GetValue(UpDownButtonMarginProperty);
        public static void SetUpDownButtonMargin(DependencyObject obj, Thickness value) => obj.SetValue(UpDownButtonMarginProperty, value);
        /// <summary>
        /// <see cref="UpDownButtonMarginProperty"/>表示UpDownButton的边距
        /// </summary>
        public static readonly DependencyProperty UpDownButtonMarginProperty =
            DependencyProperty.RegisterAttached("UpDownButtonMargin", typeof(Thickness), typeof(NumericUpDownAssistant), new PropertyMetadata(default(Thickness)));


        public static Brush GetUpDownButtonBrush(DependencyObject obj) => (Brush)obj.GetValue(UpDownButtonBrushProperty);
        public static void SetUpDownButtonBrush(DependencyObject obj, Brush value) => obj.SetValue(UpDownButtonBrushProperty, value);
        /// <summary>
        /// <see cref="UpDownButtonBrushProperty"/>表示UpDownButton的填充颜色
        /// </summary>
        public static readonly DependencyProperty UpDownButtonBrushProperty =
            DependencyProperty.RegisterAttached("UpDownButtonBrush", typeof(Brush), typeof(NumericUpDownAssistant), new PropertyMetadata(default(Brush)));
    }
}
