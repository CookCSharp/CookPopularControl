using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Threading;


namespace CookPopularControl.Controls.Dragables
{
    /// <summary>
    /// Items control which typically uses a canvas and 
    /// </summary>
    public class DragableItemsControl : ItemsControl
    {
        private object[] _previousSortQueryResult;
        internal ContainerCustomisations ContainerCustomisations { get; set; }
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


        public double ItemsPresenterWidth
        {
            get { return (double)GetValue(ItemsPresenterWidthProperty); }
            private set { SetValue(ItemsPresenterWidthPropertyKey, value); }
        }
        private static readonly DependencyPropertyKey ItemsPresenterWidthPropertyKey =
            DependencyProperty.RegisterReadOnly("ItemsPresenterWidth", typeof(double), typeof(DragableItemsControl), new PropertyMetadata(default(double)));
        public static readonly DependencyProperty ItemsPresenterWidthProperty = ItemsPresenterWidthPropertyKey.DependencyProperty;


        public double ItemsPresenterHeight
        {
            get { return (double)GetValue(ItemsPresenterHeightProperty); }
            private set { SetValue(ItemsPresenterHeightPropertyKey, value); }
        }
        private static readonly DependencyPropertyKey ItemsPresenterHeightPropertyKey =
            DependencyProperty.RegisterReadOnly("ItemsPresenterHeight", typeof(double), typeof(DragableItemsControl), new PropertyMetadata(default(double)));
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


        private void ItemDragDelta(object sender, DragableDragDeltaEventArgs eventArgs)
        {
            var bounds = new Size(ItemsPresenterWidth, ItemsPresenterHeight);
            var desiredLocation = new Point(
                eventArgs.DragableItem.X + eventArgs.DragDeltaEventArgs.HorizontalChange,
                eventArgs.DragableItem.Y + eventArgs.DragDeltaEventArgs.VerticalChange
                );
            if (ItemsOrganiser != null)
            {
                if (FixedItemCount > 0 && ItemsOrganiser.Sort(DragableItems()).Take(FixedItemCount).Contains(eventArgs.DragableItem))
                {
                    eventArgs.Handled = true;
                    return;
                }

                desiredLocation = ItemsOrganiser.ConstrainLocation(this, bounds,
                    new Point(eventArgs.DragableItem.X, eventArgs.DragableItem.Y),
                    new Size(eventArgs.DragableItem.ActualWidth, eventArgs.DragableItem.ActualHeight),
                    desiredLocation, eventArgs.DragableItem.DesiredSize);
            }

            foreach (var dragableItem in DragableItems().Except(new[] { eventArgs.DragableItem })) // how about Linq.Where() ?
            {
                dragableItem.IsSiblingDragging = true;
            }
            eventArgs.DragableItem.IsDragging = true;

            eventArgs.DragableItem.X = desiredLocation.X;
            eventArgs.DragableItem.Y = desiredLocation.Y;

            if (ItemsOrganiser != null)
                ItemsOrganiser.OrganiseOnDrag(this, bounds, DragableItems().Except(new[] { eventArgs.DragableItem }), eventArgs.DragableItem);

            eventArgs.DragableItem.BringIntoView();

            eventArgs.Handled = true;
        }

        private void ItemDragCompleted(object sender, DragableDragCompletedEventArgs eventArgs)
        {
            var dragableItems = DragableItems()
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
                ItemsOrganiser.OrganiseOnDragCompleted(this, bounds,
                    dragableItems.Except(eventArgs.DragableItem),
                    eventArgs.DragableItem);
            }

            eventArgs.Handled = true;

            //wowsers
            Dispatcher.BeginInvoke(new Action(InvalidateMeasure));
            Dispatcher.BeginInvoke(new Action(InvalidateMeasure), DispatcherPriority.Loaded);
        }

