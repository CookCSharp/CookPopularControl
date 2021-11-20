using CookPopularControl.Communal.Data;
using CookPopularControl.Controls.Dragables;
using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows;
using CookPopularCSharpToolkit.Windows.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DragableTabControl
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 10:03:31
 */
namespace CookPopularControl.Controls.Dragables
{
    /// <summary>
    /// 扩展选项卡控件支持选项卡重新定位、拖放
    /// 使用常用的WPF技术来跨选项卡持久化可视树
    /// </summary>
    [TemplatePart(Name = HeaderItemsControlPartName, Type = typeof(DragableItemsControl))]
    [TemplatePart(Name = ItemsHolderPartName, Type = typeof(Panel))]
    public class DragableTabControl : System.Windows.Controls.TabControl
    {
        public const string HeaderItemsControlPartName = "PART_HeaderItemsControl";
        public const string ItemsHolderPartName = "PART_ItemsHolder";

        public static RoutedCommand CloseItemCommand = new RoutedUICommand("Close", "Close", typeof(DragableTabControl));
        public static RoutedCommand AddItemCommand = new RoutedUICommand("Add", "Add", typeof(DragableTabControl));

        private static readonly HashSet<DragableTabControl> LoadedInstances = new HashSet<DragableTabControl>();
        private static readonly HashSet<DragableTabControl> VisibleInstances = new HashSet<DragableTabControl>();

        private Panel _itemsHolder;
        private TabItemHeaderDragStartInformation _tabItemHeaderDragStartInformation;
        private WeakReference _previousSelection;
        private DragableItemsControl _dragablzItemsControl;
        private IDisposable _templateSubscription;
        private readonly SerialDisposable _windowSubscription = new SerialDisposable();
        private InterTabTransfer _interTabTransfer;


