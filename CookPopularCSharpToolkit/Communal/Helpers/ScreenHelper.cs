using CookPopularCSharpToolkit.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Description：ScreenHelper 
 * Author： Chance_写代码的厨子
 * Company: NCATest
 * Create Time：2021-11-18 12:58:24
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) NCATest 2021 All Rights Reserved.
 */
namespace CookPopularCSharpToolkit.Communal
{
    public class ScreenHelper
    {
        /// <summary>
        /// 获取DPI百分比
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static double GetDpiRatio(Window window)
        {
            var currentGraphics = Graphics.FromHwnd(window.EnsureHandle());
            return currentGraphics.DpiX / 96;
        }

        public static double GetDpiRatio()
        {
            return GetDpiRatio(Application.Current.MainWindow);
        }

        public static double GetScreenHeight()
        {
            return SystemParameters.PrimaryScreenHeight * GetDpiRatio();
        }

        public static double GetScreenActualHeight()
        {
            return SystemParameters.PrimaryScreenHeight;
        }

        public static double GetScreenWidth()
        {
            return SystemParameters.PrimaryScreenWidth * GetDpiRatio();
        }

        public static double GetScreenActualWidth()
        {
            return SystemParameters.PrimaryScreenWidth;
        }
    }
}
