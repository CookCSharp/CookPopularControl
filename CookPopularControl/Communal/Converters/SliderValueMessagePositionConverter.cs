using CookPopularCSharpToolkit.Windows;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：SliderValueMessagePositionConverter
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-29 11:29:07
 */
namespace CookPopularControl.Communal
{
    [MarkupExtensionReturnType(typeof(double))]
    public class SliderValueLeftPositionXConverter : MarkupExtensionBase, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() < 2) return double.NaN;
            if (values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
                return double.NaN;

            return -(double)values[0] - (double)values[1] * 0.5 - 6D;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class SliderValueLeftPositionYConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * 0.5 - 18D;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
