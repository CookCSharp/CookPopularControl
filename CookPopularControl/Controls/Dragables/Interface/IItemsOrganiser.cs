using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CookPopularControl.Controls.Dragables
{
    public interface IItemsOrganiser
    {
        void Organise(DragableItemsControl requestor, Size measureBounds, IEnumerable<DragableItem> items);
        void Organise(DragableItemsControl requestor, Size measureBounds, IOrderedEnumerable<DragableItem> items);
        void OrganiseOnMouseDownWithin(DragableItemsControl requestor, Size measureBounds, List<DragableItem> siblingItems, DragableItem dragablzItem);
        void OrganiseOnDragStarted(DragableItemsControl requestor, Size measureBounds, IEnumerable<DragableItem> siblingItems, DragableItem dragItem);
        void OrganiseOnDrag(DragableItemsControl requestor, Size measureBounds, IEnumerable<DragableItem> siblingItems, DragableItem dragItem);
        void OrganiseOnDragCompleted(DragableItemsControl requestor, Size measureBounds, IEnumerable<DragableItem> siblingItems, DragableItem dragItem);        
        Point ConstrainLocation(DragableItemsControl requestor, Size measureBounds, Point itemCurrentLocation, Size itemCurrentSize, Point itemDesiredLocation, Size itemDesiredSize);
        Size Measure(DragableItemsControl requestor, Size availableSize, IEnumerable<DragableItem> items);
        IEnumerable<DragableItem> Sort(IEnumerable<DragableItem> items);
    }
}
