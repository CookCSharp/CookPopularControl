using CookPopularCSharpToolkit.Windows.Interop;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ImageBitmapExtension
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-30 20:21:36
 */
namespace CookPopularCSharpToolkit.Communal
{
    /// <summary>
    /// 提供<see cref="System.Drawing.Image"/>与<see cref="Bitmap"/>一些扩展方法
    /// </summary>
    public static class ImageBitmapExtension
    {
        /// <summary>
        /// Bitmap to ImageSource
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static ImageSource ToImageSource(this Bitmap bitmap)
        {
            IntPtr intPtr = bitmap.GetHbitmap();
            try
            {
                ImageSource imageSource = Imaging.CreateBitmapSourceFromHBitmap(intPtr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                return imageSource;
            }
            finally
            {
                InteropMethods.DeleteObject(intPtr);
            }
        }

        /// <summary>
        /// <see cref="BitmapSource"/> To <see cref="Bitmap"/>
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <returns></returns>
        public static Bitmap ToBitmap(this BitmapSource bitmapSource)
        {
            if (bitmapSource == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(ms);

                return new Bitmap(ms);
            }
        }

        public static Icon ToIcon(this Bitmap bitmap, System.Drawing.Size size)
        {
            using (Bitmap iconBm = new Bitmap(bitmap, size))
            {
                using (Icon icon = Icon.FromHandle(iconBm.GetHicon()))
                {
                    return icon;
                }
            }
        }

        public static Icon ToIcon(this ImageSource imageSource)
        {
            if (imageSource == null) return null;

            Uri uri = new Uri(imageSource.ToString());
            StreamResourceInfo streamInfo = Application.GetResourceStream(uri);

            if (streamInfo == null)
            {
                string msg = "The supplied image source '{0}' could not be resolved.";
                msg = string.Format(msg, imageSource);
                throw new ArgumentException(msg);
            }

            return new Icon(streamInfo.Stream);
        }

        /// <summary>
        /// 转换Image为Icon
        /// </summary>
        /// <param name="image">要转换为图标的Image对象</param>
        /// <param name="nullTonull">当image为null时是否返回null。false则抛空引用异常</param>
        /// <exception cref="ArgumentNullException" />
        public static Icon ToIcon(this System.Drawing.Image image, bool nullTonull = false)
        {
            if (image == null)
            {
                if (nullTonull) { return null; }
                throw new ArgumentNullException("image");
            }

            using (MemoryStream msImg = new MemoryStream(), msIco = new MemoryStream())
            {
                image.Save(msImg, ImageFormat.Png);

                using (var bin = new BinaryWriter(msIco))
                {
                    //写图标头部
                    bin.Write((short)0);      //0-1保留
                    bin.Write((short)1);      //2-3文件类型。1=图标, 2=光标
                    bin.Write((short)1);      //4-5图像数量（图标可以包含多个图像）

                    bin.Write((byte)image.Width); //6图标宽度
                    bin.Write((byte)image.Height); //7图标高度
                    bin.Write((byte)0);      //8颜色数（若像素位深>=8，填0。这是显然的，达到8bpp的颜色数最少是256，byte不够表示）
                    bin.Write((byte)0);      //9保留。必须为0
                    bin.Write((short)0);      //10-11调色板
                    bin.Write((short)32);     //12-13位深
                    bin.Write((int)msImg.Length); //14-17位图数据大小
                    bin.Write(22);         //18-21位图数据起始字节

                    //写图像数据
                    bin.Write(msImg.ToArray());

                    bin.Flush();
                    bin.Seek(0, SeekOrigin.Begin);
                    return new Icon(msIco);
                }
            }
        }

        public static void SaveAsIconFile(this Bitmap bitmap, System.Drawing.Size size, string saveFilePath)
        {
            using (Bitmap iconBm = new Bitmap(bitmap, size))
            {
                using (Icon icon = bitmap.ToIcon(true))
                {
                    using (Stream stream = new FileStream(saveFilePath, FileMode.Create))
                    {
                        icon.Save(stream);
                    }
                }
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
