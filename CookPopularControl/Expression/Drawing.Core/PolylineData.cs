using CookPopularCSharpToolkit.Communal;
using System;
using System.Collections.Generic;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：PolylineData
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:54:45
 */
namespace CookPopularControl.Expression
{
    /// <summary>
    /// Represents a polyline with a list of connecting points.
    /// A closed polygon is represented by repeating the first point at the end.
    /// The differences, normals, angles, and lengths are computed on demand.
    /// </summary>
    internal class PolylineData
    {
        /// <summary>
        /// Constructs a polyline with two or more points.
        /// </summary>
        /// <param name="points"></param>
        public PolylineData(IList<Point> points)
        {
            if (points == null)
            {
                throw new ArgumentNullException("points");
            }
            if (points.Count <= 1)
            {
                throw new ArgumentOutOfRangeException("points");
            }
            this.points = points;
        }

        /// <summary>
        /// The polyline is closed when the first and last points are repeated.
        /// </summary>
        public bool IsClosed
        {
            get
            {
                return this.points[0] == this.points.Last<Point>();
            }
        }

        /// <summary>
        /// The count of points in this polyline.
        /// </summary>
        public int Count
        {
            get
            {
                return this.points.Count;
            }
        }

        /// <summary>
        /// The total arc length of this polyline.
        /// </summary>
        public double TotalLength
        {
            get
            {
                double? num = this.totalLength;
                if (num == null)
                {
                    return this.ComputeTotalLength();
                }
                return num.GetValueOrDefault();
            }
        }

        /// <summary>
        /// The point array of this polyline.
        /// </summary>
        public IList<Point> Points
        {
            get
            {
                return this.points;
            }
        }

        /// <summary>
        /// The length between line segments, Points[i] to Points[i+1].
        /// </summary>
        public IList<double> Lengths
        {
            get
            {
                return this.lengths ?? this.ComputeLengths();
            }
        }

        /// <summary>
        /// The list of normal vectors for each segment.
        /// Normals[i] is the normal of segment p[i] to p[i + 1].
        /// Normals[N-1] == Normals[N-2].
        /// </summary>
        public IList<Vector> Normals
        {
            get
            {
                return this.normals ?? this.ComputeNormals();
            }
        }

        /// <summary>
        /// The list of Cos(angle) between two line segments on point p[i].
        /// Note: The value is cos(angle) = Dot(u, v). Not in degrees.
        /// </summary>
        public IList<double> Angles
        {
            get
            {
                return this.angles ?? this.ComputeAngles();
            }
        }

        /// <summary>
        /// The list of accumulated length from points[i] to points[0].
        /// </summary>
        public IList<double> AccumulatedLength
        {
            get
            {
                return this.accumulates ?? this.ComputeAccumulatedLength();
            }
        }

        /// The forward difference vector of polyline.
        /// Points[i] + Differences[i] = Points[i+1]
        public Vector Difference(int index)
        {
            int index2 = (index + 1) % this.Count;
            return this.points[index2].Subtract(this.points[index]);
        }

        /// <summary>
        /// Compute the normal vector of given location (lerp(index, index+1, fraction).
        /// If the location is within range of cornerRadius, interpolate the normal direction.
        /// </summary>
        /// <param name="cornerRadius">The range of normal smoothless.  If zero, no smoothness and return the exact normal on index.</param>
        public Vector SmoothNormal(int index, double fraction, double cornerRadius)
        {
            if (cornerRadius > 0.0)
            {
                double num = this.Lengths[index];
                if (MathHelper.IsVerySmall(num))
                {
                    int num2 = index - 1;
                    if (num2 < 0 && this.IsClosed)
                    {
                        num2 = this.Count - 1;
                    }
                    int num3 = index + 1;
                    if (this.IsClosed && num3 >= this.Count - 1)
                    {
                        num3 = 0;
                    }
                    if (num2 < 0 || num3 >= this.Count)
                    {
                        return this.Normals[index];
                    }
                    return GeometryHelper.Lerp(this.Normals[num3], this.Normals[num2], 0.5).Normalized();
                }
                else
                {
                    double num4 = Math.Min(cornerRadius / num, 0.5);
                    if (fraction <= num4)
                    {
                        int num5 = index - 1;
                        if (this.IsClosed && num5 == -1)
                        {
                            num5 = this.Count - 1;
                        }
                        if (num5 >= 0)
                        {
                            double alpha = (num4 - fraction) / (2.0 * num4);
                            return GeometryHelper.Lerp(this.Normals[index], this.Normals[num5], alpha).Normalized();
                        }
                    }
                    else if (fraction >= 1.0 - num4)
                    {
                        int num6 = index + 1;
                        if (this.IsClosed && num6 >= this.Count - 1)
                        {
                            num6 = 0;
                        }
                        if (num6 < this.Count)
                        {
                            double alpha2 = (fraction + num4 - 1.0) / (2.0 * num4);
                            return GeometryHelper.Lerp(this.Normals[index], this.Normals[num6], alpha2).Normalized();
                        }
                    }
                }
            }
            return this.Normals[index];
        }
        private IList<double> ComputeLengths()
        {
            this.lengths = new double[this.Count];
            for (int i = 0; i < this.Count; i++)
            {
                this.lengths[i] = this.Difference(i).Length;
            }
            return this.lengths;
        }
        private IList<Vector> ComputeNormals()
        {
            this.normals = new Vector[this.points.Count];
            for (int i = 0; i < this.Count - 1; i++)
            {
                this.normals[i] = GeometryHelper.Normal(this.points[i], this.points[i + 1]);
            }
            this.normals[this.Count - 1] = this.normals[this.Count - 2];
            return this.normals;
        }

        private IList<double> ComputeAngles()
        {
            this.angles = new double[this.Count];
            for (int i = 1; i < this.Count - 1; i++)
            {
                this.angles[i] = -GeometryHelper.Dot(this.Normals[i - 1], this.Normals[i]);
            }
            if (this.IsClosed)
            {
                double value = -GeometryHelper.Dot(this.Normals[0], this.Normals[this.Count - 2]);
                this.angles[0] = (this.angles[this.Count - 1] = value);
            }
            else
            {
                this.angles[0] = (this.angles[this.Count - 1] = 1.0);
            }
            return this.angles;
        }

        private IList<double> ComputeAccumulatedLength()
        {
            this.accumulates = new double[this.Count];
            this.accumulates[0] = 0.0;
            for (int i = 1; i < this.Count; i++)
            {
                this.accumulates[i] = this.accumulates[i - 1] + this.Lengths[i - 1];
            }
            this.totalLength = new double?(this.accumulates.Last<double>());
            return this.accumulates;
        }

        private double ComputeTotalLength()
        {
            this.ComputeAccumulatedLength();
            return this.totalLength.Value;
        }

        private IList<Point> points;

        private IList<Vector> normals;

        private IList<double> angles;

        private IList<double> lengths;

        private IList<double> accumulates;

        private double? totalLength;
    }
}
