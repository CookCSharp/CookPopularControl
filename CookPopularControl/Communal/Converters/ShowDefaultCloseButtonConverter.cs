using CookPopularCSharpToolkit.Windows;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：TabControlHeaderSizeConverter
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 14:50:39
 */
namespace CookPopularControl.Communal
{
    /// <summary>
    /// <see cref="TabControl"/>标题面板大小转换器
    /// </summary>
    [MarkupExtensionReturnType(typeof(Visibility))]
    public class ShowDefaultCloseButtonConverter : MarkupExtensionBase, IMultiValueConverter
    {
        /// <summary>
        /// [0] is owning tabcontrol ShowDefaultCloseButton value.
        /// [1] is owning tabcontrol FixedHeaderCount value.
        /// [2] is item LogicalIndex
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return ((values[0] == DependencyProperty.UnsetValue ? false : (bool)values[0]) &&
                    (values[2] == DependencyProperty.UnsetValue ? 0 : (int)values[2]) >=
                    (values[1] == DependencyProperty.UnsetValue ? 0 : (int)values[1])) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
