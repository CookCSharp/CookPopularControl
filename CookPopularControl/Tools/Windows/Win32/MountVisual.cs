//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Interop;
//using System.Windows.Media;



///*
// * Copyright (c) 2021 All Rights Reserved.
// * Description：Class1
// * Author： Chance_写代码的厨子
// * Create Time：2021-03-23 16:55:18
// */
//namespace CookPopularControl.Tools.Windows.Win32
//{
//    /// <summary>
//    /// 在 Win32 应用程序中承载视觉对象
//    /// </summary>
//    public class MountVisual
//    {
//        private static HwndSource myHwndSource;

//        // Constant values from the "winuser.h" header file.
//        public const int WS_CHILD = 0x40000000;
//        public const int WS_VISIBLE = 0x10000000;
//        public const int WM_LBUTTONUP = 0x0202;
//        public const int WM_RBUTTONUP = 0x0205;

//        /// <summary>
//        /// 创建宿主Win32窗口
//        /// </summary>
//        /// <param name="parentHwnd"></param>
//        public static void CreateHostHwnd(IntPtr parentHwnd)
//        {
//            // Set up the parameters for the host hwnd.
//            HwndSourceParameters parameters = new HwndSourceParameters("Visual Hit Test", width, height);
//            parameters.WindowStyle = WS_CHILD | WS_VISIBLE;
//            parameters.SetPosition(0, 24);
//            parameters.ParentWindow = parentHwnd;
//            parameters.HwndSourceHook = new HwndSourceHook(ApplicationMessageFilter);

//            // Create the host hwnd for the visuals.
//            myHwndSource = new HwndSource(parameters);
//            myHwndSource.CompositionTarget.BackgroundColor = System.Windows.Media.Brushes.OldLace.Color;
//        }

//        /// <summary>
//        /// 将视觉对象添加到宿主Win32窗口
//        /// </summary>
//        /// <param name="parentHwnd"></param>
//        public static void CreateShape(IntPtr parentHwnd)
//        {
//            // Create an instance of the shape.
//            MyShape myShape = new MyShape();

//            // Determine whether the host container window has been created.
//            if (myHwndSource == null)
//            {
//                // Create the host container window for the visual objects.
//                CreateHostHwnd(parentHwnd);

//                // Associate the shape with the host container window.
//                myHwndSource.RootVisual = myShape;
//            }
//            else
//            {
//                // Assign the shape as a child of the root visual.
//                ((ContainerVisual)myHwndSource.RootVisual).Children.Add(myShape);
//            }
//        }

//        /// <summary>
//        /// 实现 Win32 消息筛选器
//        /// </summary>
//        /// <param name="hwnd"></param>
//        /// <param name="message"></param>
//        /// <param name="wParam"></param>
//        /// <param name="lParam"></param>
//        /// <param name="handled"></param>
//        /// <returns></returns>
//        internal static IntPtr ApplicationMessageFilter(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
//        {
//            // Handle messages passed to the visual.
//            switch (message)
//            {
//                // Handle the left and right mouse button up messages.
//                case WM_LBUTTONUP:
//                case WM_RBUTTONUP:
//                    System.Windows.Point pt = new System.Windows.Point();
//                    pt.X = (uint)lParam & (uint)0x0000ffff;  // LOWORD = x
//                    pt.Y = (uint)lParam >> 16;               // HIWORD = y
//                    MyShape.OnHitTest(pt, message);
//                    break;
//            }

//            return IntPtr.Zero;
//        }

//        /// <summary>
//        /// 对主机 Win32 窗口中包含的视觉对象的层次结构执行命中测试
//        /// </summary>
//        /// <param name="pt"></param>
//        /// <param name="msg"></param>
//        /// <remarks>Respond to WM_LBUTTONUP or WM_RBUTTONUP messages by determining which visual object was clicked</remarks>
//        public static void OnHitTest(System.Windows.Point pt, int msg)
//        {
//            // Clear the contents of the list used for hit test results.
//            hitResultsList.Clear();

//            // Determine whether to change the color of the circle or to delete the shape.
//            if (msg == WM_LBUTTONUP)
//            {
//                MyWindow.changeColor = true;
//            }
//            if (msg == WM_RBUTTONUP)
//            {
//                MyWindow.changeColor = false;
//            }

//            // Set up a callback to receive the hit test results enumeration.
//            VisualTreeHelper.HitTest(MyWindow.myHwndSource.RootVisual,
//                                     null,
//                                     new HitTestResultCallback(CircleHitTestResult),
//                                     new PointHitTestParameters(pt));

//            // Perform actions on the hit test results list.
//            if (hitResultsList.Count > 0)
//            {
//                ProcessHitTestResultsList();
//            }
//        }
//    }
//}
