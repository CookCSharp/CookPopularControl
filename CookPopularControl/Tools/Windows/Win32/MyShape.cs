using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：MyShape
 * Author： Chance_写代码的厨子
 * Create Time：2021-10-15 14:15:35
 */
namespace CookPopularControl.Tools.Windows.Win32
{
    public class MyShape : ContainerVisual
    {
        // Constant values from the "winuser.h" header file.
        public const int WmLbuttonup = 0x0202,
            WmRbuttonup = 0x0205;

        public static int NumberCircles = 5;
        public static double Radius = 50.0d;
        public static Random MyRandom = new Random();
        public static ArrayList HitResultsList = new ArrayList();

        internal MyShape()
        {
            // Create a random x:y coordinate for the shape.
            var left = MyRandom.Next(0, MountVisual.Width);
            var top = MyRandom.Next(0, MountVisual.Height);

            var currRadius = Radius;
            if (Radius == 0.0d)
            {
                currRadius = MyRandom.Next(30, 100);
            }

            // Draw five concentric circles for the shape.
            var r = currRadius;
            for (var i = 0; i < NumberCircles; i++)
            {
                new MyCircle(this, new Point(left, top), r);
                r = currRadius * (1.0d - ((i + 1.0d) / NumberCircles));
            }
        }

        /// <summary>
        /// 对主机 Win32 窗口中包含的视觉对象的层次结构执行命中测试
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="msg"></param>
        /// <remarks>Respond to WM_LBUTTONUP or WM_RBUTTONUP messages by determining which visual object was clicked</remarks>
        public static void OnHitTest(Point pt, int msg)
        {
            // Clear the contents of the list used for hit test results.
            HitResultsList.Clear();

            // Determine whether to change the color of the circle or to delete the shape.
            if (msg == WmLbuttonup)
            {
                MountVisual.ChangeColor = true;
            }
            if (msg == WmRbuttonup)
            {
                MountVisual.ChangeColor = false;
            }

            // Set up a callback to receive the hit test results enumeration.
            VisualTreeHelper.HitTest(MountVisual.MyHwndSource.RootVisual,
                null,
                CircleHitTestResult,
                new PointHitTestParameters(pt));

            // Perform actions on the hit test results list.
            if (HitResultsList.Count > 0)
            {
                ProcessHitTestResultsList();
            }
        }

        // Handle the hit test results enumeration in the callback.
        internal static HitTestResultBehavior CircleHitTestResult(HitTestResult result)
        {
            // Add the hit test result to the list that will be processed after the enumeration.
            HitResultsList.Add(result.VisualHit);

            // Determine whether hit test should return only the top-most layer visual.
            if (MountVisual.TopmostLayer)
            {
                // Set the behavior to stop the enumeration of visuals.
                return HitTestResultBehavior.Stop;
            }
            // Set the behavior to continue the enumeration of visuals.
            // All visuals that intersect at the hit test coordinates are returned,
            // whether visible or not.
            return HitTestResultBehavior.Continue;
        }

        // Process the results of the hit test after the enumeration in the callback.
        // Do not add or remove objects from the visual tree until after the enumeration.
        internal static void ProcessHitTestResultsList()
        {
            foreach (MyCircle circle in HitResultsList)
            {
                // Determine whether to change the color of the ring or to delete the circle.
                if (MountVisual.ChangeColor)
                {
                    // Draw a different color ring for the circle.
                    circle.Render();
                }
                else
                {
                    if (circle.Parent == MountVisual.MyHwndSource.RootVisual)
                    {
                        // Remove the root visual by disposing of the host hwnd.
                        MountVisual.MyHwndSource.Dispose();
                        MountVisual.MyHwndSource = null;
                        return;
                    }
                    // Remove the shape that is the parent of the child circle.
                    ((ContainerVisual)MountVisual.MyHwndSource.RootVisual).Children.Remove((Visual)circle.Parent);
                }
            }
        }

        internal class MyCircle : DrawingVisual
        {
            public System.Windows.Point Pt;
            private readonly double _radius;

            internal MyCircle(MyShape parent, Point pt, double radius)
            {
                Pt = pt;
                _radius = radius;
                Render();

                // Add the circle as a child to the shape parent.
                parent.Children.Add(this);
            }

            internal void Render()
            {
                // Draw a circle.
                var dc = RenderOpen();
                dc.DrawEllipse(new SolidColorBrush(MyColor.GenRandomColor()), null, Pt, _radius, _radius);
                dc.Close();
            }
        }

        private class MyColor
        {
            private static readonly Random myRandom = new Random();

            public static Color GenRandomColor() => Color.FromRgb((byte)myRandom.Next(0, 255), (byte)myRandom.Next(0, 255),
                    (byte)myRandom.Next(0, 255));
        }
    }
}
