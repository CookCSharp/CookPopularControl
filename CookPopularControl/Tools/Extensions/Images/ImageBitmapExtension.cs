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
    /// 提供<see cref="System.Drawing.Image"/>与<see cref="Bitmap"/>一些扩展方法
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

        public static byte[] ToBytesStreamFromBitmap(int width, int height, int channel, Bitmap img)
        {
            byte[] bytes = new byte[width * height * channel];

            BitmapData im = img.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, img.PixelFormat);
            int stride = im.Stride;
            int offset = stride - width * channel;
            int length = stride * height;
            byte[] temp = new byte[stride * height];
            Marshal.Copy(im.Scan0, temp, 0, temp.Length);
            img.UnlockBits(im);

            int posreal = 0;
            int posscan = 0;
            for (int c = 0; c < height; c++)
            {
                for (int d = 0; d < width * channel; d++)
                {
                    bytes[posreal++] = temp[posscan++];
                }
                posscan += offset;
            }

            return bytes;
        }
    }
}
