using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：TextFieldClearButtonVisibilityConverter
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-19 17:27:36
 */
namespace CookPopularControl.Communal
{
    /// <summary>
    /// 文本清除按钮
    /// </summary>
    [MarkupExtensionReturnType(typeof(Visibility))]
    public class TextFieldClearButtonVisibilityConverter : MarkupExtensionBase, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() < 2) return Visibility.Collapsed;
            if ((bool)values[0] && !string.IsNullOrEmpty(values[1] == null ? string.Empty : values[1].ToString()))
                return ValueBoxes.VisibleBox;
            else
                return ValueBoxes.CollapsedBox;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
