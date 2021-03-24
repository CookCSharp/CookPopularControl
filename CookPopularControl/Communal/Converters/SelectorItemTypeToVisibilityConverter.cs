using CookPopularControl.Communal.Data.Enum;
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
 * Description：SelectorItemTypeToVisibilityConverter
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-24 13:44:45
 */
namespace CookPopularControl.Communal.Converters
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
