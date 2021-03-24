using CookPopularControl.Communal.Data.Enum;
using CookPopularControl.Tools.Boxes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            var comboBox = d as ComboBox;
            if (comboBox != null && (SelectorItemType)e.NewValue != SelectorItemType.Default)
            {
                if (comboBox.IsLoaded)
                    SetMultiComboBoxItems(comboBox);
                else
                    comboBox.Loaded += (s, e) => SetMultiComboBoxItems(comboBox);
            }
        }

        private static void SetMultiComboBoxItems(ComboBox comboBox)
        {
            var listBox = comboBox.Template.FindName(ComboBoxItems_ListBox, comboBox) as ListBox;
            if (listBox != null)
                listBox.ItemsSource = comboBox.ItemsSource ?? comboBox.Items;
        }


        public static double GetItemWidth(DependencyObject obj) => (double)obj.GetValue(ItemWidthProperty);
        public static void SetItemWidth(DependencyObject obj, double value) => obj.SetValue(ItemWidthProperty, value);
        /// <summary>
        /// <see cref="ItemWidthProperty"/>标识Selector子项的宽度
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.RegisterAttached("ItemWidth", typeof(double), typeof(SelectorAttached), new PropertyMetadata(ValueBoxes.Double20Box));

        public static double GetItemHeight(DependencyObject obj) => (double)obj.GetValue(ItemHeightProperty);
        public static void SetItemHeight(DependencyObject obj, double value) => obj.SetValue(ItemHeightProperty, value);
        /// <summary>
        /// <see cref="ItemHeightProperty"/>标识Selector子项的高度
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
            DependencyProperty.RegisterAttached("IsCheckBoxChecked", typeof(bool), typeof(SelectorAttached), new PropertyMetadata(ValueBoxes.FalseBox));

        public static string GetCheckBoxContent(DependencyObject obj) => (string)obj.GetValue(CheckBoxContentProperty);
        public static void SetCheckBoxContent(DependencyObject obj, string value) => obj.SetValue(CheckBoxContentProperty, value);
        public static readonly DependencyProperty CheckBoxContentProperty =
            DependencyProperty.RegisterAttached("CheckBoxContent", typeof(string), typeof(SelectorAttached), new PropertyMetadata(default(string)));

        #endregion

        #region SelectotItemType=Button

        public static string GetButtonContent(DependencyObject obj) => (string)obj.GetValue(ButtonContentProperty);
        public static void SetButtonContent(DependencyObject obj, string value) => obj.SetValue(ButtonContentProperty, value);
        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.RegisterAttached("ButtonContent", typeof(string), typeof(SelectorAttached), new PropertyMetadata(default(string)));

        #endregion

        #region SelectotItemType=Icon

        public static Brush GetIconFill(DependencyObject obj) => (Brush)obj.GetValue(IconFillProperty);
        public static void SetIconFill(DependencyObject obj, Brush value) => obj.SetValue(IconFillProperty, value);
        public static readonly DependencyProperty IconFillProperty =
            DependencyProperty.RegisterAttached("IconFill", typeof(Brush), typeof(SelectorAttached), new PropertyMetadata(default(Brush)));

        #endregion

        #region SelectotItemType=Image/Gif

        public static ImageSource GetImageSource(DependencyObject obj) => (ImageSource)obj.GetValue(ImageSourceProperty);
        public static void SetImageSource(DependencyObject obj, ImageSource value) => obj.SetValue(ImageSourceProperty, value);
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.RegisterAttached("ImageSource", typeof(ImageSource), typeof(SelectorAttached), new PropertyMetadata(default(ImageSource)));

        public static Uri GetGifSource(DependencyObject obj) => (Uri)obj.GetValue(GifSourceProperty);
        public static void SetGifSource(DependencyObject obj, Uri value) => obj.SetValue(GifSourceProperty, value);
        public static readonly DependencyProperty GifSourceProperty =
            DependencyProperty.RegisterAttached("GifSource", typeof(Uri), typeof(SelectorAttached), new PropertyMetadata(default(Uri)));

        #endregion
    }
}
