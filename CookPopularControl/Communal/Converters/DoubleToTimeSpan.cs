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
 * Description：DoubleToTimeSpan
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-27 19:31:08
 */
namespace CookPopularControl.Communal.Converters
{
    [MarkupExtensionReturnType(typeof(TimeSpan))]
    public class DoubleToTimeSpan : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TimeSpan.FromSeconds(Math.Round((double)value, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
