using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DynamicProxy
 * Author： Chance_写代码的厨子
 * Create Time：2021-09-03 13:49:40
 */
namespace MvvmTestDemo.Commumal.IL
{
    public sealed class DynamicProxy
    {
        private static readonly string AssemblyName = "DynamicProxy";
        private static readonly string ModuleName = "DynamicProxy";
        private static readonly string TypeName = "DynamicProxy";


        /// <summary>
        /// 创建动态代理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T CreateDynamicType<T>() where T : class, new()
        {
            TypeBuilder typeBuilder = CreateDynamicMoudle<T>().DefineType(TypeName + typeof(T).Name, TypeAttributes.Public | TypeAttributes.Class, typeof(T));
            TypeActuator<T>(typeBuilder);

            return Activator.CreateInstance(typeBuilder.CreateType()) as T;
        }

        private AssemblyBuilder CreateDynamicAssembly<T>() where T : class
        {
#if NET461
            return AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(AssemblyName + typeof(T).Name), AssemblyBuilderAccess.Run);
#else
            return default;
#endif
        }

        private ModuleBuilder CreateDynamicMoudle<T>() where T : class
        {
#if NET461
            return CreateDynamicAssembly<T>().DefineDynamicModule(ModuleName + typeof(T).Name);
#else
            return default;
#endif
        }

        private void TypeActuator<T>(TypeBuilder typeBuilder) where T : class
        {
            FieldBuilder fieldBuilder = typeBuilder.DefineField("_DynamicProxyActuator", typeof(T), FieldAttributes.Private);
            BuildCtorMethod(typeof(T), fieldBuilder, typeBuilder);
            MethodInfo[] methodInfos = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Instance); ;
            foreach (MethodInfo methodInfo in methodInfos)
            {
                if (!methodInfo.IsVirtual && !methodInfo.IsAbstract) continue;
                if (methodInfo.Name == "ToString") continue;
                if (methodInfo.Name == "GetHashCode") continue;
                if (methodInfo.Name == "Equals") continue;

                var parameterTypes = methodInfo.GetParameters().Select(p => p.ParameterType).ToArray();
                MethodBuilder methodBuilder = CreateMethod(typeBuilder, methodInfo.Name, MethodAttributes.Public | MethodAttributes.Virtual,
                    CallingConventions.Standard, methodInfo.ReturnType, parameterTypes);
                var ilMethod = methodBuilder.GetILGenerator();
                BuildMethod(ilMethod, methodInfo, parameterTypes);
            }
        }

        private void BuildCtorMethod(Type classType, FieldBuilder fieldBuilder, TypeBuilder typeBuilder)
        {
            var structureBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, null);
            var ilCtor = structureBuilder.GetILGenerator();
            ilCtor.Emit(OpCodes.Ldarg_0);
            ilCtor.Emit(OpCodes.Newobj, classType.GetConstructor(Type.EmptyTypes));
            ilCtor.Emit(OpCodes.Stfld, fieldBuilder);
            ilCtor.Emit(OpCodes.Ret);
        }

        private void BuildMethod(ILGenerator il, MethodInfo methodInfo, Type[] parameterTypes)
        {
            il.Emit(OpCodes.Ldarg_0);
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_S, (short)i + 1);
            }

            il.Emit(OpCodes.Call, methodInfo);
            il.Emit(OpCodes.Ret);
        }

        private MethodBuilder CreateMethod(TypeBuilder typeBuilder, string name, MethodAttributes attrs, CallingConventions callingConventions, Type type, Type[] parameterTypes)
        {
            return typeBuilder.DefineMethod(name, attrs, callingConventions, type, parameterTypes);
        }
    }
}
