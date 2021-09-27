using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DynamicGeneratorDll
 * Author： Chance_写代码的厨子
 * Create Time：2021-09-02 16:20:24
 */
namespace MvvmTestDemo.Commumal.IL
{
    public class DynamicGeneratorDll
    {
        public DynamicGeneratorDll()
        {
            //ReaderWriterLockSlim
        }

        public static void ILCreateSumAndSaveAsDll()
        {
#if NET461
            var asmName = new AssemblyName("ChanceTest");
            //创建程序集
            var asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            //定义模块
            var moudleBuilder = asmBuilder.DefineDynamicModule("ChanceTest", "ChanceTest.dll");
            //定义类
            var typeBuilder = moudleBuilder.DefineType("ILTest", TypeAttributes.Public);
            //定义方法
            var methodBuilder = typeBuilder.DefineMethod("CalculateValue", MethodAttributes.Public, typeof(int), null);
            //获取生成器            
            var iL = methodBuilder.GetILGenerator();

            //IL注册变量
            iL.DeclareLocal(typeof(int));
            iL.DeclareLocal(typeof(int));
            var i = iL.DeclareLocal(typeof(int));
            Label lbl = iL.DefineLabel();

            iL.Emit(OpCodes.Ldc_I4_1);
            iL.Emit(OpCodes.Stloc_0);
            iL.Emit(OpCodes.Ldc_I4_2);
            iL.Emit(OpCodes.Stloc_1);
            iL.Emit(OpCodes.Ldloc_0);
            iL.Emit(OpCodes.Ldloc_1);
            iL.Emit(OpCodes.Add);
            iL.Emit(OpCodes.Stloc,i);
            iL.Emit(OpCodes.Br_S,lbl);
            iL.MarkLabel(lbl);
            iL.Emit(OpCodes.Ldloc,i);
            iL.Emit(OpCodes.Ret);

            typeBuilder.CreateType();
            asmBuilder.Save("ChanceTest.dll");
#endif
        }

        public static void ILCreateHexToColorAndSaveAsDll()
        {
#if NET461
            var asmName = new AssemblyName("ChanceTest123");
            var asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);//创建程序集
            var mdlBldr = asmBuilder.DefineDynamicModule("ChanceTest123", "ChanceTest123.dll");//定义模块
            var typeBldr = mdlBldr.DefineType("ColorToArgb", TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed);//定义类
            var methodBldr = typeBldr.DefineMethod("HexadecimalToArgb", MethodAttributes.Public, CallingConventions.Standard,
                typeof(Color), new Type[] { typeof(string) });//定义方法
            var MyILGenerator = methodBldr.GetILGenerator();//获取il生成器
            MyILGenerator.DeclareLocal(typeof(string));//注册变量 string text
            MyILGenerator.DeclareLocal(typeof(int));//int num ;
            MyILGenerator.DeclareLocal(typeof(byte));//byte b;
            MyILGenerator.DeclareLocal(typeof(int));//int num2;
            var b2 = MyILGenerator.DeclareLocal(typeof(byte));
            var b3 = MyILGenerator.DeclareLocal(typeof(byte));
            var b4 = MyILGenerator.DeclareLocal(typeof(byte));
            var color = MyILGenerator.DeclareLocal(typeof(Color));


