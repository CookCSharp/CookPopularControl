﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：BezierCurveFlattener
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:40:14
 */
namespace CookPopularControl.Expression.Drawing.Core
{
	/// <summary>
	/// A utility class to flatten Bezier curves.
	/// </summary>
	internal static class BezierCurveFlattener
	{
		/// <summary>
		/// Flattens a Bezier cubic curve and adds the resulting polyline to the third parameter.
		/// </summary>
		/// <param name="controlPoints">The four Bezier cubic control points.</param>
		/// <param name="errorTolerance">The maximum distance between two corresponding points on the true curve 
		/// and on the flattened polyline. Must be strictly positive.</param>
		/// <param name="resultPolyline">Where to add the flattened polyline.</param>
		/// <param name="skipFirstPoint">True to skip the first control point when adding the flattened polyline.
		/// <param name="resultParameters">Where to add the value of the Bezier curve parameter associated with 
		/// each of the polyline vertices.</param> 
		/// If <paramref name="resultPolyline" /> is empty, the first control point 
		/// and its associated parameter are always added.</param>
		public static void FlattenCubic(Point[] controlPoints, double errorTolerance, ICollection<Point> resultPolyline, bool skipFirstPoint, ICollection<double> resultParameters = null)
		{
			if (resultPolyline == null)
			{
				throw new ArgumentNullException("resultPolyline");
			}
			if (controlPoints == null)
			{
				throw new ArgumentNullException("controlPoints");
			}
			if (controlPoints.Length != 4)
			{
				throw new ArgumentOutOfRangeException("controlPoints");
			}
			BezierCurveFlattener.EnsureErrorTolerance(ref errorTolerance);
			if (!skipFirstPoint)
			{
				resultPolyline.Add(controlPoints[0]);
				if (resultParameters != null)
				{
					resultParameters.Add(0.0);
				}
			}
			if (BezierCurveFlattener.IsCubicChordMonotone(controlPoints, errorTolerance * errorTolerance))
			{
				BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener adaptiveForwardDifferencingCubicFlattener = new BezierCurveFlattener.AdaptiveForwardDifferencingCubicFlattener(controlPoints, errorTolerance, errorTolerance, true);
				Point item = default(Point);
				double item2 = 0.0;
				while (adaptiveForwardDifferencingCubicFlattener.Next(ref item, ref item2))
				{
					resultPolyline.Add(item);
					if (resultParameters != null)
					{
						resultParameters.Add(item2);
					}
				}
			}
			else
			{
				double x = controlPoints[3].X - controlPoints[2].X + controlPoints[1].X - controlPoints[0].X;
				double y = controlPoints[3].Y - controlPoints[2].Y + controlPoints[1].Y - controlPoints[0].Y;
				double num = 1.0 / errorTolerance;
				uint num2 = BezierCurveFlattener.Log8UnsignedInt32((uint)(MathHelper.Hypotenuse(x, y) * num + 0.5));
				if (num2 > 0U)
				{
					num2 -= 1U;
				}
				if (num2 > 0U)
				{
					BezierCurveFlattener.DoCubicMidpointSubdivision(controlPoints, num2, 0.0, 1.0, 0.75 * num, resultPolyline, resultParameters);
				}
				else
				{
					BezierCurveFlattener.DoCubicForwardDifferencing(controlPoints, 0.0, 1.0, 0.75 * num, resultPolyline, resultParameters);
				}
			}
			resultPolyline.Add(controlPoints[3]);
			if (resultParameters != null)
			{
				resultParameters.Add(1.0);
			}
		}

