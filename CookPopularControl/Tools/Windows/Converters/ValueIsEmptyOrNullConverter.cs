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
 * Description：ValueIsEmptyOrNullConverter
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-19 08:46:56
 */
namespace CookPopularControl.Tools.Windows.Converters
{
    [MarkupExtensionReturnType(typeof(bool))]
    public class ValueIsEmptyOrNullConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
