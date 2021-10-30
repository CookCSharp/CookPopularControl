using System;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：TabControlHeaderDragStartInformation
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 15:30:13
 */
namespace CookPopularControl.Controls.Dragables.Core
{
    public class TabItemHeaderDragStartInformation
    {
        private readonly DragableItem _dragItem;
        private readonly double _dragableItemsControlHorizontalOffset;
        private readonly double _dragableItemControlVerticalOffset;
        private readonly double _dragableItemHorizontalOffset;
        private readonly double _dragableItemVerticalOffset;

        public TabItemHeaderDragStartInformation(DragableItem dragItem, double dragablzItemsControlHorizontalOffset, double dragablzItemControlVerticalOffset, double dragablzItemHorizontalOffset, double dragablzItemVerticalOffset)
        {
            if (dragItem == null) throw new ArgumentNullException("dragItem");

            _dragItem = dragItem;
            _dragableItemsControlHorizontalOffset = dragablzItemsControlHorizontalOffset;
            _dragableItemControlVerticalOffset = dragablzItemControlVerticalOffset;
            _dragableItemHorizontalOffset = dragablzItemHorizontalOffset;
            _dragableItemVerticalOffset = dragablzItemVerticalOffset;
        }

        public double DragablzItemsControlHorizontalOffset
        {
            get { return _dragableItemsControlHorizontalOffset; }
        }

        public double DragablzItemControlVerticalOffset
        {
            get { return _dragableItemControlVerticalOffset; }
        }

        public double DragablzItemHorizontalOffset
        {
            get { return _dragableItemHorizontalOffset; }
        }

        public double DragablzItemVerticalOffset
        {
            get { return _dragableItemVerticalOffset; }
        }

        public DragableItem DragItem
        {
            get { return _dragItem; }
        }
    }
}
