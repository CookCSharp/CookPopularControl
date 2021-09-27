﻿using CookPopularControl.Tools.Listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：StoryboardExtension
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 15:08:05
 */
namespace CookPopularControl.Tools.Extensions.Animatables
{
    public static class StoryboardExtension
    {
        private static readonly IDictionary<Storyboard, Action<Storyboard>> ContinuationIndex = new Dictionary<Storyboard, Action<Storyboard>>();

        public static void WhenComplete(this Storyboard storyboard, Action<Storyboard> continuation)
        {
            // ReSharper disable once ObjectCreationAsStatement
            new StoryboardCompletionListener(storyboard, continuation);
        }
    }
}