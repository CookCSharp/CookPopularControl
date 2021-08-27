using CookPopularControl.Communal.Data;
using CookPopularControl.Communal.Interface;
using CookPopularControl.Controls.Dragables.Core;
using CookPopularControl.References;
using CookPopularControl.Tools.Extensions;
using CookPopularControl.Tools.Extensions.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DragableItemsControl
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 10:34:46
 */
namespace CookPopularControl.Controls.Dragables
{
    /// <summary>
    /// 可拖拽的集合控件
    /// </summary>
    public class DragableItemsControl : ItemsControl
    {
        private object[] _previousSortQueryResult;
        internal CustomContainer ContainerCustomisations { get; set; }
        internal Size? LockedMeasure { get; set; }


        public int FixedItemCount
        {
            get { return (int)GetValue(FixedItemCountProperty); }
            set { SetValue(FixedItemCountProperty, value); }
        }
        public static readonly DependencyProperty FixedItemCountProperty =
            DependencyProperty.Register("FixedItemCount", typeof(int), typeof(DragableItemsControl), new PropertyMetadata(default(int)));


        public IItemsOrganiser ItemsOrganiser
        {
            get { return (IItemsOrganiser)GetValue(ItemsOrganiserProperty); }
            set { SetValue(ItemsOrganiserProperty, value); }
        }
        public static readonly DependencyProperty ItemsOrganiserProperty =
            DependencyProperty.Register("ItemsOrganiser", typeof(IItemsOrganiser), typeof(DragableItemsControl), new PropertyMetadata(default(IItemsOrganiser)));


        public PositionMonitor PositionMonitor
        {
            get { return (PositionMonitor)GetValue(PositionMonitorProperty); }
            set { SetValue(PositionMonitorProperty, value); }
        }
        public static readonly DependencyProperty PositionMonitorProperty =
            DependencyProperty.Register("PositionMonitor", typeof(PositionMonitor), typeof(DragableItemsControl), new PropertyMetadata(default(PositionMonitor)));


        private static readonly DependencyPropertyKey ItemsPresenterWidthPropertyKey =
            DependencyProperty.RegisterReadOnly("ItemsPresenterWidth", typeof(double), typeof(DragableItemsControl), new PropertyMetadata(default(double)));
        public double ItemsPresenterWidth
        {
            get { return (double)GetValue(ItemsPresenterWidthProperty); }
            private set { SetValue(ItemsPresenterWidthPropertyKey, value); }
        }
        public static readonly DependencyProperty ItemsPresenterWidthProperty = ItemsPresenterWidthPropertyKey.DependencyProperty;


        private static readonly DependencyPropertyKey ItemsPresenterHeightPropertyKey =
            DependencyProperty.RegisterReadOnly("ItemsPresenterHeight", typeof(double), typeof(DragableItemsControl), new PropertyMetadata(default(double)));
        public double ItemsPresenterHeight
        {
            get { return (double)GetValue(ItemsPresenterHeightProperty); }
            private set { SetValue(ItemsPresenterHeightPropertyKey, value); }
        }
        public static readonly DependencyProperty ItemsPresenterHeightProperty = ItemsPresenterHeightPropertyKey.DependencyProperty;


