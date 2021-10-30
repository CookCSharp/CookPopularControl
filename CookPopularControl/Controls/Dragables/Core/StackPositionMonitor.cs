using CookPopularControl.Communal.Data;
using CookPopularControl.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：StackPositionMonitor
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 14:38:28
 */
namespace CookPopularControl.Controls.Dragables.Core
{
    /// <summary>
    /// 对子项的线性监听器(水平或垂直)，参见<see cref="StackOrganiser"/>
    /// </summary>
    public abstract class StackPositionMonitor : PositionMonitor
    {
        private readonly Func<DragableItem, double> _getLocation;

        protected StackPositionMonitor()
        {

        }

        protected StackPositionMonitor(Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.Horizontal:
                    _getLocation = item => item.X;
                    break;
                case Orientation.Vertical:
                    _getLocation = item => item.Y;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("orientation");
            }
        }

        public event EventHandler<OrderChangedEventArgs> OrderChanged;

        internal virtual void OnOrderChanged(OrderChangedEventArgs e)
        {
            var handler = OrderChanged;
            if (handler != null) handler(this, e);
        }

        internal IEnumerable<DragableItem> Sort(IEnumerable<DragableItem> items)
        {
            if (items == null) throw new ArgumentNullException("items");

            return items.OrderBy(i => _getLocation(i));
        }
    }
}