        static DragableTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DragableTabControl), new FrameworkPropertyMetadata(typeof(DragableTabControl)));
            CommandManager.RegisterClassCommandBinding(typeof(FrameworkElement), new CommandBinding(CloseItemCommand, CloseItemClassHandler, CloseItemCanExecuteClassHandler));
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DragableTabControl()
        {
            AddHandler(DragableItem.DragStarted, new DragableDragStartedEventHandler(ItemDragStarted), true);
            AddHandler(DragableItem.PreviewDragDelta, new DragableDragDeltaEventHandler(PreviewItemDragDelta), true);
            AddHandler(DragableItem.DragDelta, new DragableDragDeltaEventHandler(ItemDragDelta), true);
            AddHandler(DragableItem.DragCompleted, new DragableDragCompletedEventHandler(ItemDragCompleted), true);
            CommandBindings.Add(new CommandBinding(AddItemCommand, AddItemHandler));

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            IsVisibleChanged += OnIsVisibleChanged;
        }

        /// <summary>
        /// Helper method which returns all the currently loaded instances.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DragableTabControl> GetLoadedInstances()
        {
            return LoadedInstances.Union(VisibleInstances).Distinct().ToList();
        }

        /// <summary>
        /// Helper method to close all tabs where the item is the tab's content (helpful with MVVM scenarios)
        /// </summary>
        /// <remarks>
        /// In MVVM scenarios where you don't want to bind the routed command to your ViewModel,
        /// with this helper method and embedding the DragableTabControl in a UserControl, you can keep
        /// the View-specific dependencies out of the ViewModel.
        /// </remarks>
        /// <param name="tabContentItem">An existing Tab item content (a ViewModel in MVVM scenarios) which is backing a tab control</param>
        public static void CloseItem(object tabContentItem)
        {
            if (tabContentItem == null) return; //Do nothing.

            //Find all loaded DragableTabControl instances with tabs backed by this item and close them
            foreach (var tabWithItemContent in
                GetLoadedInstances().SelectMany(tc =>
                tc._dragablzItemsControl.DragableItems().Where(di => di.Content.Equals(tabContentItem)).Select(di => new { tc, di })))
            {
                DragableTabControl.CloseItem(tabWithItemContent.di, tabWithItemContent.tc);
            }
        }

        /// <summary>
        /// Helper method to add an item next to an existing item.
        /// </summary>
        /// <remarks>
        /// Due to the organisable nature of the control, the order of items may not reflect the order in the source collection.  This method
        /// will add items to the source collection, managing their initial appearance on screen at the same time. 
        /// If you are using a <see cref="InterTabController.InterTabClient"/> this will be used to add the item into the source collection.
        /// </remarks>
        /// <param name="item">New item to add.</param>
        /// <param name="nearItem">Existing object/tab item content which defines which tab control should be used to add the object.</param>
        /// <param name="addLocationHint">Location, relative to the <paramref name="nearItem"/> object</param>
        public static void AddItem(object item, object nearItem, AddLocationHint addLocationHint)
        {
            if (nearItem == null) throw new ArgumentNullException("nearItem");

            var existingLocation = GetLoadedInstances().SelectMany(tabControl =>
                (tabControl.ItemsSource ?? tabControl.Items).OfType<object>()
                    .Select(existingObject => new { tabControl, existingObject }))
                .SingleOrDefault(a => nearItem.Equals(a.existingObject));

            if (existingLocation == null)
                throw new ArgumentException("Did not find precisely one instance of adjacentTo", "nearItem");

            existingLocation.tabControl.AddToSource(item);
            if (existingLocation.tabControl._dragablzItemsControl != null)
                existingLocation.tabControl._dragablzItemsControl.MoveItem(new MoveItemRequest(item, nearItem, addLocationHint));
        }

        /// <summary>
        /// Finds and selects an item.
        /// </summary>
        /// <param name="item"></param>
        public static void SelectItem(object item)
        {
            var existingLocation = GetLoadedInstances().SelectMany(tabControl =>
                (tabControl.ItemsSource ?? tabControl.Items).OfType<object>()
                    .Select(existingObject => new { tabControl, existingObject }))
                    .FirstOrDefault(a => item.Equals(a.existingObject));

            if (existingLocation == null) return;

            existingLocation.tabControl.SelectedItem = item;
        }


        public double AdjacentHeaderItemOffset
        {
            get { return (double)GetValue(AdjacentHeaderItemOffsetProperty); }
            set { SetValue(AdjacentHeaderItemOffsetProperty, value); }
        }
        public static readonly DependencyProperty AdjacentHeaderItemOffsetProperty =
            DependencyProperty.Register("AdjacentHeaderItemOffset", typeof(double), typeof(DragableTabControl), new PropertyMetadata(default(double), AdjacentHeaderItemOffsetPropertyChangedCallback));
        private static void AdjacentHeaderItemOffsetPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            dependencyObject.SetValue(HeaderItemsOrganiserProperty, new HorizontalOrganiser((double)dependencyPropertyChangedEventArgs.NewValue));
        }


        public IItemsOrganiser HeaderItemsOrganiser
        {
            get { return (IItemsOrganiser)GetValue(HeaderItemsOrganiserProperty); }
            set { SetValue(HeaderItemsOrganiserProperty, value); }
        }
        public static readonly DependencyProperty HeaderItemsOrganiserProperty =
            DependencyProperty.Register("HeaderItemsOrganiser", typeof(IItemsOrganiser), typeof(DragableTabControl), new PropertyMetadata(new HorizontalOrganiser()));


        public string HeaderMemberPath
        {
            get { return (string)GetValue(HeaderMemberPathProperty); }
            set { SetValue(HeaderMemberPathProperty, value); }
        }
        public static readonly DependencyProperty HeaderMemberPathProperty =
            DependencyProperty.Register("HeaderMemberPath", typeof(string), typeof(DragableTabControl), new PropertyMetadata(default(string)));


        public DataTemplate HeaderItemTemplate
        {
            get { return (DataTemplate)GetValue(HeaderItemTemplateProperty); }
            set { SetValue(HeaderItemTemplateProperty, value); }
        }
        public static readonly DependencyProperty HeaderItemTemplateProperty =
            DependencyProperty.Register("HeaderItemTemplate", typeof(DataTemplate), typeof(DragableTabControl), new PropertyMetadata(default(DataTemplate)));


        public object HeaderPrefixContent
        {
            get { return GetValue(HeaderPrefixContentProperty); }
            set { SetValue(HeaderPrefixContentProperty, value); }
        }
        public static readonly DependencyProperty HeaderPrefixContentProperty =
            DependencyProperty.Register("HeaderPrefixContent", typeof(object), typeof(DragableTabControl), new PropertyMetadata(default(object)));


        public string HeaderPrefixContentStringFormat
        {
            get { return (string)GetValue(HeaderPrefixContentStringFormatProperty); }
            set { SetValue(HeaderPrefixContentStringFormatProperty, value); }
        }
        public static readonly DependencyProperty HeaderPrefixContentStringFormatProperty =
            DependencyProperty.Register("HeaderPrefixContentStringFormat", typeof(string), typeof(DragableTabControl), new PropertyMetadata(default(string)));


        public DataTemplate HeaderPrefixContentTemplate
        {
            get { return (DataTemplate)GetValue(HeaderPrefixContentTemplateProperty); }
            set { SetValue(HeaderPrefixContentTemplateProperty, value); }
        }
        public static readonly DependencyProperty HeaderPrefixContentTemplateProperty =
            DependencyProperty.Register("HeaderPrefixContentTemplate", typeof(DataTemplate), typeof(DragableTabControl), new PropertyMetadata(default(DataTemplate)));


        public DataTemplateSelector HeaderPrefixContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(HeaderPrefixContentTemplateSelectorProperty); }
            set { SetValue(HeaderPrefixContentTemplateSelectorProperty, value); }
        }
        public static readonly DependencyProperty HeaderPrefixContentTemplateSelectorProperty =
            DependencyProperty.Register("HeaderPrefixContentTemplateSelector", typeof(DataTemplateSelector), typeof(DragableTabControl), new PropertyMetadata(default(DataTemplateSelector)));


        public object HeaderSuffixContent
        {
            get { return GetValue(HeaderSuffixContentProperty); }
            set { SetValue(HeaderSuffixContentProperty, value); }
        }
        public static readonly DependencyProperty HeaderSuffixContentProperty =
            DependencyProperty.Register("HeaderSuffixContent", typeof(object), typeof(DragableTabControl), new PropertyMetadata(default(object)));


        public string HeaderSuffixContentStringFormat
        {
            get { return (string)GetValue(HeaderSuffixContentStringFormatProperty); }
            set { SetValue(HeaderSuffixContentStringFormatProperty, value); }
        }
        public static readonly DependencyProperty HeaderSuffixContentStringFormatProperty =
            DependencyProperty.Register("HeaderSuffixContentStringFormat", typeof(string), typeof(DragableTabControl), new PropertyMetadata(default(string)));


        public DataTemplate HeaderSuffixContentTemplate
        {
            get { return (DataTemplate)GetValue(HeaderSuffixContentTemplateProperty); }
            set { SetValue(HeaderSuffixContentTemplateProperty, value); }
        }
        public static readonly DependencyProperty HeaderSuffixContentTemplateProperty =
            DependencyProperty.Register("HeaderSuffixContentTemplate", typeof(DataTemplate), typeof(DragableTabControl), new PropertyMetadata(default(DataTemplate)));


        public DataTemplateSelector HeaderSuffixContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(HeaderSuffixContentTemplateSelectorProperty); }
            set { SetValue(HeaderSuffixContentTemplateSelectorProperty, value); }
        }
        public static readonly DependencyProperty HeaderSuffixContentTemplateSelectorProperty =
            DependencyProperty.Register("HeaderSuffixContentTemplateSelector", typeof(DataTemplateSelector), typeof(DragableTabControl), new PropertyMetadata(default(DataTemplateSelector)));


        /// <summary>
        /// 标识是否应显示默认关闭按钮  
        /// 如果手动关闭可执行<see cref="DragableTabControl.CloseItemCommand"/>命令
        /// </summary>
        public bool ShowDefaultCloseButton
        {
            get { return (bool)GetValue(ShowDefaultCloseButtonProperty); }
            set { SetValue(ShowDefaultCloseButtonProperty, value); }
        }
        public static readonly DependencyProperty ShowDefaultCloseButtonProperty =
            DependencyProperty.Register("ShowDefaultCloseButton", typeof(bool), typeof(DragableTabControl), new PropertyMetadata(default(bool)));


        /// <summary>
        /// 标识是否应该显示默认的添加按钮
        /// 使用<see cref="AddItemCommand"/>可以添加<see cref="HeaderPrefixContent"/> or <see cref="HeaderSuffixContent"/>
        /// </summary>
        public bool ShowDefaultAddButton
        {
            get { return (bool)GetValue(ShowDefaultAddButtonProperty); }
            set { SetValue(ShowDefaultAddButtonProperty, value); }
        }
        public static readonly DependencyProperty ShowDefaultAddButtonProperty =
            DependencyProperty.Register("ShowDefaultAddButton", typeof(bool), typeof(DragableTabControl), new PropertyMetadata(default(bool)));


        /// <summary>
        /// 标识<see cref="DragableTabControl"/>的标头是否可见,默认True
        /// </summary>
        public bool IsHeaderPanelVisible
        {
            get { return (bool)GetValue(IsHeaderPanelVisibleProperty); }
            set { SetValue(IsHeaderPanelVisibleProperty, value); }
        }
        public static readonly DependencyProperty IsHeaderPanelVisibleProperty =
            DependencyProperty.Register("IsHeaderPanelVisible", typeof(bool), typeof(DragableTabControl), new PropertyMetadata(true));


        /// <summary>
        /// 获取或设置在标题中添加子项位置
        /// </summary>
        /// <remarks>
        /// 标题项的逻辑顺序可能与源项的内容不匹配，
        /// 所以这个属性允许控制新项应该出现在哪里
        /// </remarks>
        public AddLocationHint AddLocationHint
        {
            get { return (AddLocationHint)GetValue(AddLocationHintProperty); }
            set { SetValue(AddLocationHintProperty, value); }
        }
        public static readonly DependencyProperty AddLocationHintProperty =
            DependencyProperty.Register("AddLocationHint", typeof(AddLocationHint), typeof(DragableTabControl), new PropertyMetadata(AddLocationHint.Last));


        /// <summary>
        /// Allows a the first adjacent tabs to be fixed (no dragging, and default close button will not show).
        /// </summary>
        public int FixedHeaderCount
        {
            get { return (int)GetValue(FixedHeaderCountProperty); }
            set { SetValue(FixedHeaderCountProperty, value); }
        }
        public static readonly DependencyProperty FixedHeaderCountProperty =
            DependencyProperty.Register("FixedHeaderCount", typeof(int), typeof(DragableTabControl), new PropertyMetadata(default(int)));


        /// <summary>
        /// An <see cref="InterTabController"/> must be provided to enable tab tearing. Behaviour customisations can be applied
        /// vie the controller.
        /// </summary>
        public InterTabController InterTabController
        {
            get { return (InterTabController)GetValue(InterTabControllerProperty); }
            set { SetValue(InterTabControllerProperty, value); }
        }
        public static readonly DependencyProperty InterTabControllerProperty =
            DependencyProperty.Register("InterTabController", typeof(InterTabController), typeof(DragableTabControl), new PropertyMetadata(null, InterTabControllerPropertyChangedCallback));
        private static void InterTabControllerPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var instance = (DragableTabControl)dependencyObject;
            if (dependencyPropertyChangedEventArgs.OldValue != null)
                instance.RemoveLogicalChild(dependencyPropertyChangedEventArgs.OldValue);
            if (dependencyPropertyChangedEventArgs.NewValue != null)
                instance.AddLogicalChild(dependencyPropertyChangedEventArgs.NewValue);
        }


        /// <summary>
        /// Allows a factory to be provided for generating new items. Typically used in conjunction with <see cref="AddItemCommand"/>.
        /// </summary>
        public Func<object> NewItemFactory
        {
            get { return (Func<object>)GetValue(NewItemFactoryProperty); }
            set { SetValue(NewItemFactoryProperty, value); }
        }
        /// <summary>
        /// Allows a factory to be provided for generating new items. Typically used in conjunction with <see cref="AddItemCommand"/>.
        /// </summary>
        public static readonly DependencyProperty NewItemFactoryProperty =
            DependencyProperty.Register("NewItemFactory", typeof(Func<object>), typeof(DragableTabControl), new PropertyMetadata(default(Func<object>)));


        private static readonly DependencyPropertyKey IsEmptyPropertyKey =
            DependencyProperty.RegisterReadOnly("IsEmpty", typeof(bool), typeof(DragableTabControl), new PropertyMetadata(true, OnIsEmptyChanged));
        /// <summary>
        /// Indicates if there are no current tab items.
        /// </summary>
        public bool IsEmpty
        {
            get { return (bool)GetValue(IsEmptyProperty); }
            private set { SetValue(IsEmptyPropertyKey, value); }
        }
        public static readonly DependencyProperty IsEmptyProperty = IsEmptyPropertyKey.DependencyProperty;
        private static void OnIsEmptyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as DragableTabControl;
            var args = new RoutedPropertyChangedEventArgs<bool>((bool)e.OldValue, (bool)e.NewValue) { RoutedEvent = IsEmptyChangedEvent };
            instance?.RaiseEvent(args);
        }


        /// <summary>
        /// Raised when <see cref="IsEmpty"/> changes.
        /// </summary>
        public static readonly RoutedEvent IsEmptyChangedEvent =
            EventManager.RegisterRoutedEvent("IsEmptyChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<bool>), typeof(DragableTabControl));
        /// <summary>
        /// Event handler to list to <see cref="IsEmptyChangedEvent"/>.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<bool> IsEmptyChanged
        {
            add { AddHandler(IsEmptyChangedEvent, value); }
            remove { RemoveHandler(IsEmptyChangedEvent, value); }
        }


        /// <summary>
        /// Optionally allows a close item hook to be bound in.  If this propety is provided, the func must return true for the close to continue.
        /// </summary>
        public ItemActionCallback ClosingItemCallback
        {
            get { return (ItemActionCallback)GetValue(ClosingItemCallbackProperty); }
            set { SetValue(ClosingItemCallbackProperty, value); }
        }
        /// <summary>
        /// Optionally allows a close item hook to be bound in.  If this propety is provided, the func must return true for the close to continue.
        /// </summary>
        public static readonly DependencyProperty ClosingItemCallbackProperty =
            DependencyProperty.Register("ClosingItemCallback", typeof(ItemActionCallback), typeof(DragableTabControl), new PropertyMetadata(default(ItemActionCallback)));


        /// <summary>
        /// Set to <c>true</c> to have tabs automatically be moved to another tab is a window is closed, so that they arent lost.
        /// Can be useful for fixed/persistant tabs that may have been dragged into another Window.  You can further control
        /// this behaviour on a per tab item basis by providing <see cref="ConsolidatingOrphanedItemCallback" />.
        /// </summary>
        public bool ConsolidateOrphanedItems
        {
            get { return (bool)GetValue(ConsolidateOrphanedItemsProperty); }
            set { SetValue(ConsolidateOrphanedItemsProperty, value); }
        }
        /// <summary>
        /// Set to <c>true</c> to have tabs automatically be moved to another tab is a window is closed, so that they arent lost.
        /// Can be useful for fixed/persistant tabs that may have been dragged into another Window.  You can further control
        /// this behaviour on a per tab item basis by providing <see cref="ConsolidatingOrphanedItemCallback" />.
        /// </summary>
        public static readonly DependencyProperty ConsolidateOrphanedItemsProperty =
            DependencyProperty.Register("ConsolidateOrphanedItems", typeof(bool), typeof(DragableTabControl), new PropertyMetadata(default(bool)));


        /// <summary>
        /// Assuming <see cref="ConsolidateOrphanedItems"/> is set to <c>true</c>, consolidation of individual
        /// tab items can be cancelled by providing this call back and cancelling the <see cref="ItemActionCallbackArgs{TOwner}"/>
        /// instance.
        /// </summary>
        public ItemActionCallback ConsolidatingOrphanedItemCallback
        {
            get { return (ItemActionCallback)GetValue(ConsolidatingOrphanedItemCallbackProperty); }
            set { SetValue(ConsolidatingOrphanedItemCallbackProperty, value); }
        }
        /// <summary>
        /// Assuming <see cref="ConsolidateOrphanedItems"/> is set to <c>true</c>, consolidation of individual
        /// tab items can be cancelled by providing this call back and cancelling the <see cref="ItemActionCallbackArgs{TOwner}"/>
        /// instance.
        /// </summary>
        public static readonly DependencyProperty ConsolidatingOrphanedItemCallbackProperty =
            DependencyProperty.Register("ConsolidatingOrphanedItemCallback", typeof(ItemActionCallback), typeof(DragableTabControl), new PropertyMetadata(default(ItemActionCallback)));



        private static readonly DependencyPropertyKey IsDraggingWindowPropertyKey =
            DependencyProperty.RegisterReadOnly("IsDraggingWindow", typeof(bool), typeof(DragableTabControl), new PropertyMetadata(default(bool), OnIsDraggingWindowChanged));
        /// <summary>
        /// Readonly dependency property which indicates whether the owning <see cref="Window"/> is currently dragged 
        /// </summary>
        public bool IsDraggingWindow
        {
            get { return (bool)GetValue(IsDraggingWindowProperty); }
            private set { SetValue(IsDraggingWindowPropertyKey, value); }
        }
        public static readonly DependencyProperty IsDraggingWindowProperty = IsDraggingWindowPropertyKey.DependencyProperty;
        private static void OnIsDraggingWindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DragableTabControl)d;
            var args = new RoutedPropertyChangedEventArgs<bool>((bool)e.OldValue, (bool)e.NewValue) { RoutedEvent = IsDraggingWindowChangedEvent };
            instance.RaiseEvent(args);
        }


        /// <summary>
        /// Event indicating <see cref="IsDraggingWindow"/> has changed.
        /// </summary>
        public static readonly RoutedEvent IsDraggingWindowChangedEvent =
            EventManager.RegisterRoutedEvent("IsDraggingWindowChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<bool>), typeof(DragableTabControl));
        /// <summary>
        /// Event indicating <see cref="IsDraggingWindow"/> has changed.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<bool> IsDraggingWindowChanged
        {
            add { AddHandler(IsDraggingWindowChangedEvent, value); }
            remove { RemoveHandler(IsDraggingWindowChangedEvent, value); }
        }


        internal static void SetIsClosingAsPartOfDragOperation(Window element, bool value) => element.SetValue(IsClosingAsPartOfDragOperationProperty, value);
        /// <summary>
        /// Helper method which can tell you if a <see cref="Window"/> is being automatically closed due
        /// to a user instigated drag operation (typically when a single tab is dropped into another window.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool GetIsClosingAsPartOfDragOperation(Window element) => (bool)element.GetValue(IsClosingAsPartOfDragOperationProperty);
        /// <summary>
        /// Temporarily set by the framework if a users drag opration causes a Window to close (e.g if a tab is dragging into another tab).
        /// </summary>
        public static readonly DependencyProperty IsClosingAsPartOfDragOperationProperty =
            DependencyProperty.RegisterAttached("IsClosingAsPartOfDragOperation", typeof(bool), typeof(DragableTabControl), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.NotDataBindable));


        /// <summary>
        /// Provide a hint for how the header should size itself if there are no tabs left (and a Window is still open).
        /// </summary>
        public EmptyHeaderSizingHint EmptyHeaderSizingHint
        {
            get { return (EmptyHeaderSizingHint)GetValue(EmptyHeaderSizingHintProperty); }
            set { SetValue(EmptyHeaderSizingHintProperty, value); }
        }
        /// <summary>
        /// Provide a hint for how the header should size itself if there are no tabs left (and a Window is still open).
        /// </summary>
        public static readonly DependencyProperty EmptyHeaderSizingHintProperty =
            DependencyProperty.Register("EmptyHeaderSizingHint", typeof(EmptyHeaderSizingHint), typeof(DragableTabControl), new PropertyMetadata(default(EmptyHeaderSizingHint)));


        public static void SetIsWrappingTabItem(DependencyObject element, bool value) => element.SetValue(IsWrappingTabItemProperty, value);
        public static bool GetIsWrappingTabItem(DependencyObject element) => (bool)element.GetValue(IsWrappingTabItemProperty);
        public static readonly DependencyProperty IsWrappingTabItemProperty =
            DependencyProperty.RegisterAttached("IsWrappingTabItem", typeof(bool), typeof(DragableTabControl), new PropertyMetadata(default(bool)));


        /// <summary>
        /// Adds an item to the source collection.  If the InterTabController.InterTabClient is set that instance will be deferred to.
        /// Otherwise an attempt will be made to add to the <see cref="ItemsSource" /> property, and lastly <see cref="Items"/>.
        /// </summary>
        /// <param name="item"></param>
        public void AddToSource(object item)
        {
            if (item == null) throw new ArgumentNullException("item");

            var manualInterTabClient = InterTabController == null ? null : InterTabController.InterTabClient as IManualInterTabClient;
            if (manualInterTabClient != null)
            {
                manualInterTabClient.Add(item);
            }
            else
            {
                CollectionTeaser collectionTeaser;
                if (CollectionTeaser.TryCreate(ItemsSource, out collectionTeaser))
                    collectionTeaser.Add(item);
                else
                    Items.Add(item);
            }
        }

        /// <summary>
        /// Removes an item from the source collection.  If the InterTabController.InterTabClient is set that instance will be deferred to.
        /// Otherwise an attempt will be made to remove from the <see cref="ItemsSource" /> property, and lastly <see cref="Items"/>.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveFromSource(object item)
        {
            if (item == null) throw new ArgumentNullException("item");

            var manualInterTabClient = InterTabController == null ? null : InterTabController.InterTabClient as IManualInterTabClient;
            if (manualInterTabClient != null)
            {
                manualInterTabClient.Remove(item);
            }
            else
            {
                CollectionTeaser collectionTeaser;
                if (CollectionTeaser.TryCreate(ItemsSource, out collectionTeaser))
                    collectionTeaser.Remove(item);
                else
                    Items.Remove(item);
            }
        }

        /// <summary>
        /// Gets the header items, ordered according to their current visual position in the tab header.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DragableItem> GetOrderedHeaders()
        {
            return _dragablzItemsControl.ItemsOrganiser.Sort(_dragablzItemsControl.DragableItems());
        }

        /// <summary>
        /// Called when <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/> is called.
        /// </summary>
        public override void OnApplyTemplate()
        {
            _templateSubscription?.Dispose();
            _templateSubscription = Disposable.Empty;

            _dragablzItemsControl = GetTemplateChild(HeaderItemsControlPartName) as DragableItemsControl;
            if (_dragablzItemsControl != null)
            {
                _dragablzItemsControl.ItemContainerGenerator.StatusChanged += ItemContainerGeneratorOnStatusChanged;
                _templateSubscription =
                    Disposable.Create(
                        () =>
                            _dragablzItemsControl.ItemContainerGenerator.StatusChanged -=
                                ItemContainerGeneratorOnStatusChanged);

                _dragablzItemsControl.ContainerCustomisations = new ContainerCustomisations(null, PrepareChildContainerForItemOverride);
            }

            if (SelectedItem == null)
                SetCurrentValue(SelectedItemProperty, Items.OfType<object>().FirstOrDefault());

            _itemsHolder = GetTemplateChild(ItemsHolderPartName) as Panel;
            UpdateSelectedItem();
            MarkWrappedTabItems();
            MarkInitialSelection();

            base.OnApplyTemplate();
        }

        /// <summary>
        /// update the visible child in the ItemsHolder
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0 && e.AddedItems.Count > 0)
                _previousSelection = new WeakReference(e.RemovedItems[0]);

            base.OnSelectionChanged(e);
            UpdateSelectedItem();

            if (_dragablzItemsControl == null) return;

            Func<IList, IEnumerable<DragableItem>> notTabItems =
                l =>
                    l.Cast<object>()
                        .Where(o => !(o is TabItem))
                        .Select(o => _dragablzItemsControl.ItemContainerGenerator.ContainerFromItem(o))
                        .OfType<DragableItem>();
            foreach (var addedItem in notTabItems(e.AddedItems))
            {
                addedItem.IsSelected = true;
                addedItem.BringIntoView();
            }
            foreach (var removedItem in notTabItems(e.RemovedItems))
            {
                removedItem.IsSelected = false;
            }

            foreach (var tabItem in e.AddedItems.OfType<TabItem>().Select(t => _dragablzItemsControl.ItemContainerGenerator.ContainerFromItem(t)).OfType<DragableItem>())
            {
                tabItem.IsSelected = true;
                tabItem.BringIntoView();
            }
            foreach (var tabItem in e.RemovedItems.OfType<TabItem>().Select(t => _dragablzItemsControl.ItemContainerGenerator.ContainerFromItem(t)).OfType<DragableItem>())
            {
                tabItem.IsSelected = false;
            }
        }

        /// <summary>
        /// when the items change we remove any generated panel children and add any new ones as necessary
        /// </summary>
        /// <param name="e"></param>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (_itemsHolder == null)
            {
                return;
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    _itemsHolder.Children.Clear();

                    if (Items.Count > 0)
                    {
                        SelectedItem = base.Items[0];
                        UpdateSelectedItem();
                    }

                    break;

                case NotifyCollectionChangedAction.Add:
                    UpdateSelectedItem();
                    if (e.NewItems.Count == 1 && Items.Count > 1 && _dragablzItemsControl != null && _interTabTransfer == null)
                        _dragablzItemsControl.MoveItem(new MoveItemRequest(e.NewItems[0], SelectedItem, AddLocationHint));

                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        var cp = FindChildContentPresenter(item);
                        if (cp != null)
                            _itemsHolder.Children.Remove(cp);
                    }

                    if (SelectedItem == null)
                        RestorePreviousSelection();
                    UpdateSelectedItem();
                    break;

                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException("Replace not implemented yet");
            }

            IsEmpty = Items.Count == 0;
        }

        /// <summary>
        /// Provides class handling for the <see cref="E:System.Windows.ContentElement.KeyDown"/> routed event that occurs when the user presses a key.
        /// </summary>
        /// <param name="e">Provides data for <see cref="T:System.Windows.Input.KeyEventArgs"/>.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            var sortedDragablzItems = _dragablzItemsControl.ItemsOrganiser.Sort(_dragablzItemsControl.DragableItems()).ToList();
            DragableItem? selectDragablzItem = null;
            switch (e.Key)
            {
                case Key.Tab:
                    if (SelectedItem == null)
                    {
                        selectDragablzItem = sortedDragablzItems.FirstOrDefault();
                        break;
                    }

                    if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        var selectedDragablzItem = (DragableItem)_dragablzItemsControl.ItemContainerGenerator.ContainerFromItem(SelectedItem);
                        var selectedDragablzItemIndex = sortedDragablzItems.IndexOf(selectedDragablzItem);
                        var direction = ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                            ? -1 : 1;
                        var newIndex = selectedDragablzItemIndex + direction;
                        if (newIndex < 0) newIndex = sortedDragablzItems.Count - 1;
                        else if (newIndex == sortedDragablzItems.Count) newIndex = 0;

                        selectDragablzItem = sortedDragablzItems[newIndex];
                    }
                    break;
                case Key.Home:
                    selectDragablzItem = sortedDragablzItems.FirstOrDefault();
                    break;
                case Key.End:
                    selectDragablzItem = sortedDragablzItems.LastOrDefault();
                    break;
            }

            if (selectDragablzItem != null)
            {
                var item = _dragablzItemsControl.ItemContainerGenerator.ItemFromContainer(selectDragablzItem);
                SetCurrentValue(SelectedItemProperty, item);
                e.Handled = true;
            }

            if (!e.Handled)
                base.OnKeyDown(e);
        }

        /// <summary>
        /// Provides an appropriate automation peer implementation for this control
        /// as part of the WPF automation infrastructure.
        /// </summary>
        /// <returns>The type-specific System.Windows.Automation.Peers.AutomationPeer implementation.</returns>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new FrameworkElementAutomationPeer(this);
        }

        internal static DragableTabControl GetOwnerOfHeaderItems(DragableItemsControl itemsControl)
        {
            return LoadedInstances.FirstOrDefault(t => Equals(t._dragablzItemsControl, itemsControl));
        }

        private static void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var tabablzControl = (DragableTabControl)sender;
            if (tabablzControl.IsVisible)
                VisibleInstances.Add(tabablzControl);
            else if (VisibleInstances.Contains(tabablzControl))
                VisibleInstances.Remove(tabablzControl);
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            LoadedInstances.Add(this);
            var window = Window.GetWindow(this);
            if (window == null) return;
            window.Closing += WindowOnClosing;
            _windowSubscription.Disposable = Disposable.Create(() => window.Closing -= WindowOnClosing);
        }

        private void WindowOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            _windowSubscription.Disposable = Disposable.Empty;
            if (!ConsolidateOrphanedItems || InterTabController == null) return;

            var window = (Window)sender;

            var orphanedItems = _dragablzItemsControl.DragableItems();
            if (ConsolidatingOrphanedItemCallback != null)
            {
                orphanedItems = orphanedItems.Where(di =>
                {
                    var args = new ItemActionCallbackArgs<DragableTabControl>(window, this, di);
                    ConsolidatingOrphanedItemCallback(args);
                    return !args.IsCancelled;
                }).ToList();
            }

            var target = LoadedInstances.Except(this).FirstOrDefault(other => other.InterTabController != null &&
                                                                     other.InterTabController.Partition == InterTabController.Partition);
            if (target == null) return;

            foreach (var item in orphanedItems.Select(orphanedItem => _dragablzItemsControl.ItemContainerGenerator.ItemFromContainer(orphanedItem)))
            {
                RemoveFromSource(item);
                target.AddToSource(item);
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _windowSubscription.Disposable = Disposable.Empty;
            LoadedInstances.Remove(this);
        }

        private void MarkWrappedTabItems()
        {
            if (_dragablzItemsControl == null) return;

            foreach (var pair in _dragablzItemsControl.Items.OfType<TabItem>().Select(tabItem =>
                new
                {
                    tabItem,
                    dragablzItem = _dragablzItemsControl.ItemContainerGenerator.ContainerFromItem(tabItem) as DragableItem
                }).Where(a => a.dragablzItem != null))
            {
                var toolTipBinding = new Binding("ToolTip") { Source = pair.tabItem };
                BindingOperations.SetBinding(pair.dragablzItem, ToolTipProperty, toolTipBinding);
                SetIsWrappingTabItem(pair.dragablzItem, true);
            }
        }

        private void MarkInitialSelection()
        {
            if (_dragablzItemsControl == null ||
                _dragablzItemsControl.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated) return;

            if (_dragablzItemsControl == null || SelectedItem == null) return;

            var tabItem = SelectedItem as TabItem;
            tabItem?.SetCurrentValue(IsSelectedProperty, true);

            var containerFromItem =
                _dragablzItemsControl.ItemContainerGenerator.ContainerFromItem(SelectedItem) as DragableItem;

            containerFromItem?.SetCurrentValue(DragableItem.IsSelectedProperty, true);
        }

        private void ItemDragStarted(object sender, DragableDragStartedEventArgs e)
        {
            if (!IsMyItem(e.DragableItem)) return;

            //the thumb may steal the user selection, so we will try and apply it manually
            if (_dragablzItemsControl == null) return;

            e.DragableItem.IsDropTargetFound = false;

            var sourceOfDragItemsControl = ItemsControlFromItemContainer(e.DragableItem) as DragableItemsControl;
            if (sourceOfDragItemsControl == null || !Equals(sourceOfDragItemsControl, _dragablzItemsControl)) return;

            var itemsControlOffset = Mouse.GetPosition(_dragablzItemsControl);
            _tabItemHeaderDragStartInformation = new TabItemHeaderDragStartInformation(e.DragableItem, itemsControlOffset.X,
                itemsControlOffset.Y, e.DragStartedEventArgs.HorizontalOffset, e.DragStartedEventArgs.VerticalOffset);

            foreach (var otherItem in _dragablzItemsControl.Containers<DragableItem>().Except(e.DragableItem))
                otherItem.IsSelected = false;
            e.DragableItem.IsSelected = true;
            e.DragableItem.PartitionAtDragStart = InterTabController?.Partition;
            var item = _dragablzItemsControl.ItemContainerGenerator.ItemFromContainer(e.DragableItem);
            var tabItem = item as TabItem;
            if (tabItem != null)
                tabItem.IsSelected = true;
            SelectedItem = item;

            if (ShouldDragWindow(sourceOfDragItemsControl))
                IsDraggingWindow = true;
        }

        private bool ShouldDragWindow(DragableItemsControl sourceOfDragItemsControl)
        {
            return (Items.Count == 1
                    && (InterTabController == null || InterTabController.MoveWindowWithSolitaryTabs)
                    && !Layout.IsContainedWithinBranch(sourceOfDragItemsControl));
        }

        private void PreviewItemDragDelta(object sender, DragableDragDeltaEventArgs e)
        {
            if (_dragablzItemsControl == null) return;

            var sourceOfDragItemsControl = ItemsControlFromItemContainer(e.DragableItem) as DragableItemsControl;
            if (sourceOfDragItemsControl == null || !Equals(sourceOfDragItemsControl, _dragablzItemsControl)) return;

            if (!ShouldDragWindow(sourceOfDragItemsControl)) return;

            if (MonitorReentry(e)) return;

            var myWindow = Window.GetWindow(this);
            if (myWindow == null) return;

            if (_interTabTransfer != null)
            {
                var cursorPos = InteropMethods.GetCursorPos().ToWpfPoint();
                if (_interTabTransfer.BreachOrientation == Orientation.Vertical)
                {
                    var vector = cursorPos - _interTabTransfer.DragStartWindowOffset;
                    myWindow.Left = vector.X;
                    myWindow.Top = vector.Y;
                }
                else
                {
                    var offset = e.DragableItem.TranslatePoint(_interTabTransfer.OriginatorContainer.MouseAtDragStart, myWindow);
                    var borderVector = myWindow.PointToScreen(new Point()).ToWpfPoint() - new Point(myWindow.Left, myWindow.Top);
                    offset.Offset(borderVector.X, borderVector.Y);
                    myWindow.Left = cursorPos.X - offset.X;
                    myWindow.Top = cursorPos.Y - offset.Y;
                }
            }
            else
            {
                myWindow.Left += e.DragDeltaEventArgs.HorizontalChange;
                myWindow.Top += e.DragDeltaEventArgs.VerticalChange;
            }

            e.Handled = true;
        }

        private bool MonitorReentry(DragableDragDeltaEventArgs e)
        {
            var screenMousePosition = _dragablzItemsControl.PointToScreen(Mouse.GetPosition(_dragablzItemsControl));

            var sourceTabablzControl = (DragableTabControl)e.Source;
            if (sourceTabablzControl.Items.Count > 1 && e.DragableItem.LogicalIndex < sourceTabablzControl.FixedHeaderCount)
            {
                return false;
            }

            var otherTabablzControls = LoadedInstances
                .Where(tc => tc != this && tc.InterTabController != null && InterTabController != null
                            && Equals(tc.InterTabController.Partition, InterTabController.Partition)
                            && tc._dragablzItemsControl != null)
                .Select(tc =>
                {
                    var topLeft = tc._dragablzItemsControl.PointToScreen(new Point());
                    var lastFixedItem = tc._dragablzItemsControl.DragableItems()
                        .OrderBy(di => di.LogicalIndex)
                        .Take(tc._dragablzItemsControl.FixedItemCount)
                        .LastOrDefault();
                    //TODO work this for vert tabs
                    if (lastFixedItem != null)
                        topLeft.Offset(lastFixedItem.X + lastFixedItem.ActualWidth, 0);
                    var bottomRight =
                        tc._dragablzItemsControl.PointToScreen(new Point(tc._dragablzItemsControl.ActualWidth,
                            tc._dragablzItemsControl.ActualHeight));

                    return new { tc, topLeft, bottomRight };
                });


            var target = SortWindowsTopToBottom(Application.Current.Windows.OfType<Window>())
                        .Join(otherTabablzControls, w => w, a => Window.GetWindow(a.tc), (w, a) => a)
                        .FirstOrDefault(a => new Rect(a.topLeft, a.bottomRight).Contains(screenMousePosition));

            if (target == null) return false;

            var mousePositionOnItem = Mouse.GetPosition(e.DragableItem);

            var floatingItemSnapShots = this.GetVisualDescendantsAndSelf()
                                        .OfType<Layout>()
                                        .SelectMany(l => l.FloatingDragablzItems().Select(FloatingItemSnapShot.Take))
                                        .ToList();

            e.DragableItem.IsDropTargetFound = true;
            var item = RemoveItem(e.DragableItem);

            var interTabTransfer = new InterTabTransfer(item, e.DragableItem, mousePositionOnItem, floatingItemSnapShots);
            e.DragableItem.IsDragging = false;

            target.tc.ReceiveDrag(interTabTransfer);
            e.Cancel = true;

            return true;
        }

        private const int GW_HWNDNEXT = 2;
        public static IEnumerable<Window> SortWindowsTopToBottom(IEnumerable<Window> windows)
        {
            var windowsByHandle = windows.Select(window =>
            {
                var hwndSource = PresentationSource.FromVisual(window) as HwndSource;
                var handle = hwndSource != null ? hwndSource.Handle : IntPtr.Zero;
                return new { window, handle };
            }).Where(x => x.handle != IntPtr.Zero)
                .ToDictionary(x => x.handle, x => x.window);

            for (var hWnd = NativeMethods.GetTopWindow(IntPtr.Zero); hWnd != IntPtr.Zero; hWnd = InteropMethods.GetWindow(hWnd, GW_HWNDNEXT))
                if (windowsByHandle.ContainsKey((hWnd)))
                    yield return windowsByHandle[hWnd];
        }

        internal object RemoveItem(DragableItem dragablzItem)
        {
            var item = _dragablzItemsControl.ItemContainerGenerator.ItemFromContainer(dragablzItem);

            //stop the header shrinking if the tab stays open when empty
            var minSize = EmptyHeaderSizingHint == EmptyHeaderSizingHint.PreviousTab
                          ? new Size(_dragablzItemsControl.ActualWidth, _dragablzItemsControl.ActualHeight)
                          : new Size();

            _dragablzItemsControl.MinHeight = 0;
            _dragablzItemsControl.MinWidth = 0;

            var contentPresenter = FindChildContentPresenter(item);
            RemoveFromSource(item);
            _itemsHolder.Children.Remove(contentPresenter);

            if (Items.Count != 0) return item;

            var window = Window.GetWindow(this);
            if (window != null &&
                InterTabController != null &&
                InterTabController.InterTabClient.TabEmptiedHandler(this, window) == TabEmptiedResponse.CloseWindowOrLayoutBranch)
            {
                if (Layout.ConsolidateBranch(this)) return item;

                try
                {
                    SetIsClosingAsPartOfDragOperation(window, true);
                    window.Close();
                }
                finally
                {
                    SetIsClosingAsPartOfDragOperation(window, false);
                }
            }
            else
            {
                _dragablzItemsControl.MinHeight = minSize.Height;
                _dragablzItemsControl.MinWidth = minSize.Width;
            }
            return item;
        }

        private void ItemDragCompleted(object sender, DragableDragCompletedEventArgs e)
        {
            if (!IsMyItem(e.DragableItem)) return;

            _interTabTransfer = null;
            _dragablzItemsControl.LockedMeasure = null;
            IsDraggingWindow = false;
        }

        private void ItemDragDelta(object sender, DragableDragDeltaEventArgs e)
        {
            if (!IsMyItem(e.DragableItem)) return;
            if (FixedHeaderCount > 0 &&
                _dragablzItemsControl.ItemsOrganiser.Sort(_dragablzItemsControl.DragableItems())
                    .Take(FixedHeaderCount)
                    .Contains(e.DragableItem))
                return;

            if (_tabItemHeaderDragStartInformation == null ||
                !Equals(_tabItemHeaderDragStartInformation.DragItem, e.DragableItem) || InterTabController == null) return;

            if (InterTabController.InterTabClient == null)
                throw new InvalidOperationException("An InterTabClient must be provided on an InterTabController.");

            MonitorBreach(e);
        }

        private bool IsMyItem(DragableItem item)
        {
            return _dragablzItemsControl != null && _dragablzItemsControl.DragableItems().Contains(item);
        }

        private void MonitorBreach(DragableDragDeltaEventArgs e)
        {
            var mousePositionOnHeaderItemsControl = Mouse.GetPosition(_dragablzItemsControl);

            Orientation? breachOrientation = null;
            if (mousePositionOnHeaderItemsControl.X < -InterTabController.HorizontalPopoutGrace
                || (mousePositionOnHeaderItemsControl.X - _dragablzItemsControl.ActualWidth) > InterTabController.HorizontalPopoutGrace)
                breachOrientation = Orientation.Horizontal;
            else if (mousePositionOnHeaderItemsControl.Y < -InterTabController.VerticalPopoutGrace
                     || (mousePositionOnHeaderItemsControl.Y - _dragablzItemsControl.ActualHeight) > InterTabController.VerticalPopoutGrace)
                breachOrientation = Orientation.Vertical;

            if (!breachOrientation.HasValue) return;

            var newTabHost = InterTabController.InterTabClient.GetNewHost(InterTabController.InterTabClient, InterTabController.Partition, this);
            if (newTabHost?.DragableTabControl == null || newTabHost.Container == null)
                throw new ApplicationException("New tab host was not correctly provided");

            var item = _dragablzItemsControl.ItemContainerGenerator.ItemFromContainer(e.DragableItem);
            var isTransposing = IsTransposing(newTabHost.DragableTabControl);

            var myWindow = Window.GetWindow(this);
            if (myWindow == null) throw new ApplicationException("Unable to find owning window.");
            var dragStartWindowOffset = ConfigureNewHostSizeAndGetDragStartWindowOffset(myWindow, newTabHost, e.DragableItem, isTransposing);

            var dragableItemHeaderPoint = e.DragableItem.TranslatePoint(new Point(), _dragablzItemsControl);
            var dragableItemSize = new Size(e.DragableItem.ActualWidth, e.DragableItem.ActualHeight);
            var floatingItemSnapShots = this.GetVisualDescendantsAndSelf()
                                        .OfType<Layout>()
                                        .SelectMany(l => l.FloatingDragablzItems().Select(FloatingItemSnapShot.Take))
                                        .ToList();

            if (myWindow.WindowState == WindowState.Maximized)
            {
                var desktopMousePosition = InteropMethods.GetCursorPos().ToWpfPoint();
                newTabHost.Container.Left = desktopMousePosition.X - dragStartWindowOffset.X;
                newTabHost.Container.Top = desktopMousePosition.Y - dragStartWindowOffset.Y;
            }
            else
            {
                newTabHost.Container.Left = myWindow.Left;
                newTabHost.Container.Top = myWindow.Top;
            }
            newTabHost.Container.Show();
            var contentPresenter = FindChildContentPresenter(item);

            //stop the header shrinking if the tab stays open when empty
            var minSize = EmptyHeaderSizingHint == EmptyHeaderSizingHint.PreviousTab
                ? new Size(_dragablzItemsControl.ActualWidth, _dragablzItemsControl.ActualHeight)
                : new Size();
            System.Diagnostics.Debug.WriteLine("B " + minSize);

            RemoveFromSource(item);
            _itemsHolder.Children.Remove(contentPresenter);
            if (Items.Count == 0)
            {
                _dragablzItemsControl.MinHeight = minSize.Height;
                _dragablzItemsControl.MinWidth = minSize.Width;
                Layout.ConsolidateBranch(this);
            }

            RestorePreviousSelection();

            foreach (var dragablzItem in _dragablzItemsControl.DragableItems())
            {
                dragablzItem.IsDragging = false;
                dragablzItem.IsSiblingDragging = false;
            }

            var interTabTransfer = new InterTabTransfer(item, e.DragableItem, breachOrientation.Value, dragStartWindowOffset, e.DragableItem.MouseAtDragStart, dragableItemHeaderPoint, dragableItemSize, floatingItemSnapShots, isTransposing);
            newTabHost.DragableTabControl.ReceiveDrag(interTabTransfer);
            interTabTransfer.OriginatorContainer.IsDropTargetFound = true;

            e.Cancel = true;
        }

        private bool IsTransposing(System.Windows.Controls.TabControl target)
        {
            return IsVertical(this) != IsVertical(target);
        }

        private static bool IsVertical(System.Windows.Controls.TabControl tabControl)
        {
            return tabControl.TabStripPlacement == Dock.Left
                   || tabControl.TabStripPlacement == Dock.Right;
        }

        private void RestorePreviousSelection()
        {
            var previousSelection = _previousSelection?.Target;
            if (previousSelection != null && Items.Contains(previousSelection))
                SelectedItem = previousSelection;
            else
                SelectedItem = Items.OfType<object>().FirstOrDefault();
        }

        private Point ConfigureNewHostSizeAndGetDragStartWindowOffset(Window currentWindow, INewTabHost<Window> newTabHost, DragableItem dragablzItem, bool isTransposing)
        {
            var layout = this.GetVisualAncestorsAndSelf().OfType<Layout>().FirstOrDefault();
            Point dragStartWindowOffset;
            if (layout != null)
            {
                newTabHost.Container.Width = ActualWidth + Math.Max(0, currentWindow.RestoreBounds.Width - layout.ActualWidth);
                newTabHost.Container.Height = ActualHeight + Math.Max(0, currentWindow.RestoreBounds.Height - layout.ActualHeight);
                dragStartWindowOffset = dragablzItem.TranslatePoint(new Point(), this);
                //dragStartWindowOffset.Offset(currentWindow.RestoreBounds.Width - layout.ActualWidth, currentWindow.RestoreBounds.Height - layout.ActualHeight);
            }
            else
            {
                if (newTabHost.Container.GetType() == currentWindow.GetType())
                {
                    newTabHost.Container.Width = currentWindow.RestoreBounds.Width;
                    newTabHost.Container.Height = currentWindow.RestoreBounds.Height;
                    dragStartWindowOffset = isTransposing ? new Point(dragablzItem.MouseAtDragStart.X, dragablzItem.MouseAtDragStart.Y) : dragablzItem.TranslatePoint(new Point(), currentWindow);
                }
                else
                {
                    newTabHost.Container.Width = ActualWidth;
                    newTabHost.Container.Height = ActualHeight;
                    dragStartWindowOffset = isTransposing ? new Point() : dragablzItem.TranslatePoint(new Point(), this);
                    dragStartWindowOffset.Offset(dragablzItem.MouseAtDragStart.X, dragablzItem.MouseAtDragStart.Y);
                    return dragStartWindowOffset;
                }
            }

            dragStartWindowOffset.Offset(dragablzItem.MouseAtDragStart.X, dragablzItem.MouseAtDragStart.Y);
            var borderVector = currentWindow.PointToScreen(new Point()).ToWpfPoint() - new Point(currentWindow.GetActualLeft(), currentWindow.GetActualTop());
            dragStartWindowOffset.Offset(borderVector.X, borderVector.Y);
            return dragStartWindowOffset;
        }

        internal void ReceiveDrag(InterTabTransfer interTabTransfer)
        {
            var myWindow = Window.GetWindow(this);
            if (myWindow == null) throw new ApplicationException("Unable to find owning window.");
            myWindow.Activate();

            _interTabTransfer = interTabTransfer;

            if (Items.Count == 0)
            {
                if (interTabTransfer.IsTransposing)
                    _dragablzItemsControl.LockedMeasure = new Size(
                        interTabTransfer.ItemSize.Width,
                        interTabTransfer.ItemSize.Height);
                else
                    _dragablzItemsControl.LockedMeasure = new Size(
                        interTabTransfer.ItemPositionWithinHeader.X + interTabTransfer.ItemSize.Width,
                        interTabTransfer.ItemPositionWithinHeader.Y + interTabTransfer.ItemSize.Height);
            }

            var lastFixedItem = _dragablzItemsControl.DragableItems()
                .OrderBy(i => i.LogicalIndex)
                .Take(_dragablzItemsControl.FixedItemCount)
                .LastOrDefault();

            AddToSource(interTabTransfer.Item);
            SelectedItem = interTabTransfer.Item;

            Dispatcher.BeginInvoke(new Action(() => Layout.RestoreFloatingItemSnapShots(this, interTabTransfer.FloatingItemSnapShots)), DispatcherPriority.Loaded);
            _dragablzItemsControl.InstigateDrag(interTabTransfer.Item, newContainer =>
            {
                newContainer.PartitionAtDragStart = interTabTransfer.OriginatorContainer.PartitionAtDragStart;
                newContainer.IsDropTargetFound = true;

                if (interTabTransfer.TransferReason == InterTabTransferReason.Breach)
                {
                    if (interTabTransfer.IsTransposing)
                    {
                        newContainer.Y = 0;
                        newContainer.X = 0;
                    }
                    else
                    {
                        newContainer.Y = interTabTransfer.OriginatorContainer.Y;
                        newContainer.X = interTabTransfer.OriginatorContainer.X;
                    }
                }
                else
                {
                    if (TabStripPlacement == Dock.Top || TabStripPlacement == Dock.Bottom)
                    {
                        var mouseXOnItemsControl = InteropMethods.GetCursorPos().X - _dragablzItemsControl.PointToScreen(new Point()).X;
                        var newX = mouseXOnItemsControl - interTabTransfer.DragStartItemOffset.X;
                        if (lastFixedItem != null)
                        {
                            newX = Math.Max(newX, lastFixedItem.X + lastFixedItem.ActualWidth);
                        }
                        newContainer.X = newX;
                        newContainer.Y = 0;
                    }
                    else
                    {
                        var mouseYOnItemsControl = InteropMethods.GetCursorPos().Y - _dragablzItemsControl.PointToScreen(new Point()).Y;
                        var newY = mouseYOnItemsControl - interTabTransfer.DragStartItemOffset.Y;
                        if (lastFixedItem != null)
                        {
                            newY = Math.Max(newY, lastFixedItem.Y + lastFixedItem.ActualHeight);
                        }
                        newContainer.X = 0;
                        newContainer.Y = newY;
                    }
                }
                newContainer.MouseAtDragStart = interTabTransfer.DragStartItemOffset;
            });
        }

        /// <summary>
        /// generate a ContentPresenter for the selected item
        /// </summary>
        private void UpdateSelectedItem()
        {
            if (_itemsHolder == null)
            {
                return;
            }

            CreateChildContentPresenter(SelectedItem);

            // show the right child
            var selectedContent = GetContent(SelectedItem);
            foreach (ContentPresenter child in _itemsHolder.Children)
            {
                var isSelected = (child.Content == selectedContent);
                child.Visibility = isSelected ? Visibility.Visible : Visibility.Collapsed;
                child.IsEnabled = isSelected;
            }
        }

        private static object GetContent(object item)
        {
            return (item is TabItem) ? ((TabItem)item).Content : item;
        }

        /// <summary>
        /// create the child ContentPresenter for the given item (could be data or a TabItem)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private void CreateChildContentPresenter(object item)
        {
            if (item == null) return;

            var cp = FindChildContentPresenter(item);
            if (cp != null) return;

            // the actual child to be added.  cp.Tag is a reference to the TabItem
            cp = new ContentPresenter
            {
                Content = GetContent(item),
                ContentTemplate = ContentTemplate,
                ContentTemplateSelector = ContentTemplateSelector,
                ContentStringFormat = ContentStringFormat,
                Visibility = Visibility.Collapsed,
            };
            _itemsHolder.Children.Add(cp);
        }

        /// <summary>
        /// Find the CP for the given object.  data could be a TabItem or a piece of data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ContentPresenter FindChildContentPresenter(object data)
        {
            if (data is TabItem)
                data = ((TabItem)data).Content;

            return data == null
                   ? null
                   : _itemsHolder?.Children.Cast<ContentPresenter>().FirstOrDefault(cp => cp.Content == data);
        }

        private void ItemContainerGeneratorOnStatusChanged(object sender, EventArgs eventArgs)
        {
            MarkWrappedTabItems();
            MarkInitialSelection();
        }

        private static void CloseItem(DragableItem item, DragableTabControl owner)
        {
            if (item == null)
                throw new ApplicationException("Valid DragablzItem to close is required.");

            if (owner == null)
                throw new ApplicationException("Valid DragableTabControl container is required.");

            if (!owner.IsMyItem(item))
                throw new ApplicationException("DragableTabControl container must be an owner of the DragablzItem to close");

            var cancel = false;
            if (owner.ClosingItemCallback != null)
            {
                var callbackArgs = new ItemActionCallbackArgs<DragableTabControl>(Window.GetWindow(owner), owner, item);
                owner.ClosingItemCallback(callbackArgs);
                cancel = callbackArgs.IsCancelled;
            }

            if (!cancel)
                owner.RemoveItem(item);
        }

        private static void CloseItemCanExecuteClassHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = FindOwner(e.Parameter, e.OriginalSource) != null;
        }
        private static void CloseItemClassHandler(object sender, ExecutedRoutedEventArgs e)
        {
            var owner = FindOwner(e.Parameter, e.OriginalSource);

            if (owner == null) throw new ApplicationException("Unable to ascertain DragablzItem to close.");

            CloseItem(owner.Item1, owner.Item2);
        }

        private static Tuple<DragableItem, DragableTabControl> FindOwner(object eventParameter, object eventOriginalSource)
        {
            var dragablzItem = eventParameter as DragableItem;
            if (dragablzItem == null)
            {
                var dependencyObject = eventOriginalSource as DependencyObject;
                dragablzItem = dependencyObject.GetVisualAncestorsAndSelf().OfType<DragableItem>().FirstOrDefault();
                if (dragablzItem == null)
                {
                    var popup = dependencyObject.LogicalTreeAncestory().OfType<System.Windows.Controls.Primitives.Popup>().LastOrDefault();
                    if (popup?.PlacementTarget != null)
                    {
                        dragablzItem = popup.PlacementTarget.GetVisualAncestorsAndSelf().OfType<DragableItem>().FirstOrDefault();
                    }
                }
            }

            if (dragablzItem == null) return null;

            var tabablzControl = LoadedInstances.FirstOrDefault(tc => tc.IsMyItem(dragablzItem));

            return tabablzControl == null ? null : new Tuple<DragableItem, DragableTabControl>(dragablzItem, tabablzControl);
        }

        private void AddItemHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (NewItemFactory == null)
                throw new InvalidOperationException("NewItemFactory must be provided.");

            var newItem = NewItemFactory();
            if (newItem == null) throw new ApplicationException("NewItemFactory returned null.");

            AddToSource(newItem);
            SelectedItem = newItem;

            Dispatcher.BeginInvoke(new Action(_dragablzItemsControl.InvalidateMeasure), DispatcherPriority.Loaded);
        }

        private void PrepareChildContainerForItemOverride(DependencyObject dependencyObject, object o)
        {
            var dragablzItem = dependencyObject as DragableItem;
            if (dragablzItem != null && HeaderMemberPath != null)
            {
                var contentBinding = new Binding(HeaderMemberPath) { Source = o };
                dragablzItem.SetBinding(ContentControl.ContentProperty, contentBinding);
                dragablzItem.UnderlyingContent = o;
            }

            SetIsWrappingTabItem(dependencyObject, o is TabItem);
        }
    }
}
