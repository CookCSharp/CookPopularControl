using CookPopularControl.Communal.Data;
using CookPopularControl.Controls.Dragables;
using System;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：PositionMonitor
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 11:58:13
 */
namespace CookPopularControl.References
{
    /// <summary>
    /// 提供一个位置监视器来接收位置的更新   
    /// </summary>
    /// <remarks>
    /// <see cref="PositionMonitor"/> 
    /// 可以用来监听位置变化而不是路由事件，更适合在MVVM中使用
    /// </remarks>
    public class PositionMonitor
    {
        /// <summary>
        /// Raised when the X,Y coordinate of a <see cref="DragableItem"/> changes.
        /// </summary>
        public event EventHandler<LocationChangedEventArgs> LocationChanged;

        internal virtual void OnLocationChanged(LocationChangedEventArgs e)
        {
            if (e == null) throw new ArgumentNullException("e");

            var handler = LocationChanged;
            handler?.Invoke(this, e);
        }

        internal virtual void ItemsChanged() { }
    }
}
