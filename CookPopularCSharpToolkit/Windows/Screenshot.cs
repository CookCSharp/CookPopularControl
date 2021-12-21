using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-02-18 09:23:28
 */
namespace CookPopularCSharpToolkit.Windows
{
    /// <summary>
    /// 提供屏幕截图相关静态方法。
    /// </summary>
    [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
    public static class Screenshot
    {
        private static class NativeMethods
        {
            //internal const int LOGPIXELSX = 0x58;
            //internal const int LOGPIXELSY = 0x5A;

            internal const int DESKTOPVERTRES = 0x75;
            internal const int DESKTOPHORZRES = 0x76;

            [DllImport("user32.dll")]
            internal static extern IntPtr GetDC(IntPtr hWnd);

            [DllImport("user32.dll")]
            internal static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

            [DllImport("gdi32.dll")]
            internal static extern bool DeleteObject(IntPtr hObject);

            [DllImport("gdi32.dll")]
            internal static extern int GetDeviceCaps(IntPtr hDC, int nIndex);

            [DllImport("gdi32.dll")]
            internal static extern int BitBlt(IntPtr destDC, int destX, int destY, int width, int height, IntPtr srcDC, int srcX, int srcY, CopyPixelOperation copyPixelOperation);
        }

        ///// <summary>
        ///// 获取屏幕 PDI 缩放系数。
        ///// </summary>
        ///// <returns></returns>
        //public static double GetScale() => GetDpiX() / 96.0;

        ///// <summary>
        ///// 获取屏幕的每英寸水平逻辑像素数（DPI）。
        ///// </summary>
        ///// <returns>返回屏幕的每英寸水平逻辑像素数（DPI）。</returns>
        //public static int GetDpiX()
        //{
        //    var screenDC = NativeMethods.GetDC(IntPtr.Zero);
        //    var dpiX = NativeMethods.GetDeviceCaps(screenDC, NativeMethods.LOGPIXELSX);

        //    _ = NativeMethods.ReleaseDC(IntPtr.Zero, screenDC);

        //    return dpiX;
        //}

        ///// <summary>
        ///// 获取屏幕的每英寸垂直逻辑像素数（DPI）。
        ///// </summary>
        ///// <returns>返回屏幕的每英寸垂直逻辑像素数（DPI）。</returns>
        //public static int GetDpiY()
        //{
        //    var screenDC = NativeMethods.GetDC(IntPtr.Zero);
        //    var dpiY = NativeMethods.GetDeviceCaps(screenDC, NativeMethods.LOGPIXELSY);

        //    _ = NativeMethods.ReleaseDC(IntPtr.Zero, screenDC);

        //    return dpiY;
        //}

        /// <summary>
        /// 捕获屏幕快照。
        /// </summary>
        /// <returns>返回一个包含屏幕快照的 <see cref="Bitmap" /> GDI+ 位图对象。</returns>
        public static Bitmap GetSnapshot()
        {
            var screenDC = NativeMethods.GetDC(IntPtr.Zero);
            var screenWidth = NativeMethods.GetDeviceCaps(screenDC, NativeMethods.DESKTOPHORZRES);
            var screenHeight = NativeMethods.GetDeviceCaps(screenDC, NativeMethods.DESKTOPVERTRES);

            var snapshot = new Bitmap(screenWidth, screenHeight);

            using (var g = Graphics.FromImage(snapshot))
            {
                _ = NativeMethods.BitBlt(g.GetHdc(), 0, 0, screenWidth, screenHeight, screenDC, 0, 0, CopyPixelOperation.SourceCopy);
                _ = NativeMethods.ReleaseDC(IntPtr.Zero, screenDC);
            }

            return snapshot;
        }

        /// <summary>
        /// 获取当前屏幕的原始分辨率。
        /// </summary>
        /// <returns>返回当前屏幕的原始分辨率。</returns>
        public static System.Windows.Size GetResolution()
        {
            var screenDC = NativeMethods.GetDC(IntPtr.Zero);
            var resolution = new System.Windows.Size(NativeMethods.GetDeviceCaps(screenDC, NativeMethods.DESKTOPHORZRES), NativeMethods.GetDeviceCaps(screenDC, NativeMethods.DESKTOPVERTRES));

            _ = NativeMethods.ReleaseDC(IntPtr.Zero, screenDC);

            return resolution;
        }

        /// <summary>
        /// 使用 JPEG 图像质量级别来压缩当前 GDI+ 位图。
        /// </summary>
        /// <param name="bitmap">当前 <see cref="Bitmap" /> GDI+ 位图对象。</param>
        /// <param name="level">指定 JPEG 图像质量级别（0 ~ 100）。</param>
        /// <param name="disposing">若操作成功，则立即释放当前 <see cref="Bitmap" /> GDI+ 位图对象（默认为 <c>false</c>）。</param>
        /// <returns>返回一个已压缩的 <see cref="Bitmap" /> GDI+ 位图对象。</returns>
        public static Bitmap Compression(this Bitmap bitmap, long level, bool disposing = false)
        {
            if ((level < 0) || (level > 100))
                throw new ArgumentOutOfRangeException(nameof(level), level, "JPEG 图像质量级别的取值范围为 0 ~ 100。");

            using var ms = new MemoryStream();
            var jpegEncoder = ImageFormat.Jpeg.GetCodec();
            using var jpegEncoderParameters = new EncoderParameters(1);

            jpegEncoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, level);

            bitmap.Save(ms, jpegEncoder, jpegEncoderParameters);

            if (disposing)
                bitmap.Dispose();

            return Image.FromStream(ms) as Bitmap;
        }

        /// <summary>
        /// 基于当前 GDI+ 位图创建一个托管且冻结的 <see cref="BitmapSource" />。
        /// </summary>
        /// <param name="bitmap">当前 <see cref="Bitmap" /> GDI+ 位图对象。</param>
        /// <param name="disposing">若操作成功，则立即释放当前 <see cref="Bitmap" /> GDI+ 位图对象（默认为 <c>false</c>）。</param>
        /// <returns>返回一个托管且冻结的 <see cref="BitmapSource" /> 对象。</returns>
        public static BitmapSource ToBitmapSource(this Bitmap bitmap, bool disposing = false)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapSource source;

            try
            {
                source = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                if (disposing)
                    bitmap.Dispose();
            }
            finally
            {
                NativeMethods.DeleteObject(hBitmap);
            }

            return source;
        }

        private static ImageCodecInfo GetCodec(this ImageFormat format)
        {
            foreach (var codec in ImageCodecInfo.GetImageEncoders())
            {
                if (codec.FormatID == format.Guid)
                    return codec;
            }

            return default;
        }

        private static ImageCodecInfo GetCodec(this string mimeType)
        {
            foreach (var codec in ImageCodecInfo.GetImageEncoders())
            {
                if (codec.MimeType == mimeType)
                    return codec;
            }

            return default;
        }
    }
}
