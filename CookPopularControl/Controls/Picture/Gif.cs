using CookPopularCSharpToolkit.Windows.Interop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;



/*
 * Description：Gif 
 * Author： Chance(a cook of write code)
 * Company: NCATest
 * Create Time：2021-12-24 21:05:57
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) NCATest 2021 All Rights Reserved.
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// GIF动画
    /// </summary>
    /// <remarks>
    /// https://github.com/XamlAnimatedGif/XamlAnimatedGif
    /// </remarks>
    public class Gif : System.Windows.Controls.Image
    {
        private Bitmap _gifBitmap; 
        private BitmapSource _bitmapSource;

        /// <summary>
        /// Gif的路径
        /// </summary>
        public Uri GifSource
        {
            get { return (Uri)GetValue(GifSourceProperty); }
            set { SetValue(GifSourceProperty, value); }
        }
        /// <summary>
        /// 表示<see cref="GifSource"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty GifSourceProperty =
            DependencyProperty.Register("GifSource", typeof(Uri), typeof(Gif), new UIPropertyMetadata(default(Uri), GifSourcePropertyChanged));
        protected static void GifSourcePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is Gif gif)
            {
                gif.GifSource = e.NewValue as Uri;
                var stream = Application.GetResourceStream(gif.GifSource).Stream;
                gif._gifBitmap = new Bitmap(stream);
                gif._bitmapSource = gif.GetBitmapSource();
                gif.Source = gif._bitmapSource;
                gif.StartAnimate();
            }
        }

        public void StartAnimate()
        {
            ImageAnimator.Animate(this._gifBitmap, this.OnFrameChanged);
        }

        public void StopAnimate()
        {
            ImageAnimator.StopAnimate(this._gifBitmap, this.OnFrameChanged);
        }

        private void OnFrameChanged(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                ImageAnimator.UpdateFrames(); 
                if (this._bitmapSource != null)
                {
                    this._bitmapSource.Freeze();
                }

                this._bitmapSource = this.GetBitmapSource();
                Source = this._bitmapSource;
                this.InvalidateVisual();
            }));
        }

        /// <summary>
        /// BitmapSource用于显示System.drawing.bitmap中的图像的帧
        /// </summary>
        /// <returns></returns>
        private BitmapSource GetBitmapSource()
        {
            IntPtr handle = IntPtr.Zero;

            try
            {
                handle = this._gifBitmap.GetHbitmap();
                this._bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                if (handle != IntPtr.Zero)
                {
                    InteropMethods.DeleteObject(handle);
                }
            }
            return this._bitmapSource;
        }
    }
}
