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
        internal static class NativeMethods
        {
            [Flags]
            [SuppressMessage("Design", "CA1069:不应复制枚举值", Justification = "<挂起>")]
            internal enum SWP : uint
            {
                NOSIZE = 0x0001,
                NOMOVE = 0x0002,
                NOZORDER = 0x0004,
                NOREDRAW = 0x0008,
                NOACTIVATE = 0x0010,
                DRAWFRAME = 0x0020,
                FRAMECHANGED = 0x0020,
                SHOWWINDOW = 0x0040,
                HIDEWINDOW = 0x0080,
                NOCOPYBITS = 0x0100,
                NOOWNERZORDER = 0x0200,
                NOREPOSITION = 0x0200,
                NOSENDCHANGING = 0x0400,
                DEFERERASE = 0x2000,
                ASYNCWINDOWPOS = 0x4000,
            }

            internal enum FlashType : uint
            {
                /// <summary>
                /// 停止闪烁。
                /// </summary>
                FLASHW_STOP = 0,

                /// <summary>
                /// 只闪烁标题。
                /// </summary>
                FALSHW_CAPTION = 1,

                /// <summary>
                /// 只闪烁任务栏。
                /// </summary>
                FLASHW_TRAY = 2,

                /// <summary>
                /// 标题和任务栏同时闪烁。
                /// </summary>
                FLASHW_ALL = 3,

                /// <summary>
                /// 
                /// </summary>
                FLASHW_PARAM1 = 4,

                /// <summary>
                /// 
                /// </summary>
                FLASHW_PARAM2 = 12,

                /// <summary>
                /// 无条件闪烁任务栏直到发送停止标志或者窗口被激活，如果未激活，停止时高亮。
                /// </summary>
                FLASHW_TIMER = FLASHW_TRAY | FLASHW_PARAM1,

                /// <summary>
                /// 未激活时闪烁任务栏直到发送停止标志或者窗体被激活，停止后高亮。
                /// </summary>
                FLASHW_TIMERNOFG = FLASHW_TRAY | FLASHW_PARAM2,
            }

            /// <summary>
            /// 包含系统应在指定时间内闪烁窗口次数和闪烁状态的信息
            /// </summary>
            internal struct FLASHWINFO
            {
                /// <summary>
                /// 结构大小
                /// </summary>
                public uint cbSize;

                /// <summary>
                /// 要闪烁或停止的窗口句柄
                /// </summary>
                public IntPtr hwnd;

                /// <summary>
                /// 闪烁的类型
                /// </summary>
                public uint dwFlags;

                /// <summary>
                /// 闪烁窗口的次数
                /// </summary>
                public uint uCount;

                /// <summary>
                /// 窗口闪烁的频度，毫秒为单位；若该值为0，则为默认图标的闪烁频度
                /// </summary>
                public uint dwTimeout;
            }

            internal const int HWND_NOTOPMOST = -2;
            internal const int HWND_TOPMOST = -1;
            internal const int HWND_TOP = 0;
            internal const int HWND_BOTTOM = 1;

            internal const int GWL_EXSTYLE = -20;
            internal const int WS_EX_DLGMODALFRAME = 0x0001;
            internal const uint WS_EX_TOPMOST = 0x0008;

            [DllImport("user32.dll")]
            internal static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, SWP uFlags);

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            //[DllImport("user32.dll")]
            //internal static extern bool SetForegroundWindow(IntPtr hWnd);

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

            [DllImport("user32.dll")]
            internal static extern bool FlashWindowEx(ref FLASHWINFO pwfi);
        }

        private static readonly Func<Window, bool> getDisposedValue = CreateGetFieldValueDelegate<Window, bool>("_disposed");
        private static readonly Func<Window, bool> getIsClosingValue = CreateGetFieldValueDelegate<Window, bool>("_isClosing");
        private static readonly Func<Window, bool> getShowingAsDialogValue = CreateGetFieldValueDelegate<Window, bool>("_showingAsDialog");
        private static readonly Func<Window, bool> getHwndCreatedButNotShownValue = CreateGetFieldValueDelegate<Window, bool>("_hwndCreatedButNotShown");

        public static IntPtr EnsureHandle(this Window window) => new WindowInteropHelper(window).EnsureHandle();

        public static HwndSource GetHwndSource(this Window window) => HwndSource.FromHwnd(window.EnsureHandle());

        public static void SwitchToThisWindow(this Window window) => NativeMethods.SwitchToThisWindow(window.EnsureHandle(), true);

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
            int extendedStyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE);
            CookPopularCSharpToolkit.Windows.Interop.NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_EXSTYLE, extendedStyle | NativeMethods.WS_EX_DLGMODALFRAME);

            //更新窗口的非客户区，以反映变化
            CookPopularCSharpToolkit.Windows.Interop.NativeMethods.SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, CookPopularCSharpToolkit.Windows.Interop.NativeMethods.SWP.NOMOVE |
                  CookPopularCSharpToolkit.Windows.Interop.NativeMethods.SWP.NOSIZE | CookPopularCSharpToolkit.Windows.Interop.NativeMethods.SWP.NOZORDER | CookPopularCSharpToolkit.Windows.Interop.NativeMethods.SWP.FRAMECHANGED);
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
                var fInfo = new NativeMethods.FLASHWINFO();

                fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
                fInfo.hwnd = new WindowInteropHelper(window).EnsureHandle();
                if (!IsStopFlash)
                    fInfo.dwFlags = (uint)(NativeMethods.FlashType.FLASHW_ALL | NativeMethods.FlashType.FLASHW_TIMER);
                else
                    fInfo.dwFlags = (uint)NativeMethods.FlashType.FLASHW_STOP;
                fInfo.uCount = 3;
                fInfo.dwTimeout = 0;

                return NativeMethods.FlashWindowEx(ref fInfo);
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
                const NativeMethods.SWP uFlags = NativeMethods.SWP.ASYNCWINDOWPOS | NativeMethods.SWP.NOSIZE | NativeMethods.SWP.NOMOVE | NativeMethods.SWP.NOREDRAW | NativeMethods.SWP.NOACTIVATE;

                if (NativeMethods.SetWindowPos(hWnd, topmost ? NativeMethods.HWND_TOPMOST : NativeMethods.HWND_NOTOPMOST, 0, 0, 0, 0, uFlags))
                {
                    var exStyleWithTopmost = NativeMethods.GetWindowLong(hWnd, NativeMethods.GWL_EXSTYLE) & NativeMethods.WS_EX_TOPMOST;

                    return topmost ? (exStyleWithTopmost == NativeMethods.WS_EX_TOPMOST) : (exStyleWithTopmost != NativeMethods.WS_EX_TOPMOST);
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
