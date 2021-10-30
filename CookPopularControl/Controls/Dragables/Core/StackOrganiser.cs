﻿using CookPopularControl.Communal.Interface;
using CookPopularControl.Tools.Comparators;
using CookPopularControl.Tools.Extensions.Animatables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：StackOrganiser
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 14:59:24
 */
namespace CookPopularControl.Controls.Dragables.Core
{
    public abstract class StackOrganiser : IItemsOrganiser
    {
        private readonly Orientation _orientation;
        private readonly double _itemOffset;
        private readonly Func<DragableItem, double> _getDesiredSize;
        private readonly Func<DragableItem, double> _getLocation;
        private readonly Action<DragableItem, double> _setLocation;
        private readonly DependencyProperty _canvasDependencyProperty;

        private readonly Dictionary<DragableItem, double> _activeStoryboardTargetLocations = new Dictionary<DragableItem, double>();

        protected StackOrganiser(Orientation orientation, double itemOffset = 0)
        {
            _orientation = orientation;
            _itemOffset = itemOffset;

            switch (orientation)
            {
                case Orientation.Horizontal:
                    _getDesiredSize = item => item.DesiredSize.Width;
                    _getLocation = item => item.X;
                    _setLocation = (item, coord) => item.SetCurrentValue(DragableItem.XProperty, coord);
                    _canvasDependencyProperty = Canvas.LeftProperty;
                    break;
                case Orientation.Vertical:
                    _getDesiredSize = item => item.DesiredSize.Height;
                    _getLocation = item => item.Y;
                    _setLocation = (item, coord) => item.SetCurrentValue(DragableItem.YProperty, coord);
                    _canvasDependencyProperty = Canvas.TopProperty;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("orientation");
            }
        }

        #region LocationInfo

        private class LocationInfo
        {
            private readonly DragableItem _item;
            private readonly double _start;
            private readonly double _mid;
            private readonly double _end;

            public LocationInfo(DragableItem item, double start, double mid, double end)
            {
                _item = item;
                _start = start;
                _mid = mid;
                _end = end;
            }

            public double Start
            {
                get { return _start; }
            }

            public double Mid
            {
                get { return _mid; }
            }

            public double End
            {
                get { return _end; }
            }

            public DragableItem Item
            {
                get { return _item; }
            }
        }

        #endregion

        public virtual Orientation Orientation
        {
            get { return _orientation; }
        }

        public virtual void Organise(DragableItemsControl requestor, Size measureBounds, IEnumerable<DragableItem> items)
        {
            if (items == null) throw new ArgumentNullException("items");

            OrganiseInternal(
                requestor,
                measureBounds,
                items.Select((di, idx) => new Tuple<int, DragableItem>(idx, di))
                        .OrderBy(tuple => tuple, MultiComparer<Tuple<int, DragableItem>>.Ascending(tuple => _getLocation(tuple.Item2)).ThenAscending(tuple => tuple.Item1))
                        .Select(tuple => tuple.Item2));
        }

        public virtual void Organise(DragableItemsControl requestor, Size measureBounds, IOrderedEnumerable<DragableItem> items)
        {
            if (items == null) throw new ArgumentNullException("items");

            OrganiseInternal(requestor, measureBounds, items);
        }

        private void OrganiseInternal(DragableItemsControl requestor, Size measureBounds, IEnumerable<DragableItem> items)
        {
            var currentCoord = 0.0;
            var z = int.MaxValue;
            var logicalIndex = 0;
            foreach (var newItem in items)
            {
                Panel.SetZIndex(newItem, newItem.IsSelected ? int.MaxValue : --z);
                SetLocation(newItem, currentCoord);
                newItem.LogicalIndex = logicalIndex++;
                newItem.Measure(measureBounds);
                var desiredSize = _getDesiredSize(newItem);
                if (desiredSize == 0.0) desiredSize = 1.0; //no measure? create something to help sorting
                currentCoord += desiredSize + _itemOffset;
            }
        }


        public virtual void OrganiseOnMouseDownWithin(DragableItemsControl requestor, Size measureBounds, List<DragableItem> siblingItems, DragableItem dragablzItem)
        {

        }

        private IDictionary<DragableItem, LocationInfo> _siblingItemLocationOnDragStart;

        public virtual void OrganiseOnDragStarted(DragableItemsControl requestor, Size measureBounds,
            IEnumerable<DragableItem> siblingItems, DragableItem dragItem)
        {
            if (siblingItems == null) throw new ArgumentNullException("siblingItems");
            if (dragItem == null) throw new ArgumentNullException("dragItem");

            _siblingItemLocationOnDragStart = siblingItems.Select(GetLocationInfo).ToDictionary(loc => loc.Item);
        }

        public virtual void OrganiseOnDrag(DragableItemsControl requestor, Size measureBounds, IEnumerable<DragableItem> siblingItems, DragableItem dragItem)
        {
            if (siblingItems == null) throw new ArgumentNullException("siblingItems");
            if (dragItem == null) throw new ArgumentNullException("dragItem");

            var currentLocations = siblingItems
                .Select(GetLocationInfo)
                .Union(new[] { GetLocationInfo(dragItem) })
                .OrderBy(loc => loc.Item == dragItem ? loc.Start : _siblingItemLocationOnDragStart[loc.Item].Start);

            var currentCoord = 0.0;
            var zIndex = int.MaxValue;
            foreach (var location in currentLocations)
            {
                if (!Equals(location.Item, dragItem))
                {
                    SendToLocation(location.Item, currentCoord);
                    Panel.SetZIndex(location.Item, --zIndex);
                }
                currentCoord += _getDesiredSize(location.Item) + _itemOffset;
            }
            Panel.SetZIndex(dragItem, int.MaxValue);
        }

