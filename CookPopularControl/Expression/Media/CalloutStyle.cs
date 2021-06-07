using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CornerType
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:32:12
 */
namespace CookPopularControl.Expression.Media
{
	/// <summary>
	/// Specifies the rendering style of a callout shape.
	/// </summary>
	public enum CalloutStyle
	{
		/// <summary>
		/// A rectangular callout.
		/// </summary>
		Rectangle,
		/// <summary>
		/// A rectangular callout with rounded corners.
		/// </summary>
		RoundedRectangle,
		/// <summary>
		/// A oval-shaped callout.
		/// </summary>
		Oval,
		/// <summary>
		/// A cloud-shaped callout.
		/// </summary>
		Cloud
	}
}
