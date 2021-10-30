using CookPopularControl.Communal;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-05 14:45:14
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 表示一组矩形按照音调高低的轨迹进行运动
    /// </summary>
    /// <remarks>适合音乐播放、频率变化</remarks>
    public class RecColumnLoading : RecLoadingBase
    {
        protected override void PrepareRun()
        {
            for (int i = 0; i < RecCount; i++)
            {
                var container = CreateContainer(i);
                var frames = new DoubleAnimationUsingKeyFrames
                {
                    BeginTime = TimeSpan.FromMilliseconds(RecDelayTime * i),
                };

                var frame1 = new LinearDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                    Value = 0.5,
                };
                var frame2 = new LinearDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration / 2D)),
                    Value = 1,
                };
                var frame3 = new LinearDoubleKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(duration)),
                    Value = 0.5,
                };

                frames.KeyFrames.Add(frame1);
                frames.KeyFrames.Add(frame2);
                frames.KeyFrames.Add(frame3);

                Storyboard.SetTarget(frames, container);
                Storyboard.SetTargetProperty(frames, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(ScaleTransform.ScaleY)"));
                storyboard?.Children.Add(frames);

                recLoadingCanvas?.Children.Add(container);
            }
        }

        protected override Border CreateContainer(int index)
        {
            var rec = CreateRectangle(index);
            rec.HorizontalAlignment = HorizontalAlignment.Center;
            rec.VerticalAlignment = VerticalAlignment.Center;

            var border = new Border();
            TransformGroup transformGroup = new TransformGroup();
            TranslateTransform rt = new TranslateTransform { X = StartValue(index) + index * rec.Width + moveXDistance };
            ScaleTransform st = new ScaleTransform { ScaleY = 0.5 };
            transformGroup.Children.Add(rt);
            transformGroup.Children.Add(st);
            border.RenderTransformOrigin = new Point(0.5, 0.5);
            border.RenderTransform = transformGroup;
            border.Child = rec;
            border.Width = rec.Width;
            border.Height = Height;

            return border;
        }

        protected override Rectangle CreateRectangle(int index)
        {
            var rec = new Rectangle();
            rec.Width = RecWidth;
            rec.Height = RecHeight;
            rec.Fill = Foreground;
            rec.Stroke = BorderBrush;
            rec.StrokeThickness = BorderThickness.Left;
            rec.RadiusX = FrameworkElementBaseAttached.GetCornerRadius(this).TopLeft;
            rec.RadiusY = FrameworkElementBaseAttached.GetCornerRadius(this).TopLeft;

            return rec;
        }

        protected override double StartValue(int index) => RecInterval * index;
    }
}
