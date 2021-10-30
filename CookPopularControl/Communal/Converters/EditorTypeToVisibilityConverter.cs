using CookPopularControl.Communal.Data;
using CookPopularControl.Tools.Extensions.Markup;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：EditorTypeToVisibility
 * Author： Chance_写代码的厨子
 * Create Time：2021-10-24 15:12:55
 */
namespace CookPopularControl.Communal.Converters
{
    [MarkupExtensionReturnType(typeof(Visibility))]
    [ValueConversion(typeof(EditorType), typeof(Visibility))]
    public class EditorTypeToVisibilityConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hasParam = Enum.TryParse(parameter?.ToString(), out EditorType itemType);
            if (hasParam && ((EditorType)value).Equals(itemType))
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
