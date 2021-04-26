using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DataGridAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-02 17:03:39
 */
namespace CookPopularControl.Controls.Grid
{
    /// <summary>
    /// 提供<see cref="DataGrid"/>的附加属性基类
    /// </summary>
    [TemplatePart(Name = RowHeader, Type = typeof(DataGridRowHeader))]
    [TemplatePart(Name = RowHeaderCheckBox, Type = typeof(System.Windows.Controls.CheckBox))]
    public class DataGridAssistant
    {
        private const string RowHeader = "PART_DataGridRowHeader";
        private const string RowHeaderCheckBox = "PART_RowCheckBox";

        public static bool GetIsSelectedAll(DependencyObject obj) => (bool)obj.GetValue(IsSelectedAllProperty);
        public static void SetIsSelectedAll(DependencyObject obj, bool value) => obj.SetValue(IsSelectedAllProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsSelectedAllProperty"/>标识是否全选
        /// </summary>
        public static readonly DependencyProperty IsSelectedAllProperty =
            DependencyProperty.RegisterAttached("IsSelectedAll", typeof(bool), typeof(DataGridAssistant),
                new PropertyMetadata(ValueBoxes.FalseBox, OnIsSelectedAllChanged));

        private static void OnIsSelectedAllChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dg = d as DataGrid;
            if (dg != null)
            {
                if (!(bool)e.NewValue && dg.SelectedItems.Count == dg.Items.Count)
                {
                    dg.UnselectAll();
                }
            }
        }

        public static bool GetIsShowSerialNumber(DependencyObject obj) => (bool)obj.GetValue(IsShowSerialNumberProperty);
        public static void SetIsShowSerialNumber(DependencyObject obj, bool value) => obj.SetValue(IsShowSerialNumberProperty, value);
        /// <summary>
        /// <see cref="IsShowSerialNumberProperty"/>是否显示排序
        /// </summary>
        public static readonly DependencyProperty IsShowSerialNumberProperty =
            DependencyProperty.RegisterAttached("IsShowSerialNumber", typeof(bool), typeof(DataGridAssistant),
                new PropertyMetadata(ValueBoxes.FalseBox, OnIsShowSerialNumberChanged));

        private static void OnIsShowSerialNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dg = d as DataGrid;
            if (dg != null)
            {
                dg.LoadingRow += Gd_LoadingRow;
            }
        }

        private static void Gd_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dg = sender as DataGrid;
            var dgr = e.Row;
            if (dgr != null)
            {
                if (dgr.IsLoaded)
                    DataGridLoaded(dg!, dgr);
                else
                    dgr.Loaded += (s, e) => DataGridLoaded(dg!, dgr);
            }
        }

        private static void DataGridLoaded(DataGrid dg, DataGridRow dgr)
        {
            var rowHeader = dgr.Template.FindName(RowHeader, dgr) as DataGridRowHeader;
            var checkBox = rowHeader.Template.FindName(RowHeaderCheckBox, rowHeader) as System.Windows.Controls.CheckBox;
            if (checkBox == null) return;
            if (GetIsShowSerialNumber(dg))
                checkBox.Content = (dgr.GetIndex() + 1).ToString();
            else
                checkBox.Content = null;
            checkBox.IsChecked = GetIsSelectedAll(dg);
        }
    }
}
