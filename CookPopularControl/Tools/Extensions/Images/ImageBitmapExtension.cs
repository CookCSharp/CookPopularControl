using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ImageBitmapExtension
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-30 20:21:36
 */
namespace CookPopularControl.Tools.Extensions.Images
{
    /// <summary>
    /// 提供<see cref="System.Windows.Controls.Image"/>与<see cref="Bitmap"/>的相互转换
    /// </summary>
    public static class ImageBitmapExtension
    {
        [DllImport("Gdi32.dll")]
        private static extern bool DeleteObject(IntPtr intPtr);

        /// <summary>
        /// Bitmap to ImageSource
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static ImageSource ToImageSource(Bitmap bitmap)
        {
            IntPtr intPtr = bitmap.GetHbitmap();
            try
            {
                ImageSource imageSource = Imaging.CreateBitmapSourceFromHBitmap(intPtr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                return imageSource;
            }
            finally
            {
                DeleteObject(intPtr);
            }
        }

        /// <summary>
        /// <see cref="BitmapSource"/> To <see cref="Bitmap"/>
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <returns></returns>
        public static Bitmap ToBitmap(BitmapSource bitmapSource)
        {
            if (bitmapSource == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(ms);
                Bitmap bitmap = new Bitmap(ms);

                return bitmap;
            }
        }
    }
}
