using CookPopularControl.Controls.Dragables;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DragableDragDeltaEventArgs
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 11:26:39
 */
namespace CookPopularControl.Communal.Data
{
    public delegate void DragableDragDeltaEventHandler(object sender, DragableDragDeltaEventArgs e);

    public class DragableDragDeltaEventArgs : DragableItemEventArgs
    {
        private readonly DragDeltaEventArgs _dragDeltaEventArgs;

        public DragableDragDeltaEventArgs(DragableItem dragablzItem, DragDeltaEventArgs dragDeltaEventArgs) : base(dragablzItem)
        {
            if (dragDeltaEventArgs == null) throw new ArgumentNullException("dragDeltaEventArgs");

            _dragDeltaEventArgs = dragDeltaEventArgs;
        }

        public DragableDragDeltaEventArgs(RoutedEvent routedEvent, DragableItem dragablzItem, DragDeltaEventArgs dragDeltaEventArgs) : base(routedEvent, dragablzItem)
        {
            if (dragDeltaEventArgs == null) throw new ArgumentNullException("dragDeltaEventArgs");

            _dragDeltaEventArgs = dragDeltaEventArgs;
        }

        public DragableDragDeltaEventArgs(RoutedEvent routedEvent, object source, DragableItem dragablzItem, DragDeltaEventArgs dragDeltaEventArgs) : base(routedEvent, source, dragablzItem)
        {
            if (dragDeltaEventArgs == null) throw new ArgumentNullException("dragDeltaEventArgs");

            _dragDeltaEventArgs = dragDeltaEventArgs;
        }

        public DragDeltaEventArgs DragDeltaEventArgs
        {
            get { return _dragDeltaEventArgs; }
        }

        public bool Cancel { get; set; }
    }
}
