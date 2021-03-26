using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ScrollBarAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-26 13:22:25
 */
namespace CookPopularControl.Controls.ScrollControls
{
    /// <summary>
    /// 提供<see cref="ScrollBar"/>的附加属性基类
    /// </summary>
    public class ScrollBarAssistant
    {
        public static bool GetIsScrollBarRepeatButton(DependencyObject obj) => (bool)obj.GetValue(IsScrollBarRepeatButtonProperty);
        public static void SetIsScrollBarRepeatButton(DependencyObject obj, bool value) => obj.SetValue(IsScrollBarRepeatButtonProperty, value);
        /// <summary>
        /// <see cref="IsScrollBarRepeatButtonProperty"/>标识<see cref="RepeatButton"/>是否显示，默认True
        /// </summary>
        public static readonly DependencyProperty IsScrollBarRepeatButtonProperty =
            DependencyProperty.RegisterAttached("IsScrollBarRepeatButton", typeof(bool), typeof(ScrollBarAssistant), new PropertyMetadata(ValueBoxes.TrueBox));

        public static Thickness GetThumbInsideMargin(DependencyObject obj) => (Thickness)obj.GetValue(ThumbInsideMarginProperty);
        public static void SetThumbInsideMargin(DependencyObject obj, Thickness value) => obj.SetValue(ThumbInsideMarginProperty, value);
        /// <summary>
        /// <see cref="ThumbInsideMarginProperty"/>标识<see cref="Thumb"/>在<see cref="Track"/>中移动时距离边界的位置
        /// </summary>
        public static readonly DependencyProperty ThumbInsideMarginProperty =
            DependencyProperty.RegisterAttached("ThumbInsideMargin", typeof(Thickness), typeof(ScrollBarAssistant), new PropertyMetadata(default(Thickness)));
    }
}
