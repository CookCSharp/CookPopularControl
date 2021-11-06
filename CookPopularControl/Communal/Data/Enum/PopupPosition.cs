/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：NotifyPosition
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-21 09:04:28
 */
namespace CookPopularControl.Communal.Data
{
    /// <summary>
    /// 表示弹出控件的弹出位置；
    /// 由两部分组成，水平+垂直，总共9个弹出方向
    /// </summary>
    public enum PopupPosition
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
