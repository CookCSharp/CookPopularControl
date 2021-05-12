using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CreateObjectHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-12 10:46:19
 */
namespace CookPopularControl.Tools.Helpers
{
    /// <summary>
    /// 创建对象的帮助类
    /// </summary>
    /// <remarks>
    /// 创建对象效率高低排序
    /// 直接New>IL>Activator>泛型
    /// </remarks>
    public class CreateObjectHelper
    {
        /// <summary>
        /// IL创建对象
        /// </summary>
        /// <returns></returns>
        public static TObject CreateInstanceInIL<TObject>()
        {
            Func<TObject>? objCreator = default;
            if (objCreator == null)
            {
                var objType = typeof(TObject);
                ConstructorInfo defaultCtor = objType.GetConstructor(new Type[] { });
                DynamicMethod dynamicMethod = new DynamicMethod(string.Format("_{0:N}", Guid.NewGuid()), objType, null);

                var gen = dynamicMethod.GetILGenerator();
                gen.Emit(OpCodes.Newobj, defaultCtor);
                gen.Emit(OpCodes.Ret);

                objCreator = dynamicMethod.CreateDelegate(typeof(Func<TObject>)) as Func<TObject>;
            }

            return objCreator.Invoke();
        }

        /// <summary>
        /// <see cref="Activator"/>创建对象
        /// </summary>
        /// <returns></returns>
        public static TObject CreateInstanceInActivator<TObject>(Type type = null)
        {
            var instanceType = type ?? typeof(TObject);
            var intance = (TObject)Activator.CreateInstance(type);

            return intance;
        }

        /// <summary>
        /// 泛型方式创建对象
        /// </summary>
        /// <returns></returns>
        public static TObject CreateInstanceInGenerate<TObject>() where TObject : new()
        {
            return new TObject();
        }

        /// <summary>
        /// <see cref="Assembly"/>创建对象
        /// </summary>
        /// <param name="assemblyFilePath">程序集路径，不能是相对路径(exe/dll)</param>
        /// <param name="typeName">类的完全限定名(即包括命名空间),格式为:命名空间.类名</param>
        /// <remarks>没用引用该程序集时</remarks>
        /// <returns></returns>
        public static dynamic CreateInstanceInAssembly(string assemblyFilePath, string typeName)
        {
            var assembly = Assembly.LoadFile(assemblyFilePath);
            dynamic instance = assembly.CreateInstance(typeName);

            return instance;
        }

        /// <summary>
        /// <see cref="Assembly"/>创建对象
        /// </summary>
        /// <param name="assemblyName">程序集名称，不含后缀名</param>
        /// <param name="typeName">类的完全限定名(即包括命名空间),格式为:命名空间.类名</param>
        /// <remarks>不同程序集,装载调用</remarks>
        /// <returns></returns>
        public static dynamic CreateInstanceInDifferentAssembly(string assemblyName, string typeName)
        {
            var assembly = Assembly.Load(assemblyName);
            dynamic instance = assembly.CreateInstance(typeName, false);

            return instance;
        }

        /// <summary>
        /// <see cref="Assembly"/>创建对象
        /// </summary>
        /// <param name="typeName">类的完全限定名(即包括命名空间),格式为:命名空间.类名</param>
        /// <remarks>当前程序集</remarks>
        /// <returns></returns>
        public static dynamic CreateInstanceInAssembly(string typeName)
        {
            var assembly = Assembly.GetExecutingAssembly(); //获取当前程序集           
            dynamic instance = assembly.CreateInstance(typeName);

            //var type = Type.GetType(typeName);
            //dynamic instance = type.Assembly.CreateInstance(type.FullName);

            return instance;
        }
    }
}
