using CookPopularControl.Controls;
using CookPopularControl.Tools.Boxes;
using System.Windows;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CircleMenu
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-07 14:01:49
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 弧形、圆形菜单
    /// </summary>
    /// <remarks>子项布局采用<see cref="ArcPanel"/></remarks>
    public class CircleMenu : ItemsControl
    {
        public static readonly object DefaultRadius = 100D;
        public static readonly object DefaultArcAngle = 360D;


        /// <summary>
        /// 是否显示菜单
        /// </summary>
        public bool IsShowMenu
        {
            get { return (bool)GetValue(IsShowMenuProperty); }
            set { SetValue(IsShowMenuProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 标识<see cref="IsShowMenu"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsShowMenuProperty =
            DependencyProperty.Register("IsShowMenu", typeof(bool), typeof(CircleMenu), new PropertyMetadata(ValueBoxes.FalseBox));


        /// <summary>
        /// 弧形半径
        /// </summary>
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="Radius"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(CircleMenu), new PropertyMetadata(DefaultRadius));


        /// <summary>
        /// 弧形角度
        /// </summary>
        public double ArcAngle
        {
            get { return (double)GetValue(ArcAngleProperty); }
            set { SetValue(ArcAngleProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="ArcAngle"/>的依赖属性
        /// </summary>
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
