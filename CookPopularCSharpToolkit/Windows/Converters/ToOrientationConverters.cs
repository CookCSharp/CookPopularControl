using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;



/*
 * Description：ToOrientationConverters 
 * Author： Chance(a cook of write code)
 * Company: Chance
 * Create Time：2021-11-22 17:38:33
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) Chance 2021 All Rights Reserved.
 */
namespace CookPopularCSharpToolkit.Windows
{
    [MarkupExtensionReturnType(typeof(Orientation))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class BooleanToHorizontalOrientationConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool v)
            {
                return v ? Orientation.Horizontal : Orientation.Vertical;
            }

            return Orientation.Vertical;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Orientation o)
            {
                return o == Orientation.Horizontal ? true : false;
            }

            return false;
        }
    }


    [MarkupExtensionReturnType(typeof(Orientation))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class BooleanToVerticalOrientationConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool v)
            {
                return v ? Orientation.Vertical : Orientation.Horizontal;
            }

            return Orientation.Horizontal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Orientation o)
            {
                return o == Orientation.Vertical ? true : false;
            }

            return false;
        }
    }
}
