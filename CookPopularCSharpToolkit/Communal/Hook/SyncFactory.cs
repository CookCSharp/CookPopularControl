/*
 * Description：SyncFactory 
 * Author： Chance.Zheng
 * Create Time: 2022-11-03 17:22:39
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2020-2022 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Threading;

namespace CookPopularCSharpToolkit.Communal
{
    internal class MessageHandler : NativeWindow
    {
        internal MessageHandler()
        {
            CreateHandle(new CreateParams());
        }
    }

    internal class SyncFactory : IDisposable
    {
        private readonly Lazy<MessageHandler> messageHandler;
        private readonly Lazy<TaskScheduler> scheduler;
        private bool hasUIThread;


        internal SyncFactory()
        {            
            scheduler = new Lazy<TaskScheduler>(() =>
            {
                //if the calling thread is a UI thread then return its synchronization context
                //no need to create a message pump
                var dispatcher = Dispatcher.FromThread(Thread.CurrentThread);
                if (dispatcher != null)
                {
                    if (SynchronizationContext.Current != null)
                    {
                        hasUIThread = true;
                        return TaskScheduler.FromCurrentSynchronizationContext();
                    }
                }

                TaskScheduler current = null;

                //if current task scheduler is null, create a message pump 
                //http://stackoverflow.com/questions/2443867/message-pump-in-net-windows-service
                //use async for performance gain!
                new Task(() =>
                {
                    Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
                    {
                        Volatile.Write(ref current, TaskScheduler.FromCurrentSynchronizationContext());
                    }), DispatcherPriority.Normal);
                    Dispatcher.Run();
                }).Start();

                //we called dispatcher begin invoke to get the Message Pump Sync Context
                //we check every 10ms until synchronization context is copied
                while (Volatile.Read(ref current) == null)
                {
                    Thread.Sleep(10);
                }

                return Volatile.Read(ref current);
            });

            messageHandler = new Lazy<MessageHandler>(() =>
            {
                MessageHandler msgHandler = null;
                //get the mesage handler dummy window created using the UI sync context
                new Task(e => { Volatile.Write(ref msgHandler, new MessageHandler()); }, GetTaskScheduler()).Start();

                //wait here until the window is created on UI thread
                while (Volatile.Read(ref msgHandler) == null)
                {
                    Thread.Sleep(10);
                };

                return Volatile.Read(ref msgHandler);
            });

            GetTaskScheduler();
            GetHandle();
        }

        /// <summary>
        /// Get the UI task scheduler
        /// </summary>
        /// <returns></returns>
        internal TaskScheduler GetTaskScheduler()
        {
            return scheduler.Value;
        }

        /// <summary>
        /// Get the handle of the window we created on the UI thread
        /// </summary>
        /// <returns></returns>
        internal IntPtr GetHandle()
        {
            var handle = IntPtr.Zero;

            if (hasUIThread)
            {
                try
                {
                    handle = Process.GetCurrentProcess().MainWindowHandle;

                    if (handle != IntPtr.Zero)
                        return handle;
                }
                catch
                {
                }
            }

            return messageHandler.Value.Handle;
        }

        public void Dispose()
        {
            if (messageHandler?.Value != null)
            {
                messageHandler.Value.DestroyHandle();
            }
        }
    }
}
