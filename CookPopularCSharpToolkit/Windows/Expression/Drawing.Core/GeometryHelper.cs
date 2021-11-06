using CookPopularCSharpToolkit.Communal;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：GeometryHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:43:06
 */
namespace CookPopularCSharpToolkit.Windows.Expression
{
    /// <summary>
    /// Extension methods for geometry-related data structures (Point/Vector/Size/Rect).
    /// </summary>
    public static class GeometryHelper
    {
        internal static Rect Bounds(this Size size)
        {
            return new Rect(0.0, 0.0, size.Width, size.Height);
        }

        internal static bool HasValidArea(this Size size)
        {
            return size.Width > 0.0 && size.Height > 0.0 && !double.IsInfinity(size.Width) && !double.IsInfinity(size.Height);
        }

        internal static Size Size(this Rect rect)
        {
            return new Size(rect.Width, rect.Height);
        }

        internal static Point TopLeft(this Rect rect)
        {
            return new Point(rect.Left, rect.Top);
        }

        internal static Point TopRight(this Rect rect)
        {
            return new Point(rect.Right, rect.Top);
        }

        internal static Point BottomRight(this Rect rect)
        {
            return new Point(rect.Right, rect.Bottom);
        }

        internal static Point BottomLeft(this Rect rect)
        {
            return new Point(rect.Left, rect.Bottom);
        }

        internal static Point Center(this Rect rect)
        {
            return new Point(rect.X + rect.Width / 2.0, rect.Y + rect.Height / 2.0);
        }

        internal static Thickness Subtract(this Rect lhs, Rect rhs)
        {
            return new Thickness(rhs.Left - lhs.Left, rhs.Top - lhs.Top, lhs.Right - rhs.Right, lhs.Bottom - rhs.Bottom);
        }

        /// <summary>
        /// Resizes the rectangle to a relative size while keeping the center invariant.
        /// </summary>
        internal static Rect Resize(this Rect rect, double ratio)
        {
            return rect.Resize(ratio, ratio);
        }

        internal static Rect Resize(this Rect rect, double ratioX, double ratioY)
        {
            Point point = rect.Center();
            double num = rect.Width * ratioX;
            double num2 = rect.Height * ratioY;
            return new Rect(point.X - num / 2.0, point.Y - num2 / 2.0, num, num2);
        }

        internal static Rect ActualBounds(this FrameworkElement element)
        {
            return new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight);
        }

        internal static Thickness Negate(this Thickness thickness)
        {
            return new Thickness(-thickness.Left, -thickness.Top, -thickness.Right, -thickness.Bottom);
        }

