using System;
using System.Collections.Generic;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：PolygonGeometrySource
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:14:28
 */
namespace CookPopularCSharpToolkit.Windows.Expression
{
    public class PolygonGeometrySource : GeometrySource<IPolygonGeometrySourceParameters>
    {
        /// <summary>
        /// Polygon recognizes Stretch.None as the same as Stretch.Fill.
        /// </summary>
        protected override Rect ComputeLogicalBounds(Rect layoutBounds, IGeometrySourceParameters parameters)
        {
            Rect logicalBound = base.ComputeLogicalBounds(layoutBounds, parameters);
            return GeometryHelper.GetStretchBound(logicalBound, parameters.Stretch, new Size(1.0, 1.0));
        }

        protected override bool UpdateCachedGeometry(IPolygonGeometrySourceParameters parameters)
        {
            bool flag = false;
            int num = Math.Max(3, Math.Min(100, (int)Math.Round(parameters.PointCount)));
            double num2 = 360.0 / num;
            double num3 = Math.Max(0.0, Math.Min(1.0, parameters.InnerRadius));
            if (num3 < 1.0)
            {
                double num4 = Math.Cos(3.141592653589793 / num);
                double ratio = num3 * num4;
                double num5 = num2 / 2.0;
                this.cachedPoints.EnsureListCount(num * 2, null);
                Rect bound = base.LogicalBounds.Resize(ratio);
                for (int i = 0; i < num; i++)
                {
                    double num6 = num2 * i;
                    this.cachedPoints[i * 2] = GeometryHelper.GetArcPoint(num6, base.LogicalBounds);
                    this.cachedPoints[i * 2 + 1] = GeometryHelper.GetArcPoint(num6 + num5, bound);
                }
            }
            else
            {
                this.cachedPoints.EnsureListCount(num, null);
                for (int j = 0; j < num; j++)
                {
                    double degree = num2 * j;
                    this.cachedPoints[j] = GeometryHelper.GetArcPoint(degree, base.LogicalBounds);
                }
            }
            return flag | PathGeometryHelper.SyncPolylineGeometry(ref this.cachedGeometry, this.cachedPoints, true);
        }

        private List<Point> cachedPoints = new List<Point>();
    }
}
