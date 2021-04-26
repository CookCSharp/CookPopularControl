using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：RadioButtonAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-09 17:03:56
 */
namespace CookPopularControl.Controls.Button
{
    /// <summary>
    /// 提供<see cref="RadioButton"/>的附加属性基类
    /// </summary>
    public class RadioButtonAssistant
    {
        public static double GetCircleDiameter(DependencyObject obj) => (double)obj.GetValue(CircleDiameterProperty);
        public static void SetCircleDiameter(DependencyObject obj, double value) => obj.SetValue(CircleDiameterProperty, value);
        /// <summary>
        /// <see cref="CircleDiameterProperty"/>标识点的半径大小
        /// </summary>
        public static readonly DependencyProperty CircleDiameterProperty =
            DependencyProperty.RegisterAttached("CircleDiameter", typeof(double), typeof(RadioButtonAssistant), new PropertyMetadata(ValueBoxes.Double20Box));
    
        public static Brush GetCheckedBrush(DependencyObject obj) => (Brush)obj.GetValue(CheckedBrushProperty);
        public static void SetCheckedBrush(DependencyObject obj, Brush value) => obj.SetValue(CheckedBrushProperty, value);
        /// <summary>
        /// <see cref="CheckedBrushProperty"/>标识选中的填充背景色
        /// </summary>
        public static readonly DependencyProperty CheckedBrushProperty =
            DependencyProperty.RegisterAttached("CheckedBrush", typeof(Brush), typeof(RadioButtonAssistant), new PropertyMetadata(default(Brush)));

        public static bool GetIsFillFully(DependencyObject obj) => (bool)obj.GetValue(IsFillFullyProperty);
        public static void SetIsFillFully(DependencyObject obj, bool value) => obj.SetValue(IsFillFullyProperty, value);
        /// <summary>
        /// <see cref="IsFillFullyProperty"/>标识是否全部填充
        /// </summary>
        public static readonly DependencyProperty IsFillFullyProperty =
            DependencyProperty.RegisterAttached("IsFillFully", typeof(bool), typeof(RadioButtonAssistant), new PropertyMetadata(ValueBoxes.FalseBox));
    }
}
