using CookPopularControl.Tools.Boxes;
using CookPopularControl.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-08 18:10:45
 */
namespace CookPopularControl.Controls.Animation.Loading
{
    /// <summary>
    /// 表示简单的Loading...文字
    /// </summary>
    public class CharacterLoading : LoadingBase
    {
        /// <summary>
        /// 点(省略号)的直径
        /// </summary>
        public double DotDiameter
        {
            get { return (double)GetValue(DotDiameterProperty); }
            set { SetValue(DotDiameterProperty, value); }
        }
        /// <summary>
        /// 提供的依赖属性
        /// </summary>
        public static readonly DependencyProperty DotDiameterProperty =
            DependencyProperty.Register("DotDiameter", typeof(double), typeof(CharacterLoading),
                new FrameworkPropertyMetadata(ValueBoxes.Double10Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        /// <summary>
        /// 点(省略号)的数量
        /// </summary>
        public int DotCount
        {
            get { return (int)GetValue(DotCountProperty); }
            set { SetValue(DotCountProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="DotCount"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DotCountProperty =
            DependencyProperty.Register("DotCount", typeof(int), typeof(CharacterLoading),
                new FrameworkPropertyMetadata(ValueBoxes.Inter5Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));
        
        /// <summary>
        /// 文本内容
        /// </summary>
        public string CharacterContent
        {
            get { return (string)GetValue(CharacterContentProperty); }
            set { SetValue(CharacterContentProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="CharacterContent"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty CharacterContentProperty =
            DependencyProperty.Register("CharacterContent", typeof(string), typeof(CharacterLoading),
                new FrameworkPropertyMetadata("Loading", FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesValueChanged));

        private double CharacterWidth;

        protected override void PrepareRun()
        {
            AddText();

            for (int i = 0; i < DotCount; i++)
            {
                var dot = CreateEllipse(i);

                var framesOpacity = new ObjectAnimationUsingKeyFrames
                {
                    BeginTime = TimeSpan.FromMilliseconds(200 * i),
                };
                var frame1 = new DiscreteObjectKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.Zero),
                    Value = Visibility.Visible,
                };
                var frame2 = new DiscreteObjectKeyFrame
                {
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(totalDuration)),
                    Value = Visibility.Collapsed,
                };
                framesOpacity.KeyFrames.Add(frame1);
                framesOpacity.KeyFrames.Add(frame2);
                Storyboard.SetTarget(framesOpacity, dot);
                Storyboard.SetTargetProperty(framesOpacity, new PropertyPath("(UIElement.Visibility)"));
                storyboard?.Children.Add(framesOpacity);

                RootGrid?.Children.Add(dot);
            }
        }

        private void AddText()
        {
            TextBlock tb = new TextBlock();
            tb.Text = CharacterContent;
            tb.FontSize = FontSize;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;

            CharacterWidth = FontHelper.GetFontWidthHeight(tb,CharacterContent,FontFamily.Source,FontSize).width + 10;
            RootGrid?.Children.Add(tb);
        }

        /// <summary>
        /// 创建点
        /// </summary>
        /// <returns>圆点</returns>
        private Ellipse CreateEllipse(int index)
        {
            var ellipse = new Ellipse();
            TransformGroup transformGroup = new TransformGroup();
            ScaleTransform st = new ScaleTransform { ScaleX = 1, ScaleY = 1 };
            TranslateTransform tt = new TranslateTransform { X = StartValue(index) };
            transformGroup.Children.Add(st);
            transformGroup.Children.Add(tt);
            ellipse.RenderTransformOrigin = new Point(0.5, 0.5);
            ellipse.RenderTransform = transformGroup;
            ellipse.Width = DotDiameter;
            ellipse.Height = DotDiameter;
            ellipse.Visibility = Visibility.Collapsed;
            ellipse.SetBinding(Shape.FillProperty, new Binding(ForegroundProperty.Name) { Source = this });
            ellipse.SetBinding(Ellipse.HorizontalAlignmentProperty, new Binding(HorizontalContentAlignmentProperty.Name) { Source = this });
            ellipse.SetBinding(Ellipse.VerticalAlignmentProperty, new Binding(VerticalContentAlignmentProperty.Name) { Source = this });

            return ellipse;
        }

        private double StartValue(int index) => (5 + DotDiameter) * index + CharacterWidth;
    }
}
