using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;
using CookPopularControl.Tools.Boxes;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-03 15:56:35
 */
namespace CookPopularControl.Controls.Animation.Loading
{
    public class DotPendulumLoading : DotLoadingBase
    {
        public static double GetPendulumSwingAngle(DependencyObject obj) => (double)obj.GetValue(PendulumSwingAngleProperty);
        public static void SetPendulumSwingAngle(DependencyObject obj, double value) => obj.SetValue(PendulumSwingAngleProperty, value);
        /// <summary>
        /// 钟摆的摆幅
        /// </summary>
        public static readonly DependencyProperty PendulumSwingAngleProperty =
            DependencyProperty.RegisterAttached("PendulumSwingAngle", typeof(double), typeof(DotPendulumLoading),
                new FrameworkPropertyMetadata(ValueBoxes.Double30Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        public static double GetPendulumSlipDistance(DependencyObject obj) => (double)obj.GetValue(PendulumSlipDistanceProperty);
        public static void SetPendulumSlipDistance(DependencyObject obj, double value) => obj.SetValue(PendulumSlipDistanceProperty, value);
        /// <summary>
        /// 钟摆的滑动距离
        /// </summary>
        public static readonly DependencyProperty PendulumSlipDistanceProperty =
            DependencyProperty.RegisterAttached("PendulumSlipDistance", typeof(double), typeof(DotPendulumLoading), 
                new FrameworkPropertyMetadata(ValueBoxes.Double10Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        protected override void ConstantSpeedRun()
        {
            throw new NotImplementedException();
        }

        protected override void UDSpeedRun()
        {
            double angle;
            double distance;
            double value;
            for (int i = 0; i < DotCount; i++)
            {
                var container = CreateContainer(i);
                if (i == 0 || i == DotCount - 1)
                {
                    if (i == 0)
                        angle = -GetPendulumSwingAngle(this);
                    else
                        angle = GetPendulumSwingAngle(this);

                    value = angle;

                    if (angle.Equals(0))
                    {
                        if (i == 0)
                            distance = GetPendulumSlipDistance(this) + StartValue(i);
                        else
                            distance = -GetPendulumSlipDistance(this) + StartValue(i);

                        value = distance;
                    }

                    var frames = new DoubleAnimationUsingKeyFrames
                    {
                        BeginTime = TimeSpan.FromMilliseconds(DotDelayTime * i),
                    };

                    var frame1 = new LinearDoubleKeyFrame
                    {
                        KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                        Value = 0,
                    };
                    var frame2 = new EasingDoubleKeyFrame
                    {
                        EasingFunction = new CircleEase { EasingMode = EasingMode.EaseOut },
                        KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.65 * duration)),
                        Value = value,
                    };
                    var frame3 = new EasingDoubleKeyFrame
                    {
                        EasingFunction = new CircleEase { EasingMode = EasingMode.EaseIn },
                        KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration)),
                        Value = 0,
                    };

                    frames.KeyFrames.Add(frame1);
                    frames.KeyFrames.Add(frame2);
                    frames.KeyFrames.Add(frame3);

                    storyboard!.AutoReverse = true;
                    Storyboard.SetTarget(frames, container);
                    if (!angle.Equals(0))
                        Storyboard.SetTargetProperty(frames, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));
                    else
                        Storyboard.SetTargetProperty(frames, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.X)"));
                    storyboard?.Children.Add(frames);
                }

                dotLoadingCanvas?.Children.Add(container);
            }
        }

        protected override Border CreateContainer(int index)
        {
            var dot = CreateEllipse(index);
            dot.HorizontalAlignment = HorizontalAlignment.Center;
            dot.VerticalAlignment = VerticalAlignment.Bottom;
            var border = new Border();
            TransformGroup transformGroup = new TransformGroup();
            RotateTransform rt = new RotateTransform();
            TranslateTransform tt = new TranslateTransform() { X = StartValue(index) + halfOfAllDotsLength };
            transformGroup.Children.Add(rt);
            transformGroup.Children.Add(tt);
            if (index.Equals(0) || index.Equals(DotCount - 1))
            {
                rt.CenterX = Width / 2D;
                rt.CenterY = Height / 2D;
            }
            border.RenderTransform = transformGroup;
            border.Child = dot;
            border.Width = Width;
            border.Height = Height;

            return border;
        }

        protected override double StartValue(int index) => -DotInterval * index;
    }
}
