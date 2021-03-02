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
        /// Dot是否正在运动
        /// </summary>
        public bool IsDotRunning
        {
            get { return (bool)GetValue(IsDotRunningProperty); }
            set { SetValue(IsDotRunningProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 提供<see cref="IsDotRunning"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsDotRunningProperty =
            DependencyProperty.Register("IsDotRunning", typeof(bool), typeof(DotLoading),
                new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

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
        /// Dot半径是按照否比例系数变化
        /// </summary>
        /// <remarks>默认值为false</remarks>
        public bool IsDotRadiusEqualScale
        {
            get { return (bool)GetValue(IsDotRadiusEqualScaleProperty); }
            set { SetValue(IsDotRadiusEqualScaleProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 提供<see cref="IsDotRadiusEqualScale"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsDotRadiusEqualScaleProperty =
            DependencyProperty.Register("IsDotRadiusEqualScale", typeof(bool), typeof(DotLoading),
                new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

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
        /// 动画持续时间
        /// </summary>
        public Duration DotDuration
        {
            get { return (Duration)GetValue(DotDurationProperty); }
            set { SetValue(DotDurationProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="DotDuration"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DotDurationProperty =
            DependencyProperty.Register("DotDuration", typeof(Duration), typeof(DotLoading),
                new FrameworkPropertyMetadata(new Duration(TimeSpan.FromSeconds(4)), FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

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
        /// 点之间延迟时间
        /// </summary>
        public double DotDelayTime
        {
            get { return (double)GetValue(DotDelayTimeProperty); }
            set { SetValue(DotDelayTimeProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="DotDelayTime"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DotDelayTimeProperty =
            DependencyProperty.Register("DotDelayTime", typeof(double), typeof(DotLoading),
                new FrameworkPropertyMetadata(ValueBoxes.Double0Box, OnPropertiesValueChanged));

        /// <summary>
        /// 点是否匀速运动
        /// </summary>
        public bool IsDotRunAsConstant
        {
            get { return (bool)GetValue(IsDotRunAsConstantProperty); }
            set { SetValue(IsDotRunAsConstantProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 提供的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsDotRunAsConstantProperty =
            DependencyProperty.Register("IsDotRunAsConstant", typeof(bool), typeof(DotLoading),
                new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

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

        private Storyboard? storyboard;
        private Canvas canvas = new Canvas { ClipToBounds = true }; //绘图面板

        static DotLoading()
        {
            ContentProperty.OverrideMetadata(typeof(DotLoading), new FrameworkPropertyMetadata(typeof(DotLoading),
                FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(ContentValueChanged)));
        }

        private static void ContentValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dot = d as DotLoading;
            if (dot != null)
                dot.Content = dot.canvas;
        }

        public DotLoading()
        {
            Content = canvas;
            canvas.SetBinding(Canvas.BackgroundProperty, new Binding(BackgroundProperty.Name) { Source = this });
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            UpdateDotsAnimation();
        }

        /// <summary>
        /// 更新点动画
        /// </summary>
        private void UpdateDotsAnimation()
        {
            if (DotCount < 1) return;
            canvas.Children.Clear();

            //定义一个点动画
            storyboard = new Storyboard
            {
                RepeatBehavior = RepeatBehavior.Forever,
                SpeedRatio = DotRunSpeed,
            };

            if (IsDotRunAsConstant)
                ConstantSpeedRun();
            else
                UDSpeedRun();

            storyboard?.Begin();
            storyboard?.Freeze();

            if (IsDotRunning)
                storyboard?.Resume();
            else
                storyboard?.Pause();
        }

        /// <summary>
        /// 加速与减速运动
        /// </summary>
        private void UDSpeedRun()
        {
            var duration = DotDuration.TimeSpan.TotalSeconds;
            for (int i = 0; i < DotCount; i++)
            {
                var container = CreateContainer(i);
                var angle = -DotInterval * i / (2 * Math.PI * Width / 2) * 360;
                var frames = new DoubleAnimationUsingKeyFrames
                {
                    BeginTime = TimeSpan.FromMilliseconds(DotDelayTime * i),
                };

                var frame1 = new LinearDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                    Value = angle + 0,
                };

                //第一个180°上坡
                var frame2 = new EasingDoubleKeyFrame
                {
                    EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut },
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3 * duration)),
                    Value = angle + 180,
                };
                var frame3 = new LinearDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.35 * duration)),
                    Value = angle + 182,
                };

                //第一个180°下坡
                var frame4 = new EasingDoubleKeyFrame
                {
                    EasingFunction = new CircleEase { EasingMode = EasingMode.EaseIn },
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.5 * duration)),
                    Value = angle + 360,
                };

                //第二个180°上坡
                var frame5 = new EasingDoubleKeyFrame
                {
                    EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut },
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.8 * duration)),
                    Value = angle + 540,
                };
                var frame6 = new LinearDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.85 * duration)),
                    Value = angle + 542,
                };

                //第二个180°下坡
                var frame7 = new EasingDoubleKeyFrame
                {
                    EasingFunction = new CircleEase { EasingMode = EasingMode.EaseIn },
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1.0 * duration)),
                    Value = angle + 720,
                };

                frames.KeyFrames.Add(frame1);
                frames.KeyFrames.Add(frame2);
                frames.KeyFrames.Add(frame3);
                frames.KeyFrames.Add(frame4);
                frames.KeyFrames.Add(frame5);
                frames.KeyFrames.Add(frame6);
                frames.KeyFrames.Add(frame7);
                Storyboard.SetTarget(frames, container);
                Storyboard.SetTargetProperty(frames, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));
                storyboard?.Children.Add(frames);

                //控制Dot的显示与隐藏，模仿windows效果
                var frame8 = new DiscreteObjectKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration)),
                    Value = Visibility.Collapsed,
                };
                var frame9 = new DiscreteObjectKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                    Value = Visibility.Visible,
                };
                var framesVisibility = new ObjectAnimationUsingKeyFrames
                {
                    BeginTime = TimeSpan.FromMilliseconds(DotDelayTime * i)
                };
                framesVisibility.KeyFrames.Add(frame8);
                framesVisibility.KeyFrames.Add(frame9);
                Storyboard.SetTarget(framesVisibility, container);
                Storyboard.SetTargetProperty(framesVisibility, new PropertyPath("(UIElement.Visibility)"));
                storyboard?.Children.Add(framesVisibility);

                canvas.Children.Add(container);
            }
        }

        /// <summary>
        /// 匀速运动
        /// </summary>
        private void ConstantSpeedRun()
        {
            for (int i = 0; i < DotCount; i++)
            {
                var container = CreateContainer(i);
                var angle = -DotInterval * i / (2 * Math.PI * Width / 2) * 360;
                var frames = new DoubleAnimationUsingKeyFrames
                {
                    BeginTime = TimeSpan.Zero,
                };

                var frame1 = new LinearDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                    Value = angle + 0,
                };
                var frame2 = new LinearDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(DotDuration.TimeSpan.TotalSeconds)),
                    Value = angle + 360,
                };

                frames.KeyFrames.Add(frame1);
                frames.KeyFrames.Add(frame2);

                Storyboard.SetTarget(frames, container);
                Storyboard.SetTargetProperty(frames, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));
                storyboard?.Children.Add(frames);

                var framesVisibility = new ObjectAnimationUsingKeyFrames();
                var frame3 = new DiscreteObjectKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                    Value = Visibility.Visible,
                };
                framesVisibility.KeyFrames.Add(frame3);
                Storyboard.SetTarget(framesVisibility, container);
                Storyboard.SetTargetProperty(framesVisibility, new PropertyPath("(UIElement.Visibility)"));
                storyboard?.Children.Add(framesVisibility);

                canvas.Children.Add(container);
            }
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
            dot.VerticalAlignment = VerticalAlignment.Bottom;

            var border = new Border();
            RotateTransform rt = new RotateTransform() { Angle = -DotInterval * index / (2 * Math.PI * Width / 2) * 360 };
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(rt);
            border.RenderTransformOrigin = new Point(0.5, 0.5);
            border.RenderTransform = transformGroup;
            border.Child = dot;
            border.Visibility = Visibility.Collapsed; //隐藏Dot，由动画控制显示
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
            if (IsDotRadiusEqualScale)
            {
                ellipse.SetValue(Ellipse.WidthProperty, (index + 1D) / DotCount * DotRadius);
                ellipse.SetValue(Ellipse.HeightProperty, (index + 1D) / DotCount * DotRadius);
            }
            else
            {
                ellipse.SetBinding(Ellipse.WidthProperty, new Binding(DotRadiusProperty.Name) { Source = this });
                ellipse.SetBinding(Ellipse.HeightProperty, new Binding(DotRadiusProperty.Name) { Source = this });
            }
            ellipse.SetBinding(Shape.FillProperty, new Binding(ForegroundProperty.Name) { Source = this });
            ellipse.SetBinding(Shape.StrokeProperty, new Binding(DotStrokeProperty.Name) { Source = this });
            ellipse.SetBinding(Shape.StrokeThicknessProperty, new Binding(DotStrokeThicknessProperty.Name) { Source = this });

            return ellipse;
        }
    }
}
