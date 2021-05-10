using CookPopularControl.Communal.Attached;
using CookPopularControl.Controls.Panels;
using CookPopularControl.Tools.Boxes;
using CookPopularControl.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using OriginButton = System.Windows.Controls.Button;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Star
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-10 10:51:27
 */
namespace CookPopularControl.Controls.Starred
{
    /// <summary>
    /// 表示点赞类控件
    /// </summary>
    public class Star : ContentControl
    {
        /// <summary>
        /// 数量
        /// </summary>
        public int StarCount
        {
            get { return (int)GetValue(StarCountProperty); }
            set { SetValue(StarCountProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="StarCount"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty StarCountProperty =
            DependencyProperty.Register("StarCount", typeof(int), typeof(Star),
                new FrameworkPropertyMetadata(ValueBoxes.Inter5Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesChanged));


        /// <summary>
        /// 当前评分
        /// </summary>
        /// <remarks>当<see cref="IsAllowHalf"/>为False时，会强制转换为Int型</remarks>
        public double StarValue
        {
            get { return (double)GetValue(StarValueProperty); }
            set { SetValue(StarValueProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="StarValue"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty StarValueProperty =
            DependencyProperty.Register("StarValue", typeof(double), typeof(Star),
                new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesChanged));


        /// <summary>
        /// 图标大小
        /// </summary>
        public double StarSize
        {
            get { return (double)GetValue(StarSizeProperty); }
            set { SetValue(StarSizeProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="StarSize"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty StarSizeProperty =
            DependencyProperty.Register("StarSize", typeof(double), typeof(Star),
                new FrameworkPropertyMetadata(ValueBoxes.Double30Box, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesChanged));


        /// <summary>
        /// 是否允许点赞一半
        /// </summary>    
        /// 可通过Button的大小与Icon的大小来设置，例如btn.Width = 15; icon.Width = 30;
        public bool IsAllowHalf
        {
            get { return (bool)GetValue(IsAllowHalfProperty); }
            set { SetValue(IsAllowHalfProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 标识<see cref="IsAllowHalf"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsAllowHalfProperty =
            DependencyProperty.Register("IsAllowHalf", typeof(bool), typeof(Star),
                new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesChanged));


        /// <summary>
        /// 是否显示分数
        /// </summary>
        public bool IsShowScore
        {
            get { return (bool)GetValue(IsShowScoreProperty); }
            set { SetValue(IsShowScoreProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 标识<see cref="IsShowScore"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsShowScoreProperty =
            DependencyProperty.Register("IsShowScore", typeof(bool), typeof(Star), new PropertyMetadata(ValueBoxes.TrueBox));


        /// <summary>
        /// 点赞图标
        /// </summary>
        public Geometry StarIcon
        {
            get { return (Geometry)GetValue(StarIconProperty); }
            set { SetValue(StarIconProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="StarIcon"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty StarIconProperty =
            DependencyProperty.Register("StarIcon", typeof(Geometry), typeof(Star),
                new FrameworkPropertyMetadata(DefaultGeometry, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesChanged));


        /// <summary>
        /// 布局方向
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="Orientation"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Star),
                new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertiesChanged));


        private static void OnPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var star = d as Star;
            if (star != null)
            {
                if (e.Property == StarValueProperty)
                {
                    if (!star.IsAllowHalf)
                        star.StarValue = (int)star.StarValue;
                    star.OnValueChanged((double)e.OldValue, (double)e.NewValue);
                }
                else if(e.Property == IsAllowHalfProperty)
                {
                    if (!star.IsAllowHalf)
                        star.StarValue = (int)star.StarValue;
                }

                star.UpdateStarsLayout();
            }
        }


        [Description("StarValue更改时发生")]
        public event RoutedPropertyChangedEventHandler<double> StarValueChanged
        {
            add { this.AddHandler(StarValueChangedEvent, value); }
            remove { this.RemoveHandler(StarValueChangedEvent, value); }
        }
        public static readonly RoutedEvent StarValueChangedEvent =
            EventManager.RegisterRoutedEvent("StarValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(Star));

        protected virtual void OnValueChanged(double oldValue, double newValue)
        {
            RoutedPropertyChangedEventArgs<double> arg = new RoutedPropertyChangedEventArgs<double>(oldValue, newValue, StarValueChangedEvent);
            this.RaiseEvent(arg);
        }


        private static readonly Geometry DefaultGeometry = ResourceHelper.GetResource<Geometry>("StarGeometry");
        private StackPanel panel = new StackPanel();

        public Star()
        {
            Content = panel;
            panel.SetBinding(WidthProperty, new Binding(WidthProperty.Name) { Source = this });
            panel.SetBinding(HeightProperty, new Binding(HeightProperty.Name) { Source = this });
            panel.SetBinding(Panel.BackgroundProperty, new Binding(BackgroundProperty.Name) { Source = this });
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            UpdateStarsLayout();
        }

        private void UpdateStarsLayout()
        {
            if (StarCount < 1) return;
            panel.Children.Clear();
            panel.Orientation = Orientation;

            var step = IsAllowHalf ? 0.5 : 1;
            if (IsAllowHalf)
            {
                for (int i = 0; i < StarCount; i++)
                {
                    var btn = CreateGeometry(i);
                    var btn_Half = CreateGeometry(i);
                    btn_Half.Width = StarSize * 1.1 / 2D;
                    btn_Half.HorizontalContentAlignment = HorizontalAlignment.Left;
                    if(i + step < StarValue)
                        btn.Foreground = Foreground;
                    else if (i + step == StarValue)
                    {
                        btn_Half.Foreground = Foreground;
                        btn.Foreground = ResourceHelper.GetResource<Brush>("UnEnabledBrush");
                    }
                    else
                    {
                        btn_Half.Foreground = ResourceHelper.GetResource<Brush>("UnEnabledBrush");
                        btn.Foreground = ResourceHelper.GetResource<Brush>("UnEnabledBrush");
                    }

                    var simpleGrid = new SimpleGrid();
                    simpleGrid.Width = btn.Width;
                    simpleGrid.Height = btn.Height;
                    simpleGrid.Children.Add(btn);
                    simpleGrid.Children.Add(btn_Half);
                    btn_Half.HorizontalAlignment = HorizontalAlignment.Left;
                    panel.Children.Add(simpleGrid);
                }
            }
            else
            {
                for (int i = 0; i < StarCount; i++)
                {
                    var btn = CreateGeometry(i);
                    if (i + step <= StarValue)
                        btn.Foreground = Foreground;
                    else
                        btn.Foreground = ResourceHelper.GetResource<Brush>("UnEnabledBrush");

                    panel.Children.Add(btn);
                }
            }

            if (IsShowScore)
            {
                TextBlock tb = new TextBlock();
                tb.Text = StarValue.ToString();
                tb.Width = 20D;
                tb.FontWeight = FontWeights.Bold;
                tb.HorizontalAlignment = HorizontalContentAlignment;
                tb.VerticalAlignment = VerticalContentAlignment;
                if (panel.Orientation == Orientation.Horizontal)
                    tb.Margin = new Thickness(5, 0, 5, 0);
                else
                    tb.Margin = new Thickness(0, 5, 0, 5);
                panel.Children.Add(tb);
            }
        }

        /// <summary>
        /// 创建图形
        /// </summary>
        /// <param name="index">标识第几个图形</param>
        /// <returns></returns>
        private OriginButton CreateGeometry(int index)
        {
            OriginButton button = new OriginButton();
            button.Foreground = Foreground;
            button.BorderBrush = BorderBrush;
            button.BorderThickness = BorderThickness;
            button.Style = ResourceHelper.GetResource<Style>("ButtonTransparentIconStyle");
            FrameworkElementBaseAttached.SetIconGeometry(button, StarIcon);
            FrameworkElementBaseAttached.SetIconWidth(button, StarSize);
            FrameworkElementBaseAttached.SetIconHeight(button, StarSize);


            if (panel.Orientation == Orientation.Horizontal)
                button.Margin = new Thickness(5, 0, 5, 0);
            else
                button.Margin = new Thickness(0, 5, 0, 5);

            if (index.Equals(0))
            {
                button.MouseDoubleClick += (s, e) =>
                {
                    StarValue = 0;
                    UpdateStarsLayout();
                    e.Handled = true;
                };
            }

            Point mousePoint = default;
            button.MouseMove += (s, e) =>
            {
                mousePoint = e.GetPosition(button);
            };

            button.Click += (s, e) =>
            {
                if (IsAllowHalf)
                {
                    if (mousePoint.X <= StarSize / 2D)
                        StarValue = index + 0.5;
                    else
                        StarValue = index + 1;
                }
                else
                {
                    StarValue = index + 1;
                }

                UpdateStarsLayout();
                e.Handled = true;
            };

            return button;
        }
    }
}
