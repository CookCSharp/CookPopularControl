using CookPopularControl.Controls.Dragables;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace CookPopularControl.Controls.Dragables
{
    public delegate void DragableDragStartedEventHandler(object sender, DragableDragStartedEventArgs e);

    public class DragableDragStartedEventArgs : DragableItemEventArgs
    {
        private readonly DragStartedEventArgs _dragStartedEventArgs;

        public DragableDragStartedEventArgs(DragableItem dragableItem, DragStartedEventArgs dragStartedEventArgs)
            : base(dragableItem)
        {
            if (dragStartedEventArgs == null) throw new ArgumentNullException("dragStartedEventArgs");

            _dragStartedEventArgs = dragStartedEventArgs;
        }

        public DragableDragStartedEventArgs(RoutedEvent routedEvent, DragableItem dragableItem, DragStartedEventArgs dragStartedEventArgs)
            : base(routedEvent, dragableItem)
        {
            _dragStartedEventArgs = dragStartedEventArgs;
        }

        public DragableDragStartedEventArgs(RoutedEvent routedEvent, object source, DragableItem dragableItem, DragStartedEventArgs dragStartedEventArgs)
            : base(routedEvent, source, dragableItem)
        {
            _dragStartedEventArgs = dragStartedEventArgs;
        }

        public DragStartedEventArgs DragStartedEventArgs
        {
            get { return _dragStartedEventArgs; }
        }        
    }
}