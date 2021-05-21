using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：AnimationHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-21 09:39:08
 */
namespace CookPopularControl.Tools.Helpers
{
    public class AnimationHelper
    {
        public static DoubleAnimation CreateDoubleAnimation(double from = 0, double to = 0, double seconds = 3)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = from;
            animation.To = to;
            animation.Duration = new Duration(TimeSpan.FromSeconds(seconds));

            return animation;
        }
    }
}
