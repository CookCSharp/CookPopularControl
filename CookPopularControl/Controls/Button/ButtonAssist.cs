using CookPopularControl.Communal.Attached;
using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
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
    public class ButtonAssist
    {
        public static ImageSource GetGifSource(DependencyObject obj) => (ImageSource)obj.GetValue(GifSourceProperty);
        public static void SetGifSource(DependencyObject obj, ImageSource value) => obj.SetValue(GifSourceProperty, value);
        public static readonly DependencyProperty GifSourceProperty =
            DependencyProperty.RegisterAttached("GifSource", typeof(ImageSource), typeof(ButtonAssist), new PropertyMetadata(default(ImageSource)));

        public static double GetGifWidth(DependencyObject obj) => (double)obj.GetValue(GifWidthProperty);
        public static void SetGifWidth(DependencyObject obj, double value) => obj.SetValue(GifWidthProperty, value);
        public static readonly DependencyProperty GifWidthProperty =
            DependencyProperty.RegisterAttached("GifWidth", typeof(double), typeof(ButtonAssist), new PropertyMetadata(ValueBoxes.Double0Box));

        public static double GetGifHeight(DependencyObject obj) => (double)obj.GetValue(GifHeightProperty);
        public static void SetGifHeight(DependencyObject obj, double value) => obj.SetValue(GifHeightProperty, value);
        public static readonly DependencyProperty GifHeightProperty =
            DependencyProperty.RegisterAttached("GifHeight", typeof(double), typeof(ButtonAssist), new PropertyMetadata(ValueBoxes.Double0Box));
    }
}
