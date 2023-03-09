using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows.Interop;
using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Windows;
using System.Windows.Interop;

namespace CookPopularCSharpToolkit.Windows
{
    /// <summary>
    /// 为提 <see cref="Window" /> 供静态扩展方法。
    /// </summary>
    [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
    [SuppressMessage("Security", "CA2153:不要捕获损坏状态异常", Justification = "<挂起>")]
    public static class WindowExtension
    {
        internal const int HWND_NOTOPMOST = -2;
        internal const int HWND_TOPMOST = -1;
        internal const int HWND_TOP = 0;
        internal const int HWND_BOTTOM = 1;
        internal const int WS_EX_DLGMODALFRAME = 0x0001;
        internal const uint WS_EX_TOPMOST = 0x0008;

        private static readonly Func<Window, bool> getDisposedValue = CreateGetFieldValueDelegate<Window, bool>("_disposed");
        private static readonly Func<Window, bool> getIsClosingValue = CreateGetFieldValueDelegate<Window, bool>("_isClosing");
        private static readonly Func<Window, bool> getShowingAsDialogValue = CreateGetFieldValueDelegate<Window, bool>("_showingAsDialog");
        private static readonly Func<Window, bool> getHwndCreatedButNotShownValue = CreateGetFieldValueDelegate<Window, bool>("_hwndCreatedButNotShown");

        public static IntPtr EnsureHandle(this Window window) => new WindowInteropHelper(window).EnsureHandle();

        public static HwndSource GetHwndSource(this Window window) => HwndSource.FromHwnd(window.EnsureHandle());

        public static void SwitchToThisWindow(this Window window) => InteropMethods.SwitchToThisWindow(window.EnsureHandle(), true);

        public static Window SetOwner(this Window window, IntPtr hWnd)
        {
            new WindowInteropHelper(window).Owner = hWnd;

            return window;
        }

        public static Window GetActiveWindow() => Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);

        public static void RemoveIcon(this Window window)
        {
            //获取窗体的句柄
            IntPtr hwnd = new WindowInteropHelper(window).Handle;

            //改变窗体的样式
            int extendedStyle = InteropMethods.GetWindowLong(hwnd, (int)InteropValues.GetWindowLongFields.GWL_EXSTYLE);
            InteropMethods.SetWindowLong(hwnd, (int)InteropValues.GetWindowLongFields.GWL_EXSTYLE, new IntPtr(extendedStyle | WS_EX_DLGMODALFRAME));

            //更新窗口的非客户区，以反映变化
            InteropMethods.SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, InteropValues.WindowPositionFlags.SWP_NOMOVE | InteropValues.WindowPositionFlags.SWP_NOSIZE | InteropValues.WindowPositionFlags.SWP_NOZORDER | InteropValues.WindowPositionFlags.SWP_FRAMECHANGED);
        }

