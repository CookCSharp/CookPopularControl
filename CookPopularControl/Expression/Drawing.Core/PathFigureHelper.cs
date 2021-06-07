using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：PathFigureHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:48:49
 */
namespace CookPopularControl.Expression.Drawing.Core
{
	/// <summary>
	/// Helper class to work with PathFigure.
	/// </summary>
	// Token: 0x0200001F RID: 31
	internal static class PathFigureHelper
	{
		/// <summary>
		/// Flattens the given figure and adds result points to the given point list.
		/// </summary>
		/// <param name="tolerance">The error tolerance. Must be positive. Can be zero. Fallback to default tolerance.</param>
		internal static void FlattenFigure(PathFigure figure, IList<Point> points, double tolerance, bool removeRepeat)
		{
			if (figure == null)
			{
				throw new ArgumentNullException("figure");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			if (tolerance < 0.0)
			{
				throw new ArgumentOutOfRangeException("tolerance");
			}
			IList<Point> list = removeRepeat ? new List<Point>() : points;
			list.Add(figure.StartPoint);
			foreach (PathSegmentData pathSegmentData in figure.AllSegments())
			{
				pathSegmentData.PathSegment.FlattenSegment(list, pathSegmentData.StartPoint, tolerance);
			}
			if (figure.IsClosed)
			{
				list.Add(figure.StartPoint);
			}
			if (removeRepeat && list.Count > 0)
			{
				points.Add(list[0]);
				for (int i = 1; i < list.Count; i++)
				{
					double value = GeometryHelper.SquaredDistance(points.Last<Point>(), list[i]);
					if (!MathHelper.IsVerySmall(value))
					{
						points.Add(list[i]);
					}
				}
			}
		}

		/// <summary>
		/// Iterates all segments inside a given figure, and returns the correct start point for each segment.
		/// </summary>
		public static IEnumerable<PathSegmentData> AllSegments(this PathFigure figure)
		{
			if (figure != null && figure.Segments.Count > 0)
			{
				Point startPoint = figure.StartPoint;
				foreach (PathSegment segment in figure.Segments)
				{
					Point lastPoint = segment.GetLastPoint();
					yield return new PathSegmentData(startPoint, segment);
					startPoint = lastPoint;
				}
			}
			yield break;
		}

		/// <summary>
		/// Synchronizes the figure to the given list of points as a single polyline segment.
		/// Tries to keep the change to a minimum and returns false if nothing has been changed.
		/// </summary>
		internal static bool SyncPolylineFigure(PathFigure figure, IList<Point> points, bool isClosed, bool isFilled = true)
		{
			if (figure == null)
			{
				throw new ArgumentNullException("figure");
			}
			bool flag = false;
			if (points == null || points.Count == 0)
			{
				flag |= figure.ClearIfSet(PathFigure.StartPointProperty);
				flag |= figure.Segments.EnsureListCount(0, null);
			}
			else
			{
				flag |= figure.SetIfDifferent(PathFigure.StartPointProperty, points[0]);
				flag |= figure.Segments.EnsureListCount(1, () => new PolyLineSegment());
				flag |= PathSegmentHelper.SyncPolylineSegment(figure.Segments, 0, points, 1, points.Count - 1);
			}
			flag |= figure.SetIfDifferent(PathFigure.IsClosedProperty, isClosed);
			return flag | figure.SetIfDifferent(PathFigure.IsFilledProperty, isFilled);
		}

		/// <summary>
		/// Synchronizes the given figure to be a closed ellipse with two arc segments.
		/// </summary>
		internal static bool SyncEllipseFigure(PathFigure figure, Rect bounds, SweepDirection sweepDirection, bool isFilled = true)
		{
			bool flag = false;
			Point[] array = new Point[2];
			Size size = new Size(bounds.Width / 2.0, bounds.Height / 2.0);
			Point point = bounds.Center();
			if (size.Width > size.Height)
			{
				array[0] = new Point(bounds.Left, point.Y);
				array[1] = new Point(bounds.Right, point.Y);
			}
			else
			{
				array[0] = new Point(point.X, bounds.Top);
				array[1] = new Point(point.X, bounds.Bottom);
			}
			flag |= figure.SetIfDifferent(PathFigure.IsClosedProperty, true);
			flag |= figure.SetIfDifferent(PathFigure.IsFilledProperty, isFilled);
			flag |= figure.SetIfDifferent(PathFigure.StartPointProperty, array[0]);
			flag |= figure.Segments.EnsureListCount(2, () => new ArcSegment());
			ArcSegment dependencyObject;
			flag |= GeometryHelper.EnsureSegmentType<ArcSegment>(out dependencyObject, figure.Segments, 0, () => new ArcSegment());
			flag |= dependencyObject.SetIfDifferent(ArcSegment.PointProperty, array[1]);
			flag |= dependencyObject.SetIfDifferent(ArcSegment.SizeProperty, size);
			flag |= dependencyObject.SetIfDifferent(ArcSegment.IsLargeArcProperty, false);
			flag |= dependencyObject.SetIfDifferent(ArcSegment.SweepDirectionProperty, sweepDirection);
			flag |= GeometryHelper.EnsureSegmentType<ArcSegment>(out dependencyObject, figure.Segments, 1, () => new ArcSegment());
			flag |= dependencyObject.SetIfDifferent(ArcSegment.PointProperty, array[0]);
			flag |= dependencyObject.SetIfDifferent(ArcSegment.SizeProperty, size);
			flag |= dependencyObject.SetIfDifferent(ArcSegment.IsLargeArcProperty, false);
			return flag | dependencyObject.SetIfDifferent(ArcSegment.SweepDirectionProperty, sweepDirection);
		}
	}
}
