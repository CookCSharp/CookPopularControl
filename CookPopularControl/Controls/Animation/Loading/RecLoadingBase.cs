using CookPopularControl.Communal;
using CookPopularCSharpToolkit.Communal;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-04 08:38:44
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 表示由矩形组成的一组动画基类
    /// </summary>
    public abstract class RecLoadingBase : ContentControl
    {
        /// <summary>
        /// 是否正在运动
        /// </summary>
        public bool IsRecRunning
        {
            get { return (bool)GetValue(IsRecRunningProperty); }
            set { SetValue(IsRecRunningProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 提供<see cref="IsRecRunning"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsRecRunningProperty =
            DependencyProperty.Register("IsRecRunning", typeof(bool), typeof(RecLoadingBase),
                new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        /// <summary>
        /// 矩形宽度
        /// </summary>
        public double RecWidth
        {
            get { return (double)GetValue(RecWidthProperty); }
            set { SetValue(RecWidthProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="RecWidth"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty RecWidthProperty =
            DependencyProperty.Register("RecWidth", typeof(double), typeof(RecLoadingBase),
                new FrameworkPropertyMetadata(ValueBoxes.Double30Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        /// <summary>
        /// 矩形高度
        /// </summary>
        public double RecHeight
        {
            get { return (double)GetValue(RecHeightProperty); }
            set { SetValue(RecHeightProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="RecHeight"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty RecHeightProperty =
            DependencyProperty.Register("RecHeight", typeof(double), typeof(RecLoadingBase),
               new FrameworkPropertyMetadata(ValueBoxes.Double5Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        /// <summary>
        /// 矩形数量
        /// </summary>
        public int RecCount
        {
            get { return (int)GetValue(RecCountProperty); }
            set { SetValue(RecCountProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="RecCount"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty RecCountProperty =
            DependencyProperty.Register("RecCount", typeof(int), typeof(RecLoadingBase),
                new FrameworkPropertyMetadata(ValueBoxes.Inter5Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        /// <summary>
        /// 相邻矩形的间距
        /// </summary>
        public double RecInterval
        {
            get { return (double)GetValue(RecIntervalProperty); }
            set { SetValue(RecIntervalProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="RecInterval"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty RecIntervalProperty =
            DependencyProperty.Register("RecInterval", typeof(double), typeof(RecLoadingBase),
                new FrameworkPropertyMetadata(ValueBoxes.Double10Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        /// <summary>
        /// 动画持续时间
        /// </summary>
        public Duration RecDuration
        {
            get { return (Duration)GetValue(RecDurationProperty); }
            set { SetValue(RecDurationProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="RecDuration"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty RecDurationProperty =
            DependencyProperty.Register("RecDuration", typeof(Duration), typeof(RecLoadingBase),
                new FrameworkPropertyMetadata(new Duration(TimeSpan.FromSeconds(4)), FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        /// <summary>
        /// 速率
        /// </summary>
        public double RecRunSpeed
        {
            get { return (double)GetValue(RecRunSpeedProperty); }
            set { SetValue(RecRunSpeedProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="RecRunSpeed"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty RecRunSpeedProperty =
            DependencyProperty.Register("RecRunSpeed", typeof(double), typeof(RecLoadingBase),
                new FrameworkPropertyMetadata(ValueBoxes.Double5Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        /// <summary>
        /// 延迟时间
        /// </summary>
        public double RecDelayTime
        {
            get { return (double)GetValue(RecDelayTimeProperty); }
            set { SetValue(RecDelayTimeProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="RecDelayTime"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty RecDelayTimeProperty =
            DependencyProperty.Register("RecDelayTime", typeof(double), typeof(RecLoadingBase),
                new FrameworkPropertyMetadata(ValueBoxes.Double200Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));


        protected static void OnPropertiesValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rec = d as RecLoadingBase;
            if (rec != null)
                rec.UpdateRectangleAnimation();
        }


        protected Storyboard storyboard;
        protected Canvas recLoadingCanvas = new Canvas { ClipToBounds = true };
        protected double duration; //持续时长
        protected double moveXDistance; //移动距离，为了排列在水平中间位置
        protected double moveYDistance; //移动距离，为了排列在垂直中间位置
        protected double averageOpacity; //每等份透明度

        static RecLoadingBase()
        {
            ContentProperty.OverrideMetadata(typeof(RecLoadingBase), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ContentValueChanged)));
        }

        private static void ContentValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rec = d as RecLoadingBase;
            if (rec != null)
                rec.Content = rec.recLoadingCanvas;
        }

        protected RecLoadingBase()
        {
            Content = recLoadingCanvas;
            recLoadingCanvas.SetBinding(Canvas.BackgroundProperty, new Binding(BackgroundProperty.Name) { Source = this });
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            UpdateRectangleAnimation();
        }

        /// <summary>
        /// 更新动画
        /// </summary>
        private void UpdateRectangleAnimation()
        {
            if (RecCount < 1) return;
            recLoadingCanvas.Children.Clear();

            duration = RecDuration.TimeSpan.TotalSeconds;
            averageOpacity = Opacity / RecCount;
            moveXDistance = (Width - (RecCount - 1) * RecInterval - RecCount * RecWidth) / 2D;
            moveYDistance = (Height - (RecCount - 1) * RecInterval - RecCount * RecHeight) / 2D;

            storyboard = new Storyboard
            {
                RepeatBehavior = RepeatBehavior.Forever,
                SpeedRatio = RecRunSpeed,
            };

            PrepareRun();

            storyboard.Begin();
            storyboard.Freeze();

            if (IsRecRunning)
                storyboard!.Resume();
            else
                storyboard!.Pause();
        }

        /// <summary>
        /// 准备矩形
        /// </summary>
        protected abstract void PrepareRun();

        /// <summary>
        /// 创建一个容器装载矩形
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected abstract Border CreateContainer(int index);

        /// <summary>
        /// 创建一个矩形
        /// </summary>
        /// <returns></returns>
        protected virtual Rectangle CreateRectangle(int index)
        {
            var rec = new Rectangle();
            TransformGroup transformGroup = new TransformGroup();
            RotateTransform rt = new RotateTransform { Angle = 90 };
            transformGroup.Children.Add(rt);
            rec.RenderTransformOrigin = new Point(0.5, 0.5);
            rec.RenderTransform = transformGroup;
            rec.Width = RecWidth;
            rec.Height = RecHeight;
            rec.Fill = Foreground;
            rec.Stroke = BorderBrush;
            rec.StrokeThickness = BorderThickness.Left;
            rec.RadiusX = FrameworkElementBaseAttached.GetCornerRadius(this).TopLeft;
            rec.RadiusY = FrameworkElementBaseAttached.GetCornerRadius(this).TopLeft;

            return rec;
        }

        /// <summary>
        /// 起始值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected abstract double StartValue(int index);
    }
}
