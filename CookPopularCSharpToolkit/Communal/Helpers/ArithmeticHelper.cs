using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Media3D;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ArithmeticHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-07 17:07:27
 */
namespace CookPopularCSharpToolkit.Communal
{
    /// <summary>
    /// 算法辅助类
    /// </summary>
    public class ArithmeticHelper
    {
        /// <summary>
        ///     平分一个整数到一个数组中
        /// </summary>
        /// <param name="num"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int[] DivideInt2Arr(int num, int count)
        {
            var arr = new int[count];
            var div = num / count;
            var rest = num % count;
            for (var i = 0; i < count; i++)
            {
                arr[i] = div;
            }
            for (var i = 0; i < rest; i++)
            {
                arr[i] += 1;
            }
            return arr;
        }

        /// <summary>
        /// 计算控件在窗口中的可见坐标
        /// </summary>
        public static Point CalSafePoint(FrameworkElement element, FrameworkElement showElement, Thickness thickness = default)
        {
            if (element == null || showElement == null) return default;
            var point = element.PointToScreen(new Point(0, 0));

            if (point.X < 0) point.X = 0;
            if (point.Y < 0) point.Y = 0;

            var maxLeft = SystemParameters.WorkArea.Width -
                          ((double.IsNaN(showElement.Width) ? showElement.ActualWidth : showElement.Width) +
                           thickness.Left + thickness.Right);
            var maxTop = SystemParameters.WorkArea.Height -
                         ((double.IsNaN(showElement.Height) ? showElement.ActualHeight : showElement.Height) +
                          thickness.Top + thickness.Bottom);
            return new Point(maxLeft > point.X ? point.X : maxLeft, maxTop > point.Y ? point.Y : maxTop);
        }

        /// <summary>
        /// 计算两点的连线和x轴的夹角
        /// </summary>
        /// <param name="center"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static double CalAngle(Point center, Point p) => Math.Atan2(p.Y - center.Y, p.X - center.X) * 180 / Math.PI;

        /// <summary>
        /// 计算法线
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static Vector3D CalNormal(Point3D p0, Point3D p1, Point3D p2)
        {
            var v0 = new Vector3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            var v1 = new Vector3D(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
            return Vector3D.CrossProduct(v0, v1);
        }
    }
}
