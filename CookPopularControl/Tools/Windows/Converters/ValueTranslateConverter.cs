using CookPopularControl.Tools.Extensions.Markup;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ValueTranslateConverter
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-12 15:06:18
 */
namespace CookPopularControl.Tools.Windows.Converters
{
    /// <summary>
    /// 值转换
    /// </summary>
    public class ValueTranslateConverter : MarkupExtensionBase, IValueConverter
    {
        public static object FixedValue = 1D;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
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
}