            //Label defaultCase = MyILGenerator.DefineLabel();
            Label endOfMethod = MyILGenerator.DefineLabel();
            Label forLabel = MyILGenerator.DefineLabel();
            Label[] jumpTable = new Label[] { MyILGenerator.DefineLabel(),MyILGenerator.DefineLabel(),
                      MyILGenerator.DefineLabel(), MyILGenerator.DefineLabel() };
            MyILGenerator.Emit(OpCodes.Ldsfld, string.Empty); //压栈赋值
            MyILGenerator.Emit(OpCodes.Stloc_0);
            MyILGenerator.Emit(OpCodes.Ldc_I4, 1);
            MyILGenerator.Emit(OpCodes.Stloc_1);
            MyILGenerator.Emit(OpCodes.Ldc_I4, 0XFF);
            MyILGenerator.Emit(OpCodes.Stloc_2);
            MyILGenerator.Emit(OpCodes.Ldarg, 0);//Ldarg是加载方法参数的意思。这里arg_0事实上是对当前对象的引用即this
            MyILGenerator.Emit(OpCodes.Callvirt, typeof(string).GetProperty("Length").GetGetMethod());
            LocalBuilder length = MyILGenerator.DeclareLocal(typeof(int));
            MyILGenerator.Emit(OpCodes.Stloc_S, length);
            MyILGenerator.Emit(OpCodes.Ldloc_S, length);
            MyILGenerator.Emit(OpCodes.Ldc_I4_4);
            MyILGenerator.Emit(OpCodes.Sub);
            MyILGenerator.Emit(OpCodes.Switch, jumpTable);
            //MyILGenerator.Emit(OpCodes.Br_S, defaultCase);
            MyILGenerator.MarkLabel(jumpTable[0]);
            MyILGenerator.Emit(OpCodes.Ldc_I4_1);
            MyILGenerator.Emit(OpCodes.Stloc_3);

            MyILGenerator.Emit(OpCodes.Ldstr, "F");
            MyILGenerator.Emit(OpCodes.Stloc_0);
            MyILGenerator.Emit(OpCodes.Ldarg_0);
            MyILGenerator.Emit(OpCodes.Ldloc_1);
            MyILGenerator.Emit(OpCodes.Ldloc_3);
            MyILGenerator.Emit(OpCodes.Callvirt, typeof(string).GetMethod("Substring", new Type[] { typeof(Int32), typeof(Int32) }));
            MyILGenerator.Emit(OpCodes.Ldloc_0);
            MyILGenerator.Emit(OpCodes.Call, typeof(string).GetMethod("Concat", new Type[] { typeof(string), typeof(string) }));
            MyILGenerator.Emit(OpCodes.Ldc_I4_S, 16);
            MyILGenerator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToByte", new Type[] { typeof(string), typeof(Int32) }));
            MyILGenerator.Emit(OpCodes.Stloc_2);
            MyILGenerator.Emit(OpCodes.Ldloc_1);
            MyILGenerator.Emit(OpCodes.Ldloc_3);
            MyILGenerator.Emit(OpCodes.Add);
            MyILGenerator.Emit(OpCodes.Stloc_1);
            //MyILGenerator.Emit(OpCodes.Br_S, endOfMethod);
            MyILGenerator.MarkLabel(jumpTable[1]);
            MyILGenerator.Emit(OpCodes.Ldc_I4_2);
            MyILGenerator.Emit(OpCodes.Stloc_3);
            //MyILGenerator.Emit(OpCodes.Br_S, endOfMethod);
            MyILGenerator.MarkLabel(jumpTable[2]);
            MyILGenerator.Emit(OpCodes.Ldc_I4_2);
            MyILGenerator.Emit(OpCodes.Stloc_3);
            MyILGenerator.Emit(OpCodes.Ldarg_0);
            MyILGenerator.Emit(OpCodes.Ldloc_1);
            MyILGenerator.Emit(OpCodes.Ldloc_3);
            MyILGenerator.Emit(OpCodes.Callvirt, typeof(string).GetMethod("Substring", new Type[] { typeof(Int32), typeof(Int32) }));
            MyILGenerator.Emit(OpCodes.Ldloc_0);
            MyILGenerator.Emit(OpCodes.Call, typeof(string).GetMethod("Concat", new Type[] { typeof(string), typeof(string) }));
            MyILGenerator.Emit(OpCodes.Ldc_I4_S, 16);
            MyILGenerator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToByte", new Type[] { typeof(string), typeof(Int32) }));
            MyILGenerator.Emit(OpCodes.Stloc_2);
            MyILGenerator.Emit(OpCodes.Ldloc_1);
            MyILGenerator.Emit(OpCodes.Ldloc_3);
            MyILGenerator.Emit(OpCodes.Add);
            MyILGenerator.Emit(OpCodes.Stloc_1);
            //MyILGenerator.Emit(OpCodes.Br_S, endOfMethod);
            MyILGenerator.MarkLabel(jumpTable[3]);
            MyILGenerator.Emit(OpCodes.Ldc_I4_2);
            MyILGenerator.Emit(OpCodes.Stloc_3);
            MyILGenerator.Emit(OpCodes.Ldstr, "#FFFFFF");
            MyILGenerator.Emit(OpCodes.Starg_S);//, "Hexadecimal");
            MyILGenerator.Emit(OpCodes.Ldarg_0);
            MyILGenerator.Emit(OpCodes.Ldloc_1);
            MyILGenerator.Emit(OpCodes.Ldloc_3);
            MyILGenerator.Emit(OpCodes.Callvirt, typeof(string).GetMethod("Substring", new Type[] { typeof(Int32), typeof(Int32) }));

