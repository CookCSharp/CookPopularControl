using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DrawingPropertyChangedEventArgs
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:30:59
 */
namespace CookPopularControl.Expression.Media
{
	public class DrawingPropertyChangedEventArgs : EventArgs
	{
		public DrawingPropertyMetadata Metadata { get; set; }

		public bool IsAnimated { get; set; }
	}
}
