using CookPopularControl.Tools.Extensions.Markup;
using CookPopularControl.Tools.Windows.Converters;
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
 * Description：DoubleToThickness
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-12 15:14:22
 */
namespace CookPopularControl.Communal.Converters
{
    /// <summary>
    /// Double To Thickness
    /// </summary>
    [MarkupExtensionReturnType(typeof(Thickness))]
    public class DoubleToThickness : MarkupExtenisonBase, IValueConverter
    {
        public static Thickness FixedThickness = new Thickness(1);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isDouble = double.TryParse(parameter.ToString(), out double p);
            if (value is double v && isDouble)
            {
                return new Thickness(v * p);
            }

            return FixedThickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness thickness)
                return thickness.Left;
            else
                return double.NaN;
        }
    }
}
