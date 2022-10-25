using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Swiper
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-25 15:06:06
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 滑块视图容器
    /// </summary>
    [TemplatePart(Name = LastViewButton, Type = typeof(System.Windows.Controls.Button))]
    [TemplatePart(Name = NextViewButton, Type = typeof(System.Windows.Controls.Button))]
    [TemplatePart(Name = ContentViewTranslate, Type = typeof(TranslateTransform))]
    [TemplatePart(Name = ContentView, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = DotsPanelContainer, Type = typeof(StackPanel))]
    public class Swiper : ItemsControl
    {
        private const string LastViewButton = "PART_LastView";
        private const string NextViewButton = "PART_NextView";
        private const string ContentView = "PART_Content";
        private const string ContentViewTranslate = "PART_Translate";
        private const string DotsPanelContainer = "PART_Panel";

        private static readonly Duration DefaultDuration = new Duration(TimeSpan.FromMilliseconds(500));
        public static readonly ICommand LastViewCommand = new RoutedCommand(nameof(LastViewCommand), typeof(Swiper));
        public static readonly ICommand NextViewCommand = new RoutedCommand(nameof(NextViewCommand), typeof(Swiper));

        private System.Windows.Controls.Button lastButton;
        private System.Windows.Controls.Button nextButton;
        private ContentPresenter contentView;
        private TranslateTransform translateTransform;
        private StackPanel dotsPanel;
        private DispatcherTimer AutoPlayTimer;

        static Swiper()
        {
            CommandManager.RegisterClassCommandBinding(typeof(Swiper), new CommandBinding(LastViewCommand, Executed));
            CommandManager.RegisterClassCommandBinding(typeof(Swiper), new CommandBinding(NextViewCommand, Executed));
        }

        private static void Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var swiper = sender as Swiper;
            if (swiper.IsCyclePlay) //循环播放
            {
                if (e.Command == LastViewCommand)
                    swiper.CurrentIndex -= 1;
                else if (e.Command == NextViewCommand)
                    swiper.CurrentIndex += 1;
            }
            else  //不循环播放，到末端就停止
            {
                if (e.Command == LastViewCommand)
                    swiper.CurrentIndex -= 1;
                else if (e.Command == NextViewCommand)
                    swiper.CurrentIndex += 1;
            }
        }

        public Swiper()
        {
            this.Loaded += (s, e) =>
            {
                contentView = GetTemplateChild(ContentView) as ContentPresenter;
                lastButton = GetTemplateChild(LastViewButton) as System.Windows.Controls.Button;
                nextButton = GetTemplateChild(NextViewButton) as System.Windows.Controls.Button;
                dotsPanel = GetTemplateChild(DotsPanelContainer) as StackPanel;

                translateTransform = new TranslateTransform();
                contentView.RenderTransform = translateTransform;

                lastButton.Click += (s, e) => translateTransform.BeginAnimation(TranslateTransform.XProperty, CreateAnimation(-300D, Duration));
                nextButton.Click += (s, e) => translateTransform.BeginAnimation(TranslateTransform.XProperty, CreateAnimation(300D, Duration));

                if (!IsCyclePlay)
                {
                    lastButton.IsEnabled = false;
                    if (this.Items.Count.Equals(0))
                        nextButton.IsEnabled = false;
                }

                for (int i = 0; i < Items.Count; i++)
                {
                    dotsPanel.Children.Add(CreateDot());
                }
                (dotsPanel.Children[0] as RadioButton).IsChecked = true; //与CurrentIndex对应
            };

            this.Unloaded += (s, e) =>
            {
                dotsPanel.Children.Clear();
            };
        }

        private DoubleAnimation CreateAnimation(double from, Duration duration)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = from;
            doubleAnimation.To = 0;
            doubleAnimation.Duration = duration;

            return doubleAnimation;
        }

        private RadioButton CreateDot()
        {
            RadioButton rb = new RadioButton();
            rb.Content = null;
            rb.Width = 20;
            rb.Height = 20;
            rb.BorderBrush = IndicatorDotBrush;
            rb.Style = ResourceHelper.GetResource<Style>("FillFullyRadioButtonStyle");
            rb.Margin = new Thickness(10, 0, 10, 0);
            rb.Checked += (s, e) =>
            {
                rb.Background = IndicatorActiveDotBrush;
                CurrentIndex = dotsPanel.Children.IndexOf(rb);
            };
            rb.Unchecked += (s, e) => rb.Background = Brushes.Transparent;

            return rb;
        }


        /// <summary>
        /// 当前显示视图的索引
        /// </summary>
        public int CurrentIndex
        {
            get { return (int)GetValue(CurrentIndexProperty); }
            set { SetValue(CurrentIndexProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="CurrentIndex"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty CurrentIndexProperty =
            DependencyProperty.Register("CurrentIndex", typeof(int), typeof(Swiper),
                new PropertyMetadata(ValueBoxes.InterMinus1Box, OnCurrentIndexChanged));

        private static void OnCurrentIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var swiper = d as Swiper;
            if (swiper != null && swiper.IsLoaded)
            {
                if (swiper.lastButton == null || swiper.nextButton == null) return;

                if (swiper.CurrentIndex.Equals(-1))
                    swiper.CurrentIndex = swiper.Items.Count - 1;
                else if (swiper.CurrentIndex.Equals(swiper.Items.Count))
                    swiper.CurrentIndex = 0;

                swiper.lastButton.IsEnabled = true;
                swiper.nextButton.IsEnabled = true;

                if (!swiper.IsCyclePlay)
                {
                    if (swiper.CurrentIndex <= 0)
                        swiper.lastButton.IsEnabled = false;
                    else if (swiper.CurrentIndex >= swiper.Items.Count - 1)
                        swiper.nextButton.IsEnabled = false;
                }

                swiper.CurrentItem = swiper.Items[swiper.CurrentIndex];
                (swiper.dotsPanel.Children[swiper.CurrentIndex] as RadioButton).IsChecked = true;
            }
        }


        /// <summary>
        /// 当前显示的视图
        /// </summary>
        public object CurrentItem
        {
            get { return GetValue(CurrentItemProperty); }
            set { SetValue(CurrentItemProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="CurrentItem"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty CurrentItemProperty =
            DependencyProperty.Register("CurrentItem", typeof(object), typeof(Swiper), new PropertyMetadata(default));

        /// <summary>
        /// 是否循环播放
        /// </summary>
        public bool IsCyclePlay
        {
            get { return (bool)GetValue(IsCyclePlayProperty); }
            set { SetValue(IsCyclePlayProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="IsCyclePlay"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsCyclePlayProperty =
            DependencyProperty.Register("IsCyclePlay", typeof(bool), typeof(Swiper), new PropertyMetadata(ValueBoxes.TrueBox));

        /// <summary>
        /// 是否显示面板指示点
        /// </summary>
        public bool IsShowIndicatorDots
        {
            get { return (bool)GetValue(IsShowIndicatorDotsProperty); }
            set { SetValue(IsShowIndicatorDotsProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 标识<see cref="IsShowIndicatorDots"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsShowIndicatorDotsProperty =
            DependencyProperty.Register("IsShowIndicatorDots", typeof(bool), typeof(Swiper), new PropertyMetadata(ValueBoxes.TrueBox));

        /// <summary>
        /// 指示点颜色
        /// </summary>
        public Brush IndicatorDotBrush
        {
            get { return (Brush)GetValue(IndicatorDotBrushProperty); }
            set { SetValue(IndicatorDotBrushProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="IndicatorDotBrush"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IndicatorDotBrushProperty =
            DependencyProperty.Register("IndicatorDotBrush", typeof(Brush), typeof(Swiper), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 选中点的颜色
        /// </summary>
        public Brush IndicatorActiveDotBrush
        {
            get { return (Brush)GetValue(IndicatorActiveDotBrushProperty); }
            set { SetValue(IndicatorActiveDotBrushProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="IndicatorActiveDotBrush"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IndicatorActiveDotBrushProperty =
            DependencyProperty.Register("IndicatorActiveDotBrush", typeof(Brush), typeof(Swiper), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 是否自动切换
        /// </summary>
        public bool IsAutoPlay
        {
            get { return (bool)GetValue(IsAutoPlayProperty); }
            set { SetValue(IsAutoPlayProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="IsAutoPlay"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsAutoPlayProperty =
            DependencyProperty.Register("IsAutoPlay", typeof(bool), typeof(Swiper),
                new PropertyMetadata(ValueBoxes.FalseBox, OnIsAutoPlayChanged));

        private static void OnIsAutoPlayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var swiper = d as Swiper;
            if (swiper != null)
            {
                swiper.AutoPlayTimer = null;
                swiper.AutoPlayTimer = new DispatcherTimer(DispatcherPriority.Normal);
                swiper.AutoPlayTimer.Interval = TimeSpan.FromSeconds(swiper.Interval);
                swiper.AutoPlayTimer.Tick += (s, e) =>
                {
                    swiper.CurrentIndex += 1;
                    swiper.translateTransform?.BeginAnimation(TranslateTransform.XProperty, swiper.CreateAnimation(300D, new Duration(TimeSpan.FromSeconds(swiper.Interval))));
                };
                swiper.AutoPlayTimer.IsEnabled = (bool)e.NewValue;
            }
        }


        /// <summary>
        /// 自动切换间隔时长(s)
        /// </summary>
        public double Interval
        {
            get { return (double)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="Interval"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(double), typeof(Swiper),
                new PropertyMetadata(ValueBoxes.Double1Box, (d, e) => (d as Swiper).AutoPlayTimer.Interval = TimeSpan.FromSeconds((d as Swiper).Interval * 1.1D)));

        /// <summary>
        /// 滑动动画时长
        /// </summary>
        public Duration Duration
        {
            get { return (Duration)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="Duration"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(Duration), typeof(Swiper), new PropertyMetadata(DefaultDuration));
    }
}
