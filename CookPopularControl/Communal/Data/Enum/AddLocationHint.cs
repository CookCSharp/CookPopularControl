using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：AddLocationHint
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 13:53:46
 */
namespace CookPopularControl.Communal.Data
{
    /// <summary>
    /// 指定添加选项卡的位置,而顺序则与源项的顺序无关
    /// </summary>
    public enum AddLocationHint
    {
        /// <summary>
        /// 第一个位置
        /// </summary>
        First,
        /// <summary>
        /// 最后一个位置
        /// </summary>
        Last,
        /// <summary>
        /// 选中项或指定项之前的位置
        /// </summary>
        Prior,
        /// <summary>
        /// 选中项或指定项之后的位置
        /// </summary>
        After
    }
}
