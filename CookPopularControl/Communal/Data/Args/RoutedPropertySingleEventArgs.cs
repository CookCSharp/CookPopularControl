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
    public delegate void RoutedPropertySingleEventHandler<TSingle>(object sender, RoutedPropertySingleEventArgs<TSingle> e);

    /// <summary>
    /// 表示单个项变化的路由信息和事件数据
    /// </summary>
    /// <typeparam name="TSingle"></typeparam>
    public class RoutedPropertySingleEventArgs<TSingle> : RoutedEventArgs
    {
        public TSingle Value { get; private set; }

        public RoutedPropertySingleEventArgs(TSingle value) : base()
        {
            Value = value;
        }

        public RoutedPropertySingleEventArgs(TSingle value, RoutedEvent routedEvent) : this(value)
        {
            RoutedEvent = routedEvent;
        }

        /// <summary>
        /// This method is used to perform the proper type casting in order to
        /// call the type-safe RoutedPropertySingleEventHandler delegate for the IsCheckedChangedEvent event.
        /// </summary>
        /// <param name="genericHandler">The handler to invoke.</param>
        /// <param name="genericTarget">The current object along the event's route.</param>
        /// <returns>Nothing.</returns>
        protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
        {
            RoutedPropertySingleEventHandler<TSingle> handler = (RoutedPropertySingleEventHandler<TSingle>)genericHandler;
            handler(genericTarget, this);
        }
    }
}
