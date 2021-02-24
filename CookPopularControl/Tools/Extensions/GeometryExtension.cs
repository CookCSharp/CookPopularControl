using CookPopularControl.Tools.Boxes;
using CookPopularControl.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
