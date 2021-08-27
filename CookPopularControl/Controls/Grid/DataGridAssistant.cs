using CookPopularControl.Tools.Boxes;
using CookPopularControl.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
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
    [TemplatePart(Name = ElementScrollViewer, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = SelectAllButton, Type = typeof(System.Windows.Controls.Button))]
    [TemplatePart(Name = RowHeader, Type = typeof(DataGridRowHeader))]
    [TemplatePart(Name = RowHeaderCheckBox, Type = typeof(System.Windows.Controls.CheckBox))]
    public class DataGridAssistant
    {
        private const string ElementScrollViewer = "PART_ScrollViewer";
        private const string SelectAllButton = "PART_SelectAllButton";
        private const string RowHeader = "PART_DataGridRowHeader";
        private const string RowHeaderCheckBox = "PART_RowCheckBox";
        private const string ElementText = nameof(ElementText);
        private const string ElementComboBox = nameof(ElementComboBox);
        private const string ElementCheckBox = nameof(ElementCheckBox);


        #region IsApplyDefaultStyle


        public static bool GetIsApplyDefaultStyle(DependencyObject obj) => (bool)obj.GetValue(IsApplyDefaultStyleProperty);
        public static void SetIsApplyDefaultStyle(DependencyObject obj, bool value) => obj.SetValue(IsApplyDefaultStyleProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsApplyDefaultStyleProperty"/>标识<see cref="DataGridCell"/>是否引用CookPopularControl的默认样式
        /// </summary>
        public static readonly DependencyProperty IsApplyDefaultStyleProperty =
            DependencyProperty.RegisterAttached("IsApplyDefaultStyle", typeof(bool), typeof(DataGridAssistant), new PropertyMetadata(ValueBoxes.FalseBox, OnIsApplyDefaultStyleChanged));
        private static void OnIsApplyDefaultStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dg)
            {
                if ((bool)e.NewValue)
                {
                    if (dg.AutoGenerateColumns)
                    {
                        dg.AutoGeneratingColumn += Dg_AutoGeneratingColumn;
                        return;
                    }

                    dg.Loaded += (s, e) => SetDataGridCellDefaultStyle(dg);
                }
                else
                {
                    dg.AutoGeneratingColumn += Dg_AutoGeneratingColumn;
                }
            }
        }

        private static void Dg_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (sender is DataGrid dg)
            {
                SetDataGridCellDefaultStyle(dg);
            }
        }

        private static void SetDataGridCellDefaultStyle(DataGrid dg)
        {
            foreach (var column in dg.Columns.OfType<DataGridColumn>())
            {
                if (column.GetType() == typeof(DataGridTextColumn))
                {
                    SetDataGridCellElementStyle(column, dg, ElementText);
                    SetDataGridCellEditingElementStyle(column, dg, ElementText);
                }
                else if (column.GetType() == typeof(DataGridComboBoxColumn))
                {
                    SetDataGridCellElementStyle(column, dg, ElementComboBox);
                    SetDataGridCellEditingElementStyle(column, dg, ElementComboBox);
                }
                else if (column.GetType() == typeof(DataGridCheckBoxColumn))
                {
                    SetDataGridCellElementStyle(column, dg, ElementCheckBox);
                    SetDataGridCellEditingElementStyle(column, dg, ElementCheckBox);
                }
            }
        }

        #endregion

        #region IsRegisterSelectAll

        public static bool GetIsRegisterSelectAll(DependencyObject obj) => (bool)obj.GetValue(IsRegisterSelectAllProperty);
        public static void SetIsRegisterSelectAll(DependencyObject obj, bool value) => obj.SetValue(IsRegisterSelectAllProperty, value);
        /// <summary>
        /// <see cref="IsRegisterSelectAllProperty"/>标识注册全选与取消全选事件
        /// </summary>
        public static readonly DependencyProperty IsRegisterSelectAllProperty =
            DependencyProperty.RegisterAttached("IsRegisterSelectAll", typeof(bool), typeof(DataGridAssistant), new PropertyMetadata(ValueBoxes.FalseBox, OnIsRegisterSelectAllChanged));

        private static void OnIsRegisterSelectAllChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dataGrid)
            {
                dataGrid.Loaded += (s, arg) =>
                {
                    var scrollViewer = dataGrid.Template.FindName(ElementScrollViewer, dataGrid) as ScrollViewer;
                    var selectedAllButton = scrollViewer.Template.FindName(SelectAllButton, scrollViewer) as System.Windows.Controls.Button;

                    /**
                     * 如果注册事件，
                     * 则禁用PART_SelectAllButton控件模板中的Button的SelectAllCommand命令，
                     * 否则将达不到取消全选的效果
                     */
                    var btnButton = selectedAllButton.Template.FindName("SelectAllButton", selectedAllButton) as System.Windows.Controls.Button;
                    btnButton.Command = null;
                    if ((bool)e.NewValue)
                    {
                        selectedAllButton.Click += (s, e) => SetSelectedAllButton(dataGrid);
                    }
                    else
                    {
                        selectedAllButton.Click -= (s, e) => SetSelectedAllButton(dataGrid);
                    }
                };
            }
        }

        private static void SetSelectedAllButton(DataGrid dg)
        {
            if (dg.SelectedItems.Count < dg.Items.Count)
            {
                dg.SelectAll();
            }
            else
            {
                dg.UnselectAll();
            }
        }

        #endregion

        #region IsShowSerialNumber

        public static bool GetIsShowSerialNumber(DependencyObject obj) => (bool)obj.GetValue(IsShowSerialNumberProperty);
        public static void SetIsShowSerialNumber(DependencyObject obj, bool value) => obj.SetValue(IsShowSerialNumberProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsShowSerialNumberProperty"/>是否显示排序
        /// </summary>
        public static readonly DependencyProperty IsShowSerialNumberProperty =
            DependencyProperty.RegisterAttached("IsShowSerialNumber", typeof(bool), typeof(DataGridAssistant),
                new PropertyMetadata(ValueBoxes.FalseBox, OnIsShowSerialNumberChanged));

        private static void OnIsShowSerialNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dg)
            {
                if ((bool)e.NewValue)
                {
                    dg.LoadingRow += Gd_LoadingRow;
                }
                else
                {
                    dg.LoadingRow -= Gd_LoadingRow;
                }
            }
        }

        private static void Gd_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dg = sender as DataGrid;
            var dgr = e.Row;
            if (dg != null && dgr != null)
            {
                if (dgr.IsLoaded)
                    DataGridRowLoaded(dg, dgr);
                else
                    dgr.Loaded += (s, e) => DataGridRowLoaded(dg, dgr);
            }
        }

        private static void DataGridRowLoaded(DataGrid dg, DataGridRow dgr)
        {
            var rowHeader = dgr.Template.FindName(RowHeader, dgr) as DataGridRowHeader;
            var checkBox = rowHeader.Template.FindName(RowHeaderCheckBox, rowHeader) as System.Windows.Controls.CheckBox;
            if (checkBox == null) return;
            if (GetIsShowSerialNumber(dg))
                checkBox.Content = (dgr.GetIndex() + 1).ToString();
            else
                checkBox.Content = null;
        }

        #endregion

        #region IsShowThickness

        public static bool GetIsShowThickness(DependencyObject obj) => (bool)obj.GetValue(IsShowThicknessProperty);
        public static void SetIsShowThickness(DependencyObject obj, bool value) => obj.SetValue(IsShowThicknessProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsShowThicknessProperty"/>标识是否显示线框
        /// </summary>
        public static readonly DependencyProperty IsShowThicknessProperty =
            DependencyProperty.RegisterAttached("IsShowThickness", typeof(bool), typeof(DataGridAssistant), new PropertyMetadata(ValueBoxes.FalseBox));

        #endregion

        #region IsEnableEditWithoutFocused

        public static bool GetIsEnableEditWithoutFocused(DependencyObject obj) => (bool)obj.GetValue(IsEnableEditWithoutFocusedProperty);
        public static void SetIsEnableEditWithoutFocused(DependencyObject obj, bool value) => obj.SetValue(IsEnableEditWithoutFocusedProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsEnableEditWithoutFocusedProperty"/>标识允许使用单击后在<see cref="DataGrid"/>的<see cref="DataGridCell"/>内部编辑组件。
        /// </summary>
        /// <remarks>不需要先获取单元格焦点</remarks>
        public static readonly DependencyProperty IsEnableEditWithoutFocusedProperty =
            DependencyProperty.RegisterAttached("IsEnableEditWithoutFocused", typeof(bool), typeof(DataGridAssistant), new PropertyMetadata(ValueBoxes.FalseBox, OnIsEnableEditWithoutFocusedChanged));

        private static void OnIsEnableEditWithoutFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dg)
            {
                if ((bool)e.NewValue)
                {
                    dg.PreviewMouseLeftButtonDown += Dg_PreviewMouseLeftButtonDown;
                    dg.KeyDown += Dg_KeyDown;
                }
                else
                {
                    dg.PreviewMouseLeftButtonDown -= Dg_PreviewMouseLeftButtonDown;
                    dg.KeyDown -= Dg_KeyDown;
                }
            }
        }

        private static void Dg_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var originalSource = (DependencyObject)e.OriginalSource;
            var dataGridCell = originalSource.GetVisualAncestry().OfType<DataGridCell>().FirstOrDefault();

            if (dataGridCell?.IsReadOnly ?? true) return;

            if (dataGridCell?.Content is UIElement element)
            {
                var dataGrid = (DataGrid)sender;

                var mousePosition = e.GetPosition(element);
                var elementHitBox = new Rect(element.RenderSize);
                if (elementHitBox.Contains(mousePosition))
                {
                    if (dataGridCell.Column.GetType() == typeof(DataGridTemplateColumn))
                    {
                        return;
                    }

                    dataGrid.CurrentCell = new DataGridCellInfo(dataGridCell);
                    dataGrid.BeginEdit();

                    switch (dataGridCell?.Content)
                    {
                        case ToggleButton toggleButton:
                            {
                                var newMouseEvent = new MouseButtonEventArgs(e.MouseDevice, 0, MouseButton.Left)
                                {
                                    RoutedEvent = Mouse.MouseDownEvent,
                                    Source = dataGrid
                                };

                                toggleButton.RaiseEvent(newMouseEvent);
                                break;
                            }
                        case System.Windows.Controls.ComboBox comboBox:
                            {
                                comboBox.IsDropDownOpen = !comboBox.IsDropDownOpen;
                                e.Handled = true;
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
        }

        private static void Dg_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (sender is DataGrid dg)
            {
                if (e.Key == System.Windows.Input.Key.Space && e.OriginalSource is DataGridCell cell && !cell.IsReadOnly && cell.Column is DataGridComboBoxColumn)
                    dg.BeginEdit();
            }
        }

        #endregion

        #region DataGridCell

        public static Style GetTextColumnStyle(DependencyObject obj) => (Style)obj.GetValue(TextColumnStyleProperty);
        public static void SetTextColumnStyle(DependencyObject obj, Style value) => obj.SetValue(TextColumnStyleProperty, value);
        /// <summary>
        /// <see cref="TextColumnStyleProperty"/>标识<see cref="DataGridCell"/>默认文本样式
        /// </summary>
        public static readonly DependencyProperty TextColumnStyleProperty =
            DependencyProperty.RegisterAttached("TextColumnStyle", typeof(Style), typeof(DataGridAssistant), new PropertyMetadata(default(Style), OnTextColumnStyleChanged));

        private static void OnTextColumnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dg && e.NewValue != null)
            {
                dg.AutoGeneratingColumn += (s, e) => SetDataGridCellElementStyle(e.Column, dg, ElementText);
                dg.AutoGeneratingColumn -= (s, e) => SetDataGridCellElementStyle(e.Column, dg, ElementText);
            }
        }


        public static Style GetTextColumnEditingStyle(DependencyObject obj) => (Style)obj.GetValue(TextColumnEditingStyleProperty);
        public static void SetTextColumnEditingStyle(DependencyObject obj, Style value) => obj.SetValue(TextColumnEditingStyleProperty, value);
        /// <summary>
        /// <see cref="TextColumnStyleProperty"/>标识<see cref="DataGridCell"/>编辑时文本样式
        /// </summary>
        public static readonly DependencyProperty TextColumnEditingStyleProperty =
            DependencyProperty.RegisterAttached("TextColumnEditingStyle", typeof(Style), typeof(DataGridAssistant), new PropertyMetadata(default(Style), OnTextColumnEditingStyleChanged));

        private static void OnTextColumnEditingStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dg && e.NewValue != null)
            {
                dg.AutoGeneratingColumn += (s, e) => SetDataGridCellEditingElementStyle(e.Column, dg, ElementText);
                dg.AutoGeneratingColumn -= (s, e) => SetDataGridCellEditingElementStyle(e.Column, dg, ElementText);
            }
        }


        public static Style GetComboBoxColumnStyle(DependencyObject obj) => (Style)obj.GetValue(ComboBoxColumnStyleProperty);
        public static void SetComboBoxColumnStyle(DependencyObject obj, Style value) => obj.SetValue(ComboBoxColumnStyleProperty, value);
        public static readonly DependencyProperty ComboBoxColumnStyleProperty =
            DependencyProperty.RegisterAttached("ComboBoxColumnStyle", typeof(Style), typeof(DataGridAssistant), new PropertyMetadata(default(Style), OnComboBoxColumnStyleChanged));

        private static void OnComboBoxColumnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dg && e.NewValue != null)
            {
                dg.AutoGeneratingColumn += (s, e) => SetDataGridCellElementStyle(e.Column, dg, ElementComboBox);
                dg.AutoGeneratingColumn -= (s, e) => SetDataGridCellElementStyle(e.Column, dg, ElementComboBox);
            }
        }


        public static Style GetComboBoxColumnEditingStyle(DependencyObject obj) => (Style)obj.GetValue(ComboBoxColumnEditingStyleProperty);
        public static void SetComboBoxColumnEditingStyle(DependencyObject obj, Style value) => obj.SetValue(ComboBoxColumnEditingStyleProperty, value);
        public static readonly DependencyProperty ComboBoxColumnEditingStyleProperty =
            DependencyProperty.RegisterAttached("ComboBoxColumnEditingStyle", typeof(Style), typeof(DataGridAssistant), new PropertyMetadata(default(Style), OnComboBoxColumnEditingStyleChanged));

        private static void OnComboBoxColumnEditingStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dg && e.NewValue != null)
            {
                dg.AutoGeneratingColumn += (s, e) => SetDataGridCellEditingElementStyle(e.Column, dg, ElementComboBox);
                dg.AutoGeneratingColumn -= (s, e) => SetDataGridCellEditingElementStyle(e.Column, dg, ElementComboBox);
            }
        }


        public static Style GetCheckBoxColumnStyle(DependencyObject obj) => (Style)obj.GetValue(CheckBoxColumnStyleProperty);
        public static void SetCheckBoxColumnStyle(DependencyObject obj, Style value) => obj.SetValue(CheckBoxColumnStyleProperty, value);
        public static readonly DependencyProperty CheckBoxColumnStyleProperty =
            DependencyProperty.RegisterAttached("CheckBoxColumnStyle", typeof(Style), typeof(DataGridAssistant), new PropertyMetadata(default(Style), OnCheckBoxColumnStyleChanged));

        private static void OnCheckBoxColumnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dg && e.NewValue != null)
            {
                dg.AutoGeneratingColumn += (s, e) => SetDataGridCellElementStyle(e.Column, dg, ElementCheckBox);
                dg.AutoGeneratingColumn -= (s, e) => SetDataGridCellElementStyle(e.Column, dg, ElementCheckBox);
            }
        }


        public static Style GetCheckBoxColumnEditingStyle(DependencyObject obj) => (Style)obj.GetValue(CheckBoxColumnEditingStyleProperty);
        public static void SetCheckBoxColumnEditingStyle(DependencyObject obj, Style value) => obj.SetValue(CheckBoxColumnEditingStyleProperty, value);
        public static readonly DependencyProperty CheckBoxColumnEditingStyleProperty =
            DependencyProperty.RegisterAttached("CheckBoxColumnEditingStyle", typeof(Style), typeof(DataGridAssistant), new PropertyMetadata(default(Style), OnCheckBoxColumnEditingStyleChanged));

        private static void OnCheckBoxColumnEditingStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dg && e.NewValue != null)
            {
                dg.AutoGeneratingColumn += (s, e) => SetDataGridCellEditingElementStyle(e.Column, dg, ElementCheckBox);
                dg.AutoGeneratingColumn -= (s, e) => SetDataGridCellEditingElementStyle(e.Column, dg, ElementCheckBox);
            }
        }


        private static void SetDataGridCellElementStyle(DataGridColumn column, DataGrid dg, string elementName)
        {
            object o = elementName switch
            {
                ElementText => (column as DataGridTextColumn).ElementStyle = GetTextColumnStyle(dg),
                ElementCheckBox => (column as DataGridCheckBoxColumn).ElementStyle = GetCheckBoxColumnStyle(dg),
                ElementComboBox => (column as DataGridComboBoxColumn).ElementStyle = GetComboBoxColumnStyle(dg),
                _ => throw new NotImplementedException(),
            };
        }

        private static void SetDataGridCellEditingElementStyle(DataGridColumn column, DataGrid dg, string elementName)
        {
            object o = elementName switch
            {
                ElementText => (column as DataGridTextColumn).EditingElementStyle = GetTextColumnEditingStyle(dg),
                ElementCheckBox => (column as DataGridCheckBoxColumn).EditingElementStyle = GetCheckBoxColumnEditingStyle(dg),
                ElementComboBox => (column as DataGridComboBoxColumn).EditingElementStyle = GetComboBoxColumnEditingStyle(dg),
                _ => throw new NotImplementedException(),
            };
        }

        #endregion
    }
}
