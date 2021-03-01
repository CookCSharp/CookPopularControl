using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
 * Create Time：2021-02-26 23:03:35
 */
namespace CookPopularControl.Controls.Animation.Loading
{
    /// <summary>
    /// 由点组成的加载动画
    /// </summary>
    /// <remarks>包含圆、直线、摆钟</remarks>
    public class DotLoading : ContentControl
    {
        /// <summary>
        /// 点的数量
        /// </summary>
        public int DotCount
        {
            get { return (int)GetValue(DotCountProperty); }
            set { SetValue(DotCountProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="DotCount"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DotCountProperty =
            DependencyProperty.Register("DotCount", typeof(int), typeof(DotLoading),
                new FrameworkPropertyMetadata(ValueBoxes.Inter5Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        /// <summary>
        /// 点的半径
        /// </summary>
        public double DotRadius
        {
            get { return (double)GetValue(DotRadiusProperty); }
            set { SetValue(DotRadiusProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="DotRadius"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DotRadiusProperty =
            DependencyProperty.Register("DotRadius", typeof(double), typeof(DotLoading),
                new FrameworkPropertyMetadata(ValueBoxes.Double5Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        /// <summary>
        /// 点的BoderThcikness
        /// </summary>
        public double DotStrokeThickness
        {
            get { return (double)GetValue(DotStrokeThicknessProperty); }
            set { SetValue(DotStrokeThicknessProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="DotStrokeThickness"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DotStrokeThicknessProperty =
            DependencyProperty.Register("DotStrokeThickness", typeof(double), typeof(DotLoading),
                new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        /// <summary>
        /// 点的BorderBrush
        /// </summary>
        public Brush DotStroke
        {
            get { return (Brush)GetValue(DotStrokeProperty); }
            set { SetValue(DotStrokeProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="DotStroke"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DotStrokeProperty =
            DependencyProperty.Register("DotStroke", typeof(Brush), typeof(DotLoading),
                new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        /// <summary>
        /// 相邻点之间的间隔距离
        /// </summary>
        public double DotInterval
        {
            get { return (double)GetValue(DotIntervalProperty); }
            set { SetValue(DotIntervalProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="DotInterval"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DotIntervalProperty =
            DependencyProperty.Register("DotInterval", typeof(double), typeof(DotLoading),
                new FrameworkPropertyMetadata(ValueBoxes.Double10Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        /// <summary>
        /// 点的运行速率
        /// </summary>
        public double DotRunSpeed
        {
            get { return (double)GetValue(DotRunSpeedProperty); }
            set { SetValue(DotRunSpeedProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="DotRunSpeed"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DotRunSpeedProperty =
            DependencyProperty.Register("DotRunSpeed", typeof(double), typeof(DotLoading),
                new FrameworkPropertyMetadata(ValueBoxes.Double1Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));


        /// <summary>
        /// 依赖属性值改变时触发
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnPropertiesValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dotloading = d as DotLoading;
            if (dotloading != null)
                dotloading.UpdateDotsAnimation();
        }

        private const double duration = 4D;
        private const double delayTime = 230D;
        private Canvas canvas = new Canvas { ClipToBounds = true };

        static DotLoading()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(DotLoading), new FrameworkPropertyMetadata(typeof(DotLoading)));
        }

        public DotLoading()
        {
            Content = canvas;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            UpdateDotsAnimation();
        }

        private void UpdateDotsAnimation()
        {
            if (DotCount < 1) return;
            canvas.Children.Clear();

            Storyboard storyboard = new Storyboard
            {
                RepeatBehavior = RepeatBehavior.Forever,
                SpeedRatio = DotRunSpeed,
            };

            for (int i = 0; i < DotCount; i++)
            {
                var container = CreateContainer(i);
                var angle = -DotInterval * i + 360;
                var frames = new DoubleAnimationUsingKeyFrames
                {
                    BeginTime = TimeSpan.FromMilliseconds(delayTime * i)
                };

                var frame1 = new LinearDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                    Value = angle + 0,
                };
                var frame2 = new EasingDoubleKeyFrame
                {
                    EasingFunction = new CircleEase { EasingMode = EasingMode.EaseInOut },
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.90 * duration)),
                    Value = angle + 360,
                };

                frames.KeyFrames.Add(frame1);
                frames.KeyFrames.Add(frame2);

                Storyboard.SetTarget(frames, container);
                Storyboard.SetTargetProperty(frames, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));
                storyboard.Children.Add(frames);

                canvas.Children.Add(container);
            }

            storyboard.Begin(canvas, HandoffBehavior.SnapshotAndReplace, true);
        }

        /// <summary>
        /// 创建一个容器做旋转动画(包含Dot)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Border CreateContainer(int index)
        {
            var dot = CreateEllipse(index);
            dot.HorizontalAlignment = HorizontalAlignment.Center;
            dot.VerticalAlignment = VerticalAlignment.Top;

            var border = new Border();
            RotateTransform rt = new RotateTransform() { Angle = -DotInterval * index };
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(rt);
            border.RenderTransformOrigin = new Point(0.5, 0.5);
            border.RenderTransform = transformGroup;
            border.Child = dot;
            border.SetBinding(WidthProperty, new Binding(WidthProperty.Name) { Source = this });
            border.SetBinding(HeightProperty, new Binding(HeightProperty.Name) { Source = this });

            return border;
        }

        /// <summary>
        /// 创建Dot
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Ellipse CreateEllipse(int index)
        {
            var ellipse = new Ellipse();
            ellipse.SetBinding(WidthProperty, new Binding(DotRadiusProperty.Name) { Source = this });
            ellipse.SetBinding(HeightProperty, new Binding(DotRadiusProperty.Name) { Source = this });
            ellipse.SetBinding(Shape.FillProperty, new Binding(ForegroundProperty.Name) { Source = this });
            ellipse.SetBinding(Shape.StrokeProperty, new Binding(DotStrokeProperty.Name) { Source = this });
            ellipse.SetBinding(Shape.StrokeThicknessProperty, new Binding(DotStrokeThicknessProperty.Name) { Source = this });

            return ellipse;
        }
    }
}
