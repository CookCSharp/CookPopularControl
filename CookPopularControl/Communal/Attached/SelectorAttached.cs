using CookPopularControl.Communal.Data.Enum;
using CookPopularControl.Tools.Boxes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ItemsControlAttached
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-24 11:02:41
 */
namespace CookPopularControl.Communal.Attached
{
    /// <summary>
    /// 提供具有Items子项控件的附加属性基类
    /// </summary>
    /// <remarks>适用于ListBox, ListView，ComboBox</remarks>
    public class SelectorAttached
    {
        private const string ComboBoxItems_ListBox = "PART_ListBox";
        private const string ItemCheckBox = "ItemCheckBox";
        private const string ItemButton = "ItemButton";
        private const string ItemIcon = "ItemIcon";
        private const string ItemImageGif = "ItemImageGif";

        public static Brush GetItemsListBackground(DependencyObject obj) => (Brush)obj.GetValue(ItemsListBackgroundProperty);
        public static void SetItemsListBackground(DependencyObject obj, Brush value) => obj.SetValue(ItemsListBackgroundProperty, value);
        /// <summary>
        /// 标识<see cref="ItemsListBackgroundProperty"/>提供下拉列表的背景色附加属性
        /// </summary>
        public static readonly DependencyProperty ItemsListBackgroundProperty =
            DependencyProperty.RegisterAttached("ItemsListBackground", typeof(Brush), typeof(SelectorAttached), new PropertyMetadata(default(Brush)));


        public static Brush GetItemMouseOverBackground(DependencyObject obj) => (Brush)obj.GetValue(ItemMouseOverBackgroundProperty);
        public static void SetItemMouseOverBackground(DependencyObject obj, Brush value) => obj.SetValue(ItemMouseOverBackgroundProperty, value);
        /// <summary>
        /// 标识<see cref="ItemMouseOverBackgroundProperty"/>提供子项的背景色附加属性
        /// </summary>
        public static readonly DependencyProperty ItemMouseOverBackgroundProperty =
            DependencyProperty.RegisterAttached("ItemMouseOverBackground", typeof(Brush), typeof(SelectorAttached), new PropertyMetadata(default(Brush)));


        public static Brush GetItemSelectedBackground(DependencyObject obj) => (Brush)obj.GetValue(ItemSelectedBackgroundProperty);
        public static void SetItemSelectedBackground(DependencyObject obj, Brush value) => obj.SetValue(ItemSelectedBackgroundProperty, value);
        /// <summary>
        /// <see cref="ItemSelectedBackgroundProperty"/>标识子项选中的后背景色附加属性
        /// </summary>
        public static readonly DependencyProperty ItemSelectedBackgroundProperty =
            DependencyProperty.RegisterAttached("ItemSelectedBackground", typeof(Brush), typeof(SelectorAttached), new PropertyMetadata(default(Brush)));


        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        public static SelectorItemType GetSelectorItemType(DependencyObject obj) => (SelectorItemType)obj.GetValue(SelectorItemTypeProperty);
        public static void SetSelectorItemType(DependencyObject obj, SelectorItemType value) => obj.SetValue(SelectorItemTypeProperty, value);
        /// <summary>
        /// 标识<see cref="SelectorItemTypeProperty"/>子项类型的附加属性
        /// </summary>
        public static readonly DependencyProperty SelectorItemTypeProperty =
            DependencyProperty.RegisterAttached("SelectorItemType", typeof(SelectorItemType), typeof(SelectorAttached),
                new FrameworkPropertyMetadata(default(SelectorItemType), FrameworkPropertyMetadataOptions.AffectsRender, OnSelectorItemTypeChanged));

