using CookPopularControl.Controls.Dragables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DragableDragStartedEventArgs
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 11:23:21
 */
namespace CookPopularControl.Communal.Data
{
    public delegate void DragableDragStartedEventHandler(object sender, DragableDragStartedEventArgs e);

    public class DragableDragStartedEventArgs : DragableItemEventArgs
    {
        private readonly DragStartedEventArgs _dragStartedEventArgs;

        public DragableDragStartedEventArgs(DragableItem dragablzItem, DragStartedEventArgs dragStartedEventArgs) : base(dragablzItem)
        {
            if (dragStartedEventArgs == null) throw new ArgumentNullException("dragStartedEventArgs");

            _dragStartedEventArgs = dragStartedEventArgs;
        }

        public DragableDragStartedEventArgs(RoutedEvent routedEvent, DragableItem dragablzItem, DragStartedEventArgs dragStartedEventArgs) : base(routedEvent, dragablzItem)
        {
            _dragStartedEventArgs = dragStartedEventArgs;
        }

        public DragableDragStartedEventArgs(RoutedEvent routedEvent, object source, DragableItem dragablzItem, DragStartedEventArgs dragStartedEventArgs) : base(routedEvent, source, dragablzItem)
        {
            _dragStartedEventArgs = dragStartedEventArgs;
        }

        public DragStartedEventArgs DragStartedEventArgs
        {
            get { return _dragStartedEventArgs; }
        }
    }
}
