using CookPopularControl.Communal.Data;
using CookPopularCSharpToolkit.Windows;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：SelectorItemTypeToVisibilityConverter
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-24 13:44:45
 */
namespace CookPopularControl.Communal
{
    [MarkupExtensionReturnType(typeof(Visibility))]
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
}
