using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：PathGeometryHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:49:38
 */
namespace CookPopularControl.Expression
{
    /// <summary>
    /// Helper class to work with PathGeometry.
    /// </summary>
    internal static class PathGeometryHelper
    {
        /// <summary>
        /// Converts a string in the path mini-language into a PathGeometry.
        /// </summary>
        /// <param name="abbreviatedGeometry">A string in the path mini-language.</param>
        internal static PathGeometry ConvertToPathGeometry(string abbreviatedGeometry)
        {
            if (abbreviatedGeometry == null)
            {
                throw new ArgumentNullException("abbreviatedGeometry");
            }
            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures = new PathFigureCollection();
            int num = 0;
            while (num < abbreviatedGeometry.Length && char.IsWhiteSpace(abbreviatedGeometry, num))
            {
                num++;
            }
            if (num < abbreviatedGeometry.Length && abbreviatedGeometry[num] == 'F')
            {
                num++;
                while (num < abbreviatedGeometry.Length && char.IsWhiteSpace(abbreviatedGeometry, num))
                {
                    num++;
                }
                if (num == abbreviatedGeometry.Length || (abbreviatedGeometry[num] != '0' && abbreviatedGeometry[num] != '1'))
                {
                    throw new FormatException();
                }
                pathGeometry.FillRule = ((abbreviatedGeometry[num] == '0') ? FillRule.EvenOdd : FillRule.Nonzero);
                num++;
            }
            new PathGeometryHelper.AbbreviatedGeometryParser(pathGeometry).Parse(abbreviatedGeometry, num);
            return pathGeometry;
        }

        /// <summary>
        /// Converts the given geometry into a single PathGeometry.
        /// </summary>
        public static PathGeometry AsPathGeometry(this Geometry original)
        {
            PathGeometry pathGeometry = original as PathGeometry;
            if (pathGeometry == null)
            {
                pathGeometry = PathGeometry.CreateFromGeometry(original);
            }
            return pathGeometry;
        }

        public static bool IsStroked(this PathSegment pathSegment)
        {
            return pathSegment.IsStroked;
        }

        public static bool IsSmoothJoin(this PathSegment pathSegment)
        {
            return pathSegment.IsSmoothJoin;
        }

        public static bool IsFrozen(this Geometry geometry)
        {
            return geometry.IsFrozen;
        }

        /// <summary>
        /// Updates the given geometry as PathGeometry with a polyline matching a given point list.
        /// </summary>
        public static bool SyncPolylineGeometry(ref Geometry geometry, IList<Point> points, bool isClosed)
        {
            bool flag = false;
            PathGeometry? pathGeometry = geometry as PathGeometry;
            PathFigure figure;
            if (pathGeometry == null || pathGeometry.Figures.Count != 1 || (figure = pathGeometry.Figures[0]) == null)
            {
                geometry = (pathGeometry = new PathGeometry());
                pathGeometry.Figures.Add(figure = new PathFigure());
                flag = true;
            }
            return flag | PathFigureHelper.SyncPolylineFigure(figure, points, isClosed, true);
        }

        internal static Geometry FixPathGeometryBoundary(Geometry geometry)
        {
            return geometry;
        }

        /// <summary>
        /// Parses abbreviated geometry sytax.
        /// </summary>
        private class AbbreviatedGeometryParser
        {
            public AbbreviatedGeometryParser(PathGeometry geometry)
            {
                this.geometry = geometry;
            }