        private void ItemDragStarted(object sender, DragableDragStartedEventArgs eventArgs)
        {
            if (ItemsOrganiser != null)
            {
                var bounds = new Size(ActualWidth, ActualHeight);
                ItemsOrganiser.OrganiseOnDragStarted(this, bounds,
                    DragableItems().Except(new[] { eventArgs.DragableItem }).ToList(),
                    eventArgs.DragableItem);
            }

            eventArgs.Handled = true;

            Dispatcher.BeginInvoke(new Action(InvalidateMeasure), DispatcherPriority.Loaded);
        }

        private void ItemMouseDownWithinHandlerTarget(object sender, DragableItemEventArgs e)
        {
            if (ItemsOrganiser == null) return;

            var bounds = new Size(ActualWidth, ActualHeight);
            ItemsOrganiser.OrganiseOnMouseDownWithin(this, bounds, DragableItems().Except(e.DragableItem).ToList(), e.DragableItem);
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            if (ContainerCustomisations != null && ContainerCustomisations.ClearingContainerForItemOverride != null)
                ContainerCustomisations.ClearingContainerForItemOverride(element, item);

            base.ClearContainerForItemOverride(element, item);

            ((DragableItem)element).SizeChanged -= ItemSizeChangedEventHandler;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                var dragableItems = DragableItems().ToList();
                if (ItemsOrganiser == null) return;
                ItemsOrganiser.Organise(this, new Size(ItemsPresenterWidth, ItemsPresenterHeight), dragableItems);
                var measure = ItemsOrganiser.Measure(this, new Size(ActualWidth, ActualHeight), dragableItems);
                ItemsPresenterWidth = measure.Width;
                ItemsPresenterHeight = measure.Height;
            }), DispatcherPriority.Input);
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

        private void ItemSizeChangedEventHandler(object sender, SizeChangedEventArgs e)
        {
            InvalidateMeasure();
            //extra kick
            Dispatcher.BeginInvoke(new Action(InvalidateMeasure), DispatcherPriority.Loaded);
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

            var dragableItems = DragableItems().ToList();
            var maxConstraint = new Size(double.PositiveInfinity, double.PositiveInfinity);

            ItemsOrganiser.Organise(this, maxConstraint, dragableItems);
            var measure = ItemsOrganiser.Measure(this, new Size(ActualWidth, ActualHeight), dragableItems);

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

        internal IEnumerable<DragableItem> DragableItems()
        {
            return this.Containers<DragableItem>().ToList();
        }

        /// <summary>
        /// Adds an item to the underlying source, displaying in a specific position in rendered control.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="addLocationHint"></param>
        public void AddToSource(object item, AddLocationHint addLocationHint)
        {
            AddToSource(item, null, addLocationHint);
        }

        /// <summary>
        /// Adds an item to the underlying source, displaying in a specific position in rendered control.
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

        /// <summary>
        /// Move an item in the rendered layout.
        /// </summary>
        /// <param name="moveItemRequest"></param>
        public void MoveItem(MoveItemRequest moveItemRequest)
        {
            if (moveItemRequest == null) throw new ArgumentNullException("moveItemRequest");

            if (ItemsOrganiser == null) return;

            var dragablzItem = moveItemRequest.Item as DragableItem ??
                               ItemContainerGenerator.ContainerFromItem(
                                   moveItemRequest.Item) as DragableItem;
            var contextDragableItem = moveItemRequest.Context as DragableItem ??
                               ItemContainerGenerator.ContainerFromItem(
                                   moveItemRequest.Context) as DragableItem;

            if (dragablzItem == null) return;

            var sortedItems = DragableItems().OrderBy(di => di.LogicalIndex).ToList();
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
                    if (contextDragableItem == null)
                        return;

                    var contextIndex = sortedItems.IndexOf(contextDragableItem);
                    sortedItems.Insert(moveItemRequest.AddLocationHint == AddLocationHint.Prior ? contextIndex : contextIndex + 1, dragablzItem);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //TODO might not be too great for perf on larger lists
            var orderedEnumerable = sortedItems.OrderBy(di => sortedItems.IndexOf(di));

            ItemsOrganiser.Organise(this, new Size(ItemsPresenterWidth, ItemsPresenterHeight), orderedEnumerable);
        }
    }
}
