using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Data;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-03 15:53:06
 */
namespace CookPopularControl.Controls.Animation.Loading
{
    /// <summary>
    /// 表示点按照直线的运动轨迹形成的动画
    /// </summary>
    public class DotLineLoading : DotLoadingBase
    {
        protected override void ConstantSpeedRun()
        {
            for (int i = 0; i < DotCount; i++)
            {
                var container = CreateContainer(i);
                var x = StartValue(i) + halfOfAllDotsLength;
                var frames = new DoubleAnimationUsingKeyFrames
                {
                    BeginTime = TimeSpan.FromMilliseconds(DotDelayTime * i),
                };

                var frame1 = new LinearDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                    Value = 0 + x,
                };
                var frame2 = new LinearDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration)),
                    Value = Width + x,
                };

                frames.KeyFrames.Add(frame1);
                frames.KeyFrames.Add(frame2);

                Storyboard.SetTarget(frames, container);
                Storyboard.SetTargetProperty(frames, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
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

                dotLoadingGrid?.Children.Add(container);
            }
        }

        protected override void UDSpeedRun()
        {
            for (int i = 0; i < DotCount; i++)
            {
                var container = CreateContainer(i);
                var x = StartValue(i) + halfOfAllDotsLength;
                var frames = new DoubleAnimationUsingKeyFrames
                {
                    BeginTime = TimeSpan.FromMilliseconds(DotDelayTime * i),
                };

                var frame1 = new LinearDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                    Value = x + 0,
                };

                //运动到中间位置
                var frame2 = new EasingDoubleKeyFrame
                {
                    EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut },
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.6 * duration)),
                    Value = x + Width / 2D,
                };
                var frame3 = new LinearDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.68 * duration)),
                    Value = x + Width / 2D + dotoffset,
                };

                var frame4 = new EasingDoubleKeyFrame
                {
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn },
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration)),
                    Value = Width,
                };

                frames.KeyFrames.Add(frame1);
                frames.KeyFrames.Add(frame2);
                frames.KeyFrames.Add(frame3);
                frames.KeyFrames.Add(frame4);

                Storyboard.SetTarget(frames, container);
                Storyboard.SetTargetProperty(frames, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
                storyboard?.Children.Add(frames);

                var framesVisibility = new ObjectAnimationUsingKeyFrames
                {
                    BeginTime = TimeSpan.FromMilliseconds(DotDelayTime * i)
                };
                var frame5 = new DiscreteObjectKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                    Value = Visibility.Visible,
                };

                framesVisibility.KeyFrames.Add(frame5);
                Storyboard.SetTarget(framesVisibility, container);
                Storyboard.SetTargetProperty(framesVisibility, new PropertyPath("(UIElement.Visibility)"));
                storyboard?.Children.Add(framesVisibility);

                dotLoadingGrid?.Children.Add(container);
            }
        }

        protected override Border CreateContainer(int index)
        {
            var dot = CreateEllipse(index);
            var border = new Border();
            TransformGroup transformGroup = new TransformGroup();
            TranslateTransform tt = new TranslateTransform { X = StartValue(index) };
            transformGroup.Children.Add(tt);
            border.RenderTransform = transformGroup;
            border.Child = dot;
            border.Visibility = Visibility.Collapsed;
            border.Width = DotDiameter;
            border.Height = DotDiameter;
            border.SetBinding(VerticalAlignmentProperty, new Binding(VerticalContentAlignmentProperty.Name) { Source = this });
            border.SetBinding(HorizontalAlignmentProperty, new Binding(HorizontalContentAlignmentProperty.Name) { Source = this });

            return border;
        }

        protected override double StartValue(int index) => -(DotInterval + DotDiameter) * index;
    }
}
