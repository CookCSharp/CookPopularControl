using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media.Animation;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-09 09:48:45
 */
namespace CookPopularControl.Controls.Animation.Loading.SimpleLoadings
{
    /// <summary>
    /// 表示像手环一样的动画
    /// </summary>
    public class BraceletLoading : LoadingBase
    {
        protected override void PrepareRun()
        {
            var container = CreateContainer(5 * length / 6D);
            var frames = new DoubleAnimationUsingKeyFrames();
            var frame1 = new LinearDoubleKeyFrame
            {
                KeyTime = TimeSpan.Zero,
                Value = 0,
            };
            var frame2 = new LinearDoubleKeyFrame
            {
                KeyTime = TimeSpan.FromSeconds(totalDuration),
                Value = 360,
            };
            frames.KeyFrames.Add(frame1);
            frames.KeyFrames.Add(frame2);
            Storyboard.SetTarget(frames, container);
            Storyboard.SetTargetProperty(frames, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));
            storyboard?.Children.Add(frames);

            RootGrid.Children.Add(container);
        }

        private Border CreateContainer(double min)
        {
            var border = new Border();
            TransformGroup transformGroup = new TransformGroup();
            RotateTransform rt = new RotateTransform { Angle = 0 };
            transformGroup.Children.Add(rt);
            border.RenderTransformOrigin = new Point(0.5, 0.5);
            border.RenderTransform = transformGroup;
            border.BorderBrush = BorderBrush;
            border.BorderThickness = BorderThickness;
            border.Width = min;
            border.Height = min;
            border.CornerRadius = new CornerRadius(min);
            border.Child = CreateEllipse(min);

            return border;
        }

        private Ellipse CreateEllipse(double min)
        {
            var ellipse = new Ellipse();
            ellipse.HorizontalAlignment = HorizontalAlignment.Center;
            ellipse.VerticalAlignment = VerticalAlignment.Bottom;
            TransformGroup transformGroup = new TransformGroup();
            RotateTransform rt = new RotateTransform { Angle = 0 };
            TranslateTransform tt = new TranslateTransform { Y = min / 12D };
            transformGroup.Children.Add(rt);
            transformGroup.Children.Add(tt);
            ellipse.RenderTransformOrigin = new Point(0.5, 0.5);
            ellipse.RenderTransform = transformGroup;
            ellipse.Fill = Foreground;
            ellipse.Width = min / 6D;
            ellipse.Height = min / 6D;

            return ellipse;
        }
    }
}
