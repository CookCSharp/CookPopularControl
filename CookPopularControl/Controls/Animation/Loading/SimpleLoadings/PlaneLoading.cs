using CookPopularControl.Controls;
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
 * Create Time：2021-03-09 11:14:27
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 表示飞机飞行的动画
    /// </summary>
    public class PlaneLoading : LoadingBase
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        public string PlaneText
        {
            get { return (string)GetValue(PlaneTextProperty); }
            set { SetValue(PlaneTextProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="PlaneText"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty PlaneTextProperty =
            DependencyProperty.Register("PlaneText", typeof(string), typeof(PlaneLoading),
                new FrameworkPropertyMetadata("Loading...", FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));


        protected override void PrepareRun()
        {
            CreateText();

            var container = CreaterContainer(length);
            var frames = new DoubleAnimationUsingKeyFrames
            {
                BeginTime = TimeSpan.Zero,
            };

            var frame1 = new LinearDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                Value = 0,
            };
            var frame2 = new LinearDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(totalDuration)),
                Value = 360,
            };

            frames.KeyFrames.Add(frame1);
            frames.KeyFrames.Add(frame2);

            Storyboard.SetTarget(frames, container);
            Storyboard.SetTargetProperty(frames, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(RotateTransform.Angle)"));
            storyboard?.Children.Add(frames);

            RootGrid.Children.Add(container);
        }

        private Border CreaterContainer(double length)
        {
            var border = new Border();
            TransformGroup transformGroup = new TransformGroup();
            RotateTransform rt = new RotateTransform { Angle = 0 };
            transformGroup.Children.Add(rt);
            border.RenderTransformOrigin = new Point(0.5, 0.5);
            border.RenderTransform = transformGroup;
            border.BorderBrush = BorderBrush;
            border.BorderThickness = BorderThickness;
            border.Width = 0.8 * length;
            border.Height = 0.8 * length;
            border.CornerRadius = new CornerRadius(length);

            SimpleGrid grid = new SimpleGrid();

            var plane1 = CreatePlane(length);
            plane1.HorizontalAlignment = HorizontalAlignment.Center;
            plane1.VerticalAlignment = VerticalAlignment.Top;
            TransformGroup transformGroupPlane1 = new TransformGroup();
            TranslateTransform ttPlane1 = new TranslateTransform { Y = -0.09 * length };
            transformGroupPlane1.Children.Add(ttPlane1);
            plane1.RenderTransformOrigin = new Point(0.5, 0.5);
            plane1.RenderTransform = transformGroupPlane1;
            grid.Children.Add(plane1);

            var plane2 = CreatePlane(length);
            plane2.HorizontalAlignment = HorizontalAlignment.Center;
            plane2.VerticalAlignment = VerticalAlignment.Bottom;
            TransformGroup transformGroupPlane2 = new TransformGroup();
            RotateTransform rtPlane2 = new RotateTransform { Angle = 180 };
            TranslateTransform ttPlane2 = new TranslateTransform { Y = 0.09 * length };
            transformGroupPlane2.Children.Add(rtPlane2);
            transformGroupPlane2.Children.Add(ttPlane2);
            plane2.RenderTransformOrigin = new Point(0.5, 0.5);
            plane2.RenderTransform = transformGroupPlane2;
            grid.Children.Add(plane2);

            border.Child = grid;

            return border;
        }

        private Path CreatePlane(double length)
        {
            Path path = new Path();
            path.Width = length / 5D;
            path.Height = length / 5D;
            path.Stretch = Stretch.Uniform;
            path.Data = Application.Current.Resources["PlaneGeometry"] as PathGeometry;
            path.Fill = Foreground;
            path.Stroke = BorderBrush;
            path.StrokeThickness = BorderThickness.Left;

            return path;
        }

        private void CreateText()
        {
            TextBlock tb = new TextBlock();
            tb.Text = PlaneText;
            tb.FontSize = FontSize;
            tb.FontFamily = FontFamily;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.VerticalAlignment = VerticalAlignment.Center;
            RootGrid.Children.Add(tb);
        }
    }
}
