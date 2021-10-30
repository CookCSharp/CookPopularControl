using System;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：OrderChangedEventArgs
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 14:42:39
 */
namespace CookPopularControl.Communal.Data
{
    public class OrderChangedEventArgs : EventArgs
    {
        private readonly object[] _previousOrder;
        private readonly object[] _newOrder;

        public OrderChangedEventArgs(object[] previousOrder, object[] newOrder)
        {
            if (newOrder == null) throw new ArgumentNullException("newOrder");

            _previousOrder = previousOrder;
            _newOrder = newOrder;
        }

        public object[] PreviousOrder
        {
            get { return _previousOrder; }
        }

        public object[] NewOrder
        {
            get { return _newOrder; }
        }
    }
}
