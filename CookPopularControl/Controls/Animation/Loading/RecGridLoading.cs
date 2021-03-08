using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-05 21:15:28
 */
namespace CookPopularControl.Controls.Animation.Loading
{
    /// <summary>
    /// 表示一组矩形按照DataGrid的轨迹形成的动画
    /// </summary>
    public class RecGridLoading : RecLoadingBase
    {
        protected override void PrepareRun()
        {
            for (int i = 0; i <= 2 * RecCount; i++)
            {
                var containers = CreateContainers(i);

                foreach (var container in containers)
                {
                    CreateFrames(i, container, "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)");
                    CreateFrames(i, container, "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)");

                    recLoadingCanvas!.Children.Add(container);
                }
            }
        }

        private void CreateFrames(int index, Border container, string path)
        {
            var frames = new DoubleAnimationUsingKeyFrames
            {
                BeginTime = TimeSpan.FromMilliseconds(RecDelayTime * index),
            };

            var frame1 = new EasingDoubleKeyFrame
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn },
                KeyTime = TimeSpan.FromSeconds(duration / 2D),
                Value = 0.5,
            };
            var frame2 = new LinearDoubleKeyFrame
            {
                KeyTime = TimeSpan.FromSeconds(duration),
                Value = 0,
            };

            frames.KeyFrames.Add(frame1);
            frames.KeyFrames.Add(frame2);

            Storyboard.SetTarget(frames, container);
            Storyboard.SetTargetProperty(frames, new PropertyPath(path));
            storyboard!.Children.Add(frames);
        }

        private List<Border> CreateContainers(int index)
        {
            var containers = new List<Border>();
            var lps = CreateNN(index);
            for (int i = 0; i < lps.Count; i++)
            {
                containers.Add(CreateContainer(i, lps[i]));
            }

            return containers;
        }

        private Border CreateContainer(int index, Point p)
        {
            var rec = CreateRectangle(index);
            rec.HorizontalAlignment = HorizontalAlignment.Center;
            rec.VerticalAlignment = VerticalAlignment.Center;

            var border = new Border();
            TransformGroup transformGroup = new TransformGroup();
            ScaleTransform st = new ScaleTransform { ScaleX = 1, ScaleY = 1 };
            TranslateTransform tt = new TranslateTransform
            {
                X = StartValue((int)p.X) + p.X * rec.Width + moveXDistance,
                Y = StartValue((int)p.Y) + p.Y * rec.Height + moveYDistance,
            };
            transformGroup.Children.Add(st);
            transformGroup.Children.Add(tt);
            border.RenderTransformOrigin = new Point(0.5, 0.5);
            border.RenderTransform = transformGroup;
            border.Width = rec.Width;
            border.Height = rec.Height;
            border.Child = rec;

            return border;
        }

        [Obsolete("不使用该方法")]
        protected override Border CreateContainer(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 组建N*N的正方形点集合
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private List<Point> CreateNN(int index)
        {
            List<Point> lps = new List<Point>();
            for (int i = 0; i <= index; i++)
            {
                Point p = new Point();
                p.X = i;
                p.Y = index - i;

                if (p.X < RecCount && p.Y < RecCount)
                    lps.Add(p);
            }

            return lps;
        }

        protected override double StartValue(int index) => RecInterval * index;
    }
}
