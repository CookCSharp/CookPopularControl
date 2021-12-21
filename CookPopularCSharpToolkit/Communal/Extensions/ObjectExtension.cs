using System;
using System.ComponentModel;
using System.Reflection;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ObjectExtension
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-13 19:43:24
 */
namespace CookPopularCSharpToolkit.Communal
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


        /// <summary>
        /// 清除一个对象的某个事件所挂钩的delegate
        /// </summary>
        /// <param name="ctrl">控件对象</param>
        /// <param name="eventName">事件名称，默认的</param>
        public static void ClearEvents(this object ctrl, string eventName = "_EventAll")
        {
            if (ctrl == null) return;
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Static;
            EventInfo[] events = ctrl.GetType().GetEvents(bindingFlags);
            if (events == null || events.Length < 1) return;

            for (int i = 0; i < events.Length; i++)
            {
                try
                {
                    EventInfo ei = events[i];
                    //只删除指定的方法，默认是_EventAll，前面加_是为了和系统的区分，防以后雷同
                    if (eventName != "_EventAll" && ei.Name != eventName) continue;

                    /********************************************************
                     * class的每个event都对应了一个同名(变了，前面加了Event前缀)的private的delegate类
                     * 型成员变量（这点可以用Reflector证实）。因为private成
                     * 员变量无法在基类中进行修改，所以为了能够拿到base 
                     * class中声明的事件，要从EventInfo的DeclaringType来获取
                     * event对应的成员变量的FieldInfo并进行修改
                     ********************************************************/
                    FieldInfo fi = ei.DeclaringType.GetField("Event_" + ei.Name, bindingFlags);
                    if (fi != null)
                    {
                        // 将event对应的字段设置成null即可清除所有挂钩在该event上的delegate
                        fi.SetValue(ctrl, null);
                    }
                }
                catch { }
            }
        }
    }
}
