using CookPopularControl.Tools.Extensions.Markup;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：StringToPlaySpeedConverter
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-27 20:00:48
 */
namespace CookPopularControl.Communal.Converters
{
    [MarkupExtensionReturnType(typeof(string))]
    public class StringToPlaySpeedConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? string.Empty : value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
