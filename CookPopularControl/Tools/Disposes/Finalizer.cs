using System;
using System.Runtime.InteropServices;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Finalizer
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-27 10:24:50
 */
namespace CookPopularControl.Tools.Disposes
{
    /// <summary>
    /// 终结器(以前称为析构函数)
    /// </summary>
    /// <remarks>
    /// 用于在垃圾回收器收集类实例时执行任何必要的最终清理操作。 
    /// 在大多数情况下，通过使用 System.Runtime.InteropServices.SafeHandle 或派生类包装任何非托管句柄，可以免去编写终结器的过程。
    /// </remarks>
    public class Finalizer : IDisposable
    {
        private bool disposedValue;
        private IntPtr buffer; //Unmanaged memory buffer
        private SafeHandle resource; //Disposable handle to a resource

        public Finalizer()
        {
            //this.buffer = //Allocates memory
            //this.resource = //Allocates the resource
        }

        public void RunNativeMethod()
        {
            if (disposedValue)
                throw new ObjectDisposedException("ObjectName is disposed");

            // TODO
        }

        // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        ~Finalizer()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);

            //GC.WaitForPendingFinalizers();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    if (resource != null)
                        resource.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null

                disposedValue = true;
            }
        }
    }
}
