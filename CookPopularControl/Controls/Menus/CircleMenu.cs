using CookPopularControl.Controls.Panels;
using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using OriginButton = System.Windows.Controls.Button;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CircleMenu
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-07 14:01:49
 */
namespace CookPopularControl.Controls.Menus
{
    /// <summary>
    /// 弧形、圆形菜单
    /// 子项布局采用<see cref="ArcPanel"/>
    /// </summary>
    public class CircleMenu : ItemsControl
    {
        private static readonly object DefaultRadius = 150D;
        private static readonly object DefaultArcAngle = 360D;

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }   
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(CircleMenu), new PropertyMetadata(DefaultRadius));

        public double ArcAngle
        {
            get { return (double)GetValue(ArcAngleProperty); }
            set { SetValue(ArcAngleProperty, value); }
        }
        public static readonly DependencyProperty ArcAngleProperty =
            DependencyProperty.Register("ArcAngle", typeof(double), typeof(CircleMenu), new PropertyMetadata(DefaultArcAngle));

        /// <summary>
        /// 子项是否围绕自身旋转
        /// </summary>
        public bool IsItemRotate
        {
            get => (bool)GetValue(IsItemRotateProperty);
            set => SetValue(IsItemRotateProperty, ValueBoxes.BooleanBox(value));
        }
        /// <summary>
        /// 提供<see cref="IsItemRotate"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsItemRotateProperty =
            DependencyProperty.Register("IsItemRotate", typeof(bool), typeof(CircleMenu), new FrameworkPropertyMetadata(ValueBoxes.TrueBox));
    }
}
