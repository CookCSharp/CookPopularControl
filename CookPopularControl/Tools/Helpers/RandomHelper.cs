using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：RandomHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-05 17:02:24
 */
namespace CookPopularControl.Tools.Helpers
{
    public class RandomHelper
    {
        private static readonly WeakReference s_random = new WeakReference(null);
        public static Random Rnd
        {
            get
            {
                Contract.Ensures(Contract.Result<Random>() != null);
                var r = (Random)s_random.Target;
                if (r == null)
                {
                    s_random.Target = r = new Random();
                }
                return r;
            }
        }
    }
}
