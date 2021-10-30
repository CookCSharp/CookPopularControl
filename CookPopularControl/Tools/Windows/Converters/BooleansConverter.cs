using CookPopularControl.Tools.Extensions.Markup;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：BooleanConverter
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-25 14:24:54
 */
namespace CookPopularControl.Tools.Windows.Converters
{
    /// <summary>
    /// Bool to Re bool
    /// </summary>
    [MarkupExtensionReturnType(typeof(Visibility))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class BooleanToReBooleanConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return !b;

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
