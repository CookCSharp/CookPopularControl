using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：PathSegmentData
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:51:24
 */
namespace CookPopularControl.Expression.Drawing.Core
{
	/// <summary>
	/// A Tuple data structure for PathSegment and the corresponding StartPoint.
	/// </summary>
	internal sealed class PathSegmentData
	{
		public PathSegmentData(Point startPoint, PathSegment pathSegment)
		{
			this.PathSegment = pathSegment;
			this.StartPoint = startPoint;
		}

		public Point StartPoint { get; private set; }

		public PathSegment PathSegment { get; private set; }
	}
}
