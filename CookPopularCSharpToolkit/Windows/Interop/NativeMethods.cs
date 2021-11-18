using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace CookPopularCSharpToolkit.Windows.Interop
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
            TOPMOST = SWP.NOACTIVATE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOMOVE | SWP.NOREDRAW | SWP.NOSENDCHANGING,
        }

        [DebuggerDisplay("X:{Left}, Y:{Top}, Width:{Width}, Height:{Height}")]
        internal struct RECT
        {
#pragma warning disable CS0649
            public readonly int Left;

            public readonly int Top;

            public readonly int Right;

            public readonly int Bottom;
#pragma warning restore CS0649
            public int Width => Right - Left;

            public int Height => Bottom - Top;
        }

        internal const int GWL_STYLE = -16;
        internal const int GWL_EXSTYLE = -20;

        internal const int HWND_TOP = 0;
        internal const int HWND_BOTTOM = 1;
        internal const int HWND_TOPMOST = -1;
        internal const int HWND_NOTOPMOST = -2;

        internal const int WS_EX_NOACTIVATE = 0x08000000;

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SWP uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetFocus();

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32")]
        internal static extern IntPtr GetTopWindow(IntPtr hWnd);
    }
}
