using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DoubleExtension
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-12 12:41:54
 */
namespace CookPopularControl.Tools.Extensions.Values
{
    public static class DoubleExtension
    {        
        public static double ExtractDouble(this object val)
        {
            var d = val as double? ?? double.NaN;
            return double.IsInfinity(d) ? double.NaN : d;
        }

        /// <summary>
        /// 判断集合中是否都为数字
        /// </summary>
        /// <param name="vals"></param>
        /// <returns></returns>
        public static bool AnyNan(this IEnumerable<double> vals)
        {
            return vals.Any(double.IsNaN);
        }

        /// <summary>
        /// 判断一个数值是否在A(min)与B(max)之间
        /// </summary>
        /// <param name="targetValue">目标值</param>
        /// <param name="min">小值</param>
        /// <param name="max">大值</param>
        /// <param name="isContainMin">包含最小值</param>
        /// <param name="isContainMax">包含最大值</param>
        /// <returns>true</returns>
        public static bool BetweenMinMax(this double targetValue, double min, double max, bool isContainMin = false, bool isContainMax = false)
        {
            if(isContainMin && isContainMax)
            {
                if (targetValue >= min && targetValue <= max)
                    return true;
                else
                    return false;
            }
            else if(!isContainMin && !isContainMax)
            {
                if (targetValue > min && targetValue < max)
                    return true;
                else
                    return false;
            }
            else if (isContainMin && isContainMax)
            {
                if (targetValue >= min && targetValue < max)
                    return true;
                else
                    return false;
            }
            else if (!isContainMin && isContainMax)
            {
                if (targetValue > min && targetValue <= max)
                    return true;
                else
                    return false;
            }

            return false;
        }
    }
}
