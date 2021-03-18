using CookPopularControl.Tools.Boxes;
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
 * Description：ValueCompareConverters
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-16 13:53:10
 */
namespace CookPopularControl.Tools.Windows.Converters
{
    [MarkupExtensionReturnType(typeof(bool))]
    public class ValueBetweenZeroAnd100 : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v)
            {
                return v > 0 && v < 100 ? ValueBoxes.TrueBox : ValueBoxes.FalseBox;
            }

            return ValueBoxes.FalseBox;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
