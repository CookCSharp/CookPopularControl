using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Shapes;
using CookPopularControl.Tools.Boxes;
using System.Windows.Media.Animation;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-08 16:53:15
 */
namespace CookPopularControl.Controls.Animation.Loading
{
    /// <summary>
    /// 表示一个雨点轨迹动画
    /// </summary>
    public class RaindropLoading : LoadingBase
    {
        /// <summary>
        /// 颜色
        /// </summary>
        public Brush RaindropStroke
        {
            get { return (Brush)GetValue(RaindropStrokeProperty); }
            set { SetValue(RaindropStrokeProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="RaindropStroke"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty RaindropStrokeProperty =
            DependencyProperty.Register("RaindropStroke", typeof(Brush), typeof(RaindropLoading),
                new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        /// <summary>
        /// 厚度
        /// </summary>
        public double RaindropStrokeThickness
        {
            get { return (double)GetValue(RaindropStrokeThicknessProperty); }
            set { SetValue(RaindropStrokeThicknessProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="RaindropStrokeThickness"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty RaindropStrokeThicknessProperty =
            DependencyProperty.Register("RaindropStrokeThickness", typeof(double), typeof(RaindropLoading),
                new FrameworkPropertyMetadata(ValueBoxes.Double1Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));


        protected override void PrepareRun()
        {
            var container = CreateRaindrop();
            CreateFrames(container, "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)");
            CreateFrames(container, "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)");

            var framesOpacity = new DoubleAnimationUsingKeyFrames();
            var frame3 = new LinearDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                Value = 1,
            };
            var frame4 = new LinearDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(totalDuration / 2D)),
                Value = 0,
            };
            var frame5 = new LinearDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(totalDuration)),
                Value = 1,
            };
            framesOpacity.KeyFrames.Add(frame3);
            framesOpacity.KeyFrames.Add(frame4);
            framesOpacity.KeyFrames.Add(frame5);
            Storyboard.SetTarget(framesOpacity, container);
            Storyboard.SetTargetProperty(framesOpacity, new PropertyPath("(UIElement.Opacity)"));
            storyboard?.Children.Add(framesOpacity);

            RootGrid.Children.Add(container);
        }

        private void CreateFrames(Ellipse container, string path)
        {
            var frames = new DoubleAnimationUsingKeyFrames();

            var frame1 = new LinearDoubleKeyFrame
            {
                KeyTime = TimeSpan.FromSeconds(totalDuration / 2D),
                Value = 0,
            };
            var frame2 = new LinearDoubleKeyFrame
            {
                KeyTime = TimeSpan.FromSeconds(totalDuration),
                Value = 1,
            };

            frames.KeyFrames.Add(frame1);
            frames.KeyFrames.Add(frame2);

            Storyboard.SetTarget(frames, container);
            Storyboard.SetTargetProperty(frames, new PropertyPath(path));
            storyboard!.Children.Add(frames);
        }

        /// <summary>
        /// 创建雨点
        /// </summary>
        /// <returns>圆点</returns>
        protected virtual Ellipse CreateRaindrop()
        {
            var ellipse = new Ellipse();
            TransformGroup transformGroup = new TransformGroup();
            ScaleTransform st = new ScaleTransform { ScaleX = 1, ScaleY = 1 };
            transformGroup.Children.Add(st);
            ellipse.RenderTransformOrigin = new Point(0.5, 0.5);
            ellipse.RenderTransform = transformGroup;
            var minLength = Width > Height ? Height : Width;
            ellipse.Width = minLength;
            ellipse.Height = minLength;
            ellipse.SetBinding(Shape.FillProperty, new Binding(ForegroundProperty.Name) { Source = this });
            ellipse.SetBinding(Ellipse.StrokeProperty, new Binding(RaindropStrokeProperty.Name) { Source = this });
            ellipse.SetBinding(Ellipse.StrokeThicknessProperty, new Binding(RaindropStrokeThicknessProperty.Name) { Source = this });

            return ellipse;
        }
    }
}
