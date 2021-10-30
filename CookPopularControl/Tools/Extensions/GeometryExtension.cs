using CookPopularControl.Expression.Drawing.Core;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-02-24 11:23:04
 */
namespace CookPopularControl.Tools.Extensions
{
    /// <summary>
    /// 提供<see cref="Geometry"/>的扩展方法
    /// </summary>
    public static class GeometryExtension
    {
        /// <summary>
        /// 获取<see cref="Geometry"/>路径总长度
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns><see cref="Geometry"/>路径总长度</returns>
        public static double GetTotalLength(this Geometry geometry)
        {
            if (geometry == null) return 0;
            var pathGeometry = PathGeometry.CreateFromGeometry(geometry);
            pathGeometry.GetPointAtFractionLength(1e-4, out var point, out _);
            var length = (pathGeometry.Figures[0].StartPoint - point).Length * 1e+4;

            return length;
        }

        /// <summary>
        /// 获取<see cref="Geometry"/>路径总长度
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="size"></param>
        /// <param name="strokeThickness"></param>
        /// <returns><see cref="Geometry"/>路径总长度</returns>
        public static double GetTotalLength(this Geometry geometry, Size size, double strokeThickness = 1)
        {
            if (geometry == null) return 0;

            if (MathHelper.IsVerySmall(size.Width) || MathHelper.IsVerySmall(size.Height)) return 0;

            var length = GetTotalLength(geometry);
            var sw = geometry.Bounds.Width / size.Width;
            var sh = geometry.Bounds.Height / size.Height;
            var min = Math.Min(sw, sh);

            if (MathHelper.IsVerySmall(min) || MathHelper.IsVerySmall(strokeThickness)) return 0;

            length /= min;
            return length / strokeThickness;
        }

        /// <summary>
        /// 获取<see cref="Geometry"/>路径较为精确的总长度
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="steps">步数</param>
        /// <returns></returns>
        public static double GetTotalLength(this Geometry geometry, double steps)
        {
            Point pointOnPath;
            Point previousPointOnPath;
            Point tangent;

            double length = 0;

            var pathGeometry = PathGeometry.CreateFromGeometry(geometry);
            pathGeometry.GetPointAtFractionLength(0, out previousPointOnPath, out tangent);

            for (double progress = (1 / steps); progress < 1; progress += (1 / steps))
            {
                pathGeometry.GetPointAtFractionLength(progress, out pointOnPath, out tangent);
                length += Distance(previousPointOnPath, pointOnPath);
                previousPointOnPath = pointOnPath;
            }
            pathGeometry.GetPointAtFractionLength(1, out pointOnPath, out tangent);
            length += Distance(previousPointOnPath, pointOnPath);


            double Distance(Point p0, Point p1)
            {
                return Math.Sqrt((Math.Pow((p1.X - p0.X), 2) + Math.Pow((p1.Y - p0.Y), 2)));
            }

            return length;
        }

        /// <summary>
        /// 由文本得到绘制的几何图形路径
        /// </summary>
        /// <param name="textToFormat"></param>
        /// <param name="fontFamily"></param>
        /// <param name="fontStyle"></param>
        /// <param name="fontWeight"></param>
        /// <param name="fontStretch"></param>
        /// <returns></returns>
        public static Geometry GetGeometryFromText(this string textToFormat, FontFamily? fontFamily = null, FontStyle? fontStyle = null, FontWeight? fontWeight = null, FontStretch? fontStretch = null)
        {
            //var pixelsPerDip = VisualTreeHelper.GetDpi(visual);
            var NumberSubstitution = new NumberSubstitution(NumberCultureSource.Text, CultureInfo.GetCultureInfo("en-us"), NumberSubstitutionMethod.Context);
            fontFamily = fontFamily ?? new FontFamily("Times New Roma");
            fontStyle = fontStyle ?? FontStyles.Normal;
            fontWeight = fontWeight ?? FontWeights.Medium;
            fontStretch = fontStretch ?? FontStretches.Normal;

            var typeFace = new Typeface(fontFamily, fontStyle.Value, fontWeight.Value, fontStretch.Value);
            var formattedText = new FormattedText(
                textToFormat,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                typeFace,
                14,
                Brushes.Black,
                NumberSubstitution,
                TextFormattingMode.Ideal);
            var fontGeometry = formattedText.BuildGeometry(new Point(0, 0));

            return fontGeometry;
        }
    }
}
