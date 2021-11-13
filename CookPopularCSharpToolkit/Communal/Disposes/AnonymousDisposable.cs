using System;
using System.Threading;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：AnonymousDisposable
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 11:37:32
 */
namespace CookPopularCSharpToolkit.Communal
{
    /// <summary>
    /// 匿名可释放对象
    /// </summary>
    internal sealed class AnonymousDisposable : ICancelable, IDisposable
    {
        private volatile Action _dispose;

        public bool IsDisposed
        {
            get
            {
                return this._dispose == null;
            }
        }

        public AnonymousDisposable(Action dispose)
        {
            this._dispose = dispose;
        }

        public void Dispose()
        {
            var action = Interlocked.Exchange<Action>(ref _dispose, null);
            if (action == null)
                return;
            action();
        }
    }
}
