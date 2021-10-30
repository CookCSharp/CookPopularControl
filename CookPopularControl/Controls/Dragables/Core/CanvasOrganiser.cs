using CookPopularControl.Communal.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CanvasOrganiser
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-12 12:48:12
 */
namespace CookPopularControl.Controls.Dragables.Core
{
    public class CanvasOrganiser : IItemsOrganiser
    {
        public virtual void Organise(DragableItemsControl requestor, Size measureBounds, IEnumerable<DragableItem> items)
        {

        }

        public virtual void Organise(DragableItemsControl requestor, Size measureBounds, IOrderedEnumerable<DragableItem> items)
        {

        }

        public virtual void OrganiseOnMouseDownWithin(DragableItemsControl requestor, Size measureBounds, List<DragableItem> siblingItems, DragableItem dragablzItem)
        {
            var zIndex = int.MaxValue;
            foreach (var source in siblingItems.OrderByDescending(Panel.GetZIndex))
            {
                Panel.SetZIndex(source, --zIndex);
            }
            Panel.SetZIndex(dragablzItem, int.MaxValue);
        }

        public virtual void OrganiseOnDragStarted(DragableItemsControl requestor, Size measureBounds, IEnumerable<DragableItem> siblingItems, DragableItem dragItem)
        {

        }

        public virtual void OrganiseOnDrag(DragableItemsControl requestor, Size measureBounds, IEnumerable<DragableItem> siblingItems, DragableItem dragItem)
        {

        }

        public virtual void OrganiseOnDragCompleted(DragableItemsControl requestor, Size measureBounds, IEnumerable<DragableItem> siblingItems, DragableItem dragItem)
        {

        }

        public virtual Point ConstrainLocation(DragableItemsControl requestor, Size measureBounds, Point itemCurrentLocation, Size itemCurrentSize, Point itemDesiredLocation, Size itemDesiredSize)
        {
            //we will stop it pushing beyond the bounds...unless it's already beyond...
            var reduceBoundsWidth = itemCurrentLocation.X + itemCurrentSize.Width > measureBounds.Width
                ? 0
                : itemDesiredSize.Width;
            var reduceBoundsHeight = itemCurrentLocation.Y + itemCurrentSize.Height > measureBounds.Height
                ? 0
                : itemDesiredSize.Height;

            return new Point(
                Math.Min(Math.Max(itemDesiredLocation.X, 0), measureBounds.Width - reduceBoundsWidth),
                Math.Min(Math.Max(itemDesiredLocation.Y, 0), measureBounds.Height - reduceBoundsHeight));
        }

        public virtual Size Measure(DragableItemsControl requestor, Size availableSize, IEnumerable<DragableItem> items)
        {
            return availableSize;
        }

        public virtual IEnumerable<DragableItem> Sort(IEnumerable<DragableItem> items)
        {
            return items;
        }
    }
}
