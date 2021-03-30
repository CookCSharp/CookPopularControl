using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ListViewAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-30 12:20:18
 */
namespace CookPopularControl.Controls.Lists
{
    /// <summary>
    /// <see cref="ListView"/>的附加属性基类
    /// </summary>
    public class ListViewAssistant
    {
        public static Thickness GetListViewItemPadding(DependencyObject obj) => (Thickness)obj.GetValue(ListViewItemPaddingProperty);
        public static void SetListViewItemPadding(DependencyObject obj, Thickness value) => obj.SetValue(ListViewItemPaddingProperty, value);
        public static readonly DependencyProperty ListViewItemPaddingProperty =
            DependencyProperty.RegisterAttached("ListViewItemPadding", typeof(Thickness), typeof(ListViewAssistant), new PropertyMetadata(default(Thickness)));
    }
}
