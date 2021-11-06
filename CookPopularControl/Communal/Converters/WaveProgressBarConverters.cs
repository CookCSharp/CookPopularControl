using CookPopularCSharpToolkit.Windows;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：WaveProgressBarConverter
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-15 16:37:20
 */
namespace CookPopularControl.Communal
{
    [MarkupExtensionReturnType(typeof(double))]
    public class ProgressBarValueToTranslateTransformY : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v)
            {
                return -v / 100D;
            }

            return double.NaN;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
