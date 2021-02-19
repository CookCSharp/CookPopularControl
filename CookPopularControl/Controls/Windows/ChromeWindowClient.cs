using CookPopularControl.Tools;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ChromeWindowClient
 * Author： Chance_写代码的厨子
 * Create Time：2021-02-18 09:20:55
 */
namespace CookPopularControl.Controls.Windows
{
    /// <summary>
    /// 为 <see cref="ChromeWindow" /> 提供客户端区域。无法继承此类。
    /// </summary>
    public sealed class ChromeWindowClient : Decorator
    {
        private static class NativeMethods
        {
            internal const int SM_CXPADDEDBORDER = 0x5C;

            [SecurityCritical, DllImport("user32.dll")]
            internal static extern int GetSystemMetrics(int nIndex);
        }

        /// <summary>
        /// 初始化 <see cref="ChromeWindowClient" /> 类的新实例。
        /// </summary>
        public ChromeWindowClient()
        {
            SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
            SystemParameters.StaticPropertyChanged += SystemParameters_StaticPropertyChanged;
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e) => InvalidateVisual();

        private void SystemParameters_StaticPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SystemParameters.WindowResizeBorderThickness))
            {
                InvalidateVisual();
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (TryGetAvailableSize(constraint, out var availableSize, out var element))
            {
                element.Measure(availableSize);

                return availableSize;
            }

            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (TryGetAvailableSize(arrangeSize, out var availableSize, out var element))
            {
                element.Arrange(new Rect(availableSize));

                return availableSize;
            }

            return base.ArrangeOverride(arrangeSize);
        }

        private bool TryGetAvailableSize(Size inputSize, out Size availableSize, out UIElement element)
        {
            if (TemplatedParent is Window window)
            {
                if (window.WindowState == WindowState.Maximized)
                {
                    if ((element = Child) != default)
                    {
                        var windowResize = SystemParameters.WindowResizeBorderThickness;
                        var paddedBorder = GetPaddedBorderThickness();

                        availableSize = new Size(
                           inputSize.Width - windowResize.Left - windowResize.Right - paddedBorder.Left - paddedBorder.Right,
                           inputSize.Height - windowResize.Top - windowResize.Bottom - paddedBorder.Top - paddedBorder.Bottom);

                        return true;
                    }
                }
            }

            availableSize = default;
            element = default;

            return false;
        }

        private static Thickness GetPaddedBorderThickness()
        {
            var dpiX = Screenshot.GetDpiX();
            var paddedBorder = NativeMethods.GetSystemMetrics(NativeMethods.SM_CXPADDEDBORDER);
            var frameSizeInDpis = DeviceSizeToLogical(new Size(paddedBorder, paddedBorder), dpiX / 96.0, dpiX / 96.0);

            return new Thickness(frameSizeInDpis.X, frameSizeInDpis.Y, frameSizeInDpis.X, frameSizeInDpis.Y);
        }

        private static Point DeviceSizeToLogical(Size deviceSize, double dpiScaleX, double dpiScaleY)
        {
            var transformToDPI = Matrix.Identity;

            transformToDPI.Scale(1.0 / dpiScaleX, 1.0 / dpiScaleY);

            return transformToDPI.Transform(new Point(deviceSize.Width, deviceSize.Height));
        }
    }
}
