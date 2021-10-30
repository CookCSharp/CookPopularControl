using System.Collections.Generic;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：MarchLocation
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:45:51
 */
namespace CookPopularControl.Expression.Drawing.Core
{
    /// <summary>
    /// The data structure to communicate with the PathMarch algorithm.
    /// </summary>
    internal class MarchLocation
    {
        public static MarchLocation Create(MarchStopReason reason, int index, double before, double after, double remain)
        {
            double num = before + after;
            return new MarchLocation
            {
                Reason = reason,
                Index = index,
                Remain = remain,
                Before = MathHelper.EnsureRange(before, new double?(0.0), new double?(num)),
                After = MathHelper.EnsureRange(after, new double?(0.0), new double?(num)),
                Ratio = MathHelper.EnsureRange(MathHelper.SafeDivide(before, num, 0.0), new double?(0.0), new double?(1.0))
            };
        }

        /// <summary>
        /// The reason why this location is sampled.
        /// </summary>
        public MarchStopReason Reason { get; private set; }

        /// <summary>
        /// The index of the point on a polyline point list.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Ratio: [0, 1], which is always before / (before + after).
        /// </summary>
        public double Ratio { get; private set; }

        /// <summary>
        /// Arc length before a stop point. Non-negative and less than Length[index].
        /// </summary>
        public double Before { get; private set; }

        /// <summary>
        /// Arc length after the stop point. Non-negative and less than Length[index].
        /// </summary>
        public double After { get; private set; }

        /// <summary>
        /// Remaining length within a step to hit next stop. Positive to go forward. Negative to go backward.
        /// </summary>
        public double Remain { get; private set; }

        /// <summary>
        /// Gets the interpolated position of this MarchLocation on a given point list.
        /// </summary>
        public Point GetPoint(IList<Point> points)
        {
            return GeometryHelper.Lerp(points[this.Index], points[this.Index + 1], this.Ratio);
        }

        /// <summary>
        /// Get the interpolated normal direction of this MarchLocation on a given normal vector list.
        /// </summary>
        public Vector GetNormal(PolylineData polyline, double cornerRadius = 0.0)
        {
            return polyline.SmoothNormal(this.Index, this.Ratio, cornerRadius);
        }

        /// <summary>
        /// Gets the arc length of this MarchLocation to the start of the entire polyline.
        /// </summary>
        public double GetArcLength(IList<double> accumulatedLengths)
        {
            return MathHelper.Lerp(accumulatedLengths[this.Index], accumulatedLengths[this.Index + 1], this.Ratio);
        }
    }
}
