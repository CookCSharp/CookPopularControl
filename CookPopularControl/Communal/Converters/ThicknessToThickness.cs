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
 * Description：ThicknessToThickness
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-06 20:03:02
 */
namespace CookPopularControl.Communal.Converters
{
    [MarkupExtensionReturnType(typeof(Thickness))]
    public class ThicknessToThickness: MarkupExtensionBase, IValueConverter
    {
        public static Thickness FixedThickness = new Thickness(1);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Thickness thickness && parameter is double param)
            {
                return new Thickness(thickness.Left * param, thickness.Top * param, thickness.Right * param, thickness.Bottom * param);
            }

            return FixedThickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
