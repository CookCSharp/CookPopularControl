/*
 * Description：IDialog 
 * Author： Chance.Zheng
 * Create Time: 2023-03-08 14:58:25
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CookPopularControl.Controls
{
    public interface IDialog
    {
        /// <summary>
        /// 对话框是否关闭
        /// </summary>
        public bool IsClosed { get; }
    }
}
