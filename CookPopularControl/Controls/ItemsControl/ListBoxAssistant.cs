/*
 * Description：ListBoxAssistant 
 * Author： Chance.Zheng
 * Create Time: 2023-02-20 14:12:24
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2023 All Rights Reserved.
 */


using CookPopularControl.Communal;
using CookPopularCSharpToolkit.Windows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace CookPopularControl.Controls
{
    [MarkupExtensionReturnType(typeof(Visibility))]
    [ValueConversion(typeof(SelectorItemType), typeof(Visibility))]
    public class SelectorItemTypeToVisibilityConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hasParam = Enum.TryParse<SelectorItemType>(parameter?.ToString(), out SelectorItemType itemType);
            if (hasParam && ((SelectorItemType)value).Equals(itemType))
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ListBoxAssistant
    {
    }
}
