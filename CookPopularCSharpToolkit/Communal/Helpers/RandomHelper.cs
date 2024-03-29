﻿using System;
using System.Diagnostics.Contracts;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：RandomHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-05 17:02:24
 */
namespace CookPopularCSharpToolkit.Communal
{
    public class RandomHelper
    {
        private static readonly WeakReference _random = new WeakReference(null);
        public static Random Rnd
        {
            get
            {
                Contract.Ensures(Contract.Result<Random>() != null);
                var r = (Random)_random.Target;
                if (r == null)
                {
                    _random.Target = r = new Random();
                }
                return r;
            }
        }
    }
}
