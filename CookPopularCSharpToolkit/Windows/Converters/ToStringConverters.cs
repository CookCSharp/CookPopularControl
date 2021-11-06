using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ToStringConverters
 * Author： Chance_写代码的厨子
 * Create Time：2021-11-06 22:05:00
 */
namespace CookPopularCSharpToolkit.Windows
{
    [MarkupExtensionReturnType(typeof(object))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class DoubleToStringConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v && parameter is double param)
                return v.ToString($"N{(int)param}");

            return default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