        /// <summary>
        /// Returns the actual Left of the Window independently from the WindowState
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static double GetActualLeft(this Window window)
        {
            if (window.WindowState == WindowState.Maximized)
            {
                var leftField = typeof(Window).GetField("_actualLeft", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return leftField?.GetValue(window) as double? ?? 0;
            }

            return window.Left;
        }

        /// <summary>
        /// Returns the actual Top of the Window independently from the WindowState
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static double GetActualTop(this Window window)
        {
            if (window.WindowState == WindowState.Maximized)
            {
                var topField = typeof(Window).GetField("_actualTop", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return topField?.GetValue(window) as double? ?? 0;
            }

            return window.Top;
        }

        /// <summary>
        /// 设置当前窗口的置顶状态。
        /// </summary>
        /// <param name="window">一个 <see cref="Window" /> 对象。</param>
        /// <param name="topmost">指定是否置顶。</param>
        /// <param name="millisecondsTimeout">指定操作失败时，应重试的毫秒数。</param>
        /// <returns>操作成功，返回<c>true</c>；否则，返回<c>false</c>。</returns>
        [HandleProcessCorruptedStateExceptions]
        public static bool SetTopmost(this Window window, bool topmost, int millisecondsTimeout = 0)
        {
            var hWnd = window.EnsureHandle();

            return topmost
                ? SpinWait.SpinUntil(new Func<bool>(() => SetTopmostCore(hWnd, false)), millisecondsTimeout) && SpinWait.SpinUntil(new Func<bool>(() => SetTopmostCore(hWnd, true)), millisecondsTimeout)
                : SpinWait.SpinUntil(new Func<bool>(() => SetTopmostCore(hWnd, topmost)), millisecondsTimeout);
        }

        /// <summary>
        /// 安全的激活窗口（如果尚未创建窗口的 HWND，则创建 HWND）。
        /// </summary>
        /// <param name="window">一个 <see cref="Window" /> 对象。</param>
        /// <returns>操作成功，返回<c>true</c>；否则，返回<c>false</c>。</returns>
        [HandleProcessCorruptedStateExceptions]
        public static bool SafeActivate(this Window window)
        {
            try
            {
                return InteropMethods.SetForegroundWindow(window.EnsureHandle());
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 让窗口激活作为前台最上层窗口
        /// </summary>
        /// <param name="window"></param>
        public static void SetWindowToForeground(Window window)
        {
            // [WPF 让窗口激活作为前台最上层窗口的方法 - lindexi - 博客园](https://www.cnblogs.com/lindexi/p/12749671.html)
            var interopHelper = new WindowInteropHelper(window);
            var thisWindowThreadId = InteropMethods.GetWindowThreadProcessId(interopHelper.Handle, out _);
            var currentForegroundWindow = InteropMethods.GetForegroundWindow();
            var currentForegroundWindowThreadId = InteropMethods.GetWindowThreadProcessId(currentForegroundWindow, out _);

            // [c# - Bring a window to the front in WPF - Stack Overflow](https://stackoverflow.com/questions/257587/bring-a-window-to-the-front-in-wpf )
            // [SetForegroundWindow的正确用法 - 子坞 - 博客园](https://www.cnblogs.com/ziwuge/archive/2012/01/06/2315342.html )
            /*
                 1.得到窗口句柄FindWindow 
                2.切换键盘输入焦点AttachThreadInput 
                3.显示窗口ShowWindow(有些窗口被最小化/隐藏了) 
                4.更改窗口的Z Order，SetWindowPos使之最上，为了不影响后续窗口的Z Order,改完之后，再还原 
                5.最后SetForegroundWindow 
             */

            InteropMethods.AttachThreadInput(currentForegroundWindowThreadId, thisWindowThreadId, true);

            window.Show();
            window.Activate();
            // 去掉和其他线程的输入链接
            InteropMethods.AttachThreadInput(currentForegroundWindowThreadId, thisWindowThreadId, false);

            // 用于踢掉其他的在上层的窗口
            if (window.Topmost != true)
            {
                window.Topmost = true;
                window.Topmost = false;
            }
        }

        /// <summary>
        /// 闪烁窗口的任务栏图标。
        /// </summary>
        /// <param name="window">一个 <see cref="Window" /> 对象。</param>
        /// <param name="IsStopFlash">是否停止闪烁</param>
        /// <returns>操作成功，返回<c>true</c>；否则，返回<c>false</c>。</returns>
        [HandleProcessCorruptedStateExceptions]
        public static bool FlashWindow(this Window window, bool IsStopFlash = false)
        {
            try
            {
                var fInfo = new InteropValues.FLASHWINFO();

                fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
                fInfo.hwnd = new WindowInteropHelper(window).EnsureHandle();
                if (!IsStopFlash)
                    fInfo.dwFlags = (uint)(InteropValues.FlashType.FLASHW_ALL | InteropValues.FlashType.FLASHW_TIMER);
                else
                    fInfo.dwFlags = (uint)InteropValues.FlashType.FLASHW_STOP;
                fInfo.uCount = 3;
                fInfo.dwTimeout = 0;

                return InteropMethods.FlashWindowEx(ref fInfo);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取当前窗口是否已创建句柄但从未显示。
        /// </summary>
        /// <param name="window">一个 <see cref="Window" /> 对象。</param>
        /// <returns>若是，返回<c>true</c>；否则，返回<c>false</c>。</returns>
        public static bool HwndCreatedButNotShown(this Window window) => getHwndCreatedButNotShownValue(window);

        /// <summary>
        /// 获取当前窗口是否正在关闭。
        /// </summary>
        /// <param name="window">一个 <see cref="Window" /> 对象。</param>
        /// <returns>若是，返回<c>true</c>；否则，返回<c>false</c>。</returns>
        public static bool IsClosing(this Window window) => getIsClosingValue(window);

        /// <summary>
        /// 获取当前窗口是否已释放。
        /// </summary>
        /// <param name="window">一个 <see cref="Window" /> 对象。</param>
        /// <returns>若是，返回<c>true</c>；否则，返回<c>false</c>。</returns>
        public static bool IsDisposed(this Window window) => getDisposedValue(window);

        /// <summary>
        /// 获取当前窗口是否正在作为对话框显示。
        /// </summary>
        /// <param name="window">一个 <see cref="Window" /> 对象。</param>
        /// <returns>若是，返回<c>true</c>；否则，返回<c>false</c>。</returns>
        public static bool ShowingAsDialog(this Window window) => getShowingAsDialogValue(window);

        public static void ConstrainToBounds(this Window window)
        {
            var workArea = SystemParameters.WorkArea;

            if (window.Left < 0.0)
            {
                window.Left = 0.0;
            }
            else
            {
                var maximum = workArea.Width - window.ActualWidth;

                if (window.Left > maximum)
                {
                    window.Left = maximum;
                }
            }

            if (window.Top < 0.0)
            {
                window.Top = 0.0;
            }
            else
            {
                var maximum = workArea.Height - window.ActualHeight;

                if (window.Top > maximum)
                {
                    window.Top = maximum;
                }
            }
        }

        [HandleProcessCorruptedStateExceptions]
        private static bool SetTopmostCore(IntPtr hWnd, bool topmost)
        {
            try
            {
#if DEBUG
                if (Debugger.IsAttached)
                    return true;
#endif
                const InteropValues.WindowPositionFlags uFlags = InteropValues.WindowPositionFlags.SWP_ASYNCWINDOWPOS | InteropValues.WindowPositionFlags.SWP_NOSIZE | InteropValues.WindowPositionFlags.SWP_NOMOVE | InteropValues.WindowPositionFlags.SWP_NOREDRAW | InteropValues.WindowPositionFlags.SWP_NOACTIVATE;
                if (InteropMethods.SetWindowPos(hWnd, topmost ? new IntPtr(HWND_TOPMOST) : new IntPtr(HWND_NOTOPMOST), 0, 0, 0, 0, uFlags))
                {
                    var exStyleWithTopmost = InteropMethods.GetWindowLong(hWnd, (int)InteropValues.GetWindowLongFields.GWL_EXSTYLE) & WS_EX_TOPMOST;

                    return topmost ? (exStyleWithTopmost == WS_EX_TOPMOST) : (exStyleWithTopmost != WS_EX_TOPMOST);
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static Func<T, TResult> CreateGetFieldValueDelegate<T, TResult>(string fieldName)
        {
            var targetType = typeof(T);
            var targetField = targetType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            var parameter = System.Linq.Expressions.Expression.Parameter(targetType);
            var memberAccess = System.Linq.Expressions.Expression.MakeMemberAccess(parameter, targetField);

            return System.Linq.Expressions.Expression.Lambda<Func<T, TResult>>(memberAccess, parameter).Compile();
        }
    }
}
