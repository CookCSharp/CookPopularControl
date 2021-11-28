using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：PathSegmentData
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:51:24
 */
namespace CookPopularControl.Expression
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
