using CookPopularCSharpToolkit.Communal;
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
 * Company: CookCSharp
 * Create Time：2021-12-24 21:05:57
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2021 All Rights Reserved.
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// GIF动画控件
    /// </summary>
    /// <remarks>
    /// https://github.com/XamlAnimatedGif/XamlAnimatedGif
    /// </remarks>
    public class Gif : System.Windows.Controls.Image
    {
        private Bitmap _gifBitmap; //Gif图
        private BitmapSource _bitmapSource; //Gif图的每一帧
        private bool _isStartGif;
        private bool _isSetParameters;



        /// <summary>
        /// 是否自动启动
        /// </summary>
        public bool IsAutoStart
        {
            get => (bool)GetValue(IsAutoStartProperty);
            set => SetValue(IsAutoStartProperty, ValueBoxes.BooleanBox(value));
        }
        /// <summary>
        /// 提供<see cref="IsAutoStart"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsAutoStartProperty =
            DependencyProperty.Register("IsAutoStart", typeof(bool), typeof(Gif), new PropertyMetadata(ValueBoxes.TrueBox, OnIsAutoStartPropertyChanged));

        private static void OnIsAutoStartPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Gif gif)
            {
                var isStart = (bool)e.NewValue;

                if(gif.IsLoaded)
                {
                    ControlGif();
                }
                else
                {
                    gif.Loaded += (s, e) => ControlGif();
                }

                void ControlGif()
                {
                    if (isStart && (gif.GifSource != null || gif.GifStream != null))
                        gif.StartAnimate();
                    else
                        gif.StopAnimate();
                }
            }
        }


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
            DependencyProperty.Register("GifSource", typeof(Uri), typeof(Gif), new UIPropertyMetadata(default(Uri), OnGifSourcePropertyChanged));

        protected static void OnGifSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Gif gif)
            {
                gif.GifSource = e.NewValue as Uri;
                var stream = Application.GetResourceStream(gif.GifSource).Stream;
                gif.SetGifParameters(gif, stream);

                if (gif.IsAutoStart && !gif._isStartGif)
                    gif.StartAnimate();

                gif.GifStream = stream;
            }
        }


        /// <summary>
        /// Gif的流
        /// </summary>
        public Stream GifStream
        {
            get => (Stream)GetValue(GifStreamProperty);
            set => SetValue(GifStreamProperty, value);
        }
        /// <summary>
        /// 提供<see cref="GifStream"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty GifStreamProperty =
            DependencyProperty.Register("GifStream", typeof(Stream), typeof(Gif), new PropertyMetadata(default(Stream), OnGifStreamPropertyChanged));

        private static void OnGifStreamPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Gif gif)
            {
                gif.GifStream = e.NewValue as Stream;
                gif.SetGifParameters(gif, gif.GifStream);

                if (gif.IsAutoStart && !gif._isStartGif)
                    gif.StartAnimate();
            }
        }

        private void SetGifParameters(Gif gif, Stream stream)
        {
            if (!gif._isSetParameters)
            {
                gif._gifBitmap = new Bitmap(stream);
                gif._bitmapSource = gif.GetBitmapSource();
                gif.Source = gif._bitmapSource;
            }
            _isSetParameters = true;
        }


        public void StartAnimate()
        {
            _isStartGif = true;
            ImageAnimator.Animate(this._gifBitmap, this.OnFrameChanged);
        }

        public void StopAnimate()
        {
            _isStartGif = false;
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
