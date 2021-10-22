using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ColorBrushExtension
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-13 19:51:58
 */
namespace CookPopularControl.Tools.Extensions.Colors
{
    /// <summary>
    /// 提供<see cref="Color"/>与<see cref="Brush"/>之间的转换
    /// </summary>
    public static class ColorBrushExtension
    {
        public static SolidColorBrush ToBrushFromColor(this Color color) => new SolidColorBrush(color);

        public static Color ToColorFromBrush(this Brush brush) => (Color)(ColorConverter.ConvertFromString(brush.ToString()));

        public static string GetColorName(this Color color)
        {
            var colorProperties = typeof(System.Drawing.Color)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(System.Drawing.Color));
            foreach (var colorProperty in colorProperties)
            {
                var colorPropertyValue = (System.Drawing.Color)colorProperty.GetValue(null, null);
                if (colorPropertyValue.R == color.R && colorPropertyValue.G == color.G && colorPropertyValue.B == color.B)
                {
                    return colorPropertyValue.Name;
                }
            }

            return default;
        }
    }
}
