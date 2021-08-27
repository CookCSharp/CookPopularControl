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
 * Description：DragableDragCompletedEventArgs
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 11:29:29
 */
namespace CookPopularControl.Communal.Data
{
    public delegate void DragableDragCompletedEventHandler(object sender, DragableDragCompletedEventArgs e);

    public class DragableDragCompletedEventArgs : RoutedEventArgs
    {
        private readonly DragableItem _dragableItem;
        private readonly bool _isDropTargetFound;
        private readonly DragCompletedEventArgs _dragCompletedEventArgs;

        public DragableDragCompletedEventArgs(DragableItem dragableItem, DragCompletedEventArgs dragCompletedEventArgs)
        {
            if (dragableItem == null) throw new ArgumentNullException("dragablzItem");
            if (dragCompletedEventArgs == null) throw new ArgumentNullException("dragCompletedEventArgs");

            _dragableItem = dragableItem;
            _dragCompletedEventArgs = dragCompletedEventArgs;
        }

        public DragableDragCompletedEventArgs(RoutedEvent routedEvent, DragableItem dragableItem, DragCompletedEventArgs dragCompletedEventArgs) : base(routedEvent)
        {
            if (dragableItem == null) throw new ArgumentNullException("dragablzItem");
            if (dragCompletedEventArgs == null) throw new ArgumentNullException("dragCompletedEventArgs");

            _dragableItem = dragableItem;
            _dragCompletedEventArgs = dragCompletedEventArgs;
        }

        public DragableDragCompletedEventArgs(RoutedEvent routedEvent, object source, DragableItem dragableItem, DragCompletedEventArgs dragCompletedEventArgs) : base(routedEvent, source)
        {
            if (dragableItem == null) throw new ArgumentNullException("dragablzItem");
            if (dragCompletedEventArgs == null) throw new ArgumentNullException("dragCompletedEventArgs");

            _dragableItem = dragableItem;
            _dragCompletedEventArgs = dragCompletedEventArgs;
        }

        public DragableItem DragableItem
        {
            get { return _dragableItem; }
        }

        public DragCompletedEventArgs DragCompletedEventArgs
        {
            get { return _dragCompletedEventArgs; }
        }
    }
}
