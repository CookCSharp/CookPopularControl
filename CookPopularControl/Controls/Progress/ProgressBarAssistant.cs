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
 * Description：ProgressBarAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-11 15:32:24
 */
namespace CookPopularControl.Controls.Progress
{
    /// <summary>
    /// 表示<see cref="ProgressBar"/>的辅助类
    /// </summary>
    [TemplatePart(Name = PathGrid, Type = typeof(Grid))]
    public class ProgressBarAssistant
    {
        private const string ProgressPath = "PART_PGOGRESS";
        private const string PathGrid = "PathGrid";

        public static bool GetIsShowPercent(DependencyObject obj) => (bool)obj.GetValue(IsShowPercentProperty);
        public static void SetIsShowPercent(DependencyObject obj, bool value) => obj.SetValue(IsShowPercentProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsShowPercentProperty"/>提供是否显示百分比的附加属性
        /// </summary>
        public static readonly DependencyProperty IsShowPercentProperty =
            DependencyProperty.RegisterAttached("IsShowPercent", typeof(bool), typeof(ProgressBarAssistant),
                    new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits, OnPropertiesValueChanged));

        public static Brush GetTextColor(DependencyObject obj) => (Brush)obj.GetValue(TextColorProperty);
        public static void SetTextColor(DependencyObject obj, Brush value) => obj.SetValue(TextColorProperty, value);
        /// <summary>
        /// <see cref="TextColorProperty"/>提供文本颜色的附加属性
        /// </summary>
        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.RegisterAttached("TextColor", typeof(Brush), typeof(ProgressBarAssistant),
                new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.Inherits, OnPropertiesValueChanged));

        public static double GetArcThickness(DependencyObject obj) => (double)obj.GetValue(ArcThicknessProperty);
        public static void SetArcThickness(DependencyObject obj, double value) => obj.SetValue(ArcThicknessProperty, value);
        public static readonly DependencyProperty ArcThicknessProperty =
            DependencyProperty.RegisterAttached("ArcThickness", typeof(double), typeof(ProgressBarAssistant),
                new FrameworkPropertyMetadata(ValueBoxes.Double5Box, FrameworkPropertyMetadataOptions.Inherits, OnPropertiesValueChanged));


        private static void OnPropertiesValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var progress = d as ProgressBar;
            if (progress != null)
            {
                progress.UpdateLayout();
            }
        }
    }
}
