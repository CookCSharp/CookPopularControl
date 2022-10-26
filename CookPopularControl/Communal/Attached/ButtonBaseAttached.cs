using CookPopularCSharpToolkit.Communal;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：提供ButtonBase的附加属性
 * Author： Chance_写代码的厨子
 * Create Time：2021-02-20 11:05:19
 */
namespace CookPopularControl.Communal
{
    /// <summary>
    /// 提供<see cref="ButtonBase"/>的附加属性
    /// </summary>
    public class ButtonBaseAttached
    {
        public static bool GetIsShowRipple(DependencyObject obj) => (bool)obj.GetValue(IsShowRippleProperty);
        public static void SetIsShowRipple(DependencyObject obj, bool value) => obj.SetValue(IsShowRippleProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsShowRippleProperty"/>是否显示按钮点击的波纹效果
        /// </summary>
        public static readonly DependencyProperty IsShowRippleProperty =
            DependencyProperty.RegisterAttached("IsShowRipple", typeof(bool), typeof(ButtonBaseAttached), new PropertyMetadata(ValueBoxes.TrueBox));
    }
}
