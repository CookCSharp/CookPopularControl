using CookPopularControl.Controls.Dragables;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace CookPopularControl.Controls.Dragables
{
    public delegate void DragableItemEventHandler(object sender, DragableItemEventArgs e);

    public class DragableItemEventArgs : RoutedEventArgs
    {
        private readonly DragableItem _dragableItem;

        public DragableItemEventArgs(DragableItem dragableItem)
        {
            if (dragableItem == null) throw new ArgumentNullException("dragableItem");            

            _dragableItem = dragableItem;
        }

        public DragableItemEventArgs(RoutedEvent routedEvent, DragableItem dragableItem)
            : base(routedEvent)
        {
            _dragableItem = dragableItem;
        }

        public DragableItemEventArgs(RoutedEvent routedEvent, object source, DragableItem dragableItem)
            : base(routedEvent, source)
        {
            _dragableItem = dragableItem;
        }

        public DragableItem DragableItem
        {
            get { return _dragableItem; }
        }
    }
}