            MyILGenerator.Emit(OpCodes.Ldloc_0);
            MyILGenerator.Emit(OpCodes.Call, typeof(string).GetMethod("Concat", new Type[] { typeof(string), typeof(string) }));
            MyILGenerator.Emit(OpCodes.Ldc_I4_S, 16);
            MyILGenerator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToByte", new Type[] { typeof(string), typeof(Int32) }));
            MyILGenerator.Emit(OpCodes.Stloc_S, b2);
            MyILGenerator.Emit(OpCodes.Ldarg_0);
            MyILGenerator.Emit(OpCodes.Ldloc_1);
            MyILGenerator.Emit(OpCodes.Ldloc_3);
            MyILGenerator.Emit(OpCodes.Add);
            MyILGenerator.Emit(OpCodes.Dup);
            MyILGenerator.Emit(OpCodes.Stloc_1);
            MyILGenerator.Emit(OpCodes.Ldloc_3);
            MyILGenerator.Emit(OpCodes.Callvirt, typeof(string).GetMethod("Substring", new Type[] { typeof(Int32), typeof(Int32) }));
            MyILGenerator.Emit(OpCodes.Ldloc_0);
            MyILGenerator.Emit(OpCodes.Call, typeof(string).GetMethod("Concat", new Type[] { typeof(string), typeof(string) }));
            MyILGenerator.Emit(OpCodes.Ldc_I4_S, 16);
            MyILGenerator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToByte", new Type[] { typeof(string), typeof(Int32) }));
            MyILGenerator.Emit(OpCodes.Stloc_S, b3);
            MyILGenerator.Emit(OpCodes.Ldarg_0);
            MyILGenerator.Emit(OpCodes.Ldloc_1);
            MyILGenerator.Emit(OpCodes.Ldloc_3);
            MyILGenerator.Emit(OpCodes.Add);
            MyILGenerator.Emit(OpCodes.Ldloc_3);
            MyILGenerator.Emit(OpCodes.Callvirt, typeof(string).GetMethod("Substring", new Type[] { typeof(Int32), typeof(Int32) }));
            MyILGenerator.Emit(OpCodes.Ldloc_0);
            MyILGenerator.Emit(OpCodes.Call, typeof(string).GetMethod("Concat", new Type[] { typeof(string), typeof(string) }));
            MyILGenerator.Emit(OpCodes.Ldc_I4_S, 16);
            MyILGenerator.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToByte", new Type[] { typeof(string), typeof(Int32) }));
            MyILGenerator.Emit(OpCodes.Stloc_S, b4);
            MyILGenerator.Emit(OpCodes.Ldloc_2);
            MyILGenerator.Emit(OpCodes.Stloc_S, b2);
            MyILGenerator.Emit(OpCodes.Stloc_S, b3);
            MyILGenerator.Emit(OpCodes.Stloc_S, b4);
            MyILGenerator.Emit(OpCodes.Call, typeof(Color).GetMethod("FromArgb", new Type[] { typeof(byte), typeof(byte), typeof(byte), typeof(byte) }));
            MyILGenerator.Emit(OpCodes.Stloc_S, color);
            MyILGenerator.Emit(OpCodes.Br_S, forLabel);
            MyILGenerator.MarkLabel(forLabel);
            MyILGenerator.Emit(OpCodes.Ldloc_S, color);
            MyILGenerator.Emit(OpCodes.Ret);
            typeBldr.CreateType();
            asmBuilder.Save("ChanceTest123.dll");//方便反编译 看代码写的对不对
#endif
        }
    }
}
