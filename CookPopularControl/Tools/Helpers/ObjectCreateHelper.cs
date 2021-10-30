using System;
using System.Reflection;
using System.Reflection.Emit;



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
    /// 创建对象效率高低排序,
    /// 直接New>IL>Activator>泛型
    /// </remarks>
    public class ObjectCreateHelper
    {
        /// <summary>
        /// IL创建对象
        /// </summary>
        /// <returns></returns>
        public static TObject CreateInstanceInIL<TObject>()
        {
            try
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
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// <see cref="Activator"/>创建对象
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static TObject CreateInstanceInActivator<TObject>(Type type = null)
        {
            try
            {
                var instanceType = type ?? typeof(TObject);
                var intance = (TObject)Activator.CreateInstance(type);

                return intance;
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// <see cref="Assembly"/>创建对象
        /// </summary>
        /// <param name="nameSpaceStr">完整命名空间名称</param>
        /// <param name="className">类名称</param>
        /// <remarks>不同程序集,装载调用</remarks>
        /// <returns></returns>
        public static object CreateInstanceInClassName(string nameSpaceStr, string className)
        {
            try
            {
                var assemblyName = nameSpaceStr.Split('.')[0];
                var assembly = Assembly.Load(assemblyName);
                var instance = assembly.CreateInstance($"{nameSpaceStr}.{className}");

                //var type = assembly.GetType($"{nameSpaceStr}.{className}");
                //var instance = type == null ? null : Activator.CreateInstance(type);

                return instance;
            }
            catch (Exception)
            {
                return null;
            }
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
        public static object CreateInstanceInAssemblyFile(string assemblyFilePath, string typeName)
        {
            try
            {
                var assembly = Assembly.LoadFile(assemblyFilePath);
                var instance = assembly?.CreateInstance(typeName);

                return instance;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// <see cref="Assembly"/>创建对象
        /// </summary>
        /// <param name="assemblyName">程序集名称，不含后缀名</param>
        /// <param name="typeName">类的完全限定名(即包括命名空间),格式为:命名空间.类名</param>
        /// <remarks>不同程序集,装载调用</remarks>
        /// <returns></returns>
        public static object CreateInstanceInDifferentAssembly(string assemblyName, string typeName)
        {
            try
            {
                var assembly = Assembly.Load(assemblyName);
                var instance = assembly?.CreateInstance(typeName, false);

                return instance;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// <see cref="Assembly"/>创建对象
        /// </summary>
        /// <param name="typeName">类的完全限定名(即包括命名空间),格式为:命名空间.类名</param>
        /// <remarks>当前程序集</remarks>
        /// <returns></returns>
        public static object CreateInstanceInCurrentAssembly(string typeName)
        {
            try
            {
                var assembly = Assembly.GetEntryAssembly(); //获取当前程序集
                var instance = assembly.CreateInstance(typeName);

                //var type = Type.GetType(typeName);
                //dynamic instance = type.Assembly.CreateInstance(type.FullName);

                return instance;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
