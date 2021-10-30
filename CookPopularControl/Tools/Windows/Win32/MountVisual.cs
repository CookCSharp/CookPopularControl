using CookPopularControl.Tools.Extensions;
using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Class1
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-23 16:55:18
 */
namespace CookPopularControl.Tools.Windows.Win32
{
    /// <summary>
    /// 在 Win32 应用程序中承载视觉对象
    /// </summary>
    public class MountVisual
    {
        private static HwndSource myHwndSource;

        // Constant values from the "winuser.h" header file.
        public const int WS_CHILD = 0x40000000;
        public const int WS_VISIBLE = 0x10000000;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_RBUTTONUP = 0x0205;

        public static int Width = (int)WindowExtension.GetActiveWindow().Width;
        public static int Height = (int)WindowExtension.GetActiveWindow().Height;
        public static HwndSource MyHwndSource;
        public static bool TopmostLayer = true;
        public static bool ChangeColor;

        public static void FillWithCircles(IntPtr parentHwnd)
        {
            // Fill the client area of the form with randomly placed circles.
            for (var i = 0; i < 200; i++)
            {
                CreateShape(parentHwnd);
            }
        }

        /// <summary>
        /// 将视觉对象添加到宿主Win32窗口
        /// </summary>
        /// <param name="parentHwnd"></param>
        public static void CreateShape(IntPtr parentHwnd)
        {
            // Create an instance of the shape.
            MyShape myShape = new MyShape();

            // Determine whether the host container window has been created.
            if (myHwndSource == null)
            {
                // Create the host container window for the visual objects.
                CreateHostHwnd(parentHwnd);

                // Associate the shape with the host container window.
                myHwndSource.RootVisual = myShape;
            }
            else
            {
                // Assign the shape as a child of the root visual.
                ((ContainerVisual)myHwndSource.RootVisual).Children.Add(myShape);
            }
        }

        /// <summary>
        /// 创建宿主Win32窗口
        /// </summary>
        /// <param name="parentHwnd"></param>
        internal static void CreateHostHwnd(IntPtr parentHwnd)
        {
            // Set up the parameters for the host hwnd.
            HwndSourceParameters parameters = new HwndSourceParameters("Visual Hit Test", Width, Height);
            parameters.WindowStyle = WS_CHILD | WS_VISIBLE;
            parameters.SetPosition(0, 24);
            parameters.ParentWindow = parentHwnd;
            parameters.HwndSourceHook = new HwndSourceHook(ApplicationMessageFilter);

            // Create the host hwnd for the visuals.
            myHwndSource = new HwndSource(parameters);
            myHwndSource.CompositionTarget.BackgroundColor = System.Windows.Media.Brushes.OldLace.Color;
        }


        /// <summary>
        /// 实现 Win32 消息筛选器
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="message"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        internal static IntPtr ApplicationMessageFilter(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Handle messages passed to the visual.
            switch (message)
            {
                // Handle the left and right mouse button up messages.
                case WM_LBUTTONUP:
                case WM_RBUTTONUP:
                    var pt = new Point
                    {
                        X = (uint)lParam & 0x0000ffff,
                        Y = (uint)lParam >> 16
                    };
                    // LOWORD = x
                    // HIWORD = y
                    MyShape.OnHitTest(pt, message);
                    break;
            }

            return IntPtr.Zero;
        }
    }
}
