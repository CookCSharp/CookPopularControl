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
 * Description：表示点动画的基类 
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-03 15:16:01
 */
namespace CookPopularControl.Controls.Animation.Loading
{
    /// <summary>
    /// 表示由点组成的动画的基类
    /// </summary>
    public abstract class DotLoadingBase : ContentControl
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
            DependencyProperty.Register("IsDotRunning", typeof(bool), typeof(DotLoadingBase),
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
            DependencyProperty.Register("DotCount", typeof(int), typeof(DotLoadingBase),
                new FrameworkPropertyMetadata(ValueBoxes.Inter5Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        /// <summary>
        /// 点的半径
        /// </summary>
        public double DotDiameter
        {
            get { return (double)GetValue(DotDiameterProperty); }
            set { SetValue(DotDiameterProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="DotDiameter"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DotDiameterProperty =
            DependencyProperty.Register("DotDiameter", typeof(double), typeof(DotLoadingBase),
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
            DependencyProperty.Register("IsDotRadiusEqualScale", typeof(bool), typeof(DotLoadingBase),
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
            DependencyProperty.Register("DotStroke", typeof(Brush), typeof(DotLoadingBase),
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
            DependencyProperty.Register("DotStrokeThickness", typeof(double), typeof(DotLoadingBase),
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
            DependencyProperty.Register("DotInterval", typeof(double), typeof(DotLoadingBase),
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
            DependencyProperty.Register("DotDuration", typeof(Duration), typeof(DotLoadingBase),
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
            DependencyProperty.Register("DotRunSpeed", typeof(double), typeof(DotLoadingBase),
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
            DependencyProperty.Register("DotDelayTime", typeof(double), typeof(DotLoadingBase),
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
            DependencyProperty.Register("IsDotRunAsConstant", typeof(bool), typeof(DotLoadingBase),
                new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));


        /// <summary>
        /// 依赖属性值改变时触发
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        protected static void OnPropertiesValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dotloading = d as DotLoadingBase;
            if (dotloading != null)
                dotloading.UpdateDotsAnimation();
        }

        protected Storyboard? storyboard; //动画
        protected const double dotoffset = 5D; //缓动距离
        protected double duration; //动画持续时长
        protected double halfOfAllDotsLength; //所有点长度的一半，为了使点排列在中间位置
        protected Canvas dotLoadingCanvas = new Canvas { ClipToBounds = true }; //绘图面板


        static DotLoadingBase()
        {
            ContentProperty.OverrideMetadata(typeof(DotLoadingBase), new FrameworkPropertyMetadata(typeof(DotLoadingBase),
                FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(ContentValueChanged)));
        }

        private static void ContentValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dot = d as DotLoadingBase;
            if (dot != null)
                dot.Content = dot.dotLoadingCanvas;
        }

        public DotLoadingBase()
        {
            Content = dotLoadingCanvas;
            dotLoadingCanvas.SetBinding(Canvas.BackgroundProperty, new Binding(BackgroundProperty.Name) { Source = this });
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
            dotLoadingCanvas.Children.Clear();

            duration = DotDuration.TimeSpan.TotalSeconds;
            halfOfAllDotsLength = (DotCount - 1) * DotInterval / 2D;


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
        /// 匀速运动
        /// </summary>
        /// <remarks><see cref="IsDotRunAsConstant"/>为True时</remarks>
        protected abstract void ConstantSpeedRun();

        /// <summary>
        /// 加速与减速运动
        /// </summary>
        /// <remarks><see cref="IsDotRunAsConstant"/>为False时</remarks>
        protected abstract void UDSpeedRun();

        /// <summary>
        /// 创建一个容器来做动画
        /// </summary>
        /// <param name="index"></param>
        /// <returns>容器</returns>
        protected abstract Border CreateContainer(int index);

        /// <summary>
        /// 创建点
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected virtual Ellipse CreateEllipse(int index)
        {
            var ellipse = new Ellipse();
            if (IsDotRadiusEqualScale)
            {
                ellipse.SetValue(Ellipse.WidthProperty, (index + 1D) / DotCount * DotDiameter);
                ellipse.SetValue(Ellipse.HeightProperty, (index + 1D) / DotCount * DotDiameter);
            }
            else
            {
                ellipse.SetBinding(Ellipse.WidthProperty, new Binding(DotDiameterProperty.Name) { Source = this });
                ellipse.SetBinding(Ellipse.HeightProperty, new Binding(DotDiameterProperty.Name) { Source = this });
            }
            ellipse.SetBinding(Shape.FillProperty, new Binding(ForegroundProperty.Name) { Source = this });
            ellipse.SetBinding(Shape.StrokeProperty, new Binding(DotStrokeProperty.Name) { Source = this });
            ellipse.SetBinding(Shape.StrokeThicknessProperty, new Binding(DotStrokeThicknessProperty.Name) { Source = this });

            return ellipse;
        }

        /// <summary>
        /// 初始值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected abstract double StartValue(int index);
    }
}
