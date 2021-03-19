using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：PasswordBoxHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-18 16:07:30
 */
namespace CookPopularControl.Communal.Helpers
{
    /// <summary>
    /// <see cref="PasswordBox"/>的帮助类
    /// </summary>
    public class PasswordBoxHelper
    {
        public static bool GetIsShowIcon(DependencyObject obj) => (bool)obj.GetValue(IsShowIconProperty);
        public static void SetIsShowIcon(DependencyObject obj, bool value) => obj.SetValue(IsShowIconProperty, value);
        public static readonly DependencyProperty IsShowIconProperty =
            DependencyProperty.RegisterAttached("IsShowIcon", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(ValueBoxes.TrueBox));
    }
}