		/// <summary>
		/// Flattens a Bezier quadratic curve and adds the resulting polyline to the third parameter.
		/// Uses degree elevation for Bezier curves to reuse the code for the cubic case.
		/// </summary>
		/// <param name="controlPoints">The three Bezier quadratic control points.</param>
		/// <param name="errorTolerance">The maximum distance between two corresponding points on the true curve 
		/// and on the flattened polyline. Must be strictly positive.</param>
		/// <param name="resultPolyline">Where to add the flattened polyline.</param>
		/// <param name="skipFirstPoint">Whether to skip the first control point when adding the flattened polyline. 
		/// <param name="resultParameters">Where to add the value of the Bezier curve parameter associated with
		/// each of the polyline vertices.</param>
		/// If <paramref name="resultPolyline" /> is empty, the first control point and 
		/// its associated parameter are always added.</param>
		public static void FlattenQuadratic(Point[] controlPoints, double errorTolerance, ICollection<Point> resultPolyline, bool skipFirstPoint, ICollection<double> resultParameters = null)
		{
			if (resultPolyline == null)
			{
				throw new ArgumentNullException("resultPolyline");
			}
			if (controlPoints == null)
			{
				throw new ArgumentNullException("controlPoints");
			}
			if (controlPoints.Length != 3)
			{
				throw new ArgumentOutOfRangeException("controlPoints");
			}
			BezierCurveFlattener.EnsureErrorTolerance(ref errorTolerance);
			BezierCurveFlattener.FlattenCubic(new Point[]
			{
				controlPoints[0],
				GeometryHelper.Lerp(controlPoints[0], controlPoints[1], 0.6666666666666666),
				GeometryHelper.Lerp(controlPoints[1], controlPoints[2], 0.3333333333333333),
				controlPoints[2]
			}, errorTolerance, resultPolyline, skipFirstPoint, resultParameters);
		}

		private static void EnsureErrorTolerance(ref double errorTolerance)
		{
			if (errorTolerance <= 0.0)
			{
				errorTolerance = 0.25;
			}
		}

		private static uint Log8UnsignedInt32(uint i)
		{
			uint num = 0U;
			while (i > 0U)
			{
				i >>= 3;
				num += 1U;
			}
			return num;
		}

		private static uint Log4UnsignedInt32(uint i)
		{
			uint num = 0U;
			while (i > 0U)
			{
				i >>= 2;
				num += 1U;
			}
			return num;
		}

		private static uint Log4Double(double d)
		{
			uint num = 0U;
			while (d > 1.0)
			{
				d *= 0.25;
				num += 1U;
			}
			return num;
		}

		private static void DoCubicMidpointSubdivision(Point[] controlPoints, uint depth, double leftParameter, double rightParameter, double inverseErrorTolerance, ICollection<Point> resultPolyline, ICollection<double>? resultParameters)
		{
			Point[] array = new Point[]
			{
				controlPoints[0],
				controlPoints[1],
				controlPoints[2],
				controlPoints[3]
			};
			Point[] array2 = new Point[]
			{
				default(Point),
				default(Point),
				default(Point),
				array[3]
			};
			array[3] = GeometryHelper.Midpoint(array[3], array[2]);
			array[2] = GeometryHelper.Midpoint(array[2], array[1]);
			array[1] = GeometryHelper.Midpoint(array[1], array[0]);
			array2[2] = array[3];
			array[3] = GeometryHelper.Midpoint(array[3], array[2]);
			array[2] = GeometryHelper.Midpoint(array[2], array[1]);
			array2[1] = array[3];
			array[3] = GeometryHelper.Midpoint(array[3], array[2]);
			array2[0] = array[3];
			depth -= 1U;
			double num = (leftParameter + rightParameter) * 0.5;
			if (depth > 0U)
			{
				BezierCurveFlattener.DoCubicMidpointSubdivision(array, depth, leftParameter, num, inverseErrorTolerance, resultPolyline, resultParameters);
				resultPolyline.Add(array2[0]);
				if (resultParameters != null)
				{
					resultParameters.Add(num);
				}
				BezierCurveFlattener.DoCubicMidpointSubdivision(array2, depth, num, rightParameter, inverseErrorTolerance, resultPolyline, resultParameters);
				return;
			}
			BezierCurveFlattener.DoCubicForwardDifferencing(array, leftParameter, num, inverseErrorTolerance, resultPolyline, resultParameters);
			resultPolyline.Add(array2[0]);
			if (resultParameters != null)
			{
				resultParameters.Add(num);
			}
			BezierCurveFlattener.DoCubicForwardDifferencing(array2, num, rightParameter, inverseErrorTolerance, resultPolyline, resultParameters);
		}

