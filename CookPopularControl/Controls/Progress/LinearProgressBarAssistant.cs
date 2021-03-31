using CookPopularControl.Tools.Boxes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ProgressBarAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-11 15:32:24
 */
namespace CookPopularControl.Controls.Progress
{
    /// <summary>
    /// 表示所有<see cref="ProgressBar"/>的附加属性基类
    /// </summary>
    /// <remarks>即以LinearProgressBar为基准</remarks>
    [TemplatePart(Name = PathGrid, Type = typeof(System.Windows.Controls.Grid))]
    [TemplatePart(Name = PathProgress, Type = typeof(Path))]
    [TemplatePart(Name = Content, Type = typeof(TextBlock))]
    [TemplatePart(Name = ElementBackground, Type = typeof(Ellipse))]
    public class LinearProgressBarAssistant
    {
        private const string PathProgress = "PART_PGOGRESS";
        private const string PathGrid = "PathGrid";
        private const string Content = "PART_CONTENT";
        private const string ElementBackground = "PART_Background";

        public static bool GetIsShowPercent(DependencyObject obj) => (bool)obj.GetValue(IsShowPercentProperty);
        public static void SetIsShowPercent(DependencyObject obj, bool value) => obj.SetValue(IsShowPercentProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsShowPercentProperty"/>提供是否显示百分比的附加属性
        /// </summary>
        public static readonly DependencyProperty IsShowPercentProperty =
            DependencyProperty.RegisterAttached("IsShowPercent", typeof(bool), typeof(LinearProgressBarAssistant),
                    new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

        public static Brush GetTextColor(DependencyObject obj) => (Brush)obj.GetValue(TextColorProperty);
        public static void SetTextColor(DependencyObject obj, Brush value) => obj.SetValue(TextColorProperty, value);
        /// <summary>
        /// <see cref="TextColorProperty"/>提供文本颜色的附加属性
        /// </summary>
        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.RegisterAttached("TextColor", typeof(Brush), typeof(LinearProgressBarAssistant),
                new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.Inherits));

        public static bool GetIsShowIcon(DependencyObject obj) => (bool)obj.GetValue(IsShowIconProperty);
        public static void SetIsShowIcon(DependencyObject obj, bool value) => obj.SetValue(IsShowIconProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsShowIconProperty"/>提供是否添加图标的附加属性
        /// </summary>
        public static readonly DependencyProperty IsShowIconProperty =
            DependencyProperty.RegisterAttached("IsShowIcon", typeof(bool), typeof(LinearProgressBarAssistant), 
                new FrameworkPropertyMetadata(ValueBoxes.FalseBox,FrameworkPropertyMetadataOptions.Inherits));

        public static double GetIconSize(DependencyObject obj) => (double)obj.GetValue(IconSizeProperty);
        public static void SetIconSize(DependencyObject obj, double value) => obj.SetValue(IconSizeProperty, value);
        /// <summary>
        /// <see cref="IconSizeProperty"/>提供图标大小的附加属性
        /// </summary>
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.RegisterAttached("IconSize", typeof(double), typeof(LinearProgressBarAssistant),
                new FrameworkPropertyMetadata(ValueBoxes.Double30Box, FrameworkPropertyMetadataOptions.Inherits));
    }
}
