using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：LocationSnapShot
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 15:51:22
 */
namespace CookPopularControl.Controls.Dragables.Core
{
    internal class LocationSnapShot
    {
        private readonly double _width;
        private readonly double _height;

        public static LocationSnapShot Take(FrameworkElement frameworkElement)
        {
            if (frameworkElement == null) throw new ArgumentNullException("frameworkElement");

            return new LocationSnapShot(frameworkElement.Width, frameworkElement.Height);
        }

        private LocationSnapShot(double width, double height)
        {
            _width = width;
            _height = height;
        }

        public void Apply(FrameworkElement frameworkElement)
        {
            if (frameworkElement == null) throw new ArgumentNullException("frameworkElement");

            frameworkElement.SetCurrentValue(FrameworkElement.WidthProperty, _width);
            frameworkElement.SetCurrentValue(FrameworkElement.HeightProperty, _height);
        }
    }
}
