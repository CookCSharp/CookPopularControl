using CookPopularCSharpToolkit.Communal;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CalloutGeometrySource
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:33:22
 */
namespace CookPopularControl.Expression
{
    public class CalloutGeometrySource : GeometrySource<ICalloutGeometrySourceParameters>
    {
        protected override bool UpdateCachedGeometry(ICalloutGeometrySourceParameters parameters)
        {
            bool flag = false;
            switch (parameters.CalloutStyle)
            {
                case CalloutStyle.Rectangle:
                    flag |= this.UpdateRectangleCallout(parameters);
                    break;
                case CalloutStyle.RoundedRectangle:
                    flag |= this.UpdateRoundedRectangleCallout(parameters);
                    break;
                default:
                    flag |= this.UpdateOvalCallout(parameters);
                    break;
                case CalloutStyle.Cloud:
                    flag |= this.UpdateCloudCallout(parameters);
                    break;
            }
            if (flag)
            {
                this.cachedGeometry = PathGeometryHelper.FixPathGeometryBoundary(this.cachedGeometry);
            }
            return flag;
        }

        private bool UpdateOvalCallout(ICalloutGeometrySourceParameters parameters)
        {
            bool flag = false;
            if (CalloutGeometrySource.IsInside(parameters.CalloutStyle, parameters.AnchorPoint))
            {
                EllipseGeometry? ellipseGeometry = this.cachedGeometry as EllipseGeometry;
                if (ellipseGeometry == null)
                {
                    ellipseGeometry = (EllipseGeometry)(this.cachedGeometry = new EllipseGeometry());
                    flag = true;
                }
                flag |= ellipseGeometry.SetIfDifferent(EllipseGeometry.CenterProperty, base.LogicalBounds.Center());
                flag |= ellipseGeometry.SetIfDifferent(EllipseGeometry.RadiusXProperty, base.LogicalBounds.Width / 2.0);
                flag |= ellipseGeometry.SetIfDifferent(EllipseGeometry.RadiusYProperty, base.LogicalBounds.Height / 2.0);
            }
            else
            {
                PathGeometry? pathGeometry = this.cachedGeometry as PathGeometry;
                PathFigure pathFigure;
                ArcSegment arcSegment;
                LineSegment dependencyObject;
                if (pathGeometry == null || pathGeometry.Figures.Count != 1 || (pathFigure = pathGeometry.Figures[0]).Segments.Count != 2 || (arcSegment = (pathFigure.Segments[0] as ArcSegment)) == null || (dependencyObject = (pathFigure.Segments[1] as LineSegment)) == null)
                {
                    pathGeometry = (PathGeometry)(this.cachedGeometry = new PathGeometry());
                    pathGeometry.Figures.Add(pathFigure = new PathFigure());
                    pathFigure.Segments.Add(arcSegment = new ArcSegment());
                    pathFigure.Segments.Add(dependencyObject = new LineSegment());
                    pathFigure.IsClosed = true;
                    arcSegment.IsLargeArc = true;
                    arcSegment.SweepDirection = SweepDirection.Clockwise;
                    flag = true;
                }
                double arcAngle = GeometryHelper.GetArcAngle(parameters.AnchorPoint);
                double degree = arcAngle + 10.0;
                double degree2 = arcAngle - 10.0;
                flag |= pathFigure.SetIfDifferent(PathFigure.StartPointProperty, GeometryHelper.GetArcPoint(degree, base.LogicalBounds));
                flag |= arcSegment.SetIfDifferent(ArcSegment.PointProperty, GeometryHelper.GetArcPoint(degree2, base.LogicalBounds));
                flag |= arcSegment.SetIfDifferent(ArcSegment.SizeProperty, base.LogicalBounds.Resize(0.5).Size());
                flag |= dependencyObject.SetIfDifferent(LineSegment.PointProperty, this.GetAbsoluteAnchorPoint(parameters.AnchorPoint));
                this.cachedGeometry = pathGeometry;
            }
            return flag;
        }

