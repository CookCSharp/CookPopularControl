using CookPopularControl.Communal.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：NotifyMessageInfo
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-21 14:41:11
 */
namespace CookPopularControl.Communal.Data.Infos
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
        /// 消息通知的弹出位置
        /// </summary>
        public NotifyPopupPosition PopupPosition { get; set; }

        /// <summary>
        /// 消息在打开时如何显示动画
        /// </summary>
        public PopupAnimation PopupAnimation { get; set; }

        /// <summary>
        /// 是否显示关闭按钮
        /// </summary>
        public bool IsShowCloseButton { get; set; }

        /// <summary>
        /// 消息是否自动关闭
        /// </summary>
        public bool IsAutoClose { get; set; }

        /// <summary>
        /// 消息持续时间
        /// </summary>
        /// <remarks>单位:s</remarks>
        public double Duration { get; set; }
    }
}
