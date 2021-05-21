using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：NotifyPosition
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-21 09:04:28
 */
namespace CookPopularControl.Communal.Data.Enum
{
    /// <summary>
    /// 通知消息的弹出位置
    /// 由两部分组成，水平+垂直
    /// </summary>
    public enum NotifyPopupPosition
    {
        LeftTop,
        CenterTop,
        RightTop,
        LeftCenter,
        AllCenter,
        RightCenter,
        LeftBottom,
        CenterBottom,
        RightBottom
    }
}
