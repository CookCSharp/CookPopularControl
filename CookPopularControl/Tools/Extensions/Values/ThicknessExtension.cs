using CookPopularControl.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ThicknessExtension
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-24 17:44:39
 */
namespace CookPopularControl.Tools.Extensions.Values
{
    public static class ThicknessExtension
    {
        public static Thickness Add(this Thickness a, Thickness b) => new Thickness(a.Left + b.Left, a.Top + b.Top, a.Right + b.Right, a.Bottom + b.Bottom);

        public static bool IsZero(this Thickness thickness)
            => MathHelper.IsZero(thickness.Left)
            && MathHelper.IsZero(thickness.Top)
            && MathHelper.IsZero(thickness.Right)
            && MathHelper.IsZero(thickness.Bottom);

        public static bool IsUniform(this Thickness thickness) 
            => MathHelper.AreClose(thickness.Left, thickness.Top)
            && MathHelper.AreClose(thickness.Left, thickness.Right)
            && MathHelper.AreClose(thickness.Left, thickness.Bottom);

        public static bool IsNaN(this double value) => double.IsNaN(value);
    }
}
