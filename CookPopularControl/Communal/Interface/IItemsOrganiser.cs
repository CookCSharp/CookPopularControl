using CookPopularControl.Controls.Dragables;
using System.Collections.Generic;
using System.Linq;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：IItemsOrganiser
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 11:56:37
 */
namespace CookPopularControl.Communal.Interface
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
