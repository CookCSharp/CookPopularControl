using CookPopularControl.Tools.Extensions.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Globalization;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：TranslateToCenterConverter
 * Author： Chance_写代码的厨子
 * Create Time：2021-10-21 19:50:43
 */
namespace CookPopularControl.Tools.Windows.Converters
{
    [MarkupExtensionReturnType(typeof(double))]
    public class TranslateToCenterConverter: MarkupExtensionBase, IMultiValueConverter
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
