using CookPopularCSharpToolkit.Communal;
using System;
using System.Collections.Generic;
using System.Linq;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：PolylineHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:56:04
 */
namespace CookPopularControl.Expression
{
    /// <summary>
    /// Helper class to work with list of points
    /// </summary>
    internal static class PolylineHelper
    {
        /// <summary>
        /// March the given polyline with a given interval and output each stop through callback.
        /// </summary>
        /// <param name="polyline">The polyline points to march on.</param>
        /// <param name="startArcLength">The arc length to march before stopping at the first point.</param>
        /// <param name="cornerThreshold">The max angle between edges to be considered a corner vertex.</param>
        /// <param name="stopCallback">Callback when marching algorithm stop at a point. The callback returns the arc length for next stop.
        /// If the asked length is negative, march backwards. If callback returns NaN, finish marching.</param>
        public static void PathMarch(PolylineData polyline, double startArcLength, double cornerThreshold, Func<MarchLocation, double> stopCallback)
        {
            if (polyline == null)
            {
                throw new ArgumentNullException("polyline");
            }
            int count = polyline.Count;
            if (count <= 1)
            {
                throw new ArgumentOutOfRangeException("polyline");
            }
            bool flag = false;
            double num = startArcLength;
            double num2 = 0.0;
            int num3 = 0;
            double num4 = Math.Cos(cornerThreshold * 3.141592653589793 / 180.0);
            for (; ; )
            {
                double num5 = polyline.Lengths[num3];
                if (!MathHelper.IsFiniteDouble(num))
                {
                    break;
                }
                if (MathHelper.IsVerySmall(num))
                {
                    num = stopCallback(MarchLocation.Create(MarchStopReason.CompleteStep, num3, num2, num5 - num2, num));
                    flag = true;
                }
                else if (MathHelper.GreaterThan(num, 0.0))
                {
                    if (MathHelper.LessThanOrClose(num + num2, num5))
                    {
                        num2 += num;
                        num = stopCallback(MarchLocation.Create(MarchStopReason.CompleteStep, num3, num2, num5 - num2, 0.0));
                        flag = true;
                    }
                    else if (num3 < count - 2)
                    {
                        num3++;
                        double num6 = num5 - num2;
                        num -= num6;
                        num2 = 0.0;
                        if (flag && num4 != 1.0 && polyline.Angles[num3] > num4)
                        {
                            num5 = polyline.Lengths[num3];
                            num = stopCallback(MarchLocation.Create(MarchStopReason.CornerPoint, num3, num2, num5 - num2, num));
                        }
                    }
                    else
                    {
                        double num7 = num5 - num2;
                        num -= num7;
                        num5 = polyline.Lengths[num3];
                        num2 = polyline.Lengths[num3];
                        num = stopCallback(MarchLocation.Create(MarchStopReason.CompletePolyline, num3, num2, num5 - num2, num));
                        flag = true;
                    }
                }
                else if (MathHelper.LessThan(num, 0.0))
                {
                    if (MathHelper.GreaterThanOrClose(num + num2, 0.0))
                    {
                        num2 += num;
                        num = stopCallback(MarchLocation.Create(MarchStopReason.CompleteStep, num3, num2, num5 - num2, 0.0));
                        flag = true;
                    }
                    else if (num3 > 0)
                    {
                        num3--;
                        num += num2;
                        num2 = polyline.Lengths[num3];
                        if (flag && num4 != 1.0 && polyline.Angles[num3 + 1] > num4)
                        {
                            num5 = polyline.Lengths[num3];
                            num = stopCallback(MarchLocation.Create(MarchStopReason.CornerPoint, num3, num2, num5 - num2, num));
                        }
                    }
                    else
                    {
                        num += num2;
                        num5 = polyline.Lengths[num3];
                        num2 = 0.0;
                        num = stopCallback(MarchLocation.Create(MarchStopReason.CompletePolyline, num3, num2, num5 - num2, num));
                        flag = true;
                    }
                }
            }
        }

        /// <summary>
        /// Reorders the given list of polylines so that the polyline with a given arc length in the list is the first.
        /// Polylines that preceded this line are concatenated to the end of the list, with the first polyline at the very end. 
        /// </summary>
        /// <param name="lines">A list of polylines.</param>
        /// <param name="startArcLength">The arc length in the entire list of polylines at which to find the start line.
        /// The arc length into that line is returned in this variable.</param>
        /// <returns>The reordered and wrapped list.</returns>
        public static IEnumerable<PolylineData> GetWrappedPolylines(IList<PolylineData> lines, ref double startArcLength)
        {
            int num = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                num = i;
                startArcLength -= lines[i].TotalLength;
                if (MathHelper.LessThanOrClose(startArcLength, 0.0))
                {
                    break;
                }
            }
            if (!MathHelper.LessThanOrClose(startArcLength, 0.0))
            {
                throw new ArgumentOutOfRangeException("startArcLength");
            }
            startArcLength += lines[num].TotalLength;
            return lines.Skip(num).Concat(lines.Take(num + 1));
        }
    }
}