        private static Point ClosestConnectionPoint(Point relativePoint)
        {
            double num = double.MaxValue;
            Point result = CalloutGeometrySource.connectionPoints[0];
            foreach (Point point in CalloutGeometrySource.connectionPoints)
            {
                double num2 = GeometryHelper.Distance(relativePoint, point);
                if (num > num2)
                {
                    num = num2;
                    result = point;
                }
            }
            return result;
        }

        /// <summary>
        /// Updates the edge line, and then connects to the anchor point if necessary.
        /// </summary>
        private static bool UpdateEdge(PathSegmentCollection segments, int index, Point start, Point end, Point anchorPoint, double connection, bool connectToAnchor)
        {
            bool flag = false;
            if (connectToAnchor)
            {
                flag |= CalloutGeometrySource.UpdatePolylineSegment(segments, index, start, end, anchorPoint, connection);
            }
            else
            {
                flag |= CalloutGeometrySource.UpdateLineSegment(segments, index, end);
            }
            return flag;
        }

        /// <summary>
        /// Updates the polyline segment, and then connects start, anchor, and end points with the callout style.
        /// </summary>
        private static bool UpdatePolylineSegment(PathSegmentCollection segments, int index, Point start, Point end, Point anchor, double connection)
        {
            bool flag = false;
            Point[] array = new Point[]
            {
                GeometryHelper.Lerp(start, end, connection - 0.1),
                anchor,
                GeometryHelper.Lerp(start, end, connection + 0.1),
                end
            };
            return flag | PathSegmentHelper.SyncPolylineSegment(segments, index, array, 0, array.Length);
        }

        /// <summary>
        /// Updates the line segment to a given point.
        /// </summary>
        private static bool UpdateLineSegment(PathSegmentCollection segments, int index, Point point)
        {
            bool flag = false;
            LineSegment? lineSegment = segments[index] as LineSegment;
            if (lineSegment == null)
            {
                lineSegment = (LineSegment)(segments[index] = new LineSegment());
                flag = true;
            }
            return flag | lineSegment.SetIfDifferent(LineSegment.PointProperty, point);
        }

        private bool UpdateRectangleCallout(ICalloutGeometrySourceParameters parameters)
        {
            bool flag = false;
            PathGeometry? pathGeometry = this.cachedGeometry as PathGeometry;
            PathFigure pathFigure;
            PathSegmentCollection segments;
            if (pathGeometry == null || pathGeometry.Figures.Count != 1 || (pathFigure = pathGeometry.Figures[0]) == null || (segments = pathFigure.Segments).Count != 4)
            {
                pathGeometry = (PathGeometry)(this.cachedGeometry = new PathGeometry());
                segments = new PathSegmentCollection
                {
                    new LineSegment(),
                    new LineSegment(),
                    new LineSegment(),
                    new LineSegment()
                };
                pathFigure = new PathFigure
                {
                    Segments = segments
                };
                pathGeometry.Figures.Add(pathFigure);
                flag = true;
            }
            Point anchorPoint = parameters.AnchorPoint;
            Point point = CalloutGeometrySource.ClosestConnectionPoint(anchorPoint);
            bool flag2 = CalloutGeometrySource.IsInside(parameters.CalloutStyle, anchorPoint);
            Point absoluteAnchorPoint = this.GetAbsoluteAnchorPoint(anchorPoint);
            flag |= pathFigure.SetIfDifferent(PathFigure.StartPointProperty, base.LogicalBounds.TopLeft());
            flag |= pathFigure.SetIfDifferent(PathFigure.IsClosedProperty, true);
            flag |= CalloutGeometrySource.UpdateEdge(segments, 0, base.LogicalBounds.TopLeft(), base.LogicalBounds.TopRight(), absoluteAnchorPoint, point.X, !flag2 && point.Y == 0.0);
            flag |= CalloutGeometrySource.UpdateEdge(segments, 1, base.LogicalBounds.TopRight(), base.LogicalBounds.BottomRight(), absoluteAnchorPoint, point.Y, !flag2 && point.X == 1.0);
            flag |= CalloutGeometrySource.UpdateEdge(segments, 2, base.LogicalBounds.BottomRight(), base.LogicalBounds.BottomLeft(), absoluteAnchorPoint, 1.0 - point.X, !flag2 && point.Y == 1.0);
            return flag | CalloutGeometrySource.UpdateEdge(segments, 3, base.LogicalBounds.BottomLeft(), base.LogicalBounds.TopLeft(), absoluteAnchorPoint, 1.0 - point.Y, !flag2 && point.X == 0.0);
        }

