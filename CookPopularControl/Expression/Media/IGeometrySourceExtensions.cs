using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：IGeometrySourceExtensions
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:20:42
 */
namespace CookPopularControl.Expression.Media
{
	/// <summary>
	/// Provides helper extension methods to work with IGeometrySource and parameters.
	/// </summary>
	public static class IGeometrySourceExtensions
	{
		public static double GetHalfStrokeThickness(this IGeometrySourceParameters parameter)
		{
			if (parameter.Stroke != null)
			{
				double strokeThickness = parameter.StrokeThickness;
				if (!double.IsNaN(strokeThickness) && !double.IsInfinity(strokeThickness))
				{
					return Math.Abs(strokeThickness) / 2.0;
				}
			}
			return 0.0;
		}

		public static GeometryEffect GetGeometryEffect(this IGeometrySourceParameters parameters)
		{
			DependencyObject? dependencyObject = parameters as DependencyObject;
			if (dependencyObject == null)
			{
				return null;
			}
			GeometryEffect geometryEffect = GeometryEffect.GetGeometryEffect(dependencyObject);
			if (geometryEffect == null || !dependencyObject.Equals(geometryEffect.Parent))
			{
				return null;
			}
			return geometryEffect;
		}
	}
}
