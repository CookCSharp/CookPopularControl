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
 * Description：StringToVisibilityConverter
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-07 10:30:32
 */
namespace CookPopularControl.Tools.Windows.Converters
{
    [MarkupExtensionReturnType(typeof(Visibility))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class StringToVisibilityConverter: MarkupExtensionBase, IValueConverter
    {
        public Visibility IsEmptyValue { get; set; } = Visibility.Collapsed;
        public Visibility IsNotEmptyValue { get; set; } = Visibility.Visible;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty((value ?? "").ToString()) ? IsEmptyValue : IsNotEmptyValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
