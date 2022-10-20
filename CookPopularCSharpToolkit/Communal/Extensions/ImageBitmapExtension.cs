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
        /// Bitmap  转 BitmapImage
        /// </summary>
        /// <param name="bitmap">Bitmap 对象</param>
        /// <returns>转换后的 BitmapImage对象</returns>
        public static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            try
            {
                BitmapImage relust = new BitmapImage();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    MemoryStream memory = new MemoryStream();
                    bitmap.Save(memoryStream, ImageFormat.Png);
                    memoryStream.Position = 0;
                    relust.BeginInit();
                    relust.CacheOption = BitmapCacheOption.OnLoad;
                    memoryStream.CopyTo(memory);
                    relust.StreamSource = memory;
                    relust.EndInit();
                    relust.Freeze();
                    return relust;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// ImageSource 转为Bitmap
        /// </summary>
        /// <param name="imageSource">imageSource 对象</param>
        /// <returns>返回 Bitmap 对象</returns>
        public static Bitmap ToBitmap(ImageSource imageSource)
        {
            try
            {
                BitmapSource bitmapSource = (BitmapSource)imageSource;
                Bitmap bitmap = new Bitmap(bitmapSource.PixelWidth, bitmapSource.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                BitmapData data = bitmap.LockBits(
                    new Rectangle(System.Drawing.Point.Empty, bitmap.Size),
                    ImageLockMode.WriteOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                bitmapSource.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
                bitmap.UnlockBits(data);
                return bitmap;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;
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

        public static Icon ToIcon(this Bitmap bitmap, System.Drawing.Size? size)
        {
            using (Bitmap iconBm = size.HasValue ? new Bitmap(bitmap, size.Value) : new Bitmap(bitmap))
            {
                using (Icon icon = Icon.FromHandle(iconBm.GetHicon()))
                {
                    return icon;
                }
            }
        }

        /// <summary>
        /// 转换Image为Icon
        /// </summary>
        /// <param name="image">要转换为图标的Image对象</param>
        /// <param name="nullTonull">当image为null时是否返回null。false则抛空引用异常</param>
        /// <exception cref="ArgumentNullException" />
        public static Icon ToIcon(this Image image, bool nullTonull = false)
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

        public static void SaveAsIconFile(this Bitmap bitmap, string saveFilePath, System.Drawing.Size? size)
        {
            using (Icon icon = size.HasValue ? bitmap.ToIcon(size) : bitmap.ToIcon())
            {
                using (Stream stream = new FileStream(saveFilePath, FileMode.Create))
                {
                    icon.Save(stream);
                }
            }
        }

        public static byte[] ToBytesArray(this Bitmap img, int width, int height, int channel)
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

        /// <summary>
        /// BitmapImage 转为byte[]
        /// </summary>
        /// <param name="bitmapImage">BitmapImage 对象</param>
        /// <returns>byte[] 数组</returns>
        public static byte[] ToByteArray(BitmapImage bitmapImage)
        {
            byte[] buffer = new byte[] { };
            try
            {
                Stream stream = bitmapImage.StreamSource;
                if (stream != null && stream.Length > 0)
                {
                    stream.Position = 0;
                    using (BinaryReader binary = new BinaryReader(stream))
                    {
                        buffer = binary.ReadBytes((int)stream.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return buffer;
        }

        /// <summary>
        /// 图片压缩
        /// </summary>
        /// <param name="bitmap">要压缩的源图像</param>
        /// <param name="height">要求的高</param>
        /// <param name="width">要求的宽</param>
        /// <returns></returns>
        public static Bitmap GetPicThumbnail(Bitmap bitmap, int height, int width)
        {
            try
            {
                lock (bitmap)
                {
                    Bitmap iSource = bitmap;
                    ImageFormat imageFormat = iSource.RawFormat;
                    int sw = width, sh = height;
                    //按比例缩放
                    Bitmap ob = new Bitmap(width, height);
                    Graphics graphics = Graphics.FromImage(ob);
                    graphics.Clear(System.Drawing.Color.WhiteSmoke);
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(iSource, new System.Drawing.Rectangle((width - sw) / 2, (height - sh) / 2, sw, sh), 0, 0, iSource.Width, iSource.Height, System.Drawing.GraphicsUnit.Pixel);
                    graphics.Dispose();
                    return ob;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return bitmap;
        }
    }
}