        private static void OnSelectorItemTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Selector selector)
            {
                if (selector.IsLoaded)
                    SetSelectorItems(selector);
                else
                    selector.Loaded += (s, e) => SetSelectorItems(selector);
            }
        }

        private static void SetSelectorItems(Selector selector)
        {
            if (selector is ComboBox comboBox && GetSelectorItemType(selector) != SelectorItemType.Default)
            {
                var listBox = comboBox.Template.FindName(ComboBoxItems_ListBox, comboBox) as ListBox;
                if (listBox != null)
                {
                    //也可以在xaml中绑定Items
                    listBox.ItemsSource = comboBox.ItemsSource ?? comboBox.Items;

                    listBox.SelectionChanged += (s, e) =>
                    {
                        var comboBoxText = string.Empty;
                        foreach (var item in listBox.SelectedItems)
                        {
                            if (item is string)
                                comboBoxText += item.ToString() + ",";
                            if(item is SelectorItem pic)
                                comboBoxText += pic.Content.ToString() + ",";
                        }
                        if (comboBoxText.Length > 0)
                            comboBox.Text = comboBoxText.Remove(comboBoxText.Length - 1);
                        else
                            comboBox.Text = comboBoxText;
                    };
                }
            }
            else if(selector is ListBox listBox && GetSelectorItemType(selector) != SelectorItemType.Default)
            {

            }
        }


        public static double GetItemWidth(DependencyObject obj) => (double)obj.GetValue(ItemWidthProperty);
        public static void SetItemWidth(DependencyObject obj, double value) => obj.SetValue(ItemWidthProperty, value);
        /// <summary>
        /// <see cref="ItemWidthProperty"/>标识子项的宽度
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.RegisterAttached("ItemWidth", typeof(double), typeof(SelectorAttached), new PropertyMetadata(double.NaN));


        public static double GetItemHeight(DependencyObject obj) => (double)obj.GetValue(ItemHeightProperty);
        public static void SetItemHeight(DependencyObject obj, double value) => obj.SetValue(ItemHeightProperty, value);
        /// <summary>
        /// <see cref="ItemHeightProperty"/>标识子项的高度
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.RegisterAttached("ItemHeight", typeof(double), typeof(SelectorAttached), new PropertyMetadata(ValueBoxes.Double20Box));


        public static double GetItemControlWidth(DependencyObject obj) => (double)obj.GetValue(ItemControlWidthProperty);
        public static void SetItemControlWidth(DependencyObject obj, double value) => obj.SetValue(ItemControlWidthProperty, value);
        /// <summary>
        /// <see cref="ItemControlWidthProperty"/>标识Selector子项控件的宽度
        /// </summary>
        public static readonly DependencyProperty ItemControlWidthProperty =
            DependencyProperty.RegisterAttached("ItemControlWidth", typeof(double), typeof(SelectorAttached), new PropertyMetadata(ValueBoxes.Double20Box));


        public static double GetItemControlHeight(DependencyObject obj) => (double)obj.GetValue(ItemControlHeightProperty);
        public static void SetItemControlHeight(DependencyObject obj, double value) => obj.SetValue(ItemControlHeightProperty, value);
        /// <summary>
        /// <see cref="ItemControlHeightProperty"/>标识Selector子项控件的高度
        /// </summary>
        public static readonly DependencyProperty ItemControlHeightProperty =
            DependencyProperty.RegisterAttached("ItemControlHeight", typeof(double), typeof(SelectorAttached), new PropertyMetadata(ValueBoxes.Double20Box));


        #region SelectotItemType=CheckBox

        public static bool GetIsCheckBoxChecked(DependencyObject obj) => (bool)obj.GetValue(IsCheckBoxCheckedProperty);
        public static void SetIsCheckBoxChecked(DependencyObject obj, bool value) => obj.SetValue(IsCheckBoxCheckedProperty, ValueBoxes.BooleanBox(value));
        public static readonly DependencyProperty IsCheckBoxCheckedProperty =
            DependencyProperty.RegisterAttached("IsCheckBoxChecked", typeof(bool), typeof(SelectorAttached),
                new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits, OnIsCheckBoxCheckedChanged));

        private static void OnIsCheckBoxCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var checkBox = d as CheckBox;
            if (checkBox != null)
            {
                var listBoxItem = checkBox.TemplatedParent as ListBoxItem;
                checkBox.Checked += (s, e) => OnIsItemChecked(listBoxItem!, false, true);
                checkBox.Unchecked += (s, e) => OnIsItemChecked(listBoxItem!, true, false);
            }
        }

        /// <summary>
        /// <see cref="IsItemCheckedEvent"/>标识子项ItemCheck是否选中的事件
        /// </summary>
        public static readonly RoutedEvent IsItemCheckedEvent =
            EventManager.RegisterRoutedEvent("IsItemChecked", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(SelectorAttached));
        public static void AddIsItemCheckedHandler(DependencyObject d, RoutedPropertyChangedEventHandler<object> handler)
        {
            (d as UIElement)?.AddHandler(IsItemCheckedEvent, handler);
        }
        public static void RemoveIsItemCheckedHandler(DependencyObject d, RoutedPropertyChangedEventHandler<object> handler)
        {
            (d as UIElement)?.RemoveHandler(IsItemCheckedEvent, handler);
        }
        private static void OnIsItemChecked(object sender, object oldValue, object newValue)
        {
            var element = sender as UIElement;
            RoutedPropertyChangedEventArgs<object> arg = new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, IsItemCheckedEvent);
            element?.RaiseEvent(arg);
        }

        [Obsolete("备用", false)]
        public static string GetCheckBoxContent(DependencyObject obj) => (string)obj.GetValue(CheckBoxContentProperty);
        [Obsolete("备用", false)]
        public static void SetCheckBoxContent(DependencyObject obj, string value) => obj.SetValue(CheckBoxContentProperty, value);
        [Obsolete("备用", false)]
        public static readonly DependencyProperty CheckBoxContentProperty =
            DependencyProperty.RegisterAttached("CheckBoxContent", typeof(string), typeof(SelectorAttached), new PropertyMetadata(default(string)));

        #endregion

        #region SelectotItemType=Button

        public static object GetButtonContent(DependencyObject obj) => (object)obj.GetValue(ButtonContentProperty);
        public static void SetButtonContent(DependencyObject obj, object value) => obj.SetValue(ButtonContentProperty, value);
        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.RegisterAttached("ButtonContent", typeof(object), typeof(SelectorAttached), new PropertyMetadata(default(object)));

        public static bool GetIsButtonDeleteItem(DependencyObject obj) => (bool)obj.GetValue(IsButtonDeleteItemProperty);
        public static void SetIsButtonDeleteItem(DependencyObject obj, bool value) => obj.SetValue(IsButtonDeleteItemProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsButtonDeleteItemProperty"/>用于触发Button的Click事件
        /// </summary>
        public static readonly DependencyProperty IsButtonDeleteItemProperty =
            DependencyProperty.RegisterAttached("IsButtonDeleteItem", typeof(bool), typeof(SelectorAttached),
                new FrameworkPropertyMetadata(ValueBoxes.FalseBox, OnIsButtonDeleteItemChanged));

        private static void OnIsButtonDeleteItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var button = d as Button;
            if (button != null)
            {
                var listBoxItem = button.TemplatedParent as ListBoxItem;
                if (listBoxItem != null)
                {
                    button.Click += (s, e) =>
                    {
                        OnIsItemDelete(listBoxItem, false, true);
                    };
                }
            }
        }

        /// <summary>
        /// <see cref="IsItemDeleteEvent"/>标识是否删除该项的事件
        /// </summary>
        public static readonly RoutedEvent IsItemDeleteEvent =
            EventManager.RegisterRoutedEvent("IsItemDelete", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(SelectorAttached));
        public static void AddIsItemDeleteHandler(DependencyObject d, RoutedPropertyChangedEventHandler<object> handler)
        {
            (d as UIElement)?.AddHandler(IsItemDeleteEvent, handler);
        }
        public static void RemoveIsItemDeleteHandler(DependencyObject d, RoutedPropertyChangedEventHandler<object> handler)
        {
            (d as UIElement)?.RemoveHandler(IsItemDeleteEvent, handler);
        }
        private static void OnIsItemDelete(object sender, object oldValue, object newValue)
        {
            var element = sender as UIElement;
            RoutedPropertyChangedEventArgs<object> arg = new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, IsItemDeleteEvent);
            element?.RaiseEvent(arg);
        }

        #endregion

        #region SelectotItemType=Icon

        /**
         * 当使用Icon时，在设置的ItemsSource每个子项中必须含有属性GeometryData,属于<see cref="Geometry"/>
         * 因为ComboBoxItem.Template中Path的绑定方式为Content.GeometryData
         * **/

        public static Brush GetIconFill(DependencyObject obj) => (Brush)obj.GetValue(IconFillProperty);
        public static void SetIconFill(DependencyObject obj, Brush value) => obj.SetValue(IconFillProperty, value);
        public static readonly DependencyProperty IconFillProperty =
            DependencyProperty.RegisterAttached("IconFill", typeof(Brush), typeof(SelectorAttached), new PropertyMetadata(default(Brush)));

        #endregion

        #region SelectotItemType=Image/Gif

        public static ImageSource GetImageSource(DependencyObject obj) => (ImageSource)obj.GetValue(ImageSourceProperty);
        public static void SetImageSource(DependencyObject obj, ImageSource value) => obj.SetValue(ImageSourceProperty, value);
        [Description("仅当ItemsSource的每个子项都为同一张图片时使用")]
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.RegisterAttached("ImageSource", typeof(ImageSource), typeof(SelectorAttached), new PropertyMetadata(default(ImageSource)));

        public static Uri GetGifSource(DependencyObject obj) => (Uri)obj.GetValue(GifSourceProperty);
        public static void SetGifSource(DependencyObject obj, Uri value) => obj.SetValue(GifSourceProperty, value);
        [Description("仅当ItemsSource的每个子项都为同一张图片时使用")]
        public static readonly DependencyProperty GifSourceProperty =
            DependencyProperty.RegisterAttached("GifSource", typeof(Uri), typeof(SelectorAttached), new PropertyMetadata(default(Uri)));

        public static bool GetIsPreviewImage(DependencyObject obj) => (bool)obj.GetValue(IsPreviewImageProperty);
        public static void SetIsPreviewImage(DependencyObject obj, bool value) => obj.SetValue(IsPreviewImageProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsPreviewImageProperty"/>表示是否可以预览图片
        /// </summary>
        public static readonly DependencyProperty IsPreviewImageProperty =
            DependencyProperty.RegisterAttached("IsPreviewImage", typeof(bool), typeof(SelectorAttached), new PropertyMetadata(ValueBoxes.FalseBox));
        
        #endregion
    }

    /// <summary>
    /// 当ListBoxItem的子项显示Icon或Image或Gif时必须使用该类，方可加入ItemsSource
    /// </summary>
    public class SelectorItem
    {
        public Geometry GeometryData { get; set; }

        public ImageSource ImageSource { get; set; }

        public Uri GifSource { get; set; }

        public object Content { get; set; }
    }
}
