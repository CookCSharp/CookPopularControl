using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：提供Gif创建的扩展方法
 * Author： Chance_写代码的厨子
 * Create Time：2021-02-26 17:35:04
 */
namespace CookPopularCSharpToolkit.Communal
{
    /// <summary>
    /// 提供Gif创建的扩展方法
    /// </summary>
    public class GifExtension
    {
        /// <summary>
        /// 从指定的文件名创建Gif图像
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="savePath">保存路径</param>
        public static void CreateGifFromImageFile(string fileName, string savePath)
        {
            var image = Image.FromFile(fileName);
            CreateGifFromImage(image, savePath);
        }

        /// <summary>
        /// 使用文件流创建gif图像
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="savePath">保存路径</param>
        public static void CreateGifFromStream(Stream stream, string savePath)
        {
            var image = Image.FromStream(stream);
            CreateGifFromImage(image, savePath);
        }

        /// <summary>
        /// 使用句柄创建gif图像
        /// </summary>
        /// <param name="hbitmap">句柄</param>
        /// <param name="savePath">保存路径</param>
        public static void CreateGifFromImageFile(IntPtr hbitmap, string savePath)
        {
            var image = Image.FromHbitmap(hbitmap);
            CreateGifFromImage(image, savePath);
        }

        /// <summary>
        /// 创建gif文件
        /// </summary>
        /// <param name="image"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        private static Bitmap? CreateGifFromImage(Image image, string savePath)
        {
            GifBitmapEncoder gifEncoder = new GifBitmapEncoder();
            bool isStop = false;
            Bitmap? bitmap = default;
            while (!isStop)
            {
                bitmap = (Bitmap)image;
                var bmp = bitmap.GetHbitmap();
                var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                gifEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                System.Threading.Thread.Sleep(200);
                if (bitmapSource != null) isStop = true;
            }
            using (FileStream fs = new FileStream(savePath, FileMode.Create))
            {
                gifEncoder.Save(fs);
            }

            return bitmap;
        }
    }
}
