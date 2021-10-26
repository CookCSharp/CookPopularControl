using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：GroupBoxAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-10-26 17:39:20
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 提供<see cref="System.Windows.Controls.GroupBox"/>的附加属性
    /// </summary>
    public class GroupBoxAssistant
    {
        public static HorizontalAlignment GetHeaderHorizontalAlignment(DependencyObject obj) => (HorizontalAlignment)obj.GetValue(HeaderHorizontalAlignmentProperty);
        public static void SetHeaderHorizontalAlignment(DependencyObject obj, HorizontalAlignment value) => obj.SetValue(HeaderHorizontalAlignmentProperty, value);
        /// <summary>
        /// <see cref="HeaderHorizontalAlignmentProperty"/>标识Header的水平方位
        /// </summary>
        public static readonly DependencyProperty HeaderHorizontalAlignmentProperty =
            DependencyProperty.RegisterAttached("HeaderHorizontalAlignment", typeof(HorizontalAlignment), typeof(GroupBoxAssistant), new PropertyMetadata(default(HorizontalAlignment)));


        public static Brush GetHeaderBackground(DependencyObject obj) => (Brush)obj.GetValue(HeaderBackgroundProperty);
        public static void SetHeaderBackground(DependencyObject obj, Brush value) => obj.SetValue(HeaderBackgroundProperty, value);
        /// <summary>
        /// <see cref="HeaderBackgroundProperty"/>标识Header的背景颜色
        /// </summary>
        public static readonly DependencyProperty HeaderBackgroundProperty =
            DependencyProperty.RegisterAttached("HeaderBackground", typeof(Brush), typeof(GroupBoxAssistant), new PropertyMetadata(default(Brush)));
    }
}
