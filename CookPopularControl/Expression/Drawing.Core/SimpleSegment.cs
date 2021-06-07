using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：SimpleSegment
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:57:17
 */
namespace CookPopularControl.Expression.Drawing.Core
{
	internal class SimpleSegment
	{
		public SimpleSegment.SegmentType Type { get; private set; }

		/// <summary>
		/// Control points of path segment.  Length is variant.
		/// Line segment has 2 points, Cubic bezier has 4 points.
		/// </summary>
		public Point[] Points { get; private set; }

		public void Flatten(IList<Point> resultPolyline, double tolerance, IList<double>? resultParameters)
		{
			switch (this.Type)
			{
				case SimpleSegment.SegmentType.Line:
					resultPolyline.Add(this.Points[1]);
					if (resultParameters != null)
					{
						resultParameters.Add(1.0);
						return;
					}
					break;
				case SimpleSegment.SegmentType.CubicBeizer:
					BezierCurveFlattener.FlattenCubic(this.Points, tolerance, resultPolyline, true, resultParameters);
					break;
				default:
					return;
			}
		}

		/// <summary>
		/// Private constructor. Force to use factory methods.
		/// </summary>
		private SimpleSegment()
		{
		}

		/// <summary>
		///  Creates a line segment
		/// </summary>
		public static SimpleSegment Create(Point point0, Point point1)
		{
			return new SimpleSegment
			{
				Type = SimpleSegment.SegmentType.Line,
				Points = new Point[]
				{
					point0,
					point1
				}
			};
		}

		/// <summary>
		///  Creates a cubic bezier segment from quatratic curve (3 control points)
		/// </summary>
		public static SimpleSegment Create(Point point0, Point point1, Point point2)
		{
			Point point3 = GeometryHelper.Lerp(point0, point1, 0.6666666666666666);
			Point point4 = GeometryHelper.Lerp(point1, point2, 0.3333333333333333);
			return new SimpleSegment
			{
				Type = SimpleSegment.SegmentType.CubicBeizer,
				Points = new Point[]
				{
					point0,
					point3,
					point4,
					point2
				}
			};
		}

		/// <summary>
		///  Creates a cubic bezier segment with 4 control points.
		/// </summary>
		public static SimpleSegment Create(Point point0, Point point1, Point point2, Point point3)
		{
			return new SimpleSegment
			{
				Type = SimpleSegment.SegmentType.CubicBeizer,
				Points = new Point[]
				{
					point0,
					point1,
					point2,
					point3
				}
			};
		}

		public enum SegmentType
		{
			Line,
			CubicBeizer
		}
	}
}
