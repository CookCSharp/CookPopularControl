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
 * Create Time：2021-03-03 15:38:18
 */
namespace CookPopularControl.Controls.Animation.Loading
{
    /// <summary>
    /// 表示点按照圆的运动轨迹形成的动画
    /// </summary>
    public class DotCircleLoading : DotLoadingBase
    {
        protected override void ConstantSpeedRun()
        {
            for (int i = 0; i < DotCount; i++)
            {
                var container = CreateContainer(i);
                var angle = StartValue(i);
                var frames = new DoubleAnimationUsingKeyFrames
                {
                    BeginTime = TimeSpan.FromMilliseconds(DotDelayTime * i),
                };

                var frame1 = new LinearDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                    Value = angle + 0,
                };
                var frame2 = new LinearDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration)),
                    Value = angle + 360,
                };

                frames.KeyFrames.Add(frame1);
                frames.KeyFrames.Add(frame2);

                Storyboard.SetTarget(frames, container);
                Storyboard.SetTargetProperty(frames, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));
                storyboard?.Children.Add(frames);

                var framesVisibility = new ObjectAnimationUsingKeyFrames
                {
                    BeginTime = TimeSpan.FromMilliseconds(DotDelayTime * i)
                };
                var frame3 = new DiscreteObjectKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                    Value = Visibility.Visible,
                };
                framesVisibility.KeyFrames.Add(frame3);
                Storyboard.SetTarget(framesVisibility, container);
                Storyboard.SetTargetProperty(framesVisibility, new PropertyPath("(UIElement.Visibility)"));
                storyboard?.Children.Add(framesVisibility);

                dotLoadingGrid.Children.Add(container);
            }
        }

        protected override void UDSpeedRun()
        {
            for (int i = 0; i < DotCount; i++)
            {
                var container = CreateContainer(i);
                var angle = StartValue(i);
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
                    Value = angle + 180 + dotoffset,
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
                    Value = angle + 540 + dotoffset,
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
                var framesVisibility = new ObjectAnimationUsingKeyFrames
                {
                    BeginTime = TimeSpan.FromMilliseconds(DotDelayTime * i)
                };
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
                framesVisibility.KeyFrames.Add(frame8);
                framesVisibility.KeyFrames.Add(frame9);
                Storyboard.SetTarget(framesVisibility, container);
                Storyboard.SetTargetProperty(framesVisibility, new PropertyPath("(UIElement.Visibility)"));
                storyboard?.Children.Add(framesVisibility);

                dotLoadingGrid.Children.Add(container);
            }
        }

        protected override Border CreateContainer(int index)
        {
            var dot = CreateEllipse(index);
            dot.HorizontalAlignment = HorizontalAlignment.Center;
            dot.VerticalAlignment = VerticalAlignment.Bottom;

            var border = new Border();
            TransformGroup transformGroup = new TransformGroup();
            RotateTransform rt = new RotateTransform() { Angle = StartValue(index) };
            transformGroup.Children.Add(rt);
            border.RenderTransformOrigin = new Point(0.5, 0.5);
            border.RenderTransform = transformGroup;
            border.Child = dot;
            border.Visibility = Visibility.Collapsed; //隐藏Dot，由动画控制显示
            var minLength = Width > Height ? Height : Width;
            border.Width = minLength;
            border.Height = minLength;
            border.SetBinding(VerticalAlignmentProperty, new Binding(VerticalContentAlignmentProperty.Name) { Source = this });
            border.SetBinding(HorizontalAlignmentProperty, new Binding(HorizontalContentAlignmentProperty.Name) { Source = this });

            return border;
        }

        protected override double StartValue(int index) => -(DotInterval * index + (index + 1) * DotDiameter) * averageUnitAngle;
    }
}