            public void Parse(string data, int startIndex)
            {
                this.buffer = data;
                this.length = data.Length;
                this.index = startIndex;
                bool flag = true;
                while (this.ReadToken())
                {
                    char c = this.token;
                    if (flag)
                    {
                        if (c != 'M' && c != 'm')
                        {
                            throw new FormatException();
                        }
                        flag = false;
                    }
                    char c2 = c;
                    if (c2 <= 'Z')
                    {
                        if (c2 <= 'M')
                        {
                            switch (c2)
                            {
                                case 'A':
                                    break;
                                case 'B':
                                    goto IL_364;
                                case 'C':
                                    goto IL_225;
                                default:
                                    if (c2 == 'H')
                                    {
                                        goto IL_193;
                                    }
                                    switch (c2)
                                    {
                                        case 'L':
                                            goto IL_165;
                                        case 'M':
                                            goto IL_11B;
                                        default:
                                            goto IL_364;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            switch (c2)
                            {
                                case 'Q':
                                    goto IL_2BD;
                                case 'R':
                                    goto IL_364;
                                case 'S':
                                    goto IL_276;
                                default:
                                    if (c2 == 'V')
                                    {
                                        goto IL_1DA;
                                    }
                                    if (c2 != 'Z')
                                    {
                                        goto IL_364;
                                    }
                                    goto IL_35B;
                            }
                        }
                    }
                    else if (c2 <= 'm')
                    {
                        switch (c2)
                        {
                            case 'a':
                                break;
                            case 'b':
                                goto IL_364;
                            case 'c':
                                goto IL_225;
                            default:
                                if (c2 == 'h')
                                {
                                    goto IL_193;
                                }
                                switch (c2)
                                {
                                    case 'l':
                                        goto IL_165;
                                    case 'm':
                                        goto IL_11B;
                                    default:
                                        goto IL_364;
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (c2)
                        {
                            case 'q':
                                goto IL_2BD;
                            case 'r':
                                goto IL_364;
                            case 's':
                                goto IL_276;
                            default:
                                if (c2 == 'v')
                                {
                                    goto IL_1DA;
                                }
                                if (c2 != 'z')
                                {
                                    goto IL_364;
                                }
                                goto IL_35B;
                        }
                    }
                    do
                    {
                        Size size = this.ReadSize(false);
                        double rotationAngle = this.ReadDouble(true);
                        bool isLargeArc = this.ReadBool01(true);
                        SweepDirection sweepDirection = this.ReadBool01(true) ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
                        this.lastPoint = this.ReadPoint(c, true);
                        this.ArcTo(size, rotationAngle, isLargeArc, sweepDirection, this.lastPoint);
                    }
                    while (this.IsNumber(true));
                    this.EnsureFigure();
                    continue;
                    IL_11B:
                    this.lastPoint = this.ReadPoint(c, false);
                    this.BeginFigure(this.lastPoint);
                    char command = 'M';
                    while (this.IsNumber(true))
                    {
                        this.lastPoint = this.ReadPoint(command, false);
                        this.LineTo(this.lastPoint);
                        command = 'L';
                    }
                    continue;
                    IL_165:
                    this.EnsureFigure();
                    do
                    {
                        this.lastPoint = this.ReadPoint(c, false);
                        this.LineTo(this.lastPoint);
                    }
                    while (this.IsNumber(true));
                    continue;
                    IL_193:
                    this.EnsureFigure();
                    do
                    {
                        double num = this.ReadDouble(false);
                        if (c == 'h')
                        {
                            num += this.lastPoint.X;
                        }
                        this.lastPoint.X = num;
                        this.LineTo(this.lastPoint);
                    }
                    while (this.IsNumber(true));
                    continue;
                    IL_1DA:
                    this.EnsureFigure();
                    do
                    {
                        double num2 = this.ReadDouble(false);
                        if (c == 'v')
                        {
                            num2 += this.lastPoint.Y;
                        }
                        this.lastPoint.Y = num2;
                        this.LineTo(this.lastPoint);
                    }
                    while (this.IsNumber(true));
                    continue;
                    IL_225:
                    this.EnsureFigure();
                    do
                    {
                        Point point = this.ReadPoint(c, false);
                        this.secondLastPoint = this.ReadPoint(c, true);
                        this.lastPoint = this.ReadPoint(c, true);
                        this.BezierTo(point, this.secondLastPoint, this.lastPoint);
                    }
                    while (this.IsNumber(true));
                    continue;
                    IL_276:
                    this.EnsureFigure();
                    do
                    {
                        Point smoothBeizerFirstPoint = this.GetSmoothBeizerFirstPoint();
                        Point point2 = this.ReadPoint(c, false);
                        this.lastPoint = this.ReadPoint(c, true);
                        this.BezierTo(smoothBeizerFirstPoint, point2, this.lastPoint);
                    }
                    while (this.IsNumber(true));
                    continue;
                    IL_2BD:
                    this.EnsureFigure();
                    do
                    {
                        Point point3 = this.ReadPoint(c, false);
                        this.lastPoint = this.ReadPoint(c, true);
                        this.QuadraticBezierTo(point3, this.lastPoint);
                    }
                    while (this.IsNumber(true));
                    continue;
                    IL_35B:
                    this.FinishFigure(true);
                    continue;
                    IL_364:
                    throw new NotSupportedException();
                }
                this.FinishFigure(false);
            }

            private bool ReadToken()
            {
                this.SkipWhitespace(false);
                if (this.index < this.length)
                {
                    this.token = this.buffer[this.index++];
                    return true;
                }
                return false;
            }

            private Point ReadPoint(char command, bool allowComma)
            {
                double num = this.ReadDouble(allowComma);
                double num2 = this.ReadDouble(true);
                if (command >= 'a')
                {
                    num += this.lastPoint.X;
                    num2 += this.lastPoint.Y;
                }
                return new Point(num, num2);
            }

            private Size ReadSize(bool allowComma)
            {
                double width = this.ReadDouble(allowComma);
                double height = this.ReadDouble(true);
                return new Size(width, height);
            }

            private bool ReadBool01(bool allowComma)
            {
                double num = this.ReadDouble(allowComma);
                if (num == 0.0)
                {
                    return false;
                }
                if (num == 1.0)
                {
                    return true;
                }
                throw new FormatException();
            }

            private double ReadDouble(bool allowComma)
            {
                if (!this.IsNumber(allowComma))
                {
                    throw new FormatException();
                }
                bool flag = true;
                int i = this.index;
                if (this.index < this.length && (this.buffer[this.index] == '-' || this.buffer[this.index] == '+'))
                {
                    this.index++;
                }
                if (this.index < this.length && this.buffer[this.index] == 'I')
                {
                    this.index = Math.Min(this.index + 8, this.length);
                    flag = false;
                }
                else if (this.index < this.length && this.buffer[this.index] == 'N')
                {
                    this.index = Math.Min(this.index + 3, this.length);
                    flag = false;
                }
                else
                {
                    this.SkipDigits(false);
                    if (this.index < this.length && this.buffer[this.index] == '.')
                    {
                        flag = false;
                        this.index++;
                        this.SkipDigits(false);
                    }
                    if (this.index < this.length && (this.buffer[this.index] == 'E' || this.buffer[this.index] == 'e'))
                    {
                        flag = false;
                        this.index++;
                        this.SkipDigits(true);
                    }
                }
                if (flag && this.index <= i + 8)
                {
                    int num = 1;
                    if (this.buffer[i] == '+')
                    {
                        i++;
                    }
                    else if (this.buffer[i] == '-')
                    {
                        i++;
                        num = -1;
                    }
                    int num2 = 0;
                    while (i < this.index)
                    {
                        num2 = num2 * 10 + (this.buffer[i] - '0');
                        i++;
                    }
                    return num2 * num;
                }
                string value = this.buffer.Substring(i, this.index - i);
                double result;
                try
                {
                    result = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    throw new FormatException();
                }
                return result;
            }

            private void SkipDigits(bool signAllowed)
            {
                if (signAllowed && this.index < this.length && (this.buffer[this.index] == '-' || this.buffer[this.index] == '+'))
                {
                    this.index++;
                }
                while (this.index < this.length && this.buffer[this.index] >= '0' && this.buffer[this.index] <= '9')
                {
                    this.index++;
                }
            }

            private bool IsNumber(bool allowComma)
            {
                bool flag = this.SkipWhitespace(allowComma);
                if (this.index < this.length)
                {
                    this.token = this.buffer[this.index];
                    if (this.token == '.' || this.token == '-' || this.token == '+' || (this.token >= '0' && this.token <= '9') || this.token == 'I' || this.token == 'N')
                    {
                        return true;
                    }
                }
                if (flag)
                {
                    throw new FormatException();
                }
                return false;
            }

            private bool SkipWhitespace(bool allowComma)
            {
                bool result = false;
                while (this.index < this.length)
                {
                    char c = this.buffer[this.index];
                    char c2 = c;
                    switch (c2)
                    {
                        case '\t':
                        case '\n':
                        case '\r':
                            break;
                        case '\v':
                        case '\f':
                            goto IL_4F;
                        default:
                            if (c2 != ' ')
                            {
                                if (c2 != ',')
                                {
                                    goto IL_4F;
                                }
                                if (!allowComma)
                                {
                                    throw new FormatException();
                                }
                                result = true;
                                allowComma = false;
                            }
                            break;
                    }
                    IL_65:
                    this.index++;
                    continue;
                    IL_4F:
                    if (c > ' ' && c <= 'z')
                    {
                        return result;
                    }
                    if (!char.IsWhiteSpace(c))
                    {
                        return result;
                    }
                    goto IL_65;
                }
                return false;
            }

            private void BeginFigure(Point startPoint)
            {
                this.FinishFigure(false);
                this.EnsureFigure();
                this.figure.StartPoint = startPoint;
                this.figure.IsFilled = true;
            }

            private void EnsureFigure()
            {
                if (this.figure == null)
                {
                    this.figure = new PathFigure();
                    this.figure.Segments = new PathSegmentCollection();
                }
            }

            private void FinishFigure(bool figureExplicitlyClosed)
            {
                if (this.figure != null)
                {
                    if (figureExplicitlyClosed)
                    {
                        this.figure.IsClosed = true;
                    }
                    this.geometry.Figures.Add(this.figure);
                    this.figure = null;
                }
            }

            private void LineTo(Point point)
            {
                LineSegment lineSegment = new LineSegment();
                lineSegment.Point = point;
                this.figure.Segments.Add(lineSegment);
            }

            private void BezierTo(Point point1, Point point2, Point point3)
            {
                BezierSegment bezierSegment = new BezierSegment();
                bezierSegment.Point1 = point1;
                bezierSegment.Point2 = point2;
                bezierSegment.Point3 = point3;
                this.figure.Segments.Add(bezierSegment);
            }

            private void QuadraticBezierTo(Point point1, Point point2)
            {
                QuadraticBezierSegment quadraticBezierSegment = new QuadraticBezierSegment();
                quadraticBezierSegment.Point1 = point1;
                quadraticBezierSegment.Point2 = point2;
                this.figure.Segments.Add(quadraticBezierSegment);
            }

            private void ArcTo(Size size, double rotationAngle, bool isLargeArc, SweepDirection sweepDirection, Point point)
            {
                ArcSegment arcSegment = new ArcSegment();
                arcSegment.Size = size;
                arcSegment.RotationAngle = rotationAngle;
                arcSegment.IsLargeArc = isLargeArc;
                arcSegment.SweepDirection = sweepDirection;
                arcSegment.Point = point;
                this.figure.Segments.Add(arcSegment);
            }

            private Point GetSmoothBeizerFirstPoint()
            {
                Point result = this.lastPoint;
                if (this.figure.Segments.Count > 0)
                {
                    BezierSegment bezierSegment = this.figure.Segments[this.figure.Segments.Count - 1] as BezierSegment;
                    if (bezierSegment != null)
                    {
                        Point point = bezierSegment.Point2;
                        result.X += this.lastPoint.X - point.X;
                        result.Y += this.lastPoint.Y - point.Y;
                    }
                }
                return result;
            }

            private PathGeometry geometry;

            private PathFigure figure;

            private Point lastPoint;

            private Point secondLastPoint;

            private string buffer;

            private int index;

            private int length;

            private char token;
        }
    }
}
