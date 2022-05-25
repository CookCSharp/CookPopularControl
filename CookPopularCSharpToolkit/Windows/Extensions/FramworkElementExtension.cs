using CookPopularCSharpToolkit.Communal;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：FramworkElementExtension
 * Author： Chance_写代码的厨子
 * Create Time：2021-10-14 10:08:57
 */
namespace CookPopularCSharpToolkit.Windows
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
            var dpiX = DpiHelper.DeviceDpiX;
            var dpiY = DpiHelper.DeviceDpiY;

            double elementWidth = 0;
            double elementHeight = 0;
            CheckElementSide(ref elementWidth, ref elementHeight);

            int width = (int)(elementWidth * DpiHelper.GetScaleX());
            int height = (int)(elementHeight * DpiHelper.GetScaleX());
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

            void CheckElementSide(ref double elementWidth, ref double elementHeight)
            {
                if (!double.IsNaN(element.ActualWidth))
                    elementWidth = element.ActualWidth;
                else if (element.Width.CompareTo(0) > 0)
                    elementWidth = element.Width;
                else
                    elementWidth = 100;

                if (!double.IsNaN(element.ActualHeight))
                    elementHeight = element.ActualHeight;
                else if (element.Height.CompareTo(0) > 0)
                    elementHeight = element.Height;
                else
                    elementHeight = 100;
            }
        }
    }
}
