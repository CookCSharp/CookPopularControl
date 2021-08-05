using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ComparerHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-05 16:34:45
 */
namespace CookPopularControl.Tools.Helpers
{
    public static class ComparerHelper
    {
        public static IEqualityComparer<T> ToEqualityComparer<T>(this Func<T, T, bool> func)
        {
            Contract.Requires(func != null);
            return new FuncEqualityComparer<T>(func);
        }

        public static IComparer<T> ToComparer<T>(this Func<T, T, int> compareFunction)
        {
            Contract.Requires(compareFunction != null);
            return new FuncComparer<T>(compareFunction);
        }

        public static IComparer<T> ToComparer<T>(this Comparison<T> compareFunction)
        {
            Contract.Requires(compareFunction != null);
            return new ComparisonComparer<T>(compareFunction);
        }

        public static IComparer<string> ToComparer<T>(this CompareInfo compareInfo)
        {
            Contract.Requires(compareInfo != null);
            return new FuncComparer<string>(compareInfo.Compare);
        }
    }
}