        public virtual void OrganiseOnDragCompleted(DragableItemsControl requestor, Size measureBounds, IEnumerable<DragableItem> siblingItems, DragableItem dragItem)
        {
            if (siblingItems == null) throw new ArgumentNullException("siblingItems");
            var currentLocations = siblingItems
                .Select(GetLocationInfo)
                .Union(new[] { GetLocationInfo(dragItem) })
                .OrderBy(loc => loc.Item == dragItem ? loc.Start : _siblingItemLocationOnDragStart[loc.Item].Start);

            var currentCoord = 0.0;
            var z = int.MaxValue;
            var logicalIndex = 0;
            foreach (var location in currentLocations)
            {
                SetLocation(location.Item, currentCoord);
                currentCoord += _getDesiredSize(location.Item) + _itemOffset;
                Panel.SetZIndex(location.Item, --z);
                location.Item.LogicalIndex = logicalIndex++;
            }
            Panel.SetZIndex(dragItem, int.MaxValue);
        }

        public virtual Point ConstrainLocation(DragableItemsControl requestor, Size measureBounds, Point itemCurrentLocation, Size itemCurrentSize, Point itemDesiredLocation, Size itemDesiredSize)
        {
            var fixedItems = requestor.FixedItemCount;
            var lowerBound = fixedItems == 0
                ? -1d
                : GetLocationInfo(requestor.DragablzItems()
                    .Take(fixedItems)
                    .Last()).End + _itemOffset - 1;

            return new Point(
                _orientation == Orientation.Vertical
                    ? 0
                    : Math.Min(Math.Max(lowerBound, itemDesiredLocation.X), (measureBounds.Width) + 1),
                _orientation == Orientation.Horizontal
                    ? 0
                    : Math.Min(Math.Max(lowerBound, itemDesiredLocation.Y), (measureBounds.Height) + 1)
                );
        }

        public virtual Size Measure(DragableItemsControl requestor, Size availableSize, IEnumerable<DragableItem> items)
        {
            if (items == null) throw new ArgumentNullException("items");

            var size = new Size(double.PositiveInfinity, double.PositiveInfinity);

            double width = 0, height = 0;
            var isFirst = true;
            foreach (var dragablzItem in items)
            {
                dragablzItem.Measure(size);
                if (_orientation == Orientation.Horizontal)
                {
                    width += !dragablzItem.IsLoaded ? dragablzItem.DesiredSize.Width : dragablzItem.ActualWidth;
                    if (!isFirst)
                        width += _itemOffset;
                    height = Math.Max(height, !dragablzItem.IsLoaded ? dragablzItem.DesiredSize.Height : dragablzItem.ActualHeight);
                }
                else
                {
                    width = Math.Max(width, !dragablzItem.IsLoaded ? dragablzItem.DesiredSize.Width : dragablzItem.ActualWidth);
                    height += !dragablzItem.IsLoaded ? dragablzItem.DesiredSize.Height : dragablzItem.ActualHeight;
                    if (!isFirst)
                        height += _itemOffset;
                }

                isFirst = false;
            }

            return new Size(Math.Max(width, 0), Math.Max(height, 0));
        }

        public virtual IEnumerable<DragableItem> Sort(IEnumerable<DragableItem> items)
        {
            if (items == null) throw new ArgumentNullException("items");

            return items.OrderBy(i => GetLocationInfo(i).Start);
        }

        private void SetLocation(DragableItem dragablzItem, double location)
        {
            _setLocation(dragablzItem, location);
        }

        private void SendToLocation(DragableItem dragablzItem, double location)
        {
            double activeTarget;
            if (Math.Abs(_getLocation(dragablzItem) - location) < 1.0 ||
                _activeStoryboardTargetLocations.TryGetValue(dragablzItem, out activeTarget) &&
                Math.Abs(activeTarget - location) < 1.0)
            {
                return;
            }

            _activeStoryboardTargetLocations[dragablzItem] = location;

            var storyboard = new Storyboard { FillBehavior = FillBehavior.Stop };
            storyboard.WhenComplete(sb =>
            {
                _setLocation(dragablzItem, location);
                sb.Remove(dragablzItem);
                _activeStoryboardTargetLocations.Remove(dragablzItem);
            });

            var timeline = new DoubleAnimationUsingKeyFrames();
            timeline.SetValue(Storyboard.TargetPropertyProperty, new PropertyPath(_canvasDependencyProperty));
            timeline.KeyFrames.Add(new EasingDoubleKeyFrame(location, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200)))
            {
                EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
            });

            storyboard.Children.Add(timeline);
            storyboard.Begin(dragablzItem, true);
        }

        private LocationInfo GetLocationInfo(DragableItem item)
        {
            var size = _getDesiredSize(item);
            double startLocation;
            if (!_activeStoryboardTargetLocations.TryGetValue(item, out startLocation))
                startLocation = _getLocation(item);
            var midLocation = startLocation + size / 2;
            var endLocation = startLocation + size;

            return new LocationInfo(item, startLocation, midLocation, endLocation);
        }
    }
}
