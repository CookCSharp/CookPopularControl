using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MvvmTestDemo.DemosView;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ClassFactory
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-26 15:28:17
 */
namespace MvvmTestDemo.Commumal
{
    public class ObjectFactory
    {
        private static HashSet<UserControl> AllControlsDemo = new();
        private static readonly ConcurrentDictionary<string, object> DemoCacheDic = new ConcurrentDictionary<string, object>();

        public static void Register(string name, object instance) => DemoCacheDic[name] = instance;

        public static object ResolveIntance(string key)
        {
            if(DemoCacheDic.TryGetValue(key,out object instance))
            {
                return instance;
            }

            return null;
        }

        /// <summary>
        /// <see cref="Activator"/>创建对象
        /// </summary>
        /// <param name="className">类名称</param>
        /// <returns></returns>
        public static object CreateInstanceInActivator(string className)
        {
            var assembly = Assembly.GetExecutingAssembly(); //获取当前程序集
            var namespaceStr = assembly.GetName().Name;

            var type = Type.GetType($"{namespaceStr}.DemosView.{className}");
            var instance = type == null ? null : Activator.CreateInstance(type);

            return instance;
        }
    }
}
