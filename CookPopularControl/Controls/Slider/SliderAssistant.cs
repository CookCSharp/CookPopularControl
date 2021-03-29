using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：SliderAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-27 16:48:57
 */
namespace CookPopularControl.Controls.Slider
{
    /// <summary>
    /// 提供<see cref="System.Windows.Controls.Slider"/>的附加属性基类
    /// </summary>
    public class SliderAssistant
    {
        public static bool GetIsShowValue(DependencyObject obj) => (bool)obj.GetValue(IsShowValueProperty);
        public static void SetIsShowValue(DependencyObject obj, bool value) => obj.SetValue(IsShowValueProperty, value);
        /// <summary>
        /// <see cref="IsShowValueProperty"/>标识显示进度值的附加属性
        /// </summary>
        public static readonly DependencyProperty IsShowValueProperty =
            DependencyProperty.RegisterAttached("IsShowValue", typeof(bool), typeof(SliderAssistant), new PropertyMetadata(ValueBoxes.FalseBox));

        public static Brush GetSliderSelectionRangeFill(DependencyObject obj) => (Brush)obj.GetValue(SliderSelectionRangeFillProperty);
        public static void SetSliderSelectionRangeFill(DependencyObject obj, Brush value) => obj.SetValue(SliderSelectionRangeFillProperty, value);
        /// <summary>
        /// <see cref="SliderSelectionRangeFillProperty"/>标识选定范围的填充背景色
        /// </summary>
        public static readonly DependencyProperty SliderSelectionRangeFillProperty =
            DependencyProperty.RegisterAttached("SliderSelectionRangeFill", typeof(Brush), typeof(SliderAssistant), new PropertyMetadata(default(Brush)));
    }
}
