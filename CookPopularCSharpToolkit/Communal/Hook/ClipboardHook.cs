/*
 * Description：ClipboardHook 
 * Author： Chance.Zheng
 * Create Time: 2022-11-04 16:22:41
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2020-2022 All Rights Reserved.
 */


using CookPopularCSharpToolkit.Windows.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using CookPopularCSharpToolkit.Windows;
using System.Windows.Threading;

namespace CookPopularCSharpToolkit.Communal
{
    internal class ClipboardHook
    {
        private IntPtr _clipboardViewerNext;
        private IntPtr _hookId = IntPtr.Zero;
        private HwndSource _hWndSource;

        internal event EventHandler ClipboardContentChanged = delegate { };

        /// <summary>
        /// Register this window as a Clipboard Viewer application
        /// </summary>
        internal void RegisterClipboardViewer()
        {
            _clipboardViewerNext = InteropMethods.SetClipboardViewer(_hookId);
        }

        /// <summary>
        /// Remove this window from the Clipboard Viewer list
        /// </summary>
        internal void UnregisterClipboardViewer()
        {
            InteropMethods.ChangeClipboardChain(_hookId, _clipboardViewerNext);
        }

        internal void Start()
        {
            _hookId = new Window().EnsureHandle();
            _hWndSource = HwndSource.FromHwnd(_hookId);
            if (_hWndSource != null)
            {
                _hWndSource.AddHook(WinProc);
                InteropMethods.AddClipboardFormatListener(_hookId);
            }
        }

        internal void Stop()
        {
            _hWndSource.RemoveHook(WinProc);
            InteropMethods.RemoveClipboardFormatListener(_hookId);

            _hookId = IntPtr.Zero;
        }

        private void GetClipboardData()
        {
            Dispatcher.CurrentDispatcher.InvokeAsync(() =>
            {
                var data = Clipboard.GetDataObject();
                ClipboardContentChanged(data, new EventArgs());
            }).Wait();
        }

        private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == InteropValues.WM_CLIPBOARDUPDATE)
            {
                //GetClipboardData();
            }
            else if (msg == (int)InteropValues.WM.DRAWCLIPBOARD)
            {
                GetClipboardData();

                InteropMethods.SendMessage(_clipboardViewerNext, msg, wParam, lParam);
            }
            else if (msg == (int)InteropValues.WM.CHANGECBCHAIN)
            {
                if (wParam == _clipboardViewerNext)
                {
                    _clipboardViewerNext = lParam;
                }
                else
                {
                    InteropMethods.SendMessage(_clipboardViewerNext, msg, wParam, lParam);
                }
            }

