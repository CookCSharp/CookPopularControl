using CookPopularControl.Tools.Extensions.Markup;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DoubleToCornerRadius
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-27 22:02:29
 */
namespace CookPopularControl.Communal.Converters
{
    /// <summary>
    /// Double To CornerRadius
    /// </summary>
    [MarkupExtensionReturnType(typeof(CornerRadius))]
    public class DoubleToCornerRadius : MarkupExtensionBase, IValueConverter
    {
        public static CornerRadius FixedCornerRadius = new CornerRadius(1);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            parameter = parameter ?? 1;
            var isDouble = double.TryParse(parameter.ToString(), out double p);
            if (value is double v && isDouble)
            {
                return new CornerRadius(v * p);
            }

            return FixedCornerRadius;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius cornerRadius)
                return cornerRadius.TopLeft;
            else
                return double.NaN;
        }
    }
}
