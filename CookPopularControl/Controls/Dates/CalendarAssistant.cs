using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CalendarAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-06 11:16:52
 */
namespace CookPopularControl.Controls.Dates
{
    /// <summary>
    /// 提供<see cref="Calendar"/>的附加属性基类
    /// </summary>
    public class CalendarAssistant
    {
        public static Brush GetTitleBackgroundBrush(DependencyObject obj) => (Brush)obj.GetValue(TitleBackgroundBrushProperty);
        public static void SetTitleBackgroundBrush(DependencyObject obj, Brush value) => obj.SetValue(TitleBackgroundBrushProperty, value);
        public static readonly DependencyProperty TitleBackgroundBrushProperty =
            DependencyProperty.RegisterAttached("TitleBackgroundBrush", typeof(Brush), typeof(CalendarAssistant), new PropertyMetadata(default(Brush)));

        public static Brush GetWeekForegroundBrush(DependencyObject obj) => (Brush)obj.GetValue(WeekForegroundBrushProperty);
        public static void SetWeekForegroundBrush(DependencyObject obj, Brush value) => obj.SetValue(WeekForegroundBrushProperty, value);
        public static readonly DependencyProperty WeekForegroundBrushProperty =
            DependencyProperty.RegisterAttached("WeekForegroundBrush", typeof(Brush), typeof(CalendarAssistant), new PropertyMetadata(default(Brush)));
    }
}
