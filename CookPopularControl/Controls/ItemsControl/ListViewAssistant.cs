using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ListViewAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-30 12:20:18
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 表示<see cref="GridViewColumn"/>的附加属性基类
    /// </summary>
    public class GridViewColumnAssistant
    {
        public static GridLength GetColumnMinWidth(DependencyObject obj) => (GridLength)obj.GetValue(ColumnMinWidthProperty);
        public static void SetColumnMinWidth(DependencyObject obj, GridLength value) => obj.SetValue(ColumnMinWidthProperty, value);
        /// <summary>
        /// 标识<see cref="GridViewColumnHeader"/>列项最小宽度
        /// </summary>
        public static readonly DependencyProperty ColumnMinWidthProperty =
            DependencyProperty.RegisterAttached("ColumnMinWidth", typeof(GridLength), typeof(GridViewColumnAssistant), new PropertyMetadata(GridLength.Auto, OnColumnMinWidthChanged));

        private static void OnColumnMinWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GridViewColumn column)
            {
                var defaultGridViewColumnHeaderStyle = ResourceHelper.GetResource<Style>(typeof(GridViewColumnHeader));
                Style style = new Style(typeof(GridViewColumnHeader), defaultGridViewColumnHeaderStyle);
                var setter = new Setter(GridViewColumnHeader.MinWidthProperty, GetColumnMinWidth(column).Value);
                style.Setters.Add(setter);
                column.HeaderContainerStyle = style;
            }
        }


        public static Thickness GetColumnHeaderPadding(DependencyObject obj) => (Thickness)obj.GetValue(ColumnHeaderPaddingProperty);
        internal static void SetColumnHeaderPadding(DependencyObject obj, Thickness value) => obj.SetValue(ColumnHeaderPaddingProperty, value);
        /// <summary>
        /// 标识<see cref="GridViewColumnHeader"/>的内边距
        /// 暂时无效
        /// </summary>
        public static readonly DependencyProperty ColumnHeaderPaddingProperty =
            DependencyProperty.RegisterAttached("ColumnHeaderPadding", typeof(Thickness), typeof(GridViewColumnAssistant), new PropertyMetadata(default(Thickness), OnColumnHeaderPaddingChanged));

        private static void OnColumnHeaderPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GridViewColumn column)
            {
                var defaultGridViewColumnHeaderStyle = ResourceHelper.GetResource<Style>(typeof(GridViewColumnHeader));
                Style style = new Style(typeof(GridViewColumnHeader), defaultGridViewColumnHeaderStyle);
                var setter = new Setter(ColumnHeaderPaddingProperty, GetColumnHeaderPadding(column));
                //var setter = new Setter(GridViewColumnHeader.PaddingProperty, GetColumnHeaderPadding(column));

                //var setter = (Setter)style.Setters.FirstOrDefault(setter => ((Setter)setter).Property == GridViewColumnHeader.PaddingProperty)!;
                //setter.Value = GetColumnHeaderPadding(column);

                style.Setters.Add(setter);
                column.HeaderContainerStyle = style;
            }
        }
    }


    /// <summary>
    /// 表示<see cref="ListView"/>的附加属性基类
    /// </summary>
    public class ListViewAssistant
    {
        public static double GetColumnHeaderHeight(DependencyObject obj) => (double)obj.GetValue(ColumnHeaderHeightProperty);
        public static void SetColumnHeaderHeight(DependencyObject obj, double value) => obj.SetValue(ColumnHeaderHeightProperty, value);
        /// <summary>
        /// 标识<see cref="GridViewColumnHeader"/>的标头高度
        /// </summary>
        public static readonly DependencyProperty ColumnHeaderHeightProperty =
            DependencyProperty.RegisterAttached("ColumnHeaderHeight", typeof(double), typeof(ListViewAssistant), new PropertyMetadata(ValueBoxes.Double30Box));


        public static bool GetIsColumnHeaderFontWeight(DependencyObject obj) => (bool)obj.GetValue(IsColumnHeaderFontWeightProperty);
        public static void SetIsColumnHeaderFontWeight(DependencyObject obj, bool value) => obj.SetValue(IsColumnHeaderFontWeightProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// 表示<see cref="GridViewColumnHeader"/>是否采用粗体
        /// </summary>
        public static readonly DependencyProperty IsColumnHeaderFontWeightProperty =
            DependencyProperty.RegisterAttached("IsColumnHeaderFontWeight", typeof(bool), typeof(ListViewAssistant), new PropertyMetadata(ValueBoxes.FalseBox));


        public static Thickness GetListViewItemPadding(DependencyObject obj) => (Thickness)obj.GetValue(ListViewItemPaddingProperty);
        public static void SetListViewItemPadding(DependencyObject obj, Thickness value) => obj.SetValue(ListViewItemPaddingProperty, value);
        /// <summary>
        /// 表示<see cref="ListViewItem"/>的内边距
        /// </summary>
        public static readonly DependencyProperty ListViewItemPaddingProperty =
            DependencyProperty.RegisterAttached("ListViewItemPadding", typeof(Thickness), typeof(ListViewAssistant), new PropertyMetadata(default(Thickness)));
    }
}