        static DragableItemsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DragableItemsControl), new FrameworkPropertyMetadata(typeof(DragableItemsControl)));
        }

        public DragableItemsControl()
        {
            ItemContainerGenerator.StatusChanged += ItemContainerGeneratorOnStatusChanged;
            ItemContainerGenerator.ItemsChanged += ItemContainerGeneratorOnItemsChanged;
            AddHandler(DragableItem.XChangedEvent, new RoutedPropertyChangedEventHandler<double>(ItemXChanged));
            AddHandler(DragableItem.YChangedEvent, new RoutedPropertyChangedEventHandler<double>(ItemYChanged));
            AddHandler(DragableItem.DragDelta, new DragableDragDeltaEventHandler(ItemDragDelta));
            AddHandler(DragableItem.DragCompleted, new DragableDragCompletedEventHandler(ItemDragCompleted));
            AddHandler(DragableItem.DragStarted, new DragableDragStartedEventHandler(ItemDragStarted));
            AddHandler(DragableItem.MouseDownWithinEvent, new DragableItemEventHandler(ItemMouseDownWithinHandlerTarget));
        }

        private void ItemContainerGeneratorOnStatusChanged(object sender, EventArgs eventArgs)
        {
            if (ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated) return;

            InvalidateMeasure();
            //extra kick
            Dispatcher.BeginInvoke(new Action(InvalidateMeasure), DispatcherPriority.Loaded);
        }

        private void ItemContainerGeneratorOnItemsChanged(object sender, ItemsChangedEventArgs itemsChangedEventArgs)
        {

        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            if (ContainerCustomisations != null && ContainerCustomisations.ClearingContainerForItemOverride != null)
                ContainerCustomisations.ClearingContainerForItemOverride(element, item);

            base.ClearContainerForItemOverride(element, item);

            ((DragableItem)element).SizeChanged -= ItemSizeChangedEventHandler;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                var dragablzItems = DragablzItems().ToList();
                if (ItemsOrganiser == null) return;
                ItemsOrganiser.Organise(this, new Size(ItemsPresenterWidth, ItemsPresenterHeight), dragablzItems);
                var measure = ItemsOrganiser.Measure(this, new Size(ActualWidth, ActualHeight), dragablzItems);
                ItemsPresenterWidth = measure.Width;
                ItemsPresenterHeight = measure.Height;
            }), DispatcherPriority.Input);
        }


        /// <summary>
        /// 将项添加到基础源，显示在呈现控件的特定位置
        /// </summary>
        /// <param name="item"></param>
        /// <param name="addLocationHint"></param>
        public void AddToSource(object item, AddLocationHint addLocationHint)
        {
            AddToSource(item, null, addLocationHint);
        }

        /// <summary>
        /// 将项添加到基础源，显示在呈现控件的特定位置
        /// </summary>
        /// <param name="item"></param>
        /// <param name="nearItem"></param>
        /// <param name="addLocationHint"></param>
        public void AddToSource(object item, object nearItem, AddLocationHint addLocationHint)
        {
            CollectionTeaser collectionTeaser;
            if (CollectionTeaser.TryCreate(ItemsSource, out collectionTeaser))
                collectionTeaser.Add(item);
            else
                Items.Add(item);
            MoveItem(new MoveItemRequest(item, nearItem, addLocationHint));
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            var dragablzItem = item as DragableItem;
            if (dragablzItem == null) return false;

            return true;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var result = ContainerCustomisations != null && ContainerCustomisations.GetContainerForItemOverride != null
                ? ContainerCustomisations.GetContainerForItemOverride()
                : new DragableItem();

            result.SizeChanged += ItemSizeChangedEventHandler;

            return result;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            if (ContainerCustomisations != null && ContainerCustomisations.PrepareContainerForItemOverride != null)
                ContainerCustomisations.PrepareContainerForItemOverride(element, item);

            base.PrepareContainerForItemOverride(element, item);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (ItemsOrganiser == null) return base.MeasureOverride(constraint);

            if (LockedMeasure.HasValue)
            {
                ItemsPresenterWidth = LockedMeasure.Value.Width;
                ItemsPresenterHeight = LockedMeasure.Value.Height;
                return LockedMeasure.Value;
            }

            var dragablzItems = DragablzItems().ToList();
            var maxConstraint = new Size(double.PositiveInfinity, double.PositiveInfinity);

            ItemsOrganiser.Organise(this, maxConstraint, dragablzItems);
            var measure = ItemsOrganiser.Measure(this, new Size(ActualWidth, ActualHeight), dragablzItems);

            ItemsPresenterWidth = measure.Width;
            ItemsPresenterHeight = measure.Height;

            var width = double.IsInfinity(constraint.Width) ? measure.Width : constraint.Width;
            var height = double.IsInfinity(constraint.Height) ? measure.Height : constraint.Height;

            return new Size(width, height);
        }

        internal void InstigateDrag(object item, Action<DragableItem> continuation)
        {
            var dragablzItem = (DragableItem)ItemContainerGenerator.ContainerFromItem(item);
            dragablzItem.InstigateDrag(continuation);
        }

        /// <summary>
        /// Move an item in the rendered layout.
        /// </summary>
        /// <param name="moveItemRequest"></param>
        public void MoveItem(MoveItemRequest moveItemRequest)
        {
            if (moveItemRequest == null) throw new ArgumentNullException("moveItemRequest");

            if (ItemsOrganiser == null) return;

            var dragablzItem = moveItemRequest.Item as DragableItem ??
                               ItemContainerGenerator.ContainerFromItem(moveItemRequest.Item) as DragableItem;
            var contextDragablzItem = moveItemRequest.Context as DragableItem ??
                               ItemContainerGenerator.ContainerFromItem(moveItemRequest.Context) as DragableItem;

            if (dragablzItem == null) return;

            var sortedItems = DragablzItems().OrderBy(di => di.LogicalIndex).ToList();
            sortedItems.Remove(dragablzItem);

            switch (moveItemRequest.AddLocationHint)
            {
                case AddLocationHint.First:
                    sortedItems.Insert(0, dragablzItem);
                    break;
                case AddLocationHint.Last:
                    sortedItems.Add(dragablzItem);
                    break;
                case AddLocationHint.Prior:
                case AddLocationHint.After:
                    if (contextDragablzItem == null)
                        return;

                    var contextIndex = sortedItems.IndexOf(contextDragablzItem);
                    sortedItems.Insert(moveItemRequest.AddLocationHint == AddLocationHint.Prior ? contextIndex : contextIndex + 1, dragablzItem);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //TODO might not be too great for perf on larger lists
            var orderedEnumerable = sortedItems.OrderBy(di => sortedItems.IndexOf(di));

            ItemsOrganiser.Organise(this, new Size(ItemsPresenterWidth, ItemsPresenterHeight), orderedEnumerable);
        }

        internal IEnumerable<DragableItem> DragablzItems()
        {
            return this.Containers<DragableItem>().ToList();
        }

        private void ItemDragStarted(object sender, DragableDragStartedEventArgs eventArgs)
        {
            if (ItemsOrganiser != null)
            {
                var bounds = new Size(ActualWidth, ActualHeight);
                ItemsOrganiser.OrganiseOnDragStarted(this, bounds, DragablzItems().Except(new[] { eventArgs.DragableItem }).ToList(), eventArgs.DragableItem);
            }

            eventArgs.Handled = true;

            Dispatcher.BeginInvoke(new Action(InvalidateMeasure), DispatcherPriority.Loaded);
        }

        private void ItemDragCompleted(object sender, DragableDragCompletedEventArgs eventArgs)
        {
            var dragablzItems = DragablzItems()
                .Select(i =>
                {
                    i.IsDragging = false;
                    i.IsSiblingDragging = false;
                    return i;
                })
                .ToList();

            if (ItemsOrganiser != null)
            {
                var bounds = new Size(ActualWidth, ActualHeight);
                ItemsOrganiser.OrganiseOnDragCompleted(this, bounds, dragablzItems.Except(eventArgs.DragableItem), eventArgs.DragableItem);
            }

            eventArgs.Handled = true;

            //wowsers
            Dispatcher.BeginInvoke(new Action(InvalidateMeasure));
            Dispatcher.BeginInvoke(new Action(InvalidateMeasure), DispatcherPriority.Loaded);
        }

        private void ItemDragDelta(object sender, DragableDragDeltaEventArgs eventArgs)
        {
            var bounds = new Size(ItemsPresenterWidth, ItemsPresenterHeight);
            var desiredLocation = new Point(
                eventArgs.DragableItem.X + eventArgs.DragDeltaEventArgs.HorizontalChange,
                eventArgs.DragableItem.Y + eventArgs.DragDeltaEventArgs.VerticalChange
                );
            if (ItemsOrganiser != null)
            {
                if (FixedItemCount > 0 && ItemsOrganiser.Sort(DragablzItems()).Take(FixedItemCount).Contains(eventArgs.DragableItem))
                {
                    eventArgs.Handled = true;
                    return;
                }

                desiredLocation = ItemsOrganiser.ConstrainLocation(this, bounds,
                    new Point(eventArgs.DragableItem.X, eventArgs.DragableItem.Y),
                    new Size(eventArgs.DragableItem.ActualWidth, eventArgs.DragableItem.ActualHeight),
                    desiredLocation, eventArgs.DragableItem.DesiredSize);
            }

            foreach (var dragableItem in DragablzItems().Except(new[] { eventArgs.DragableItem })) // how about Linq.Where() ?
            {
                dragableItem.IsSiblingDragging = true;
            }
            eventArgs.DragableItem.IsDragging = true;

            eventArgs.DragableItem.X = desiredLocation.X;
            eventArgs.DragableItem.Y = desiredLocation.Y;

            if (ItemsOrganiser != null)
                ItemsOrganiser.OrganiseOnDrag(this, bounds, DragablzItems().Except(new[] { eventArgs.DragableItem }), eventArgs.DragableItem);

            eventArgs.DragableItem.BringIntoView();

            eventArgs.Handled = true;
        }

        private void ItemXChanged(object sender, RoutedPropertyChangedEventArgs<double> routedPropertyChangedEventArgs)
        {
            UpdateMonitor(routedPropertyChangedEventArgs);
        }

        private void ItemYChanged(object sender, RoutedPropertyChangedEventArgs<double> routedPropertyChangedEventArgs)
        {
            UpdateMonitor(routedPropertyChangedEventArgs);
        }

        private void UpdateMonitor(RoutedEventArgs routedPropertyChangedEventArgs)
        {
            if (PositionMonitor == null) return;

            var dragablzItem = (DragableItem)routedPropertyChangedEventArgs.OriginalSource;

            if (!Equals(ItemsControlFromItemContainer(dragablzItem), this)) return;

            PositionMonitor.OnLocationChanged(new LocationChangedEventArgs(dragablzItem.Content, new Point(dragablzItem.X, dragablzItem.Y)));

            var linearPositionMonitor = PositionMonitor as StackPositionMonitor;
            if (linearPositionMonitor == null) return;

            var sortedItems = linearPositionMonitor.Sort(this.Containers<DragableItem>()).Select(di => di.Content).ToArray();
            if (_previousSortQueryResult == null || !_previousSortQueryResult.SequenceEqual(sortedItems))
                linearPositionMonitor.OnOrderChanged(new OrderChangedEventArgs(_previousSortQueryResult, sortedItems));

            _previousSortQueryResult = sortedItems;
        }

        private void ItemMouseDownWithinHandlerTarget(object sender, DragableItemEventArgs e)
        {
            if (ItemsOrganiser == null) return;

            var bounds = new Size(ActualWidth, ActualHeight);
            ItemsOrganiser.OrganiseOnMouseDownWithin(this, bounds, DragablzItems().Except(e.DragableItem).ToList(), e.DragableItem);
        }

        private void ItemSizeChangedEventHandler(object sender, SizeChangedEventArgs e)
        {
            InvalidateMeasure();
            //extra kick
            Dispatcher.BeginInvoke(new Action(InvalidateMeasure), DispatcherPriority.Loaded);
        }
    }
}
