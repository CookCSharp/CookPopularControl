/*
 * Description：MouseHook 
 * Author： Chance.Zheng
 * Create Time: 2022-11-03 15:55:21
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2020-2022 All Rights Reserved.
 */


using CookPopularCSharpToolkit.Windows.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace CookPopularCSharpToolkit.Communal
{
    public enum MouseMessageType
    {
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_WHEELBUTTONDOWN = 0x207,
        WM_WHEELBUTTONUP = 0x208,
        WM_XBUTTONDOWN = 0x020B,
        WM_XBUTTONUP = 0x020C
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IntPoint
    {
        public readonly int X;
        public int Y;

        public IntPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class MouseHookEventArgs : EventArgs
    {
        public MouseMessageType MessageType { get; set; }
        public IntPoint Point { get; set; }
        public uint MouseData { get; set; }
    }

    internal class MouseHook
    {
        private const int WH_MOUSE_LL = 14;
        private static IntPtr _hookId = IntPtr.Zero;

        private readonly InteropValues.HookProc Proc;

        public MouseHook()
        {
            Proc = HookCallback;
        }

        public event EventHandler<MouseHookEventArgs> MouseAction = delegate { };

        public void Start()
        {
            _hookId = SetHook(Proc);
        }

        public void Stop()
        {
            InteropMethods.UnhookWindowsHookEx(_hookId);
        }

        private static IntPtr SetHook(InteropValues.HookProc proc)
        {
            var hook = InteropMethods.SetWindowsHookEx(WH_MOUSE_LL, proc, InteropMethods.GetModuleHandle("user32"), 0);
            if (hook == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            return hook;
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            InteropValues.MOUSEHOOKSTRUCT hookStruct;
            if (nCode < 0)
            {
                return InteropMethods.CallNextHookEx(_hookId, nCode, wParam, lParam);
            }

            hookStruct = (InteropValues.MOUSEHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(InteropValues.MOUSEHOOKSTRUCT));

            MouseAction.Invoke(null, new MouseHookEventArgs { MessageType = (MouseMessageType)wParam, Point = new IntPoint(hookStruct.pt.X, hookStruct.pt.Y), MouseData = hookStruct.mouseData });

            return InteropMethods.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }
    }

    public class MouseWatcher
    {
        private readonly object Accesslock = new object();
        private MouseHook mouseHook;
        private readonly SyncFactory factory;
        private AsyncConcurrentQueue<object> mouseQueue;
        private CancellationTokenSource taskCancellationTokenSource;
        private bool isRunning;

        public event EventHandler<MouseHookEventArgs> OnMouseInput;

        internal MouseWatcher(SyncFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Start watching mouse events
        /// </summary>
        public void Start()
        {
            lock (Accesslock)
            {
                if (!isRunning)
                {
                    taskCancellationTokenSource = new CancellationTokenSource();
                    mouseQueue = new AsyncConcurrentQueue<object>(taskCancellationTokenSource.Token);
                    //This needs to run on UI thread context
                    //So use task factory with the shared UI message pump thread
                    Task.Factory.StartNew(() =>
                    {
                        mouseHook = new MouseHook();
                        mouseHook.MouseAction += MouseListener;
                        mouseHook.Start();
                    }, CancellationToken.None, TaskCreationOptions.None, factory.GetTaskScheduler()).Wait();

                    Task.Factory.StartNew(() => ConsumeKeyAsync());

                    isRunning = true;
                }
            }
        }

        /// <summary>
        /// Stop watching mouse events
        /// </summary>
        public void Stop()
        {
            lock (Accesslock)
            {
                if (isRunning)
                {
                    if (mouseHook != null)
                    {
                        //This needs to run on UI thread context
                        //So use task factory with the shared UI message pump thread
                        Task.Factory.StartNew(() =>
                        {
                            mouseHook.MouseAction -= MouseListener;
                            mouseHook.Stop();
                            mouseHook = null;
                        }, CancellationToken.None, TaskCreationOptions.None, factory.GetTaskScheduler());
                    }

                    mouseQueue.Enqueue(false);
                    isRunning = false;
                    taskCancellationTokenSource.Cancel();
                }
            }
        }

        /// <summary>
        /// Add mouse event to our producer queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseListener(object sender, MouseHookEventArgs e)
        {
            mouseQueue.Enqueue(e);
        }

        /// <summary>
        /// Consume mouse events in our producer queue asynchronously
        /// </summary>
        /// <returns></returns>
        private async Task ConsumeKeyAsync()
        {
            while (isRunning)
            {
                //blocking here until a key is added to the queue
                var item = await mouseQueue.DequeueAsync();

                if (item is null)
                {
                    continue;
                }

                if (item is bool)
                {
                    break;
                }

                OnMouseInput?.Invoke(null, item as MouseHookEventArgs);
            }
        }
    }
}
