using CookPopularControl.Communal.Data.Enum;
using CookPopularControl.Tools.Extensions.Markup;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CheckBoxFillTypeToVisibilityConverter
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-16 16:02:21
 */
namespace CookPopularControl.Communal.Converters
{
    [MarkupExtensionReturnType(typeof(Visibility))]
    public class CheckBoxFillTypeToVisibilityConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var param = Enum.TryParse<FillType>(parameter.ToString(), out FillType fillType);
            if (value is FillType && param && value.Equals(fillType))
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
