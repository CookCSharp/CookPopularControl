using CookPopularControl.Controls.Dragables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DragableItemEventArgs
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 11:18:21
 */
namespace CookPopularControl.Communal.Data
{
    public delegate void DragableItemEventHandler(object sender, DragableItemEventArgs e);

    public class DragableItemEventArgs : RoutedEventArgs
    {
        private readonly DragableItem _dragableItem;

        public DragableItemEventArgs(DragableItem dragableItem)
        {
            if (dragableItem == null) throw new ArgumentNullException("dragablzItem");

            _dragableItem = dragableItem;
        }

        public DragableItemEventArgs(RoutedEvent routedEvent, DragableItem dragableItem) : base(routedEvent)
        {
            _dragableItem = dragableItem;
        }

        public DragableItemEventArgs(RoutedEvent routedEvent, object source, DragableItem dragableItem) : base(routedEvent, source)
        {
            _dragableItem = dragableItem;
        }

        public DragableItem DragableItem
        {
            get { return _dragableItem; }
        }
    }
}
