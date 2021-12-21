using CookPopularCSharpToolkit.Windows.Interop;
using System;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DpiHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-24 17:41:50
 */
namespace CookPopularCSharpToolkit.Communal
{
    public static class DpiHelper
    {
        private const double LogicalDpi = 96.0;
        private const int LOGPIXELSX = 0x58;
        private const int LOGPIXELSY = 0x5A;

        [ThreadStatic]
        private static Matrix _transformToDevice;
        [ThreadStatic]
        private static Matrix _transformToDip;

        static DpiHelper()
        {
            var dC = InteropMethods.GetDC(IntPtr.Zero);
            if (dC != IntPtr.Zero)
            {
                // 沿着屏幕宽度每逻辑英寸的像素数。在具有多个显示器的系统中，这个值对所有显示器都是相同的
                const int logicPixelsX = 88;
                // 沿着屏幕高度每逻辑英寸的像素数
                const int logicPixelsY = 90;
                DeviceDpiX = InteropMethods.GetDeviceCaps(dC, logicPixelsX);
                DeviceDpiY = InteropMethods.GetDeviceCaps(dC, logicPixelsY);
                InteropMethods.ReleaseDC(IntPtr.Zero, dC);
            }
            else
            {
                DeviceDpiX = LogicalDpi;
                DeviceDpiY = LogicalDpi;
            }

            _ = InteropMethods.ReleaseDC(IntPtr.Zero, dC);

            var identity = Matrix.Identity;
            var identity2 = Matrix.Identity;
            identity.Scale(DeviceDpiX / LogicalDpi, DeviceDpiY / LogicalDpi);
            identity2.Scale(LogicalDpi / DeviceDpiX, LogicalDpi / DeviceDpiY);
            TransformFromDevice = new MatrixTransform(identity2);
            TransformFromDevice.Freeze();
            TransformToDevice = new MatrixTransform(identity);
            TransformToDevice.Freeze();
        }

        public static MatrixTransform TransformFromDevice { get; }

        public static MatrixTransform TransformToDevice { get; }

        public static double DeviceDpiX { get; }

        public static double DeviceDpiY { get; }

        public static double LogicalToDeviceUnitsScalingFactorX => TransformToDevice.Matrix.M11;

        public static double LogicalToDeviceUnitsScalingFactorY => TransformToDevice.Matrix.M22;

        /// <summary>
        /// 获取屏幕水平Dpi缩放系数
        /// </summary>
        /// <returns></returns>
        public static double GetScaleX() => GetDeviceDpiX() / LogicalDpi;
        
        /// <summary>
        /// 获取屏幕垂直Dpi缩放系数
        /// </summary>
        /// <returns></returns>
        public static double GetScaleY() => GetDeviceDpiY() / LogicalDpi;

        /// <summary>
        /// 获取屏幕的每英寸水平逻辑像素数（DPI）。
        /// </summary>
        /// <returns>返回屏幕的每英寸水平逻辑像素数（DPI）。</returns>
        public static int GetDeviceDpiX()
        {
            var screenDC = InteropMethods.GetDC(IntPtr.Zero);
            var dpiX = InteropMethods.GetDeviceCaps(screenDC, LOGPIXELSX);

            _ = InteropMethods.ReleaseDC(IntPtr.Zero, screenDC);

            return dpiX;
        }

        /// <summary>
        /// 获取屏幕的每英寸垂直逻辑像素数（DPI）。
        /// </summary>
        /// <returns>返回屏幕的每英寸垂直逻辑像素数（DPI）。</returns>
        public static int GetDeviceDpiY()
        {
            var screenDC = InteropMethods.GetDC(IntPtr.Zero);
            var dpiY = InteropMethods.GetDeviceCaps(screenDC, LOGPIXELSY);

            _ = InteropMethods.ReleaseDC(IntPtr.Zero, screenDC);

            return dpiY;
        }

        public static Point LogicalPixelsToDevice(Point logicalPoint, double dpiScaleX, double dpiScaleY)
        {
            _transformToDevice = Matrix.Identity;
            _transformToDevice.Scale(dpiScaleX, dpiScaleY);
            return _transformToDevice.Transform(logicalPoint);
        }

        public static Point DevicePixelsToLogical(Point devicePoint, double dpiScaleX, double dpiScaleY)
        {
            _transformToDip = Matrix.Identity;
            _transformToDip.Scale(1d / dpiScaleX, 1d / dpiScaleY);
            return _transformToDip.Transform(devicePoint);
        }

        public static Rect LogicalRectToDevice(Rect logicalRectangle, double dpiScaleX, double dpiScaleY)
        {
            Point topLeft = LogicalPixelsToDevice(new Point(logicalRectangle.Left, logicalRectangle.Top), dpiScaleX, dpiScaleY);
            Point bottomRight = LogicalPixelsToDevice(new Point(logicalRectangle.Right, logicalRectangle.Bottom), dpiScaleX, dpiScaleY);

            return new Rect(topLeft, bottomRight);
        }

        public static Rect DeviceRectToLogical(Rect deviceRectangle, double dpiScaleX, double dpiScaleY)
        {
            Point topLeft = DevicePixelsToLogical(new Point(deviceRectangle.Left, deviceRectangle.Top), dpiScaleX, dpiScaleY);
            Point bottomRight = DevicePixelsToLogical(new Point(deviceRectangle.Right, deviceRectangle.Bottom), dpiScaleX, dpiScaleY);

            return new Rect(topLeft, bottomRight);
        }

        public static Size LogicalSizeToDevice(Size logicalSize, double dpiScaleX, double dpiScaleY)
        {
            Point pt = LogicalPixelsToDevice(new Point(logicalSize.Width, logicalSize.Height), dpiScaleX, dpiScaleY);

            return new Size { Width = pt.X, Height = pt.Y };
        }

        public static Size DeviceSizeToLogical(Size deviceSize, double dpiScaleX, double dpiScaleY)
        {
            var pt = DevicePixelsToLogical(new Point(deviceSize.Width, deviceSize.Height), dpiScaleX, dpiScaleY);
            return new Size(pt.X, pt.Y);
        }

        public static Rect LogicalToDeviceUnits(this Rect logicalRect)
        {
            var result = logicalRect;
            result.Transform(TransformToDevice.Matrix);
            return result;
        }

        public static Rect DeviceToLogicalUnits(this Rect deviceRect)
        {
            var result = deviceRect;
            result.Transform(TransformFromDevice.Matrix);
            return result;
        }

        public static Thickness LogicalThicknessToDevice(Thickness logicalThickness, double dpiScaleX, double dpiScaleY)
        {
            Point topLeft = LogicalPixelsToDevice(new Point(logicalThickness.Left, logicalThickness.Top), dpiScaleX, dpiScaleY);
            Point bottomRight = LogicalPixelsToDevice(new Point(logicalThickness.Right, logicalThickness.Bottom), dpiScaleX, dpiScaleY);

            return new Thickness(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
        }

        public static double RoundLayoutValue(double value, double dpiScale)
        {
            double newValue;

            if (!MathHelper.AreClose(dpiScale, 1.0))
            {
                newValue = Math.Round(value * dpiScale) / dpiScale;
                if (double.IsNaN(newValue) || double.IsInfinity(newValue) || MathHelper.AreClose(newValue, double.MaxValue))
                {
                    newValue = value;
                }
            }
            else
            {
                newValue = Math.Round(value);
            }

            return newValue;
        }
    }
}
