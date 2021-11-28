using CookPopularCSharpToolkit.Communal;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Animation;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：AnimationHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-21 09:39:08
 */
namespace CookPopularCSharpToolkit.Windows
{
    public class AnimationHelper
    {
        public static DoubleAnimation CreateDoubleAnimation(double toValue, double milliseconds = 200)
        {
            return new DoubleAnimation(toValue, new Duration(TimeSpan.FromMilliseconds(milliseconds)))
            {
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut }
            };
        }

        public static DoubleAnimation CreateDoubleAnimation(double from, double to, double seconds = 3)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = from;
            animation.To = to;
            animation.Duration = new Duration(TimeSpan.FromSeconds(seconds));

            return animation;
        }
    }
}
