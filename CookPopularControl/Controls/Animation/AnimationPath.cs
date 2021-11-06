using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-02-24 09:49:09
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 提供<see cref="Geometry"/>的路径动画控件
    /// </summary>
    public class AnimationPath : Shape
    {
        /// <summary>
        /// 动画面板
        /// </summary>
        private Storyboard? _storyboard;

        /// <summary>
        /// 动画路径长度
        /// </summary>
        private double _pathLength;

        static AnimationPath()
        {
            StretchProperty.AddOwner(typeof(AnimationPath), new FrameworkPropertyMetadata(Stretch.Uniform, OnValuePropertyChanged));
            StrokeThicknessProperty.AddOwner(typeof(AnimationPath), new FrameworkPropertyMetadata(ValueBoxes.Double1Box, OnValuePropertyChanged));
        }

        public AnimationPath() => Loaded += (s, e) => UpdatePath();

        public Geometry Data
        {
            get { return (Geometry)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(Geometry), typeof(AnimationPath),
                new FrameworkPropertyMetadata(default(Geometry), OnValuePropertyChanged));

        protected override Geometry DefiningGeometry => Data ?? Geometry.Empty;

        /// <summary>
        /// Geometry路径长度
        /// </summary>
        public double PathLength
        {
            get { return (double)GetValue(PathLengthProperty); }
            set { SetValue(PathLengthProperty, value); }
        }
        public static readonly DependencyProperty PathLengthProperty =
            DependencyProperty.Register("PathLength", typeof(double), typeof(AnimationPath),
                new FrameworkPropertyMetadata(ValueBoxes.Double0Box, OnValuePropertyChanged));

        /// <summary>
        /// 动画持续时间
        /// </summary>
        public Duration Duration
        {
            get { return (Duration)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(Duration), typeof(AnimationPath),
                new FrameworkPropertyMetadata(new Duration(TimeSpan.FromSeconds(2)), OnValuePropertyChanged));

        /// <summary>
        /// 指定动画的重复行为
        /// </summary>
        public RepeatBehavior RepeatBehavior
        {
            get { return (RepeatBehavior)GetValue(RepeatBehaviorProperty); }
            set { SetValue(RepeatBehaviorProperty, value); }
        }
        public static readonly DependencyProperty RepeatBehaviorProperty =
            DependencyProperty.Register("RepeatBehavior", typeof(RepeatBehavior), typeof(AnimationPath),
                new FrameworkPropertyMetadata(RepeatBehavior.Forever, OnValuePropertyChanged));

        /// <summary>
        /// 动画是否播放
        /// </summary>
        public bool IsPlay
        {
            get { return (bool)GetValue(IsPlayProperty); }
            set { SetValue(IsPlayProperty, value.BooleanBox()); }
        }
        public static readonly DependencyProperty IsPlayProperty =
            DependencyProperty.Register("IsPlay", typeof(bool), typeof(AnimationPath),
                new FrameworkPropertyMetadata(ValueBoxes.TrueBox, OnIsPlayValueChanged));
        private static void OnIsPlayValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var gif = d as AnimationPath;
            if ((bool)e.NewValue)
                gif?.UpdatePath();
            else
                gif?._storyboard?.Pause();
        }


        [Description("动画完成时发生")]
        public event EventHandler Completed
        {
            add { this.AddHandler(CompletedEvent, value); }
            remove { this.RemoveHandler(CompletedEvent, value); }
        }
        public static readonly RoutedEvent CompletedEvent =
            EventManager.RegisterRoutedEvent("Completed", RoutingStrategy.Bubble, typeof(EventHandler), typeof(AnimationPath));

        protected virtual void OnCompleted(string oldString, string newString)
        {
            RoutedPropertyChangedEventArgs<string> arg = new RoutedPropertyChangedEventArgs<string>(oldString, newString, CompletedEvent);
            this.RaiseEvent(arg);
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimationPath gif)
            {
                gif.UpdatePath();
            }
        }


        private void UpdatePath()
        {
            if (!Duration.HasTimeSpan || !IsPlay) return;

            _pathLength = PathLength > 0 ? PathLength : Data.GetTotalLength(new Size(ActualWidth, ActualHeight), StrokeThickness);

            if (MathHelper.IsVerySmall(_pathLength)) return;

            StrokeDashOffset = _pathLength;
            StrokeDashArray = new DoubleCollection(new List<double> { _pathLength, _pathLength });

            if (_storyboard != null)
            {
                _storyboard.Stop();
                _storyboard.Completed -= Storyboard_Completed;
            }
            _storyboard = new Storyboard
            {
                RepeatBehavior = RepeatBehavior,
            };
            _storyboard.Completed += Storyboard_Completed;

            var frames = new DoubleAnimationUsingKeyFrames();
            //开始
            var frame1 = new LinearDoubleKeyFrame()
            {
                Value = _pathLength,
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
            };
            //结束
            var frame2 = new LinearDoubleKeyFrame()
            {
                Value = -_pathLength,
                KeyTime = KeyTime.FromTimeSpan(Duration.TimeSpan),
            };

            frames.KeyFrames.Add(frame1);
            frames.KeyFrames.Add(frame2);

            Storyboard.SetTarget(frames, this);
            Storyboard.SetTargetProperty(frames, new PropertyPath(StrokeDashOffsetProperty));
            _storyboard.Children.Add(frames);

            _storyboard.Begin();
        }

        private void Storyboard_Completed(object sender, EventArgs e) => RaiseEvent(new RoutedEventArgs(CompletedEvent));
    }
}
