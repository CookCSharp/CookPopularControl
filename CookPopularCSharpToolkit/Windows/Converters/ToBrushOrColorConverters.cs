using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;


/*
 * Description：ToBrushOrColorConverters 
 * Author： Chance.Zheng
 * Create Time: 2022-08-28 15:58:08
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2020-2022 All Rights Reserved.
 */
namespace CookPopularCSharpToolkit.Windows
{
    [MarkupExtensionReturnType(typeof(Color))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class BrushToColorConverter : MarkupExtensionBase, IValueConverter
    {
        public static readonly BrushToColorConverter Instance = new BrushToColorConverter();

        public BrushToColorConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = value as SolidColorBrush;
            if (brush != null)
                return brush.Color;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