            return IntPtr.Zero;
        }
    }

    public enum ClipboardContentTypes
    {
        PlainText = 0,
        RichText = 1,
        Html = 2,
        Csv = 3,
        UnicodeText = 4
    }

    public class ClipboardHookEventArgs : EventArgs
    {
        public ClipboardContentTypes DataFormat { get; set; }
        public object Data { get; set; }
    }

    public class ClipboardWatcher
    {
        private readonly object Accesslock = new object();

        private readonly SyncFactory factory;
        private AsyncConcurrentQueue<object> clipQueue;
        private CancellationTokenSource taskCancellationTokenSource;
        private ClipboardHook clipboardHook;
        private bool isRunning;

        public event EventHandler<ClipboardHookEventArgs> OnClipboardContentChanged;

        internal ClipboardWatcher(SyncFactory factory)
        {
            this.factory = factory;
        }

        public void Start()
        {
            lock (Accesslock)
            {
                if (!isRunning)
                {
                    taskCancellationTokenSource = new CancellationTokenSource();
                    clipQueue = new AsyncConcurrentQueue<object>(taskCancellationTokenSource.Token);

                    //This needs to run on UI thread context
                    //So use task factory with the shared UI message pump thread
                    //Task.Factory.StartNew(() =>
                    //{
                    //    clipboardHook = new ClipboardHook();
                    //    clipboardHook.ClipboardContentChanged += ClipboardHandler;
                    //    clipboardHook.Start();
                    //    clipboardHook.RegisterClipboardViewer();
                    //}, CancellationToken.None, TaskCreationOptions.None, factory.GetTaskScheduler()).Wait();

                    Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                    {
                        clipboardHook = new ClipboardHook();
                        clipboardHook.ClipboardContentChanged += ClipboardHandler;
                        clipboardHook.Start();
                        clipboardHook.RegisterClipboardViewer();
                    }, DispatcherPriority.Normal).Wait();

                    Task.Factory.StartNew(() => ClipConsumerAsync());

                    isRunning = true;
                }
            }
        }

        public void Stop()
        {
            lock (Accesslock)
            {
                if (isRunning)
                {
                    if (clipboardHook != null)
                    {
                        //This needs to run on UI thread context
                        //So use task factory with the shared UI message pump thread
                        //Task.Factory.StartNew(() =>
                        //{
                        //    clipboardHook.ClipboardContentChanged -= ClipboardHandler;
                        //    clipboardHook.UnregisterClipboardViewer();
                        //    clipboardHook.Stop();
                        //    clipboardHook = null;
                        //}, CancellationToken.None, TaskCreationOptions.None, factory.GetTaskScheduler());

                        Dispatcher.CurrentDispatcher.Invoke(() =>
                        {
                            clipboardHook.ClipboardContentChanged -= ClipboardHandler;
                            clipboardHook.UnregisterClipboardViewer();
                            clipboardHook.Stop();
                            clipboardHook = null;
                        }, DispatcherPriority.Normal);
                    }

                    isRunning = false;
                    clipQueue.Enqueue(false);
                    taskCancellationTokenSource.Cancel();
                }
            }
        }

        /// <summary>
        /// Add event to producer queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClipboardHandler(object sender, EventArgs e)
        {
            clipQueue.Enqueue(sender);
        }

        /// <summary>
        /// Consume event from producer queue asynchronously
        /// </summary>
        /// <returns></returns>
        private async Task ClipConsumerAsync()
        {
            while (isRunning)
            {
                var item = await clipQueue.DequeueAsync();

                if (item is null)
                {
                    continue;
                }

                if (item is bool)
                {
                    break;
                }

                ClipboardHandler(item);
            }
        }

        private void ClipboardHandler(object sender)
        {
            IDataObject iData = (DataObject)sender;

            var format = default(ClipboardContentTypes);

            object data = null;

            bool validDataType = false;
            if (iData.GetDataPresent(DataFormats.Text))
            {
                format = ClipboardContentTypes.PlainText;
                data = iData.GetData(DataFormats.Text);
                validDataType = true;
            }
            else if (iData.GetDataPresent(DataFormats.Rtf))
            {
                format = ClipboardContentTypes.RichText;
                data = iData.GetData(DataFormats.Rtf);
                validDataType = true;
            }
            else if (iData.GetDataPresent(DataFormats.CommaSeparatedValue))
            {
                format = ClipboardContentTypes.Csv;
                data = iData.GetData(DataFormats.CommaSeparatedValue);
                validDataType = true;
            }
            else if (iData.GetDataPresent(DataFormats.Html))
            {
                format = ClipboardContentTypes.Html;
                data = iData.GetData(DataFormats.Html);
                validDataType = true;
            }

            else if (iData.GetDataPresent(DataFormats.StringFormat))
            {
                format = ClipboardContentTypes.PlainText;
                data = iData.GetData(DataFormats.StringFormat);
                validDataType = true;
            }
            else if (iData.GetDataPresent(DataFormats.UnicodeText))
            {
                format = ClipboardContentTypes.UnicodeText;
                data = iData.GetData(DataFormats.UnicodeText);
                validDataType = true;
            }

            if (!validDataType)
            {
                return;
            }

            OnClipboardContentChanged?.Invoke(null, new ClipboardHookEventArgs { Data = data, DataFormat = format });
        }
    }
}
