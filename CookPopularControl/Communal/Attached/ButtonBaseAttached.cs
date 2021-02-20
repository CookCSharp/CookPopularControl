using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;


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
    public class ButtonBaseAttached : FrameworkElementBaseAttached
    {
        public static Dock GetIconDirection(DependencyObject obj) => (Dock)obj.GetValue(IconDirectionProperty);
        public static void SetIconDirection(DependencyObject obj, Dock value) => obj.SetValue(IconDirectionProperty, value);
        public static readonly DependencyProperty IconDirectionProperty =
            DependencyProperty.RegisterAttached("IconDirection", typeof(Dock), typeof(ButtonBaseAttached), new PropertyMetadata(Dock.Right));
    }
}
