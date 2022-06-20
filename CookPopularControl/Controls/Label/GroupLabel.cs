using CookPopularControl.Communal.Data;
using CookPopularCSharpToolkit.Communal;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：GroupLabel
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-11 11:37:42
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 标签组
    /// </summary>
    /// <remarks>子项必须使用<see cref="GroupLableItem"/></remarks>
    public class GroupLabel : ItemsControl
    {
        public static readonly ICommand CloseCommand = new RoutedCommand(nameof(CloseCommand), typeof(GroupLabel));

        public GroupLabel()
        {
            CommandBindings.Add(new CommandBinding(CloseCommand, (s, e) =>
            {
                var gl = e.Source as GroupLabel;
                var label = (e.OriginalSource as System.Windows.Controls.Button).TemplatedParent as Label;
                if (gl != null && label != null)
                {
                    var item = gl.ItemContainerGenerator.ItemFromContainer(label);
                    OnItemClosed(item);
                    gl.GetActualItems()?.Remove(item);
                    gl.Items.Refresh();
                }
            }, (s, e) => e.CanExecute = true));
        }

        private IList GetActualItems()
        {
            IList? list;
            if (ItemsSource != null)
            {
                list = ItemsSource as IList;
            }
            else
            {
                list = Items;
            }

            return list;
        }


        /// <summary>
        /// 是否有关闭功能
        /// </summary>
        public bool HasCloseButton
        {
            get { return (bool)GetValue(HasCloseButtonProperty); }
            set { SetValue(HasCloseButtonProperty, value); }
        }
        public static readonly DependencyProperty HasCloseButtonProperty =
            DependencyProperty.Register("HasCloseButton", typeof(bool), typeof(GroupLabel), new PropertyMetadata(ValueBoxes.TrueBox));



        [Description("Item子项被删除时发生")]
        public event EventHandler<object> ItemClosed
        {
            add { this.AddHandler(ItemClosedEvent, value); }
            remove { this.RemoveHandler(ItemClosedEvent, value); }
        }
        /// <summary>
        /// <see cref="ItemClosedEvent"/>标识子项删除事件
        /// </summary>
        public static readonly RoutedEvent ItemClosedEvent =
            EventManager.RegisterRoutedEvent("ItemClosed", RoutingStrategy.Bubble, typeof(EventHandler<object>), typeof(GroupLabel));

        protected virtual void OnItemClosed(object item)
        {
            RoutedPropertySingleEventArgs<object> arg = new RoutedPropertySingleEventArgs<object>(item, ItemClosedEvent);
            this.RaiseEvent(arg);
        }


        protected override DependencyObject GetContainerForItemOverride() => new Label();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is Label;
    }

    /// <summary>
    /// <see cref="GroupLabel"/>的子项数据
    /// </summary>
    public class GroupLableItem
    {
        public object Header { get; set; }
        public object Content { get; set; }
    }
}
