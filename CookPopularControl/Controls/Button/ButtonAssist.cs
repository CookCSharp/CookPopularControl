using CookPopularControl.Communal.Attached;
using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-02-23 09:35:49
 */
namespace CookPopularControl.Controls.Button
{
    /// <summary>
    /// 提供<see cref="System.Windows.Controls.Button"/>的附加属性
    /// </summary>
    public class ButtonAssist
    {
        public static Uri GetGifSource(DependencyObject obj) => (Uri)obj.GetValue(GifSourceProperty);
        public static void SetGifSource(DependencyObject obj, Uri value) => obj.SetValue(GifSourceProperty, value);
        public static readonly DependencyProperty GifSourceProperty =
            DependencyProperty.RegisterAttached("GifSource", typeof(Uri), typeof(ButtonAssist), new PropertyMetadata(default(Uri)));

        public static Stream GetGifStream(DependencyObject obj) => (Stream)obj.GetValue(GifStreamProperty);
        public static void SetGifStream(DependencyObject obj, Stream value) => obj.SetValue(GifStreamProperty, value);
        /// <summary>
        /// 提供<see cref="Stream"/>的Gif图像
        /// </summary>
        public static readonly DependencyProperty GifStreamProperty =
            DependencyProperty.RegisterAttached("GifStream", typeof(Stream), typeof(ButtonAssist), new PropertyMetadata(default(Stream)));

        public static ImageSource GetImageSource(DependencyObject obj) => (ImageSource)obj.GetValue(ImageSourceProperty);
        public static void SetImageSource(DependencyObject obj, ImageSource value) => obj.SetValue(ImageSourceProperty, value);
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.RegisterAttached("ImageSource", typeof(ImageSource), typeof(ButtonAssist), new PropertyMetadata(default(ImageSource)));

        public static double GetImageWidth(DependencyObject obj) => (double)obj.GetValue(ImageWidthProperty);
        public static void SetImageWidth(DependencyObject obj, double value) => obj.SetValue(ImageWidthProperty, value);
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.RegisterAttached("ImageWidth", typeof(double), typeof(ButtonAssist), new PropertyMetadata(ValueBoxes.Double0Box));

        public static double GetImageHeight(DependencyObject obj) => (double)obj.GetValue(ImageHeightProperty);
        public static void SetImageHeight(DependencyObject obj, double value) => obj.SetValue(ImageHeightProperty, value);
        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.RegisterAttached("ImageHeight", typeof(double), typeof(ButtonAssist), new PropertyMetadata(ValueBoxes.Double0Box));

        public static double GetProgressValue(DependencyObject obj) => (double)obj.GetValue(ProgressValueProperty);
        public static void SetProgressValue(DependencyObject obj, double value) => obj.SetValue(ProgressValueProperty, value);
        public static readonly DependencyProperty ProgressValueProperty =
            DependencyProperty.RegisterAttached("ProgressValue", typeof(double), typeof(ButtonAssist), new PropertyMetadata(ValueBoxes.Double0Box));

        public static bool GetIsShowProgress(DependencyObject obj) => (bool)obj.GetValue(IsShowProgressProperty);
        public static void SetIsShowProgress(DependencyObject obj, bool value) => obj.SetValue(IsShowProgressProperty, ValueBoxes.BooleanBox(value));
        public static readonly DependencyProperty IsShowProgressProperty =
            DependencyProperty.RegisterAttached("IsShowProgress", typeof(bool), typeof(ButtonAssist), new PropertyMetadata(ValueBoxes.FalseBox));
    }
}
