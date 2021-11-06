using CookPopularCSharpToolkit.Windows.Interop;
using System;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：HslColor
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-09 15:48:02
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 使用HSL(Hue, Saturation, Lightness )来描绘颜色而不是RGB (Red, Green, Blue)
    /// </summary>
    internal class HslColor
    {
        /// <summary>
        /// 色度[0°,360°)
        /// </summary>
        public double Hue { get; protected set; }

        /// <summary>
        /// 饱和度[0,1]
        /// </summary>
        public double Saturation { get; protected set; }

        /// <summary>
        /// 亮度[0,1]
        /// </summary>
        public double Lightness { get; protected set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="hue"></param>
        /// <param name="saturation"></param>
        /// <param name="lightness"></param>
        public HslColor(double hue, double saturation, double lightness)
        {
            if (hue < 0)
                throw new ArgumentOutOfRangeException(string.Format("Hue: {0}", hue));

            if (hue > 360)
            {
                hue = ((int)hue) % 360;

                if (hue > 359)
                    hue = 359;
            }

            if (saturation < 0 || saturation > 1.0)
                throw new ArgumentOutOfRangeException(string.Format("Saturation: {0}", saturation));

            if (lightness < 0 || lightness > 1.0)
                throw new ArgumentOutOfRangeException(string.Format("Lightness: {0}", lightness));

            this.Hue = hue;
            this.Saturation = saturation;
            this.Lightness = lightness;
        }

        /// <summary>
        /// 将RGB转为HSL
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static HslColor RGBToHSL(Color? color)
        {
            if (color is null)
                return new HslColor(0, 0, 0);

            System.Drawing.Color convColor = System.Drawing.Color.FromArgb(color.Value.A, color.Value.R, color.Value.G, color.Value.B);

            int max = Math.Max(convColor.R, Math.Max(convColor.G, convColor.B));
            int min = Math.Min(convColor.R, Math.Min(convColor.G, convColor.B));

            double hue = convColor.GetHue();
            if (hue > 359)
                hue = 359;

            double saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            double value = max / 255d;

            return new HslColor(hue, saturation, value);
        }

        /// <summary>
        /// 将HSL转为RGB
        /// </summary>
        /// <param name="hslColor"></param>
        /// <returns></returns>
        public static Color HSLToRGB(HslColor hslColor)
        {
            int hi = Convert.ToInt32(Math.Floor(hslColor.Hue / 60)) % 6;
            double f = hslColor.Hue / 60 - Math.Floor(hslColor.Hue / 60);

            double value = hslColor.Lightness * 255;
            byte v = Convert.ToByte(value);
            byte p = Convert.ToByte(value * (1 - hslColor.Saturation));
            byte q = Convert.ToByte(value * (1 - f * hslColor.Saturation));
            byte t = Convert.ToByte(value * (1 - (1 - f) * hslColor.Saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }


        /// <summary>
        /// 获取鼠标点所在位置的颜色
        /// </summary>
        /// <returns></returns>
        private Color GetPixelColor()
        {
            InteropMethods.GetCursorPos(out var mousePoint);
            IntPtr hdc = InteropMethods.GetDC(IntPtr.Zero);
            uint pixel = InteropMethods.GetPixel(hdc, mousePoint.X, mousePoint.Y);
            InteropMethods.ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromRgb((byte)(pixel & 0x000000FF), (byte)((pixel & 0x0000FF00) >> 8), (byte)((pixel & 0x00FF0000) >> 16));
            return color;
        }
    }
}
