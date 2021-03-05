using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Media.Animation;
using CookPopularControl.Tools.Boxes;
using System.Text.RegularExpressions;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-04 09:01:00
 */
namespace CookPopularControl.Controls.Animation.Loading
{
    /// <summary>
    /// 表示一组矩形按照圆的轨迹形成的动画
    /// </summary>
    public class RecCircleLoading : RecLoadingBase
    {
        /// <summary>
        /// Opacity是否从小到大变化
        /// </summary>
        /// <remarks>由小到大为false，恒定为true</remarks>
        public bool IsOpacityChanging
        {
            get { return (bool)GetValue(IsOpacityChangingProperty); }
            set { SetValue(IsOpacityChangingProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 提供<see cref="IsOpacityChanging"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsOpacityChangingProperty =
            DependencyProperty.Register("IsOpacityChanging", typeof(bool), typeof(RecCircleLoading),
                new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        protected override void PrepareRun()
        {
            for (int i = 0; i < RecCount; i++)
            {
                var container = CreateContainer(i);
                var angle = StartValue(i);
                var frames = new DoubleAnimationUsingKeyFrames
                {
                    BeginTime = TimeSpan.FromMilliseconds(RecDelayTime * i),
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

                if (IsOpacityChanging)
                    GetChangingFramesOpacity(container, i);

                recLoadingCanvas.Children.Add(container);
            }
        }

        private void GetChangingFramesOpacity(Border container, int index)
        {
            var framesOpacity = new DoubleAnimationUsingKeyFrames
            {
                BeginTime = TimeSpan.FromMilliseconds(RecDelayTime * index)
            };

            var frameIndex = new LinearDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                Value = Opacity - index * averageOpacity,
            };
            framesOpacity.KeyFrames.Add(frameIndex);

            for (int i = 0; i < RecCount; i++)
            {
                var frame = new LinearDoubleKeyFrame();
                frame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds((double)(i + 1) * duration / RecCount));
                frame.Value = (framesOpacity.KeyFrames[i].Value + averageOpacity) > Opacity ? 0 : framesOpacity.KeyFrames[i].Value + averageOpacity;
                framesOpacity.KeyFrames.Add(frame);
            }

            Storyboard.SetTarget(framesOpacity, container);
            Storyboard.SetTargetProperty(framesOpacity, new PropertyPath("(UIElement.Opacity)"));
            storyboard?.Children.Add(framesOpacity);
        }

        protected override Border CreateContainer(int index)
        {
            var rec = CreateRectangle(index);
            rec.HorizontalAlignment = HorizontalAlignment.Center;
            rec.VerticalAlignment = VerticalAlignment.Bottom;

            var border = new Border();
            TransformGroup transformGroup = new TransformGroup();
            RotateTransform rt = new RotateTransform() { Angle = StartValue(index) };
            transformGroup.Children.Add(rt);
            border.RenderTransformOrigin = new Point(0.5, 0.5);
            border.RenderTransform = transformGroup;
            border.Child = rec;
            border.Padding = new Thickness(0, 0, 0, RecWidth / 2D + 5);
            border.SetBinding(WidthProperty, new Binding(WidthProperty.Name) { Source = this });
            border.SetBinding(HeightProperty, new Binding(HeightProperty.Name) { Source = this });

            return border;
        }

        protected override double StartValue(int index) => -360 / RecCount * index;
    }
}
