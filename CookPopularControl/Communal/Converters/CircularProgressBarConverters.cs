using CookPopularControl.Tools.Extensions.Values;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CircularProgressBarConverter
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-12 12:21:19
 */
namespace CookPopularControl.Communal.Converters
{
    public class ArcStartPointConverter : IValueConverter
    {
        [Obsolete]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double && ((double)value > 0.0))
            {
                return new Point((double)value / 2D, 0);
            }

            return new Point();
        }

        [Obsolete]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

    }

    public class ArcSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double && ((double)value > 0.0))
            {
                return new Size((double)value / 2, (double)value / 2);
            }

            return new Point();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class ArcEndPointConverter : IMultiValueConverter
    {
        public const bool IsParameterMidPoint = true;
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var actualWidth = values[0].ExtractDouble();
            var actualHeight = values[1].ExtractDouble();
            var value = values[2].ExtractDouble();
            var minimum = values[3].ExtractDouble();
            var maximum = values[4].ExtractDouble();

            if (new[] { actualWidth, actualHeight, value, minimum, maximum }.AnyNan())
                return Binding.DoNothing;

            var circilarDiameter = Math.Min(actualWidth, actualHeight);
            var circilarRadius = circilarDiameter / 2D;

            if (values.Length == 6)
            {
                var fullIndeterminateScaling = values[5].ExtractDouble();
                if (!double.IsNaN(fullIndeterminateScaling) && fullIndeterminateScaling > 0.0)
                {
                    value = (maximum - minimum) * fullIndeterminateScaling;
                }
            }

            var percent = maximum <= minimum ? 1.0 : (value - minimum) / (maximum - minimum);
            if (Equals(parameter, IsParameterMidPoint))
                percent /= 2;

            var degrees = 360 * percent;
            var radians = degrees * (Math.PI / 180);

            var centre = new Point(circilarRadius, circilarRadius);
            var adjacent = Math.Cos(radians) * circilarRadius;
            var opposite = Math.Sin(radians) * circilarRadius;

            return new Point(centre.X + opposite, centre.Y - adjacent);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
