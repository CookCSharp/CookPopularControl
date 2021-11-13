using CookPopularControl.Controls.Dragables;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace CookPopularControl.Controls.Dragables
{
    public delegate void DragableDragDeltaEventHandler(object sender, DragableDragDeltaEventArgs e);

    public class DragableDragDeltaEventArgs : DragableItemEventArgs
    {
        private readonly DragDeltaEventArgs _dragDeltaEventArgs;

        public DragableDragDeltaEventArgs(DragableItem dragableItem, DragDeltaEventArgs dragDeltaEventArgs)
            : base(dragableItem)
        {
            if (dragDeltaEventArgs == null) throw new ArgumentNullException("dragDeltaEventArgs");

            _dragDeltaEventArgs = dragDeltaEventArgs;
        }

        public DragableDragDeltaEventArgs(RoutedEvent routedEvent, DragableItem dragableItem, DragDeltaEventArgs dragDeltaEventArgs) 
            : base(routedEvent, dragableItem)
        {
            if (dragDeltaEventArgs == null) throw new ArgumentNullException("dragDeltaEventArgs");

            _dragDeltaEventArgs = dragDeltaEventArgs;
        }

        public DragableDragDeltaEventArgs(RoutedEvent routedEvent, object source, DragableItem dragableItem, DragDeltaEventArgs dragDeltaEventArgs) 
            : base(routedEvent, source, dragableItem)
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