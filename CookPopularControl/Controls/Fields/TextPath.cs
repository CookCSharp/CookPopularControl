using CookPopularCSharpToolkit.Windows;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：TextPath
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-23 15:43:41
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 自定义绘制文本
    /// </summary>
    public class TextPath : Shape
    {
        protected override Geometry DefiningGeometry => Data ?? Geometry.Empty;

        /// <summary>
        /// 获取或设置用于指定所要绘制的形状的<see cref="Geometry"/>
        /// </summary>
        public Geometry Data
        {
            get { return (Geometry)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="Data"/>依赖属性。
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(Geometry), typeof(TextPath),
                new FrameworkPropertyMetadata(Geometry.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 获取或设置用于指定所要绘制的文本
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="Text"/>依赖属性。
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextPath),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesChanged));

        public static ImageSource GetFillImageSource(DependencyObject obj) => (ImageSource)obj.GetValue(FillImageSourceProperty);
        public static void SetFillImageSource(DependencyObject obj, ImageSource value) => obj.SetValue(FillImageSourceProperty, value);
        /// <summary>
        /// <see cref="FillImageSourceProperty"/>标识填充的图片资源附加属性
        /// </summary>
        public static readonly DependencyProperty FillImageSourceProperty =
            DependencyProperty.RegisterAttached("FillImageSource", typeof(ImageSource), typeof(TextPath),
                new FrameworkPropertyMetadata(default(ImageSource), FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesChanged));

        public static Rect GetDrawingRec(DependencyObject obj) => (Rect)obj.GetValue(DrawingRecProperty);
        public static void SetDrawingRec(DependencyObject obj, Rect value) => obj.SetValue(DrawingRecProperty, value);
        /// <summary>
        /// <see cref="DrawingRecProperty"/>标识获取或设置在其中绘制图像的区域
        /// </summary>
        public static readonly DependencyProperty DrawingRecProperty =
            DependencyProperty.RegisterAttached("DrawingRec", typeof(Rect), typeof(TextPath),
                new FrameworkPropertyMetadata(new Rect(0, 0, 20, 20), FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesChanged));

        public static Brush GetDrawingBrush(DependencyObject obj) => (Brush)obj.GetValue(DrawingBrushProperty);
        public static void SetDrawingBrush(DependencyObject obj, Brush value) => obj.SetValue(DrawingBrushProperty, value);
        /// <summary>
        /// <see cref="DrawingBrushProperty"/>标识填充IconGeometry的画刷
        /// </summary>
        public static readonly DependencyProperty DrawingBrushProperty =
            DependencyProperty.RegisterAttached("DrawingBrush", typeof(Brush), typeof(TextPath),
                new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesChanged));

        private static void OnPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var path = d as TextPath;
            if (path != null)
                path.UpdatePath();
        }

        public TextPath()
        {
            this.Loaded += (s, e) => UpdatePath();
        }

        private void UpdatePath()
        {
            Data = Text.GetGeometryFromText();
        }
    }
}
