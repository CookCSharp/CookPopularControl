using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：提供ButtonBase的附加属性
 * Author： Chance_写代码的厨子
 * Create Time：2021-02-20 11:05:19
 */
namespace CookPopularControl.Communal.Attached
{
    /// <summary>
    /// 提供<see cref="ButtonBase"/>的附加属性
    /// </summary>
    public class ButtonBaseAttached
    {
        public static Brush GetButtonBaseMouseOverBackground(DependencyObject obj) => (Brush)obj.GetValue(ButtonBaseMouseOverBackgroundProperty);
        public static void SetButtonBaseMouseOverBackground(DependencyObject obj, Brush value) => obj.SetValue(ButtonBaseMouseOverBackgroundProperty, value);
        public static readonly DependencyProperty ButtonBaseMouseOverBackgroundProperty =
            DependencyProperty.RegisterAttached("ButtonBaseMouseOverBackground", typeof(Brush), typeof(ButtonBaseAttached), new PropertyMetadata());

        public static Brush GetButtonBasePressBackground(DependencyObject obj) => (Brush)obj.GetValue(ButtonBasePressBackgroundProperty);
        public static void SetButtonBasePressBackground(DependencyObject obj, Brush value) => obj.SetValue(ButtonBasePressBackgroundProperty, value);
        public static readonly DependencyProperty ButtonBasePressBackgroundProperty =
            DependencyProperty.RegisterAttached("ButtonBasePressBackground", typeof(Brush), typeof(ButtonBaseAttached), new PropertyMetadata());
    }
}
