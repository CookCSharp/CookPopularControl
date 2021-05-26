using CookPopularControl.Communal.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：PopupMenu
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-25 14:58:39
 */
namespace CookPopularControl.Controls.Menus
{
    /// <summary>
    /// 弹出菜单
    /// </summary>
    /// <remarks>子项布局采用<see cref="StackPanel"/></remarks>
    public class PopupMenu : ItemsControl
    {
        /// <summary>
        /// 弹出方向
        /// </summary>
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
