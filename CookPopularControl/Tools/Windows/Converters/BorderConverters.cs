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
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：BorderConverter
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-11 15:00:32
 */
namespace CookPopularControl.Tools.Windows.Converters
{
    [MarkupExtensionReturnType(typeof(CornerRadius))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class BorderCircularConverter : MarkupExtensionBase, IMultiValueConverter
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


    [MarkupExtensionReturnType(typeof(Geometry))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class BorderClipToolConverter : MarkupExtensionBase, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 3 && values[0] is double width && values[1] is double height && values[2] is CornerRadius radius)
            {
                if (width < double.Epsilon || height < double.Epsilon)
                {
                    return Geometry.Empty;
                }

                var clip = new PathGeometry
                {
                    Figures = new PathFigureCollection
                    {
                        new PathFigure(new Point(radius.TopLeft, 0), new PathSegment[]
                        {
                            new LineSegment(new Point(width - radius.TopRight, 0), false),
                            new ArcSegment(new Point(width, radius.TopRight), new Size(radius.TopRight, radius.TopRight), 90, false, SweepDirection.Clockwise, false),
                            new LineSegment(new Point(width, height - radius.BottomRight), false),
                            new ArcSegment(new Point(width - radius.BottomRight, height), new Size(radius.BottomRight, radius.BottomRight), 90, false, SweepDirection.Clockwise, false),
                            new LineSegment(new Point(radius.BottomLeft, height), false),
                            new ArcSegment(new Point(0, height - radius.BottomLeft), new Size(radius.BottomLeft, radius.BottomLeft), 90, false, SweepDirection.Clockwise, false),
                            new LineSegment(new Point(0, radius.TopLeft), false),
                            new ArcSegment(new Point(radius.TopLeft, 0), new Size(radius.TopLeft, radius.TopLeft), 90, false, SweepDirection.Clockwise, false),
                        }, false)
                    }
                };
                clip.Freeze();

                return clip;
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }


    [MarkupExtensionReturnType(typeof(Geometry))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class BorderCircularClipConverter : MarkupExtensionBase, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 3 && values[0] is double width && values[1] is double height && values[2] is CornerRadius radius)
            {
                if (width < double.Epsilon || height < double.Epsilon)
                {
                    return Geometry.Empty;
                }

                var clip = new RectangleGeometry(new Rect(0, 0, width, height), radius.TopLeft, radius.TopLeft);
                clip.Freeze();

                return clip;
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
