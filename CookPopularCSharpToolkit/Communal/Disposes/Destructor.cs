using System;
using System.Runtime.InteropServices;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Destructor
 * Author： Chance_写代码的厨子
 * Create Time：2021-07-23 10:59:54
 */
namespace CookPopularCSharpToolkit.Communal
{
    /// <summary>
    /// 析构器
    /// </summary>
    public class DestructorHandle
    {
        // 释放非托管资源
        [DllImport("Kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool CloseHandle(IntPtr handle);


        private IntPtr handle;  //保存一个非托管资源的win32句柄


        public DestructorHandle(IntPtr handle)
        {
            this.handle = handle;
        }

        //当垃圾收集执行的时候，下面的析构器（Finalize）方法
        //将被调用，它将关闭非托管资源句柄。
        ~DestructorHandle()
        {
            CloseHandle(handle);
        }

        // 返回所有封装的handle句柄
        public IntPtr ToHandle()
        {
            return handle;
        }

        // 隐式转换操作符也用于返回所封装的Handle句柄
        public static implicit operator IntPtr(DestructorHandle osHandle)
        {
            return osHandle.ToHandle();
        }
    }
}