        /// <summary>
        /// Computes the corner points in a clockwise direction, with eight points for the four corners.
        /// </summary>
        // Token: 0x06000217 RID: 535 RVA: 0x0000C53C File Offset: 0x0000A73C
        private Point[] ComputeCorners(double radius)
        {
            double left = base.LogicalBounds.Left;
            double top = base.LogicalBounds.Top;
            double right = base.LogicalBounds.Right;
            double bottom = base.LogicalBounds.Bottom;
            return new Point[]
            {
                new Point(left, top + radius),
                new Point(left + radius, top),
                new Point(right - radius, top),
                new Point(right, top + radius),
                new Point(right, bottom - radius),
                new Point(right - radius, bottom),
                new Point(left + radius, bottom),
                new Point(left, bottom - radius)
            };
        }

        /// <summary>
        /// The corner arc is always smaller than a 90-degree arc.
        /// </summary>
        private static bool UpdateCornerArc(PathSegmentCollection segments, int index, Point start, Point end)
        {
            bool flag = false;
            ArcSegment? arcSegment = segments[index] as ArcSegment;
            if (arcSegment == null)
            {
                arcSegment = (ArcSegment)(segments[index] = new ArcSegment());
                flag = true;
            }
            double width = Math.Abs(end.X - start.X);
            double height = Math.Abs(end.Y - start.Y);
            flag |= arcSegment.SetIfDifferent(ArcSegment.IsLargeArcProperty, false);
            flag |= arcSegment.SetIfDifferent(ArcSegment.PointProperty, end);
            flag |= arcSegment.SetIfDifferent(ArcSegment.SizeProperty, new Size(width, height));
            return flag | arcSegment.SetIfDifferent(ArcSegment.SweepDirectionProperty, SweepDirection.Clockwise);
        }

        private bool UpdateRoundedRectangleCallout(ICalloutGeometrySourceParameters parameters)
        {
            bool flag = false;
            double radius = Math.Min(base.LogicalBounds.Width, base.LogicalBounds.Height) / 10.0;
            Point[] array = this.ComputeCorners(radius);
            PathGeometry? pathGeometry = this.cachedGeometry as PathGeometry;
            PathFigure pathFigure;
            PathSegmentCollection segments;
            if (pathGeometry == null || pathGeometry.Figures.Count != 1 || (pathFigure = pathGeometry.Figures[0]) == null || (segments = pathFigure.Segments).Count != 8)
            {
                pathGeometry = (PathGeometry)(this.cachedGeometry = new PathGeometry());
                segments = new PathSegmentCollection
                {
                    new ArcSegment(),
                    new LineSegment(),
                    new ArcSegment(),
                    new LineSegment(),
                    new ArcSegment(),
                    new LineSegment(),
                    new ArcSegment(),
                    new LineSegment()
                };
                pathFigure = new PathFigure
                {
                    Segments = segments
                };
                pathGeometry.Figures.Add(pathFigure);
                flag = true;
            }
            Point anchorPoint = parameters.AnchorPoint;
            Point point = CalloutGeometrySource.ClosestConnectionPoint(anchorPoint);
            bool flag2 = CalloutGeometrySource.IsInside(parameters.CalloutStyle, anchorPoint);
            Point absoluteAnchorPoint = this.GetAbsoluteAnchorPoint(anchorPoint);
            flag |= pathFigure.SetIfDifferent(PathFigure.StartPointProperty, array[0]);
            flag |= pathFigure.SetIfDifferent(PathFigure.IsClosedProperty, true);
            flag |= CalloutGeometrySource.UpdateCornerArc(segments, 0, array[0], array[1]);
            flag |= CalloutGeometrySource.UpdateEdge(segments, 1, array[1], array[2], absoluteAnchorPoint, point.X, !flag2 && point.Y == 0.0);
            flag |= CalloutGeometrySource.UpdateCornerArc(segments, 2, array[2], array[3]);
            flag |= CalloutGeometrySource.UpdateEdge(segments, 3, array[3], array[4], absoluteAnchorPoint, point.Y, !flag2 && point.X == 1.0);
            flag |= CalloutGeometrySource.UpdateCornerArc(segments, 4, array[4], array[5]);
            flag |= CalloutGeometrySource.UpdateEdge(segments, 5, array[5], array[6], absoluteAnchorPoint, 1.0 - point.X, !flag2 && point.Y == 1.0);
            flag |= CalloutGeometrySource.UpdateCornerArc(segments, 6, array[6], array[7]);
            return flag | CalloutGeometrySource.UpdateEdge(segments, 7, array[7], array[0], absoluteAnchorPoint, 1.0 - point.Y, !flag2 && point.X == 0.0);
        }

