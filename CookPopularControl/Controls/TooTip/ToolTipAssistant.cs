using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ToolTipAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-18 14:04:20
 */
namespace CookPopularControl.Controls.TooTip
{
    /// <summary>
    /// <see cref="System.Windows.Controls.ToolTip"/>的附加属性基类
    /// </summary>
    public static class ToolTipAssistant
    {
        public static CustomPopupPlacementCallback CustomPopupPlacementCallback => CustomPopupPlacementCallbackImplement;

        /// <summary>
        /// 自定义TooTip的位置
        /// </summary>
        /// <param name="popupSize"></param>
        /// <param name="targetSize"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static CustomPopupPlacement[] CustomPopupPlacementCallbackImplement(Size popupSize, Size targetSize, Point offset)
        {
            return new CustomPopupPlacement[]
            {
                new CustomPopupPlacement(new Point(targetSize.Width/2 - popupSize.Width/2, targetSize.Height + 14), PopupPrimaryAxis.Horizontal)
            };
        }

        public static Brush GetCookPopularToolTipBackground(DependencyObject obj) => (Brush)obj.GetValue(CookPopularToolTipBackgroundProperty);
        public static void SetCookPopularToolTipBackground(DependencyObject obj, Brush value) => obj.SetValue(CookPopularToolTipBackgroundProperty, value);
        /// <summary>
        /// 提供<see cref="System.Windows.Controls.ToolTip"/>的背景色
        /// </summary>
        public static readonly DependencyProperty CookPopularToolTipBackgroundProperty =
            DependencyProperty.RegisterAttached("CookPopularToolTipBackground", typeof(Brush), typeof(ToolTipAssistant), new PropertyMetadata(default(Brush)));
    }
}
