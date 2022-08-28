using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;



/*
 * Description：ControlCommands 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-01-07 17:09:24
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */
namespace CookPopularControl.Communal
{
    /// <summary>
    /// 统一所有控件中所用到的命令(方便管理)
    /// </summary>
    public static class ControlCommands
    {
        /// <summary>
        /// 按照类别排序
        /// </summary>
        public static ICommand SortByCategoryCommand { get; } = new RoutedCommand(nameof(SortByCategoryCommand), typeof(ControlCommands));

        /// <summary>
        /// 按照名称排序
        /// </summary>
        public static ICommand SortByNameCommand { get; } = new RoutedCommand(nameof(SortByNameCommand), typeof(ControlCommands));

        public static ICommand ConfirmCommand { get; } = new RoutedCommand(nameof(ConfirmCommand), typeof(ControlCommands));
        public static ICommand YesCommand { get; } = new RoutedCommand(nameof(YesCommand), typeof(ControlCommands));
        public static ICommand NoCommand { get; } = new RoutedCommand(nameof(NoCommand), typeof(ControlCommands));
        public static ICommand CancelCommand { get; } = new RoutedCommand(nameof(CancelCommand), typeof(ControlCommands));
    }
}
