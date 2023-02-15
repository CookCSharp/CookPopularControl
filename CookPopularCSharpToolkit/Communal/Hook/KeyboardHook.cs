/*
 * Description：KeyboardHook 
 * Author： Chance.Zheng
 * Create Time: 2022-11-04 13:56:17
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2020-2022 All Rights Reserved.
 */


using CookPopularCSharpToolkit.Windows.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace CookPopularCSharpToolkit.Communal
{
    internal class KeyboardHook
    {
        enum KeyEvent
        {
            /// <summary>
            ///     Key down
            /// </summary>
            WM_KEYDOWN = 256,

            /// <summary>
            ///     Key up
            /// </summary>
            WM_KEYUP = 257,

            /// <summary>
            ///     System key up
            /// </summary>
            WM_SYSKEYUP = 261,

            /// <summary>
            ///     System key down
            /// </summary>
            WM_SYSKEYDOWN = 260
        }

        /// <summary>
        /// Asynchronous callback hook.
        /// </summary>
        /// <param name="character">Character</param>
        /// <param name="keyEvent">Keyboard event</param>
        /// <param name="vkCode">VKCode</param>
        private delegate void KeyboardCallbackAsync(KeyEvent keyEvent, int vkCode, string character);

        private Dispatcher _dispatcher;
        private IntPtr _hookId;
        //http://stackoverflow.com/questions/6193711/call-has-been-made-on-garbage-collected-delegate-in-c
        private InteropValues.HookProc _hookProcDelegateToAvoidGC;
        /// <summary>
        /// Event to be invoked asynchronously (BeginInvoke) each time key is pressed.
        /// </summary>
        private KeyboardCallbackAsync _hookedKeyboardCallbackAsync;

        private uint _lastVkCode;
        private uint _lastScanCode;
        private byte[] _lastKeyState = new byte[255];
        private bool _lastIsDead;

        internal event EventHandler<KeyboardHookEventArgs> KeyDown = delegate { };
        internal event EventHandler<KeyboardHookEventArgs> KeyUp = delegate { };

        [MethodImpl(MethodImplOptions.NoInlining)]
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (wParam.ToInt32() == (int)KeyEvent.WM_KEYDOWN ||
                    wParam.ToInt32() == (int)KeyEvent.WM_KEYUP ||
                    wParam.ToInt32() == (int)KeyEvent.WM_SYSKEYDOWN ||
                    wParam.ToInt32() == (int)KeyEvent.WM_SYSKEYUP)
                {
                    // Captures the character(s) pressed only on WM_KEYDOWN
                    string chars = VkCodeToString((uint)Marshal.ReadInt32(lParam),
                        wParam.ToInt32() == (int)KeyEvent.WM_KEYDOWN ||
                        wParam.ToInt32() == (int)KeyEvent.WM_SYSKEYDOWN);

                    var s = Marshal.ReadInt32(lParam);
                    
                    _hookedKeyboardCallbackAsync.Invoke((KeyEvent)wParam.ToInt32(), Marshal.ReadInt32(lParam), chars);
                }
            }

            return InteropMethods.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        /// <summary>
        /// Convert VKCode to Unicode.
        /// </summary>
        /// <param name="vkCode">VKCode</param>
        /// <param name="isKeyDown">Is the key down event?</param>
        /// <remarks>isKeyDown is required for because of keyboard state inconsistencies!</remarks>
        /// <returns>String representing single unicode character.</returns>
        private string VkCodeToString(uint vkCode, bool isKeyDown)
        {
            // ToUnicodeEx needs StringBuilder, it populates that during execution.
            var sbString = new StringBuilder(5);

            var bKeyState = new byte[255];
            bool bKeyStateStatus;
            bool isDead = false;

            // Gets the current windows window handle, threadID, processID
            var currentHWnd = InteropMethods.GetForegroundWindow();
            uint currentProcessId;
            uint currentWindowThreadId = InteropMethods.GetWindowThreadProcessId(currentHWnd, out currentProcessId);

            // This programs Thread ID
            uint thisProgramThreadId = InteropMethods.GetCurrentThreadId();

            // Attach to active thread so we can get that keyboard state
            if (InteropMethods.AttachThreadInput(thisProgramThreadId, currentWindowThreadId, true))
            {
                // Current state of the modifiers in keyboard
                bKeyStateStatus = InteropMethods.GetKeyboardState(bKeyState);

                // Detach
                InteropMethods.AttachThreadInput(thisProgramThreadId, currentWindowThreadId, false);
            }
            else
            {
                // Could not attach, perhaps it is this process?
                bKeyStateStatus = InteropMethods.GetKeyboardState(bKeyState);
            }

            // On failure we return empty string.
            if (!bKeyStateStatus)
            {
                return "";
            }

            // Gets the layout of keyboard
            var hkl = InteropMethods.GetKeyboardLayout(currentWindowThreadId);

            // Maps the virtual keycode
            uint lScanCode = InteropMethods.MapVirtualKeyEx(vkCode, 0, hkl);

            // Keyboard state goes inconsistent if this is not in place. In other words, we need to call above commands in UP events also.
            if (!isKeyDown)
            {
                return "";
            }

            // Converts the VKCode to unicode
            int relevantKeyCountInBuffer = InteropMethods.ToUnicodeEx(vkCode, lScanCode, bKeyState, sbString, sbString.Capacity, 0, hkl);
            string ret = string.Empty;

            void ClearKeyboardBuffer(uint vk, uint sc, IntPtr hkl)
            {
                var sb = new StringBuilder(10);

                int rc;
                do
                {
                    var lpKeyStateNull = new byte[255];
                    rc = InteropMethods.ToUnicodeEx(vk, sc, lpKeyStateNull, sb, sb.Capacity, 0, hkl);
                } while (rc < 0);
            }

            switch (relevantKeyCountInBuffer)
            {
                // Dead keys (^,`...)
                case -1:
                    isDead = true;
                    // We must clear the buffer because ToUnicodeEx messed it up, see below.
                    ClearKeyboardBuffer(vkCode, lScanCode, hkl);
                    break;
                case 0:
                    break;
                // Single character in buffer
                case 1:
                    ret = sbString[0].ToString();
                    break;
                // Two or more (only two of them is relevant)
                default:
                    ret = sbString.ToString().Substring(0, 2);
                    break;
            }

            // We inject the last dead key back, since ToUnicodeEx removed it.
            // More about this peculiar behavior see e.g: 
            // http://www.experts-exchange.com/Programming/System/Windows__Programming/Q_23453780.html
            // http://blogs.msdn.com/michkap/archive/2005/01/19/355870.aspx
            // http://blogs.msdn.com/michkap/archive/2007/10/27/5717859.aspx
            if (_lastVkCode != 0 && _lastIsDead)
            {
                var sbTemp = new StringBuilder(5);
                InteropMethods.ToUnicodeEx(_lastVkCode, _lastScanCode, _lastKeyState, sbTemp, sbTemp.Capacity, 0, hkl);
                _lastVkCode = 0;

                return ret;
            }

            // Save these
            _lastScanCode = lScanCode;
            _lastVkCode = vkCode;
            _lastIsDead = isDead;
            _lastKeyState = (byte[])bKeyState.Clone();

            return ret;
        }

        private IntPtr SetHook(InteropValues.HookProc proc)
        {
            using var curProcess = Process.GetCurrentProcess();
            using var curModule = curProcess.MainModule;
            if (curModule != null)
            {
                return InteropMethods.SetWindowsHookEx((int)InteropValues.HookType.WH_KEYBOARD_LL, proc,
                    InteropMethods.GetModuleHandle(curModule.ModuleName), 0);
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// HookCallbackAsync procedure that calls accordingly the KeyDown or KeyUp events.
        /// </summary>
        /// <param name="keyEvent">Keyboard event</param>
        /// <param name="vkCode">VKCode</param>
        /// <param name="character">Character as string.</param>
        private void KeyboardListener_KeyboardCallbackAsync(KeyEvent keyEvent, int vkCode, string character)
        {
            switch (keyEvent)
            {
                case KeyEvent.WM_KEYDOWN:
                    if (KeyDown != null)
                    {
                        _dispatcher.BeginInvoke(new EventHandler<KeyboardHookEventArgs>(KeyDown), this, new KeyboardHookEventArgs(vkCode, false, character, 0));
                    }
                    break;
                case KeyEvent.WM_SYSKEYDOWN:
                    if (KeyDown != null)
                    {
                        _dispatcher.BeginInvoke(new EventHandler<KeyboardHookEventArgs>(KeyDown), this, new KeyboardHookEventArgs(vkCode, true, character, 0));
                    }
                    break;
                case KeyEvent.WM_KEYUP:
                    if (KeyUp != null)
                    {
                        _dispatcher.BeginInvoke(new EventHandler<KeyboardHookEventArgs>(KeyUp), this, new KeyboardHookEventArgs(vkCode, false, character, 1));
                    }
                    break;
                case KeyEvent.WM_SYSKEYUP:
                    if (KeyUp != null)
                    {
                        _dispatcher.BeginInvoke(new EventHandler<KeyboardHookEventArgs>(KeyUp), this, new KeyboardHookEventArgs(vkCode, true, character, 1));
                    }
                    break;
            }
        }

        internal void Start()
        {
            // Dispatcher thread handling the KeyDown/KeyUp events.
            _dispatcher = Dispatcher.CurrentDispatcher;

            // We have to store the HookCallback, so that it is not garbage collected runtime
            _hookProcDelegateToAvoidGC = HookCallback;

            // Set the hook
            _hookId = SetHook(_hookProcDelegateToAvoidGC);

            // Assign the asynchronous callback event
            _hookedKeyboardCallbackAsync = KeyboardListener_KeyboardCallbackAsync;
        }

        internal void Stop()
        {
            InteropMethods.UnhookWindowsHookEx(_hookId);
            _hookId = IntPtr.Zero;
        }
    }

    public class KeyboardHookEventArgs : EventArgs
    {
        /// <summary>
        /// Unicode character of key pressed.
        /// </summary>
        public string Character;

        /// <summary>
        /// Down(0)、Up(1)
        /// </summary>
        public int EventType;

        /// <summary>
        /// Is the hitted key system key.
        /// </summary>
        public bool IsSysKey;

        /// <summary>
        /// WPF Key of the key.
        /// </summary>
        public Key Key;

        /// <summary>
        /// VKCode of the key.
        /// </summary>
        public int VkCode;

        public KeyboardHookEventArgs(int vkCode, bool isSysKey, string character, int type)
        {
            VkCode = vkCode;
            IsSysKey = isSysKey;
            Character = character;
            Key = KeyInterop.KeyFromVirtualKey(vkCode);
            EventType = type;
        }

        /// <summary>
        /// Convert to string.
        /// </summary>
        /// <returns>
        /// Returns string representation of this key, if not possible empty string is returned.
        /// </returns>
        public override string ToString()
        {
            return Character;
        }
    }

    public class KeyboardWatcher
    {
        private readonly object accesslock = new object();

        private readonly SyncFactory factory;
        private AsyncConcurrentQueue<object> keyQueue;
        private CancellationTokenSource taskCancellationTokenSource;
        private KeyboardHook keyboardHook;
        private bool isRunning;

        public event EventHandler<KeyboardHookEventArgs> OnKeyInput;

        internal KeyboardWatcher(SyncFactory factory)
        {
            this.factory = factory;
        }

        public void Start()
        {
            lock (accesslock)
            {
                if (!isRunning)
                {
                    taskCancellationTokenSource = new CancellationTokenSource();
                    keyQueue = new AsyncConcurrentQueue<object>(taskCancellationTokenSource.Token);

                    //This needs to run on UI thread context
                    //So use task factory with the shared UI message pump thread
                    Task.Factory.StartNew(() =>
                    {
                        keyboardHook = new KeyboardHook();
                        keyboardHook.KeyDown += KeyboardListener;
                        keyboardHook.KeyUp += KeyboardListener;
                        keyboardHook.Start();
                    }, CancellationToken.None, TaskCreationOptions.None, factory.GetTaskScheduler()).Wait();

                    Task.Factory.StartNew(() => ConsumeKeyAsync());

                    isRunning = true;
                }
            }
        }

        public void Stop()
        {
            lock (accesslock)
            {
                if (isRunning)
                {
                    if (keyboardHook != null)
                    {
                        //This needs to run on UI thread context
                        //So use task factory with the shared UI message pump thread
                        Task.Factory.StartNew(() =>
                        {
                            keyboardHook.KeyDown -= KeyboardListener;
                            keyboardHook.KeyUp -= KeyboardListener;
                            keyboardHook.Stop();
                            keyboardHook = null;
                        }, CancellationToken.None, TaskCreationOptions.None, factory.GetTaskScheduler());
                    }

                    keyQueue.Enqueue(false);
                    isRunning = false;
                    taskCancellationTokenSource.Cancel();
                }
            }
        }

        /// <summary>
        /// Add key event to the producer queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyboardListener(object sender, KeyboardHookEventArgs e)
        {
            keyQueue.Enqueue(e);
        }

        /// <summary>
        /// Consume events from the producer queue asynchronously
        /// </summary>
        /// <returns></returns>
        private async Task ConsumeKeyAsync()
        {
            while (isRunning)
            {
                //blocking here until a key is added to the queue
                var item = await keyQueue.DequeueAsync();

                if (item is null)
                {
                    continue;
                }

                if (item is bool)
                {
                    break;
                }

                OnKeyInput?.Invoke(null, item as KeyboardHookEventArgs);
            }
        }
    }
}
