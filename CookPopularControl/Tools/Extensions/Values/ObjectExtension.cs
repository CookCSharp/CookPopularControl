using System;
using System.ComponentModel;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ObjectExtension
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-13 19:43:24
 */
namespace CookPopularControl.Tools.Extensions.Values
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 装箱
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object ToObject(this object obj) => obj;

        /// <summary>
        /// 拆箱
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="obj"></param>
        /// <returns>TType</returns>
        /// <remarks>类型一致则返回值,否则返回该类型(TType)的默认值</remarks>
        public static TType ToTargetType<TType>(this object obj)
            => obj.GetType().Equals(typeof(TType)) ? (TType)obj : default;

        /// <summary>
        /// 拆箱
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="obj"></param>
        /// <returns>TType</returns>
        /// <remarks>强制转换</remarks>
        public static TType ToCastTargetType<TType>(this object obj)
            => new Converter<object, TType>(p =>
            (TType)TypeDescriptor.GetConverter(typeof(TType)).ConvertFrom(obj)).Invoke(obj);

    }
}