        /// <summary>
        /// Gets the difference vector between two points.
        /// </summary>
        internal static Vector Subtract(this Point lhs, Point rhs)
        {
            return new Vector(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }

        /// <summary>
        /// Memberwise plus for Point.
        /// </summary>
        internal static Point Plus(this Point lhs, Point rhs)
        {
            return new Point(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }

        /// <summary>
        /// Memberwise minus for Point.
        /// </summary>
        internal static Point Minus(this Point lhs, Point rhs)
        {
            return new Point(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }

        internal static Vector Normalized(this Vector vector)
        {
            Vector vector2 = new Vector(vector.X, vector.Y);
            double length = vector2.Length;
            if (MathHelper.IsVerySmall(length))
            {
                return new Vector(0.0, 1.0);
            }
            return vector2 / length;
        }

        internal static void ApplyTransform(this IList<Point> points, GeneralTransform transform)
        {
            for (int i = 0; i < points.Count; i++)
            {
                points[i] = transform.Transform(points[i]);
            }
        }

        /// <summary>
        /// Converts a string of mini-languages to a <see cref="T:PathGeometry" />.
        /// </summary>
        /// <remarks>See: Path Markup Syntax(http://msdn.microsoft.com/en-us/library/cc189041(VS.95).aspx)</remarks>
        /// <param name="abbreviatedGeometry">The string of path mini-languages for describing geometric paths.</param>
        /// <returns>A <see cref="T:PathGeometry" /> converted from the the path mini-languages.</returns>
        public static PathGeometry ConvertToPathGeometry(string abbreviatedGeometry)
        {
            return PathGeometryHelper.ConvertToPathGeometry(abbreviatedGeometry);
        }

        /// <summary>
        /// Flattens a <see cref="T:PathFigure" /> and adds result points to a given <paramref name="points" />.
        /// </summary>
        /// <param name="figure">The input <see cref="T:PathFigure" />.</param>
        /// <param name="points">The point list to which result points will append.</param>
        /// <param name="tolerance">A positive number specifying the maximum allowed error from the result points to the input path figure. A Value of zero allows the algorithm to pick the tolerance automatically.</param>
        public static void FlattenFigure(PathFigure figure, IList<Point> points, double tolerance)
        {
            bool removeRepeat = false;
            PathFigureHelper.FlattenFigure(figure, points, tolerance, removeRepeat);
        }

        internal static Point Lerp(Point pointA, Point pointB, double alpha)
        {
            return new Point(MathHelper.Lerp(pointA.X, pointB.X, alpha), MathHelper.Lerp(pointA.Y, pointB.Y, alpha));
        }

        internal static Vector Lerp(Vector vectorA, Vector vectorB, double alpha)
        {
            return new Vector(MathHelper.Lerp(vectorA.X, vectorB.X, alpha), MathHelper.Lerp(vectorA.Y, vectorB.Y, alpha));
        }

        internal static Rect Inflate(Rect rect, double offset)
        {
            return GeometryHelper.Inflate(rect, new Thickness(offset));
        }

        internal static Rect Inflate(Rect rect, double offsetX, double offsetY)
        {
            return GeometryHelper.Inflate(rect, new Thickness(offsetX, offsetY, offsetX, offsetY));
        }

        internal static Rect Inflate(Rect rect, Size size)
        {
            return GeometryHelper.Inflate(rect, new Thickness(size.Width, size.Height, size.Width, size.Height));
        }

        internal static Rect Inflate(Rect rect, Thickness thickness)
        {
            double num = rect.Width + thickness.Left + thickness.Right;
            double num2 = rect.Height + thickness.Top + thickness.Bottom;
            double num3 = rect.X - thickness.Left;
            if (num < 0.0)
            {
                num3 += num / 2.0;
                num = 0.0;
            }
            double num4 = rect.Y - thickness.Top;
            if (num2 < 0.0)
            {
                num4 += num2 / 2.0;
                num2 = 0.0;
            }
            return new Rect(num3, num4, num, num2);
        }

        /// <summary>
        /// Gets the normalized arc in a (0,0)(1,1) box.
        /// Zero degrees is mapped to [0.5, 0] (up), and clockwise.
        /// </summary>
        internal static Point GetArcPoint(double degree)
        {
            double num = degree * 3.141592653589793 / 180.0;
            return new Point(0.5 + 0.5 * Math.Sin(num), 0.5 - 0.5 * Math.Cos(num));
        }

        /// <summary>
        /// Gets the absolute arc point in a given bound with a given relative radius.
        /// </summary>
        internal static Point GetArcPoint(double degree, Rect bound)
        {
            Point arcPoint = GeometryHelper.GetArcPoint(degree);
            return GeometryHelper.RelativeToAbsolutePoint(bound, arcPoint);
        }

        /// <summary>
        /// Gets the angle on an arc relative to a (0,0)(1,1) box.
        /// Zero degrees is mapped to [0.5, 0] (up), and clockwise.
        /// </summary>
        internal static double GetArcAngle(Point point)
        {
            return Math.Atan2(point.Y - 0.5, point.X - 0.5) * 180.0 / 3.141592653589793 + 90.0;
        }

        /// <summary>
        /// Gets the angle on an arc from a given absolute point relative to a bound.
        /// </summary>
        internal static double GetArcAngle(Point point, Rect bound)
        {
            Point point2 = GeometryHelper.AbsoluteToRelativePoint(bound, point);
            return GeometryHelper.GetArcAngle(point2);
        }

        /// <summary>
        /// Computes the transform that moves "Rect from" to "Rect to".
        /// </summary>
        internal static Transform RelativeTransform(Rect from, Rect to)
        {
            Point point = from.Center();
            Point point2 = to.Center();
            return new TransformGroup
            {
                Children = new TransformCollection
                {
                    new TranslateTransform
                    {
                        X = -point.X,
                        Y = -point.Y
                    },
                    new ScaleTransform
                    {
                        ScaleX = MathHelper.SafeDivide(to.Width, from.Width, 1.0),
                        ScaleY = MathHelper.SafeDivide(to.Height, from.Height, 1.0)
                    },
                    new TranslateTransform
                    {
                        X = point2.X,
                        Y = point2.Y
                    }
                }
            };
        }

        /// <summary>
        /// Computes the transform from the coordinate space of one <c>UIElement</c> to another.
        /// </summary>
        /// <param name="from">The source element.</param>
        /// <param name="to">The destination element.</param>
        /// <returns>The transform between the <c>UIElement</c>s, or null if it cannot be computed.</returns>
        internal static GeneralTransform RelativeTransform(UIElement from, UIElement to)
        {
            if (from == null || to == null)
            {
                return null;
            }
            GeneralTransform result;
            try
            {
                result = from.TransformToVisual(to);
            }
            catch (ArgumentException)
            {
                result = null;
            }
            catch (InvalidOperationException)
            {
                result = null;
            }
            return result;
        }

        internal static Point SafeTransform(GeneralTransform transform, Point point)
        {
            Point result = point;
            if (transform != null && transform.TryTransform(point, out result))
            {
                return result;
            }
            return point;
        }

        /// <summary>
        /// Maps a relative point to an absolute point using the mapping from a given bound to a (0,0)(1,1) box.
        /// </summary>
        internal static Point RelativeToAbsolutePoint(Rect bound, Point relative)
        {
            return new Point(bound.X + relative.X * bound.Width, bound.Y + relative.Y * bound.Height);
        }

        /// <summary>
        /// Maps an absolute point to a relative point using the mapping from a (0,0)(1,1) box to a given bound.
        /// </summary>
        internal static Point AbsoluteToRelativePoint(Rect bound, Point absolute)
        {
            return new Point(MathHelper.SafeDivide(absolute.X - bound.X, bound.Width, 1.0), MathHelper.SafeDivide(absolute.Y - bound.Y, bound.Height, 1.0));
        }

        /// <summary>
        /// Computes the bound after stretching within a given logical bound.
        /// If stretch to uniform, use given aspectRatio.
        /// If aspectRatio is empty, it's equivalent to Fill.
        /// If stretch is None, it's equivalent to Fill or Uniform.
        /// </summary>
        internal static Rect GetStretchBound(Rect logicalBound, Stretch stretch, Size aspectRatio)
        {
            if (stretch == Stretch.None)
            {
                stretch = Stretch.Fill;
            }
            if (stretch == Stretch.Fill || !aspectRatio.HasValidArea())
            {
                return logicalBound;
            }
            Point point = logicalBound.Center();
            if (stretch == Stretch.Uniform)
            {
                if (aspectRatio.Width * logicalBound.Height < logicalBound.Width * aspectRatio.Height)
                {
                    logicalBound.Width = logicalBound.Height * aspectRatio.Width / aspectRatio.Height;
                }
                else
                {
                    logicalBound.Height = logicalBound.Width * aspectRatio.Height / aspectRatio.Width;
                }
            }
            else if (stretch == Stretch.UniformToFill)
            {
                if (aspectRatio.Width * logicalBound.Height < logicalBound.Width * aspectRatio.Height)
                {
                    logicalBound.Height = logicalBound.Width * aspectRatio.Height / aspectRatio.Width;
                }
                else
                {
                    logicalBound.Width = logicalBound.Height * aspectRatio.Width / aspectRatio.Height;
                }
            }
            return new Rect(point.X - logicalBound.Width / 2.0, point.Y - logicalBound.Height / 2.0, logicalBound.Width, logicalBound.Height);
        }

        /// <summary>
        /// Returns the mid point of two points.
        /// </summary>
        /// <param name="lhs">The first point.</param>
        /// <param name="rhs">The second point.</param>
        /// <returns>The mid point between <paramref name="lhs" /> and <paramref name="rhs" />.</returns>
        internal static Point Midpoint(Point lhs, Point rhs)
        {
            return new Point((lhs.X + rhs.X) / 2.0, (lhs.Y + rhs.Y) / 2.0);
        }

        /// <summary>
        /// Returns the dot product of two vectors.
        /// </summary>
        /// <param name="lhs">The first vector.</param>
        /// <param name="rhs">The second vector.</param>
        /// <returns>The dot product of <paramref name="lhs" /> and <paramref name="rhs" />.</returns>
        internal static double Dot(Vector lhs, Vector rhs)
        {
            return lhs.X * rhs.X + lhs.Y * rhs.Y;
        }

        /// <summary>
        /// Returns the dot product of two points.
        /// </summary>
        internal static double Dot(Point lhs, Point rhs)
        {
            return lhs.X * rhs.X + lhs.Y * rhs.Y;
        }

        /// <summary>
        /// Returns the distance between two points.
        /// </summary>
        /// <param name="lhs">The first point.</param>
        /// <param name="rhs">The second point.</param>
        /// <returns>The distance between <paramref name="lhs" /> and <paramref name="rhs" />.</returns>
        internal static double Distance(Point lhs, Point rhs)
        {
            double num = lhs.X - rhs.X;
            double num2 = lhs.Y - rhs.Y;
            return Math.Sqrt(num * num + num2 * num2);
        }

        /// <summary>
        /// Returns the square of the distance between two points.
        /// </summary>
        /// <param name="lhs">The first point.</param>
        /// <param name="rhs">The second point.</param>
        /// <returns>The square of the distance between <paramref name="lhs" /> and <paramref name="rhs" />.</returns>
        internal static double SquaredDistance(Point lhs, Point rhs)
        {
            double num = lhs.X - rhs.X;
            double num2 = lhs.Y - rhs.Y;
            return num * num + num2 * num2;
        }

        /// <summary>
        /// Determinant of the cross product. Equivalent to directional area.
        /// </summary>
        internal static double Determinant(Point lhs, Point rhs)
        {
            return lhs.X * rhs.Y - lhs.Y * rhs.X;
        }

        /// <summary>
        /// Computes the normal direction vector of given line segments.
        /// </summary>
        internal static Vector Normal(Point lhs, Point rhs)
        {
            return new Vector(lhs.Y - rhs.Y, rhs.X - lhs.X).Normalized();
        }

        /// <summary>
        /// Computes the perpendicular vector, 90-degrees, counter-clockwise.
        /// Vector to the right perpendicular results in a vector to up.
        /// </summary>
        internal static Vector Perpendicular(this Vector vector)
        {
            return new Vector(-vector.Y, vector.X);
        }

        /// <summary>
        /// Returns whether the two geometries are identical.
        /// </summary>
        internal static bool GeometryEquals(Geometry firstGeometry, Geometry secondGeometry)
        {
            if (firstGeometry == secondGeometry)
            {
                return true;
            }
            if (firstGeometry == null || secondGeometry == null)
            {
                return false;
            }
            if (firstGeometry.GetType() != secondGeometry.GetType())
            {
                return false;
            }
            if (!firstGeometry.Transform.TransformEquals(secondGeometry.Transform))
            {
                return false;
            }
            StreamGeometry streamGeometry = firstGeometry as StreamGeometry;
            StreamGeometry streamGeometry2 = secondGeometry as StreamGeometry;
            if (streamGeometry != null && streamGeometry2 != null)
            {
                return streamGeometry.ToString() == streamGeometry2.ToString();
            }
            PathGeometry pathGeometry = firstGeometry as PathGeometry;
            PathGeometry pathGeometry2 = secondGeometry as PathGeometry;
            if (pathGeometry != null && pathGeometry2 != null)
            {
                return GeometryHelper.PathGeometryEquals(pathGeometry, pathGeometry2);
            }
            LineGeometry lineGeometry = firstGeometry as LineGeometry;
            LineGeometry lineGeometry2 = secondGeometry as LineGeometry;
            if (lineGeometry != null && lineGeometry2 != null)
            {
                return GeometryHelper.LineGeometryEquals(lineGeometry, lineGeometry2);
            }
            RectangleGeometry rectangleGeometry = firstGeometry as RectangleGeometry;
            RectangleGeometry rectangleGeometry2 = secondGeometry as RectangleGeometry;
            if (rectangleGeometry != null && rectangleGeometry2 != null)
            {
                return GeometryHelper.RectangleGeometryEquals(rectangleGeometry, rectangleGeometry2);
            }
            EllipseGeometry ellipseGeometry = firstGeometry as EllipseGeometry;
            EllipseGeometry ellipseGeometry2 = secondGeometry as EllipseGeometry;
            if (ellipseGeometry != null && ellipseGeometry2 != null)
            {
                return GeometryHelper.EllipseGeometryEquals(ellipseGeometry, ellipseGeometry2);
            }
            GeometryGroup geometryGroup = firstGeometry as GeometryGroup;
            GeometryGroup geometryGroup2 = secondGeometry as GeometryGroup;
            return geometryGroup != null && geometryGroup2 != null && GeometryHelper.GeometryGroupEquals(geometryGroup, geometryGroup2);
        }

        internal static bool PathGeometryEquals(PathGeometry firstGeometry, PathGeometry secondGeometry)
        {
            if (firstGeometry.FillRule != secondGeometry.FillRule)
            {
                return false;
            }
            if (firstGeometry.Figures.Count != secondGeometry.Figures.Count)
            {
                return false;
            }
            for (int i = 0; i < firstGeometry.Figures.Count; i++)
            {
                if (!GeometryHelper.PathFigureEquals(firstGeometry.Figures[i], secondGeometry.Figures[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool PathFigureEquals(PathFigure firstFigure, PathFigure secondFigure)
        {
            if (firstFigure.IsClosed != secondFigure.IsClosed)
            {
                return false;
            }
            if (firstFigure.IsFilled != secondFigure.IsFilled)
            {
                return false;
            }
            if (firstFigure.StartPoint != secondFigure.StartPoint)
            {
                return false;
            }
            for (int i = 0; i < firstFigure.Segments.Count; i++)
            {
                if (!GeometryHelper.PathSegmentEquals(firstFigure.Segments[i], secondFigure.Segments[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool PathSegmentEquals(PathSegment firstSegment, PathSegment secondSegment)
        {
            if (firstSegment == secondSegment)
            {
                return true;
            }
            if (firstSegment == null || secondSegment == null)
            {
                return false;
            }
            if (firstSegment.GetType() != secondSegment.GetType())
            {
                return false;
            }
            if (firstSegment.IsStroked() != secondSegment.IsStroked())
            {
                return false;
            }
            if (firstSegment.IsSmoothJoin() != secondSegment.IsSmoothJoin())
            {
                return false;
            }
            LineSegment lineSegment = firstSegment as LineSegment;
            LineSegment lineSegment2 = secondSegment as LineSegment;
            if (lineSegment != null && lineSegment2 != null)
            {
                return GeometryHelper.LineSegmentEquals(lineSegment, lineSegment2);
            }
            BezierSegment bezierSegment = firstSegment as BezierSegment;
            BezierSegment bezierSegment2 = secondSegment as BezierSegment;
            if (bezierSegment != null && bezierSegment2 != null)
            {
                return GeometryHelper.BezierSegmentEquals(bezierSegment, bezierSegment2);
            }
            QuadraticBezierSegment quadraticBezierSegment = firstSegment as QuadraticBezierSegment;
            QuadraticBezierSegment quadraticBezierSegment2 = secondSegment as QuadraticBezierSegment;
            if (quadraticBezierSegment != null && quadraticBezierSegment2 != null)
            {
                return GeometryHelper.QuadraticBezierSegmentEquals(quadraticBezierSegment, quadraticBezierSegment2);
            }
            ArcSegment arcSegment = firstSegment as ArcSegment;
            ArcSegment arcSegment2 = secondSegment as ArcSegment;
            if (arcSegment != null && arcSegment2 != null)
            {
                return GeometryHelper.ArcSegmentEquals(arcSegment, arcSegment2);
            }
            PolyLineSegment polyLineSegment = firstSegment as PolyLineSegment;
            PolyLineSegment polyLineSegment2 = secondSegment as PolyLineSegment;
            if (polyLineSegment != null && polyLineSegment2 != null)
            {
                return GeometryHelper.PolyLineSegmentEquals(polyLineSegment, polyLineSegment2);
            }
            PolyBezierSegment polyBezierSegment = firstSegment as PolyBezierSegment;
            PolyBezierSegment polyBezierSegment2 = secondSegment as PolyBezierSegment;
            if (polyBezierSegment != null && polyBezierSegment2 != null)
            {
                return GeometryHelper.PolyBezierSegmentEquals(polyBezierSegment, polyBezierSegment2);
            }
            PolyQuadraticBezierSegment polyQuadraticBezierSegment = firstSegment as PolyQuadraticBezierSegment;
            PolyQuadraticBezierSegment polyQuadraticBezierSegment2 = secondSegment as PolyQuadraticBezierSegment;
            return polyQuadraticBezierSegment != null && polyQuadraticBezierSegment2 != null && GeometryHelper.PolyQuadraticBezierSegmentEquals(polyQuadraticBezierSegment, polyQuadraticBezierSegment2);
        }

        private static bool LineSegmentEquals(LineSegment firstLineSegment, LineSegment secondLineSegment)
        {
            return firstLineSegment.Point == secondLineSegment.Point;
        }

        private static bool BezierSegmentEquals(BezierSegment firstBezierSegment, BezierSegment secondBezierSegment)
        {
            return firstBezierSegment.Point1 == secondBezierSegment.Point1 && firstBezierSegment.Point2 == secondBezierSegment.Point2 && firstBezierSegment.Point3 == secondBezierSegment.Point3;
        }

        private static bool QuadraticBezierSegmentEquals(QuadraticBezierSegment firstQuadraticBezierSegment, QuadraticBezierSegment secondQuadraticBezierSegment)
        {
            return firstQuadraticBezierSegment.Point1 == secondQuadraticBezierSegment.Point1 && firstQuadraticBezierSegment.Point1 == secondQuadraticBezierSegment.Point1;
        }

        private static bool ArcSegmentEquals(ArcSegment firstArcSegment, ArcSegment secondArcSegment)
        {
            return firstArcSegment.Point == secondArcSegment.Point && firstArcSegment.IsLargeArc == secondArcSegment.IsLargeArc && firstArcSegment.RotationAngle == secondArcSegment.RotationAngle && firstArcSegment.Size == secondArcSegment.Size && firstArcSegment.SweepDirection == secondArcSegment.SweepDirection;
        }

        private static bool PolyLineSegmentEquals(PolyLineSegment firstPolyLineSegment, PolyLineSegment secondPolyLineSegment)
        {
            if (firstPolyLineSegment.Points.Count != secondPolyLineSegment.Points.Count)
            {
                return false;
            }
            for (int i = 0; i < firstPolyLineSegment.Points.Count; i++)
            {
                if (firstPolyLineSegment.Points[i] != secondPolyLineSegment.Points[i])
                {
                    return false;
                }
            }
            return true;
        }

        private static bool PolyBezierSegmentEquals(PolyBezierSegment firstPolyBezierSegment, PolyBezierSegment secondPolyBezierSegment)
        {
            if (firstPolyBezierSegment.Points.Count != secondPolyBezierSegment.Points.Count)
            {
                return false;
            }
            for (int i = 0; i < firstPolyBezierSegment.Points.Count; i++)
            {
                if (firstPolyBezierSegment.Points[i] != secondPolyBezierSegment.Points[i])
                {
                    return false;
                }
            }
            return true;
        }

        private static bool PolyQuadraticBezierSegmentEquals(PolyQuadraticBezierSegment firstPolyQuadraticBezierSegment, PolyQuadraticBezierSegment secondPolyQuadraticBezierSegment)
        {
            if (firstPolyQuadraticBezierSegment.Points.Count != secondPolyQuadraticBezierSegment.Points.Count)
            {
                return false;
            }
            for (int i = 0; i < firstPolyQuadraticBezierSegment.Points.Count; i++)
            {
                if (firstPolyQuadraticBezierSegment.Points[i] != secondPolyQuadraticBezierSegment.Points[i])
                {
                    return false;
                }
            }
            return true;
        }

        private static bool EllipseGeometryEquals(EllipseGeometry firstGeometry, EllipseGeometry secondGeometry)
        {
            return firstGeometry.Center == secondGeometry.Center && firstGeometry.RadiusX == secondGeometry.RadiusX && firstGeometry.RadiusY == secondGeometry.RadiusY;
        }

        private static bool RectangleGeometryEquals(RectangleGeometry firstGeometry, RectangleGeometry secondGeometry)
        {
            return firstGeometry.Rect == secondGeometry.Rect && firstGeometry.RadiusX == secondGeometry.RadiusX && firstGeometry.RadiusY == secondGeometry.RadiusY;
        }

        private static bool LineGeometryEquals(LineGeometry firstGeometry, LineGeometry secondGeometry)
        {
            return firstGeometry.StartPoint == secondGeometry.StartPoint && firstGeometry.EndPoint == secondGeometry.EndPoint;
        }

        private static bool GeometryGroupEquals(GeometryGroup firstGeometry, GeometryGroup secondGeometry)
        {
            if (firstGeometry.FillRule != secondGeometry.FillRule)
            {
                return false;
            }
            if (firstGeometry.Children.Count != secondGeometry.Children.Count)
            {
                return false;
            }
            for (int i = 0; i < firstGeometry.Children.Count; i++)
            {
                if (!GeometryHelper.GeometryEquals(firstGeometry.Children[i], secondGeometry.Children[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Ensures the value is an instance of result type (T). If not, replace with a new instance of type (T).
        /// </summary>
        internal static bool EnsureGeometryType<T>(out T result, ref Geometry value, Func<T> factory) where T : Geometry
        {
            result = (value as T);
            if (result == null)
            {
                value = (result = factory());
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ensures the list[index] is an instance of result type (T). If not, replace with a new instance of type (T).
        /// </summary>
        internal static bool EnsureSegmentType<T>(out T result, IList<PathSegment> list, int index, Func<T> factory) where T : PathSegment
        {
            result = (list[index] as T);
            if (result == null)
            {
                list[index] = (result = factory());
                return true;
            }
            return false;
        }
    }
}
