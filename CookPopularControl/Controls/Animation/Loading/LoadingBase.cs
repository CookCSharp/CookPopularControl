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


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-08 16:39:33
 */
namespace CookPopularControl.Controls.Animation.Loading
{
    /// <summary>
    /// 简单动画的基类
    /// </summary>
    public abstract class LoadingBase : ContentControl
    {
        /// <summary>
        /// 是否正在运动
        /// </summary>
        public bool IsRunning
        {
            get { return (bool)GetValue(IsRunningProperty); }
            set { SetValue(IsRunningProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 提供<see cref="IsRunning"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsRunningProperty =
            DependencyProperty.Register("IsRunning", typeof(bool), typeof(LoadingBase),
                new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        /// <summary>
        /// 动画持续时长
        /// </summary>
        public Duration Duration
        {
            get { return (Duration)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="Duration"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(Duration), typeof(LoadingBase), 
                new FrameworkPropertyMetadata(new Duration(TimeSpan.FromSeconds(2)), FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        /// <summary>
        /// 速率
        /// </summary>
        public double RunSpeed
        {
            get { return (double)GetValue(RunSpeedProperty); }
            set { SetValue(RunSpeedProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="RunSpeed"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty RunSpeedProperty =
            DependencyProperty.Register("RunSpeed", typeof(double), typeof(LoadingBase),
                new FrameworkPropertyMetadata(ValueBoxes.Double1Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        protected static void OnPropertiesValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var loading = d as LoadingBase;
            if (loading != null)
                loading.UpdateLoadingAnimation();
        }

        protected Storyboard storyboard; 
        protected Grid RootGrid = new Grid { ClipToBounds = true };
        protected double totalDuration;


        static LoadingBase()
        {
            ContentProperty.OverrideMetadata(typeof(LoadingBase), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ContentValueChanged)));
        }

        private static void ContentValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var loading = d as LoadingBase;
            if (loading != null)
                loading.Content = loading.RootGrid;
        }

        public LoadingBase()
        {
            this.AddChild(RootGrid);
            RootGrid.SetBinding(WidthProperty, new Binding(WidthProperty.Name) { Source = this });
            RootGrid.SetBinding(HeightProperty, new Binding(HeightProperty.Name) { Source = this });
            RootGrid.SetBinding(BackgroundProperty, new Binding(BackgroundProperty.Name) { Source = this });
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            UpdateLoadingAnimation();
        }

        private void UpdateLoadingAnimation()
        {
            RootGrid?.Children.Clear();

            totalDuration = Duration.TimeSpan.TotalSeconds;

            //定义一个点动画
            storyboard = new Storyboard
            {
                RepeatBehavior = RepeatBehavior.Forever,
                SpeedRatio = RunSpeed,
            };

            PrepareRun();

            storyboard?.Begin();
            storyboard?.Freeze();

            if (IsRunning)
                storyboard?.Resume();
            else
                storyboard?.Pause();
        }

        /// <summary>
        /// 准备图形
        /// </summary>
        protected abstract void PrepareRun();
    }
}
