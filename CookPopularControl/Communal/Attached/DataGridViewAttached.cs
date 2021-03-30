using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DataGridViewAttached
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-30 11:21:15
 */
namespace CookPopularControl.Communal.Attached
{
    /// <summary>
    /// 提供<see cref="DataGrid"/>与<see cref="GridView"/>的附加属性基类
    /// </summary>
    public class DataGridViewAttached
    {
        public static double GetColumnMinWidth(DependencyObject obj) => (double)obj.GetValue(ColumnMinWidthProperty);
        public static void SetColumnMinWidth(DependencyObject obj, double value) => obj.SetValue(ColumnMinWidthProperty, value);
        /// <summary>
        /// <see cref="ColumnMinWidthProperty"/>标识列项最小宽度
        /// </summary>
        public static readonly DependencyProperty ColumnMinWidthProperty =
            DependencyProperty.RegisterAttached("ColumnMinWidth", typeof(double), typeof(DataGridViewAttached), new PropertyMetadata(ValueBoxes.Double30Box));

        public static double GetColumnHeaderHeight(DependencyObject obj) => (double)obj.GetValue(ColumnHeaderHeightProperty);
        public static void SetColumnHeaderHeight(DependencyObject obj, double value) => obj.SetValue(ColumnHeaderHeightProperty, value);
        /// <summary>
        /// 标识ColumnHeader的高度
        /// </summary>
        public static readonly DependencyProperty ColumnHeaderHeightProperty =
            DependencyProperty.RegisterAttached("ColumnHeaderHeight", typeof(double), typeof(DataGridViewAttached), new PropertyMetadata(ValueBoxes.Double20Box));

        public static Thickness GetColumnHeaderPadding(DependencyObject obj) => (Thickness)obj.GetValue(ColumnHeaderPaddingProperty);
        public static void SetColumnHeaderPadding(DependencyObject obj, Thickness value) => obj.SetValue(ColumnHeaderPaddingProperty, value);
        /// <summary>
        /// 标识ColumnHeader的内容的内边距
        /// </summary>
        public static readonly DependencyProperty ColumnHeaderPaddingProperty =
            DependencyProperty.RegisterAttached("ColumnHeaderPadding", typeof(Thickness), typeof(DataGridViewAttached), new PropertyMetadata(default(Thickness)));
    }
}
