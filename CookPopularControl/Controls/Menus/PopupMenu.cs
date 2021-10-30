using CookPopularControl.Communal.Data.Enum;
using CookPopularControl.Tools.Boxes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：PopupMenu
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-25 14:58:39
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 弹出菜单
    /// </summary>
    /// <remarks>
    /// 子项布局采用<see cref="StackPanel"/>;
    /// 建议菜单子项属性设置Width=50，Height=50，可以保证对齐
    /// </remarks>
    public class PopupMenu : ItemsControl
    {
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
            DependencyProperty.Register("IsShowMenu", typeof(bool), typeof(PopupMenu), new PropertyMetadata(ValueBoxes.FalseBox));


        /// <summary>
        /// 弹出方向
        /// </summary>
        /// <remarks>共9个方向，详细看<see cref="CookPopularControl.Communal.Data.Enum.PopupPosition"/></remarks>
        public PopupPosition PopupPosition
        {
            get { return (PopupPosition)GetValue(PopupPositionProperty); }
            set { SetValue(PopupPositionProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="PopupPosition"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty PopupPositionProperty =
            DependencyProperty.Register("PopupPosition", typeof(PopupPosition), typeof(PopupMenu), new PropertyMetadata(PopupPosition.RightTop));


        /// <summary>
        /// 弹出动画
        /// </summary>
        /// <remarks>共4中动画</remarks>
        public PopupAnimation PopupAnimation
        {
            get { return (PopupAnimation)GetValue(PopupAnimationProperty); }
            set { SetValue(PopupAnimationProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="PopupAnimation"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty PopupAnimationProperty =
            DependencyProperty.Register("PopupAnimation", typeof(PopupAnimation), typeof(PopupMenu), new PropertyMetadata(default(PopupAnimation)));
    }
}
