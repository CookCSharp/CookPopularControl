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
 * Description：ToIntConverters 
 * Author： Chance_写代码的厨子
 * Company: Chance
 * Create Time：2021-11-16 13:50:24
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) Chance 2021 All Rights Reserved.
 */
namespace CookPopularCSharpToolkit.Windows
{
    [MarkupExtensionReturnType(typeof(int))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class EnumToIntConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum t)
            {
                if (Enum.IsDefined(value.GetType(), value) == true)
                    return t.GetHashCode();
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.Parse(targetType, value.ToString());
        }
    }
}
