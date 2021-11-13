using CookPopularCSharpToolkit.Communal;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ToBooleanConverters
 * Author： Chance_写代码的厨子
 * Create Time：2021-11-06 20:06:29
 */
namespace CookPopularCSharpToolkit.Windows
{
    /// <summary>
    /// Bool to Re bool
    /// </summary>
    [MarkupExtensionReturnType(typeof(bool))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class BooleanToReBooleanConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
                return !b;

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }


    /// <summary>
    /// 值在于(0,100)之间，返回Ture，否则返回False
    /// </summary>
    [MarkupExtensionReturnType(typeof(bool))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class ValueBetween0And100ToBooleanConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v)
            {
                return v > 0 && v < 100 ? ValueBoxes.TrueBox : ValueBoxes.FalseBox;
            }

            return ValueBoxes.FalseBox;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }


    [MarkupExtensionReturnType(typeof(bool))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class EqualityToBooleanConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.Equals(true) == true ? parameter : Binding.DoNothing;
        }
    }


    [MarkupExtensionReturnType(typeof(bool))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class ValueLessThanTargetValueToBooleanConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v && double.TryParse(parameter.ToString(), out double p))
            {
                return v < p ? ValueBoxes.TrueBox : ValueBoxes.FalseBox;
            }

            return ValueBoxes.FalseBox;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }


    [MarkupExtensionReturnType(typeof(bool))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class ValueMoreThanTargetValueToBooleanConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v && double.TryParse(parameter.ToString(), out double p))
            {
                return v > p ? ValueBoxes.TrueBox : ValueBoxes.FalseBox;
            }

            return ValueBoxes.FalseBox;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }


    [MarkupExtensionReturnType(typeof(bool))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class EmptyOrNullToBooleanConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    [MarkupExtensionReturnType(typeof(bool))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    [ValueConversion(typeof(Enum), typeof(bool))]
    public class EnumToBooleanConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? parameterString = parameter as string;
            if (parameterString == null)
                return DependencyProperty.UnsetValue;

            if (Enum.IsDefined(value.GetType(), value) == false)
                return DependencyProperty.UnsetValue;

            object parameterValue = Enum.Parse(value.GetType(), parameterString);

            return parameterValue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? parameterString = parameter as string;
            if (parameterString == null)
                return DependencyProperty.UnsetValue;

            return Enum.Parse(targetType, parameterString);
        }
    }
}
