using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：SerialDisposable
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 10:44:21
 */
namespace CookPopularControl.References
{
    /// <summary>
    /// 串行可释放对象 
    /// </summary>
    public class SerialDisposable : ICancelable, IDisposable
    {
        private readonly object _gate = new object();
        private IDisposable _current;
        private bool _disposed;

        /// <summary>
        /// 获取一个值，该值指示是否已释放该对象
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                lock (this._gate)
                    return this._disposed;
            }
        }

        /// <summary>
        /// 释放潜在的对象
        /// </summary>
        public void Dispose()
        {
            IDisposable? disposable = default;
            lock (this._gate)
            {
                if (!this._disposed)
                {
                    this._disposed = true;
                    disposable = this._current;
                    this._current = default;
                }
            }
            if (disposable == null)
                return;
            disposable.Dispose();
        }

        /// <summary>
        /// 获取或设置可释放的对象
        /// </summary>
        /// <remarks>
        /// 如果SerialDisposable已经被释放，则对该属性的赋值将导致立即释放给定的可释放对象。
        /// 指定此属性将处理前一个可释放对象。
        /// </remarks>
        public IDisposable Disposable
        {
            get
            {
                return this._current;
            }
            set
            {
                bool flag = false;
                IDisposable? disposable = null;
                lock (this._gate)
                {
                    flag = this._disposed;
                    if (!flag)
                    {
                        disposable = this._current;
                        this._current = value;
                    }
                }
                if (disposable != null)
                    disposable.Dispose();
                if (!flag || value == null)
                    return;
                value.Dispose();
            }
        }
    }
}
