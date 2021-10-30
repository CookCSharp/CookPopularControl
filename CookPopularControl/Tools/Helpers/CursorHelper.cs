using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CursorHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-11 19:19:06
 */
namespace CookPopularControl.Tools.Helpers
{
    /// <summary>
    /// 光标辅助类
    /// </summary>
    public class CursorHelper
    {
        public static Cursor ConvertToCursor(FrameworkElement visual, System.Windows.Point hotSpot)
        {
            int width = (int)visual.Width;
            int height = (int)visual.Height;

            var dpiX = Screenshot.GetDpiX();
            var dpiY = Screenshot.GetDpiY();

            // Render to a bitmap
            var bitmapSource = new RenderTargetBitmap(width, height, dpiX, dpiY, PixelFormats.Pbgra32);
            bitmapSource.Render(visual);

            // Convert to System.Drawing.Bitmap
            var pixels = new int[width * height];
            bitmapSource.CopyPixels(pixels, width * 4, 0);
            var bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(pixels[y * width + x]));

            // Save to .ico format
            var stream = new MemoryStream();
            Icon.FromHandle(bitmap.GetHicon()).Save(stream);

            // Convert saved file into .cur format
            stream.Seek(2, SeekOrigin.Begin);
            stream.WriteByte(2);
            stream.Seek(10, SeekOrigin.Begin);
            stream.WriteByte((byte)(int)(hotSpot.X * width));
            stream.WriteByte((byte)(int)(hotSpot.Y * height));
            stream.Seek(0, SeekOrigin.Begin);

            return new Cursor(stream);
        }
    }
}
