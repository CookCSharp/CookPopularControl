using CookPopularControl.Communal.Data;
using System;
using System.Windows.Controls.Primitives;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：NotifyMessageInfo
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-21 14:41:11
 */
namespace CookPopularControl.Communal.Data
{
    /// <summary>
    /// 通知消息附带的信息类
    /// </summary>
    public class NotifyMessageInfo
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// 消息图标
        /// </summary>
        public Geometry MessageIcon { get; set; }

        /// <summary>
        /// 消息图标颜色
        /// </summary>
        public Brush MessageIconBrush { get; set; }

        /// <summary>
        /// 消息通知的弹出位置
        /// </summary>
        public PopupPosition PopupPosition { get; set; }

        /// <summary>
        /// 消息在打开时如何显示动画
        /// </summary>
        public PopupAnimation PopupAnimation { get; set; } = PopupAnimation.Slide;

        /// <summary>
        /// 是否显示关闭按钮
        /// </summary>
        public bool IsShowCloseButton { get; set; } = true;

        /// <summary>
        /// 消息是否自动关闭
        /// </summary>
        public bool IsAutoClose { get; set; } = true;

        /// <summary>
        /// 消息持续时间
        /// </summary>
        /// <remarks>单位:s</remarks>
        public double Duration { get; set; } = 3;

        /// <summary>
        /// 消息关闭前触发的方法
        /// </summary>
        public Func<bool, bool> ActionBeforeClose { get; set; }
    }
}
