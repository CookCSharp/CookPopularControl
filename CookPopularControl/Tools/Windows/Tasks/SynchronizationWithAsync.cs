using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：SynchronizationWithAsync
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-13 19:36:28
 */
namespace CookPopularControl.Tools.Windows.Tasks
{
    /// <summary>
    /// 表示同步异步的委托方法
    /// </summary>
    /// <remarks><see cref="Dispatcher"/>与<see cref="SynchronizationContext"/></remarks>
    public sealed class SynchronizationWithAsync
    {
        public static void AppInvokeAsync(Action action)
        {
            Application.Current.Dispatcher.InvokeAsync(action, DispatcherPriority.Normal);
        }

        public static void AppInvoke(Action action)
        {
            Application.Current.Dispatcher.Invoke(action, DispatcherPriority.Normal);
        }

        public static void AppSyncPost(SendOrPostCallback action, SynchronizationContext synchronizationContext, object state = null)
        {
            if (synchronizationContext == null) synchronizationContext = new DispatcherSynchronizationContext(Application.Current.Dispatcher, DispatcherPriority.Normal);
            SynchronizationContext.SetSynchronizationContext(synchronizationContext);
            SynchronizationContext.Current.Post(action, state);
        }

        public static void AppSyncSend(SendOrPostCallback action, SynchronizationContext synchronizationContext, object state = null)
        {
            if (synchronizationContext == null) synchronizationContext = new DispatcherSynchronizationContext(Application.Current.Dispatcher, DispatcherPriority.Normal);
            SynchronizationContext.SetSynchronizationContext(synchronizationContext);
            SynchronizationContext.Current.Send(action, state);
        }
    }
}
