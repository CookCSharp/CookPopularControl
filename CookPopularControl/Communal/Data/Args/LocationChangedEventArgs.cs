using System;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：LocationChangedEventArgs
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 13:43:42
 */
namespace CookPopularControl.Communal.Data
{
    public class LocationChangedEventArgs : EventArgs
    {
        private readonly object _item;
        private readonly Point _location;

        public LocationChangedEventArgs(object item, Point location)
        {
            if (item == null) throw new ArgumentNullException("item");

            _item = item;
            _location = location;
        }

        public object Item => _item;

        public Point Location => _location;

    }
}
