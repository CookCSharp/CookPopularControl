using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ToCornerRadiusConverters
 * Author： Chance_写代码的厨子
 * Create Time：2021-11-06 21:46:18
 */
namespace CookPopularCSharpToolkit.Windows
{
    [MarkupExtensionReturnType(typeof(CornerRadius))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class BorderCircularToCornerRadiusConverter : MarkupExtensionBase, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is double width && values[1] is double height)
            {
                if (width < double.Epsilon || height < double.Epsilon)
                {
                    return new CornerRadius();
                }

                var min = Math.Min(width, height);
                return new CornerRadius(min / 2);
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Double To CornerRadius
    /// </summary>
    [MarkupExtensionReturnType(typeof(CornerRadius))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    [ValueConversion(typeof(CornerRadius), typeof(CornerRadius))]
    public class DoubleToCornerRadiusConverter : MarkupExtensionBase, IValueConverter
    {
        public static CornerRadius FixedCornerRadius = new CornerRadius(1);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            parameter = parameter ?? 1;
            var isDouble = double.TryParse(parameter.ToString(), out double p);
            if (value is double v && isDouble)
            {
                return new CornerRadius(v * p);
            }

            return FixedCornerRadius;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius cornerRadius)
                return cornerRadius.TopLeft;
            else
                return double.NaN;
        }
    }

    [MarkupExtensionReturnType(typeof(Thickness))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    [ValueConversion(typeof(CornerRadius), typeof(CornerRadius))]
    public class CornerRadiusToCornerRadiusConverter : MarkupExtensionBase, IValueConverter
    {
        public static CornerRadius FixedCornerRadius = new CornerRadius(1);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius cornerRadius && parameter is double param)
            {
                return new CornerRadius(cornerRadius.TopLeft * param, cornerRadius.TopRight * param, cornerRadius.BottomRight * param, cornerRadius.BottomLeft * param);
            }

            return FixedCornerRadius;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
