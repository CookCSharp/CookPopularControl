using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CircularProgress
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-11 17:57:20
 */
namespace CookPopularControl.Controls.Progress
{
    /// <summary>
    /// 表示圆形进度条的附加属性类
    /// </summary>
    public class CircularProgressAssistant
    {
        public static double GetArcThickness(DependencyObject obj) => (double)obj.GetValue(ArcThicknessProperty);
        public static void SetArcThickness(DependencyObject obj, double value) => obj.SetValue(ArcThicknessProperty, value);
        public static readonly DependencyProperty ArcThicknessProperty =
            DependencyProperty.RegisterAttached("ArcThickness", typeof(double), typeof(CircularProgressAssistant),
                new FrameworkPropertyMetadata(ValueBoxes.Double5Box, FrameworkPropertyMetadataOptions.Inherits));
    }
}
