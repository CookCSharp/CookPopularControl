using CookPopularControl.Communal.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：WrapOrganiser
 * Author： Chance_写代码的厨子
 * Create Time：2021-09-09 15:11:59
 */
namespace CookPopularControl.Controls.Dragables.Core
{
    public class WrapOrganiser : IItemsOrganiser
    {
        public void Organise(DragableItemsControl requestor, Size measureBounds, IEnumerable<DragableItem> items)
        {

        }

        public void Organise(DragableItemsControl requestor, Size measureBounds, IOrderedEnumerable<DragableItem> items)
        {

        }

        public void OrganiseOnMouseDownWithin(DragableItemsControl requestor, Size measureBounds, List<DragableItem> siblingItems, DragableItem dragablzItem)
        {

        }

        public void OrganiseOnDragStarted(DragableItemsControl requestor, Size measureBounds, IEnumerable<DragableItem> siblingItems, DragableItem dragItem)
        {

        }

        public void OrganiseOnDrag(DragableItemsControl requestor, Size measureBounds, IEnumerable<DragableItem> siblingItems, DragableItem dragItem)
        {

        }

        public void OrganiseOnDragCompleted(DragableItemsControl requestor, Size measureBounds, IEnumerable<DragableItem> siblingItems, DragableItem dragItem)
        {
 
        }

        public Point ConstrainLocation(DragableItemsControl requestor, Size measureBounds, Point itemCurrentLocation, Size itemCurrentSize, Point itemDesiredLocation, Size itemDesiredSize)
        {
            throw new NotImplementedException();
        }

        public Size Measure(DragableItemsControl requestor, Size availableSize, IEnumerable<DragableItem> items)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DragableItem> Sort(IEnumerable<DragableItem> items)
        {
            throw new NotImplementedException();
        }
    }
}
