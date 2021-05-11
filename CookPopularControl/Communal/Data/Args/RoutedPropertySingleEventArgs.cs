using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：RoutedPropertyDeleteEventArgs
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-11 17:05:09
 */
namespace CookPopularControl.Communal.Data.Args
{
    /// <summary>
    /// 表示单个项变化的路由信息和事件数据
    /// </summary>
    /// <typeparam name="TSingle"></typeparam>
    public class RoutedPropertySingleEventArgs<TSingle> : RoutedEventArgs
    {
        public TSingle Single { get; private set; }

        public RoutedPropertySingleEventArgs(TSingle value)
        {
            Single = value;
        }

        public RoutedPropertySingleEventArgs(object source, RoutedEvent routedEvent) : base(routedEvent, source)
        {

        }
    }
}
