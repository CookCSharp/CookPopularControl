using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-08 11:48:51
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 表示一组点按照直线轨迹运动并附加有透明度变化的加载动画
    /// </summary>
    public class DotOpacityLoading : DotLoadingBase
    {
        protected override void ConstantSpeedRun()
        {
            for (int i = 0; i < DotCount; i++)
            {
                var container = CreateContainer(i);
                var frames = new DoubleAnimationUsingKeyFrames
                {
                    BeginTime = TimeSpan.FromMilliseconds(DotDelayTime * i),
                };

                var frame1 = new LinearDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                    Value = 1,
                };
                var frame2 = new LinearDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration)),
                    Value = 0.1,
                };

                frames.KeyFrames.Add(frame1);
                frames.KeyFrames.Add(frame2);

                Storyboard.SetTarget(frames, container);
                Storyboard.SetTargetProperty(frames, new PropertyPath("(UIElement.Opacity)"));
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
            ConstantSpeedRun();
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

        protected override double StartValue(int index) => (DotInterval + DotDiameter) * index;
    }
}
