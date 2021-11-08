using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ToDoubleConverters
 * Author： Chance_写代码的厨子
 * Create Time：2021-11-06 20:46:41
 */
namespace CookPopularCSharpToolkit.Windows
{
    /// <summary>
    /// 值转换
    /// </summary>
    [MarkupExtensionReturnType(typeof(double))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class ValueTranslateToDoubleConverter : MarkupExtensionBase, IValueConverter
    {
        public static object FixedValue = 1D;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is null) parameter = 1;
            var isDouble = double.TryParse(parameter.ToString(), out double p);
            if (value is double v && isDouble)
            {
                return v * p;
            }

            return FixedValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [MarkupExtensionReturnType(typeof(double))]
    [ValueConversion(typeof(double[]), typeof(double))]
    public class TranslateToCenterDoubleConverter : MarkupExtensionBase, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) throw new ArgumentException("values 异常" + values.ToString());
            if (values[0] is double && (double)values[0] > 0.0 &&
                values[1] is double && (double)values[1] > 0.0)
            {
                return (double)values[0] - ((double)values[1]) / 2D;
            }

            return default;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
