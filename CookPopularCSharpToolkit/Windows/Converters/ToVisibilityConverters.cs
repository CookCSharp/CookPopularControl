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
 * Description：ToVisibilityConverters
 * Author： Chance_写代码的厨子
 * Create Time：2021-11-05 22:31:13
 */
//#if NOTINCONTROL
//namespace CookPopularCSharpToolkit.Windows
//#else
//using CookPopularCSharpToolkit.Windows;
//namespace CookPopularControl.Tools.Windows
//#endif
namespace CookPopularCSharpToolkit.Windows
{
    /// <summary>
    /// True to Visibility.Visible
    /// </summary>
    [MarkupExtensionReturnType(typeof(Visibility))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class BooleanToVisibilityConverter : MarkupExtensionBase, IValueConverter
    {
        /// <summary>
        /// Convert bool or Nullable&lt;bool&gt; to Visibility
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bValue = false;
            if (value is bool)
            {
                bValue = (bool)value;
            }
            else if (value is Nullable<bool>)
            {
                Nullable<bool> tmp = (Nullable<bool>)value;
                bValue = tmp.HasValue ? tmp.Value : false;
            }
            return (bValue) ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Convert Visibility to boolean
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                return (Visibility)value == Visibility.Visible;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// False to Visibility.Visible
    /// </summary>
    [MarkupExtensionReturnType(typeof(Visibility))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class BooleanToVisibilityReConverter : MarkupExtensionBase, IValueConverter
    {
        /// <summary>
        /// Convert bool or Nullable&lt;bool&gt; to Visibility
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bValue = false;
            if (value is bool)
            {
                bValue = (bool)value;
            }
            else if (value is bool?)
            {
                bool? tmp = (bool?)value;
                bValue = tmp.HasValue ? tmp.Value : false;
            }
            return (bValue) ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Convert Visibility to boolean
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                return (Visibility)value == Visibility.Collapsed;
            }
            else
            {
                return true;
            }
        }
    }

    [MarkupExtensionReturnType(typeof(Visibility))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class EqualityToVisibilityConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, parameter) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    [MarkupExtensionReturnType(typeof(Visibility))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class BooleanAllToVisibilityConverter : MarkupExtensionBase, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                return Visibility.Collapsed;

            return values.Select(GetBool).All(b => b) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

        private bool GetBool(object value)
        {
            if (value is bool)
            {
                return (bool)value;
            }

            return false;
        }
    }

    [MarkupExtensionReturnType(typeof(Visibility))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class NullOrEmptyToVisibilityConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value?.ToString()) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    /// <summary>
    /// Visibility to Re Visibility
    /// </summary>
    [MarkupExtensionReturnType(typeof(Visibility))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class VisibilityToVisibilityReConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
