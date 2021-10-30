using System.Globalization;
using System.Windows;
using System.Windows.Media;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-09 09:25:13
 */
namespace CookPopularControl.Tools.Helpers
{
    public class FontHelper
    {
        /// <summary>
        /// 获取字体的宽度与高度
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="textToFormat"></param>
        /// <param name="fontFamily"></param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
        public static (double width, double height) GetFontWidthHeight(Visual visual, string textToFormat, string fontFamily, double fontSize)
        {
            //var pixelsPerDip = VisualTreeHelper.GetDpi(visual).PixelsPerDip;
            var pixelsPerDip = Screenshot.GetDpiX();
            FormattedText ft = new FormattedText(textToFormat,
                                                CultureInfo.CurrentCulture,
                                                FlowDirection.LeftToRight,
                                                new Typeface(fontFamily),
                                                fontSize,
                                                Brushes.Black);

            return (ft.Width, ft.Height);
        }
    }
}