		private static void DoCubicForwardDifferencing(Point[] controlPoints, double leftParameter, double rightParameter, double inverseErrorTolerance, ICollection<Point> resultPolyline, ICollection<double>? resultParameters)
		{
			double num = controlPoints[1].X - controlPoints[0].X;
			double num2 = controlPoints[1].Y - controlPoints[0].Y;
			double num3 = controlPoints[2].X - controlPoints[1].X;
			double num4 = controlPoints[2].Y - controlPoints[1].Y;
			double num5 = controlPoints[3].X - controlPoints[2].X;
			double num6 = controlPoints[3].Y - controlPoints[2].Y;
			double num7 = num3 - num;
			double num8 = num4 - num2;
			double num9 = num5 - num3;
			double num10 = num6 - num4;
			double num11 = num9 - num7;
			double num12 = num10 - num8;
			Vector vector = controlPoints[3].Subtract(controlPoints[0]);
			double length = vector.Length;
			double num13;
			if (!MathHelper.IsVerySmall(length))
			{
				num13 = Math.Max(0.0, Math.Max(Math.Abs((num7 * vector.Y - num8 * vector.X) / length), Math.Abs((num9 * vector.Y - num10 * vector.X) / length)));
			}
			else
			{
				num13 = Math.Max(0.0, Math.Max(GeometryHelper.Distance(controlPoints[1], controlPoints[0]), GeometryHelper.Distance(controlPoints[2], controlPoints[0])));
			}
			uint num14 = 0U;
			if (num13 > 0.0)
			{
				double num15 = num13 * inverseErrorTolerance;
				num14 = ((num15 < 2147483647.0) ? BezierCurveFlattener.Log4UnsignedInt32((uint)(num15 + 0.5)) : BezierCurveFlattener.Log4Double(num15));
			}
			int num16 = (int)(-(int)num14);
			int num17 = num16 + num16;
			int exp = num17 + num16;
			double num18 = MathHelper.DoubleFromMantissaAndExponent(3.0 * num7, num17);
			double num19 = MathHelper.DoubleFromMantissaAndExponent(3.0 * num8, num17);
			double num20 = MathHelper.DoubleFromMantissaAndExponent(6.0 * num11, exp);
			double num21 = MathHelper.DoubleFromMantissaAndExponent(6.0 * num12, exp);
			double num22 = MathHelper.DoubleFromMantissaAndExponent(3.0 * num, num16) + num18 + 0.16666666666666666 * num20;
			double num23 = MathHelper.DoubleFromMantissaAndExponent(3.0 * num2, num16) + num19 + 0.16666666666666666 * num21;
			double num24 = 2.0 * num18 + num20;
			double num25 = 2.0 * num19 + num21;
			double num26 = controlPoints[0].X;
			double num27 = controlPoints[0].Y;
			Point item = new Point(0.0, 0.0);
			int num28 = 1 << (int)num14;
			double num29 = (num28 > 0) ? ((rightParameter - leftParameter) / (double)num28) : 0.0;
			double num30 = leftParameter;
			for (int i = 1; i < num28; i++)
			{
				num26 += num22;
				num27 += num23;
				item.X = num26;
				item.Y = num27;
				resultPolyline.Add(item);
				num30 += num29;
				if (resultParameters != null)
				{
					resultParameters.Add(num30);
				}
				num22 += num24;
				num23 += num25;
				num24 += num20;
				num25 += num21;
			}
		}

		private static bool IsCubicChordMonotone(Point[] controlPoints, double squaredTolerance)
		{
			double num = GeometryHelper.SquaredDistance(controlPoints[0], controlPoints[3]);
			if (num <= squaredTolerance)
			{
				return false;
			}
			Vector lhs = controlPoints[3].Subtract(controlPoints[0]);
			Vector rhs = controlPoints[1].Subtract(controlPoints[0]);
			double num2 = GeometryHelper.Dot(lhs, rhs);
			if (num2 < 0.0 || num2 > num)
			{
				return false;
			}
			Vector rhs2 = controlPoints[2].Subtract(controlPoints[0]);
			double num3 = GeometryHelper.Dot(lhs, rhs2);
			return num3 >= 0.0 && num3 <= num && num2 <= num3;
		}

