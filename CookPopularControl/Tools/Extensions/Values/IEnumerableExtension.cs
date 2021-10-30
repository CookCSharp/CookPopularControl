using System.Collections.Generic;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：IEnumerableExtension
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-13 19:44:45
 */
namespace CookPopularControl.Tools.Extensions.Values
{
    public static class IEnumerableExtension
    {
        /// <summary>
        /// 向某个集合中插入<see cref="char"/>字符组成新的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="insertsymbol"></param>
        /// <returns>由集合组成的字符串</returns>
        public static string ToCombiner<T>(this IEnumerable<T> ts, char insertsymbol)
        {
            return ToCombiner(ts, insertsymbol.ToString());
        }

        /// <summary>
        /// 向某个集合中插入<see cref="string"/>字符串组成新的字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="insertsymbol"></param>
        /// <returns>由集合组成的字符串</returns>
        public static string ToCombiner<T>(this IEnumerable<T> ts, string insertsymbol = "")
        {
            string resultStr = string.Empty;
            foreach (var item in ts)
            {
                resultStr += item + insertsymbol;
            }
            return resultStr.Remove(resultStr.Length - 1);
        }

    }
}