        private bool UpdateCloudCallout(ICalloutGeometrySourceParameters parameters)
        {
            bool flag = false;
            int num = 3;
            if (CalloutGeometrySource.IsInside(parameters.CalloutStyle, parameters.AnchorPoint))
            {
                num = 0;
            }
            PathGeometry pathGeometry;
            flag |= GeometryHelper.EnsureGeometryType<PathGeometry>(out pathGeometry, ref this.cachedGeometry, () => new PathGeometry());
            flag |= pathGeometry.SetIfDifferent(PathGeometry.FillRuleProperty, FillRule.Nonzero);
            flag |= pathGeometry.Figures.EnsureListCount(1 + num, () => new PathFigure());
            Point[] array = CalloutGeometrySource.cloudPoints.ToArray<Point>();
            Transform transform = GeometryHelper.RelativeTransform(CalloutGeometrySource.cloudBounds, base.LogicalBounds);
            array.ApplyTransform(transform);
            PathFigure pathFigure = pathGeometry.Figures[0];
            flag |= pathFigure.SetIfDifferent(PathFigure.IsFilledProperty, true);
            flag |= pathFigure.SetIfDifferent(PathFigure.IsClosedProperty, true);
            flag |= pathFigure.SetIfDifferent(PathFigure.StartPointProperty, transform.Transform(CalloutGeometrySource.cloudStartPoint));
            flag |= pathFigure.Segments.EnsureListCount(1, () => new PolyBezierSegment());
            flag |= PathSegmentHelper.SyncPolyBezierSegment(pathFigure.Segments, 0, array, 0, array.Length);
            for (int i = 0; i < num; i++)
            {
                double alpha = i / (double)num;
                Point point = GeometryHelper.Lerp(this.GetAbsoluteAnchorPoint(parameters.AnchorPoint), base.LogicalBounds.Center(), MathHelper.Lerp(0.0, 0.5, alpha));
                double alpha2 = MathHelper.Lerp(0.05, 0.2, alpha);
                double num2 = MathHelper.Lerp(0.0, base.LogicalBounds.Width / 2.0, alpha2);
                double num3 = MathHelper.Lerp(0.0, base.LogicalBounds.Height / 2.0, alpha2);
                Rect bounds = new Rect(point.X - num2, point.Y - num3, 2.0 * num2, 2.0 * num3);
                bool flag2 = flag;
                bool isFilled = true;
                flag = (flag2 | PathFigureHelper.SyncEllipseFigure(pathGeometry.Figures[i + 1], bounds, SweepDirection.Counterclockwise, isFilled));
            }
            return flag;
        }

