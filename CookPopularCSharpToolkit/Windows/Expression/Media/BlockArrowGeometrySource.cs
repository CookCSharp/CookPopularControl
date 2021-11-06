using CookPopularCSharpToolkit.Windows.Expression;
using CookPopularCSharpToolkit.Communal;
using System;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：BlockArrowGeometrySource
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:35:22
 */
namespace CookPopularCSharpToolkit.Windows.Expression
{
    public class BlockArrowGeometrySource : GeometrySource<IBlockArrowGeometrySourceParameters>
    {
        protected override bool UpdateCachedGeometry(IBlockArrowGeometrySourceParameters parameters)
        {
            bool flag = false;
            BlockArrowGeometrySource.ArrowBuilder builder = BlockArrowGeometrySource.GetBuilder(parameters.Orientation);
            double num = builder.ArrowLength(base.LogicalBounds);
            double num2 = builder.ArrowWidth(base.LogicalBounds);
            double num3 = num2 / 2.0 / num;
            double num4 = MathHelper.EnsureRange(parameters.ArrowheadAngle, new double?(0.0), new double?(180.0));
            double num5 = Math.Tan(num4 * 3.141592653589793 / 180.0 / 2.0);
            if (num5 < num3)
            {
                this.EnsurePoints(3);
                this.points[0] = builder.ComputePointA(num, num2);
                this.points[1] = builder.ComputePointB(num, num);
                this.points[2] = builder.GetMirrorPoint(this.points[1], num2);
            }
            else
            {
                double offset = num2 / 2.0 / num5;
                double num6 = MathHelper.EnsureRange(parameters.ArrowBodySize, new double?(0.0), new double?(1.0));
                double thickness = num2 / 2.0 * (1.0 - num6);
                this.EnsurePoints(7);
                this.points[0] = builder.ComputePointA(num, num2);
                this.points[1] = builder.ComputePointB(num, offset);
                Tuple<Point, Point> tuple = builder.ComputePointCD(num, offset, thickness);
                this.points[2] = tuple.Item1;
                this.points[3] = tuple.Item2;
                this.points[4] = builder.GetMirrorPoint(this.points[3], num2);
                this.points[5] = builder.GetMirrorPoint(this.points[2], num2);
                this.points[6] = builder.GetMirrorPoint(this.points[1], num2);
            }
            for (int i = 0; i < this.points.Length; i++)
            {
                Point[] array = this.points;
                int num7 = i;
                array[num7].X = array[num7].X + base.LogicalBounds.Left;
                Point[] array2 = this.points;
                int num8 = i;
                array2[num8].Y = array2[num8].Y + base.LogicalBounds.Top;
            }
            return flag | PathGeometryHelper.SyncPolylineGeometry(ref this.cachedGeometry, this.points, true);
        }

        private static BlockArrowGeometrySource.ArrowBuilder GetBuilder(ArrowOrientation orientation)
        {
            switch (orientation)
            {
                case ArrowOrientation.Left:
                    return new BlockArrowGeometrySource.LeftArrowBuilder();
                case ArrowOrientation.Up:
                    return new BlockArrowGeometrySource.UpArrowBuilder();
                case ArrowOrientation.Down:
                    return new BlockArrowGeometrySource.DownArrowBuilder();
            }
            return new BlockArrowGeometrySource.RightArrowBuilder();
        }

        private void EnsurePoints(int count)
        {
            if (this.points == null || this.points.Length != count)
            {
                this.points = new Point[count];
            }
        }

        private Point[] points;

        /// <summary>
        ///      B
        ///     /|
        ///    / C--D
        ///   A     |
        ///    \ C--D
        ///     \|
        ///      B
        /// Algorithm only uses Width/Height assuming top-left at 0,0.
        /// </summary>
        private abstract class ArrowBuilder
        {
            public abstract double ArrowLength(Rect bounds);

            public abstract double ArrowWidth(Rect bounds);

            public abstract Point GetMirrorPoint(Point point, double width);

            public abstract Point ComputePointA(double length, double width);

            public abstract Point ComputePointB(double length, double offset);

            public abstract Tuple<Point, Point> ComputePointCD(double length, double offset, double thickness);
        }

        private abstract class HorizontalArrowBuilder : BlockArrowGeometrySource.ArrowBuilder
        {
            public override double ArrowLength(Rect bounds)
            {
                return bounds.Width;
            }

            public override double ArrowWidth(Rect bounds)
            {
                return bounds.Height;
            }

            public override Point GetMirrorPoint(Point point, double width)
            {
                return new Point(point.X, width - point.Y);
            }
        }

        private abstract class VerticalArrowBuilder : BlockArrowGeometrySource.ArrowBuilder
        {
            public override double ArrowLength(Rect bounds)
            {
                return bounds.Height;
            }

            public override double ArrowWidth(Rect bounds)
            {
                return bounds.Width;
            }

            public override Point GetMirrorPoint(Point point, double width)
            {
                return new Point(width - point.X, point.Y);
            }
        }

        private class LeftArrowBuilder : BlockArrowGeometrySource.HorizontalArrowBuilder
        {
            public override Point ComputePointA(double length, double width)
            {
                return new Point(0.0, width / 2.0);
            }

            public override Point ComputePointB(double length, double offset)
            {
                return new Point(offset, 0.0);
            }

            public override Tuple<Point, Point> ComputePointCD(double length, double offset, double thickness)
            {
                return new Tuple<Point, Point>(new Point(offset, thickness), new Point(length, thickness));
            }
        }

        private class RightArrowBuilder : BlockArrowGeometrySource.HorizontalArrowBuilder
        {
            public override Point ComputePointA(double length, double width)
            {
                return new Point(length, width / 2.0);
            }

            public override Point ComputePointB(double length, double offset)
            {
                return new Point(length - offset, 0.0);
            }

            public override Tuple<Point, Point> ComputePointCD(double length, double offset, double thickness)
            {
                return new Tuple<Point, Point>(new Point(length - offset, thickness), new Point(0.0, thickness));
            }
        }

        private class UpArrowBuilder : BlockArrowGeometrySource.VerticalArrowBuilder
        {
            public override Point ComputePointA(double length, double width)
            {
                return new Point(width / 2.0, 0.0);
            }

            public override Point ComputePointB(double length, double offset)
            {
                return new Point(0.0, offset);
            }

            public override Tuple<Point, Point> ComputePointCD(double length, double offset, double thickness)
            {
                return new Tuple<Point, Point>(new Point(thickness, offset), new Point(thickness, length));
            }
        }

        private class DownArrowBuilder : BlockArrowGeometrySource.VerticalArrowBuilder
        {
            public override Point ComputePointA(double length, double width)
            {
                return new Point(width / 2.0, length);
            }

            public override Point ComputePointB(double length, double offset)
            {
                return new Point(0.0, length - offset);
            }

            public override Tuple<Point, Point> ComputePointCD(double length, double offset, double thickness)
            {
                return new Tuple<Point, Point>(new Point(thickness, length - offset), new Point(thickness, 0.0));
            }
        }
    }
}
