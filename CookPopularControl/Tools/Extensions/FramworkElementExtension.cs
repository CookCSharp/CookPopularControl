using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：FramworkElementExtension
 * Author： Chance_写代码的厨子
 * Create Time：2021-10-14 10:08:57
 */
namespace CookPopularControl.Tools.Extensions
{
    public static class FramworkElementExtension
    {
        /// <summary>
        /// 将<see cref="FrameworkElement"/>保存为位图
        /// </summary>
        /// <param name="element">元素</param>
        /// <param name="fileName">文件路径及文件名</param>
        public static void SaveAsPicture(this FrameworkElement element, string fileName)
        {
            var dpiX = Screenshot.GetDpiX();
            var dpiY = Screenshot.GetDpiY();
            int width = (int)(element.ActualWidth * dpiX / 88);
            int height = (int)(element.ActualHeight * dpiX / 96);
            var bitmapSource = new RenderTargetBitmap(width, height, dpiX, dpiY, PixelFormats.Default);
            bitmapSource.Render(element);

            //using var ms = new MemoryStream();
            //BitmapEncoder encoder = new BmpBitmapEncoder();
            //encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            //encoder.Save(ms);
            //下面这种方式生成文件很慢
            //FileStream fs = File.Open(filePath, FileMode.OpenOrCreate);
            //encoder.Save(fs);

            //生成透明背景图片
            //using var bitmap = new Bitmap(ms);
            //bitmap.MakeTransparent();
            //bitmap.Save(fileName);

            //生成透明背景图片
            var pixels = new int[width * height];
            bitmapSource.CopyPixels(pixels, width * 4, 0);
            using (var bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb))
            {
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                        bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(pixels[y * width + x]));
                bitmap.Save(fileName);
            }
        }
    }
}