        private static bool IsInside(CalloutStyle style, Point point)
        {
            switch (style)
            {
                default:
                    return Math.Abs(point.X - 0.5) <= 0.5 && Math.Abs(point.Y - 0.5) <= 0.5;
                case CalloutStyle.Oval:
                case CalloutStyle.Cloud:
                    return GeometryHelper.Distance(point, new Point(0.5, 0.5)) <= 0.5;
            }
        }

        // Token: 0x0600021C RID: 540 RVA: 0x0000CD48 File Offset: 0x0000AF48
        private Point GetAbsoluteAnchorPoint(Point relativePoint)
        {
            return GeometryHelper.RelativeToAbsolutePoint(base.LayoutBounds, relativePoint);
        }

        private const double centerRatioA = 0.0;

        private const double centerRatioB = 0.5;

        private const double radiusRatioA = 0.05;

        private const double radiusRatioB = 0.2;

        private static readonly Point[] connectionPoints = new Point[]
        {
            new Point(0.25, 0.0),
            new Point(0.75, 0.0),
            new Point(0.25, 1.0),
            new Point(0.75, 1.0),
            new Point(0.0, 0.25),
            new Point(0.0, 0.75),
            new Point(1.0, 0.25),
            new Point(1.0, 0.75)
        };

        private static readonly Point cloudStartPoint = new Point(86.42, 23.3);

        private static readonly Point[] cloudPoints = new Point[]
        {
            new Point(86.42, 23.18),
            new Point(86.44, 23.07),
            new Point(86.44, 22.95),
            new Point(86.44, 16.53),
            new Point(81.99, 11.32),
            new Point(76.51, 11.32),
            new Point(75.12, 11.32),
            new Point(73.79, 11.66),
            new Point(72.58, 12.27),
            new Point(70.81, 5.74),
            new Point(65.59, 1.0),
            new Point(59.43, 1.0),
            new Point(54.48, 1.0),
            new Point(50.15, 4.06),
            new Point(47.71, 8.65),
            new Point(46.62, 7.08),
            new Point(44.97, 6.08),
            new Point(43.11, 6.08),
            new Point(41.21, 6.08),
            new Point(39.53, 7.13),
            new Point(38.45, 8.76),
            new Point(35.72, 5.49),
            new Point(31.93, 3.46),
            new Point(27.73, 3.46),
            new Point(21.26, 3.46),
            new Point(15.77, 8.27),
            new Point(13.67, 14.99),
            new Point(13.36, 14.96),
            new Point(13.05, 14.94),
            new Point(12.73, 14.94),
            new Point(6.25, 14.94),
            new Point(1.0, 21.1),
            new Point(1.0, 28.69),
            new Point(1.0, 35.68),
            new Point(5.45, 41.44),
            new Point(11.21, 42.32),
            new Point(11.65, 47.61),
            new Point(15.45, 51.74),
            new Point(20.08, 51.74),
            new Point(22.49, 51.74),
            new Point(24.66, 50.63),
            new Point(26.27, 48.82),
            new Point(27.38, 53.36),
            new Point(30.95, 56.69),
            new Point(35.18, 56.69),
            new Point(39.0, 56.69),
            new Point(42.27, 53.98),
            new Point(43.7, 50.13),
            new Point(45.33, 52.69),
            new Point(47.92, 54.35),
            new Point(50.86, 54.35),
            new Point(55.0, 54.35),
            new Point(58.48, 51.03),
            new Point(59.49, 46.53),
            new Point(61.53, 51.17),
            new Point(65.65, 54.35),
            new Point(70.41, 54.35),
            new Point(77.09, 54.35),
            new Point(82.51, 48.1),
            new Point(82.69, 40.32),
            new Point(83.3, 40.51),
            new Point(83.95, 40.63),
            new Point(84.62, 40.63),
            new Point(88.77, 40.63),
            new Point(92.13, 36.69),
            new Point(92.13, 31.83),
            new Point(92.13, 27.7),
            new Point(89.69, 24.25),
            new Point(86.42, 23.3)
        };

        private static readonly Rect cloudBounds = new Rect(1.0, 1.0, 91.129997253418, 55.689998626709);
    }
}