		public const double StandardFlatteningTolerance = 0.25;

		private class AdaptiveForwardDifferencingCubicFlattener
		{
			internal AdaptiveForwardDifferencingCubicFlattener(Point[] controlPoints, double flatnessTolerance, double distanceTolerance, bool doParameters)
			{
				this.flatnessTolerance = 3.0 * flatnessTolerance;
				this.distanceTolerance = distanceTolerance;
				this.doParameters = doParameters;
				this.aX = -controlPoints[0].X + 3.0 * (controlPoints[1].X - controlPoints[2].X) + controlPoints[3].X;
				this.aY = -controlPoints[0].Y + 3.0 * (controlPoints[1].Y - controlPoints[2].Y) + controlPoints[3].Y;
				this.bX = 3.0 * (controlPoints[0].X - 2.0 * controlPoints[1].X + controlPoints[2].X);
				this.bY = 3.0 * (controlPoints[0].Y - 2.0 * controlPoints[1].Y + controlPoints[2].Y);
				this.cX = 3.0 * (-controlPoints[0].X + controlPoints[1].X);
				this.cY = 3.0 * (-controlPoints[0].Y + controlPoints[1].Y);
				this.dX = controlPoints[0].X;
				this.dY = controlPoints[0].Y;
			}

			private AdaptiveForwardDifferencingCubicFlattener()
			{
			}

			internal bool Next(ref Point p, ref double u)
			{
				while (this.MustSubdivide(this.flatnessTolerance))
				{
					this.HalveStepSize();
				}
				if ((this.numSteps & 1) == 0)
				{
					while (this.numSteps > 1 && !this.MustSubdivide(this.flatnessTolerance * 0.25))
					{
						this.DoubleStepSize();
					}
				}
				this.IncrementDifferencesAndParameter();
				p.X = this.dX;
				p.Y = this.dY;
				u = this.parameter;
				return this.numSteps != 0;
			}

			private void DoubleStepSize()
			{
				this.aX *= 8.0;
				this.aY *= 8.0;
				this.bX *= 4.0;
				this.bY *= 4.0;
				this.cX += this.cX;
				this.cY += this.cY;
				if (this.doParameters)
				{
					this.dParameter *= 2.0;
				}
				this.numSteps >>= 1;
			}

			private void HalveStepSize()
			{
				this.aX *= 0.125;
				this.aY *= 0.125;
				this.bX *= 0.25;
				this.bY *= 0.25;
				this.cX *= 0.5;
				this.cY *= 0.5;
				if (this.doParameters)
				{
					this.dParameter *= 0.5;
				}
				this.numSteps <<= 1;
			}

			private void IncrementDifferencesAndParameter()
			{
				this.dX = this.aX + this.bX + this.cX + this.dX;
				this.dY = this.aY + this.bY + this.cY + this.dY;
				this.cX = this.aX + this.aX + this.aX + this.bX + this.bX + this.cX;
				this.cY = this.aY + this.aY + this.aY + this.bY + this.bY + this.cY;
				this.bX = this.aX + this.aX + this.aX + this.bX;
				this.bY = this.aY + this.aY + this.aY + this.bY;
				this.numSteps--;
				this.parameter += this.dParameter;
			}

			private bool MustSubdivide(double flatnessTolerance)
			{
				double num = -(this.aY + this.bY + this.cY);
				double num2 = this.aX + this.bX + this.cX;
				double num3 = Math.Abs(num) + Math.Abs(num2);
				if (num3 > this.distanceTolerance)
				{
					num3 *= flatnessTolerance;
					return Math.Abs(this.cX * num + this.cY * num2) > num3 || Math.Abs((this.bX + this.cX + this.cX) * num + (this.bY + this.cY + this.cY) * num2) > num3;
				}
				return false;
			}

			private double aX;

			private double aY;

			private double bX;

			private double bY;

			private double cX;

			private double cY;

			private double dX;

			private double dY;

			private int numSteps = 1;

			private double flatnessTolerance;

			private double distanceTolerance;

			private bool doParameters;

			private double parameter;

			private double dParameter = 1.0;
		}
	}
}
