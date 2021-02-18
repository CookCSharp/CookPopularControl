using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：提供WPF控件附加属性的基类
 * Author： Chance_写代码的厨子
 * Create Time：2021-02-18 16:07:05
 */
namespace CookPopularControl.Communal.Attached
{
    /// <summary>
    /// 提供WPF控件附加属性的基类
    /// </summary>
    public static class FrameworkElementBaseAttached
    {
        public static CornerRadius GetCornerRadius(DependencyObject obj) => (CornerRadius)obj.GetValue(CornerRadiusProperty);
        public static void SetCornerRadius(DependencyObject obj, CornerRadius value) => obj.SetValue(CornerRadiusProperty, value);
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(FrameworkElementBaseAttached), new PropertyMetadata(new CornerRadius(5)));
    }
}
