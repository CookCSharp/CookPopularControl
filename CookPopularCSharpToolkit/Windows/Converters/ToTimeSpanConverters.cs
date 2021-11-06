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
 * Description：ToTimeSpanConverters
 * Author： Chance_写代码的厨子
 * Create Time：2021-11-06 22:08:43
 */
namespace CookPopularCSharpToolkit.Windows
{
    [MarkupExtensionReturnType(typeof(TimeSpan))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    [ValueConversion(typeof(double), typeof(TimeSpan))]
    public class DoubleToTimeSpanConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is double d)
                return TimeSpan.FromSeconds(Math.Round(d, 0));

            return TimeSpan.Zero;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
