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
using System.Windows.Media.Effects;
using System.Windows.Data;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-03 15:56:35
 */
namespace CookPopularControl.Controls.Animation.Loading
{
    /// <summary>
    /// 表示点按照钟摆运动的轨迹形成的动画
    /// </summary>
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
            double angle;
            double distance;
            for (int i = 0; i < DotCount; i++)
            {
                var container = CreateContainer(i);
                if (i == 0 || i == DotCount - 1)
                {
                    var frames = new DoubleAnimationUsingKeyFrames
                    {
                        BeginTime = TimeSpan.FromMilliseconds(DotDelayTime * i),
                    };

                    if (i == 0)
                        angle = -GetPendulumSwingAngle(this);
                    else
                        angle = GetPendulumSwingAngle(this);

                    if (angle.Equals(0))
                    {
                        if (i == 0)
                            distance = StartValue(i) + halfOfAllDotsLength + GetPendulumSlipDistance(this);
                        else
                            distance = StartValue(i) + halfOfAllDotsLength - GetPendulumSlipDistance(this);

                        GetFramesWhenAngleChanged(frames, i, distance, 1);
                    }
                    else
                        GetFramesWhenAngleChanged(frames, i, angle, 0);

                    Storyboard.SetTarget(frames, container);
                    if (!angle.Equals(0))
                        Storyboard.SetTargetProperty(frames, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));
                    else
                        Storyboard.SetTargetProperty(frames, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.X)"));
                    storyboard?.Children.Add(frames);
                }

                dotLoadingGrid?.Children.Add(container);
            }
        }

        protected override void UDSpeedRun()
        {
            double angle;
            double distance;
            for (int i = 0; i < DotCount; i++)
            {
                var container = CreateContainer(i);
                if (i == 0 || i == DotCount - 1)
                {
                    var frames = new DoubleAnimationUsingKeyFrames
                    {
                        BeginTime = TimeSpan.FromMilliseconds(DotDelayTime * i),
                    };

                    if (i == 0)
                        angle = -GetPendulumSwingAngle(this);
                    else
                        angle = GetPendulumSwingAngle(this);

                    if (angle.Equals(0))
                    {
                        if (i == 0)
                            distance = StartValue(i) + halfOfAllDotsLength + GetPendulumSlipDistance(this);
                        else
                            distance = StartValue(i) + halfOfAllDotsLength - GetPendulumSlipDistance(this);

                        GetFramesWhenAngleChanged(frames, i, distance, 1);
                    }
                    else
                        GetFramesWhenAngleChanged(frames, i, angle, 0);

                    Storyboard.SetTarget(frames, container);
                    if (!angle.Equals(0))
                        Storyboard.SetTargetProperty(frames, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));
                    else
                        Storyboard.SetTargetProperty(frames, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.X)"));
                    storyboard?.Children.Add(frames);
                }

                dotLoadingGrid?.Children.Add(container);
            }
        }

        /// <summary>
        /// 获取动画的<see cref="DoubleKeyFrame"/>对象的集合
        /// </summary>
        /// <param name="frames"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="flag">标志位,angle为0时值为1，angle不为0是，值为0</param>
        private void GetFramesWhenAngleChanged(DoubleAnimationUsingKeyFrames frames, int index, double value, int flag)
        {
            var frame1 = new LinearDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                Value = (StartValue(index) + halfOfAllDotsLength) * flag,
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
                Value = (StartValue(index) + halfOfAllDotsLength) * flag,
            };

            frames.KeyFrames.Add(frame1);
            frames.KeyFrames.Add(frame2);
            frames.KeyFrames.Add(frame3);
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
            border.SetBinding(VerticalAlignmentProperty, new Binding(VerticalContentAlignmentProperty.Name) { Source = this });
            border.SetBinding(HorizontalAlignmentProperty, new Binding(HorizontalContentAlignmentProperty.Name) { Source = this });
            //border.Effect = Application.Current.Resources["ShadowEffectDepth2"] as DropShadowEffect;

            return border;
        }

        protected override double StartValue(int index) => -(DotInterval + DotDiameter) * index;
    }
}
