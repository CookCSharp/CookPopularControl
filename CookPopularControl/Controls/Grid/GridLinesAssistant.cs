using CookPopularControl.Tools.Boxes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OriginGrid = System.Windows.Controls.Grid;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：GridLineHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-31 17:42:13
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 提供<see cref="System.Windows.Controls.Grid.ShowGridLines"/>的辅助类
    /// </summary>
    public class GridLinesAssistant
    {
        #region ShowBorder

        public static bool GetShowBorder(DependencyObject obj) => (bool)obj.GetValue(ShowBorderProperty);
        public static void SetShowBorder(DependencyObject obj, bool value) => obj.SetValue(ShowBorderProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="ShowBorderProperty"/>标识是否显示外边框
        /// </summary>
        public static readonly DependencyProperty ShowBorderProperty =
                DependencyProperty.RegisterAttached("ShowBorder", typeof(bool), typeof(GridLinesAssistant), new PropertyMetadata(OnGridPropertiesChanged));

        #endregion

        #region GridLineBrush

        public static Brush GetGridLineBrush(DependencyObject obj) => (Brush)obj.GetValue(GridLineBrushProperty) ?? Brushes.Gray;
        public static void SetGridLineBrush(DependencyObject obj, Brush value) => obj.SetValue(GridLineBrushProperty, value);
        public static readonly DependencyProperty GridLineBrushProperty =
                DependencyProperty.RegisterAttached("GridLineBrush", typeof(Brush), typeof(GridLinesAssistant), new PropertyMetadata(OnGridPropertiesChanged));

        #endregion

        #region GridLineThickness

        public static double GetGridLineThickness(DependencyObject obj) => (double)obj.GetValue(GridLineThicknessProperty);
        public static void SetGridLineThickness(DependencyObject obj, double value) => obj.SetValue(GridLineThicknessProperty, value);
        public static readonly DependencyProperty GridLineThicknessProperty =
                DependencyProperty.RegisterAttached("GridLineThickness", typeof(double), typeof(GridLinesAssistant), new PropertyMetadata(OnGridPropertiesChanged));

        #endregion


        private static void OnGridPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as OriginGrid;
            if (grid is null) return;

            if (grid.IsLoaded)
                GridLoaded(grid);
            else
                grid.Loaded += (s, e) => GridLoaded(grid);
        }


        private static void GridLoaded(OriginGrid grid)
        {
            var row_count = grid.RowDefinitions.Count;
            var column_count = grid.ColumnDefinitions.Count;

            var controls = grid.Children;
            var count = controls.Count;

            for (int i = 0; i < count; i++)
            {
                var item = controls[i] as FrameworkElement;
                var row = OriginGrid.GetRow(item);
                var column = OriginGrid.GetColumn(item);
                var rowspan = OriginGrid.GetRowSpan(item);
                var columnspan = OriginGrid.GetColumnSpan(item);

                var settingThickness = GetGridLineThickness(grid);
                Thickness thickness = new Thickness(settingThickness / 2);

                if (row == 0)
                    thickness.Top = settingThickness;
                if (row + rowspan == row_count)
                    thickness.Bottom = settingThickness;
                if (column == 0)
                    thickness.Left = settingThickness;
                if (column + columnspan == column_count)
                    thickness.Right = settingThickness;

                //如果Child是Border就不要重新实例化一个Border，否则会造成Border叠加
                Border border;
                if (item is Border child)
                {
                    child.BorderBrush = GetGridLineBrush(grid);
                    child.BorderThickness = thickness;
                    child.Padding = new Thickness(0);
                    border = child;
                }
                else
                {
                    border = new Border()
                    {
                        BorderBrush = GetGridLineBrush(grid),
                        BorderThickness = thickness,
                        Padding = new Thickness(0),
                    };
                }

                //判断是否画Grid最外层边框
                if (!GetShowBorder(grid))
                {
                    if (row == 0 && column == 0)
                        border.BorderThickness = new Thickness(0, 0, border.BorderThickness.Right, border.BorderThickness.Bottom);
                    else if (row == 0 && column > 0 && column < column_count - 1)
                        border.BorderThickness = new Thickness(border.BorderThickness.Left, 0, border.BorderThickness.Right, border.BorderThickness.Bottom);
                    else if (row == 0 && column == column_count - 1)
                        border.BorderThickness = new Thickness(border.BorderThickness.Left, 0, 0, border.BorderThickness.Bottom);

                    else if (column == 0 && row > 0 && row < row_count - 1)
                        border.BorderThickness = new Thickness(0, border.BorderThickness.Top, border.BorderThickness.Right, border.BorderThickness.Bottom);
                    else if (column == column_count - 1 && row > 0 && row < row_count - 1)
                        border.BorderThickness = new Thickness(border.BorderThickness.Left, border.BorderThickness.Top, 0, border.BorderThickness.Bottom);

                    else if (row == row_count - 1 && column == 0)
                        border.BorderThickness = new Thickness(0, border.BorderThickness.Top, border.BorderThickness.Right, 0);
                    else if (row == row_count - 1 && column > 0 && column < column_count - 1)
                        border.BorderThickness = new Thickness(border.BorderThickness.Left, border.BorderThickness.Top, border.BorderThickness.Right, 0);
                    else if (row == row_count - 1 && column == column_count - 1)
                        border.BorderThickness = new Thickness(border.BorderThickness.Left, border.BorderThickness.Top, 0, 0);
                }

                OriginGrid.SetRow(border, row);
                OriginGrid.SetColumn(border, column);
                OriginGrid.SetRowSpan(border, rowspan);
                OriginGrid.SetColumnSpan(border, columnspan);

                //只有当Child是Border的时候进行删除操作即可，Child是Border的时候改变边框属性即可
                if (item is not Border)
                {
                    grid.Children.RemoveAt(i);
                    border.Child = item;
                    grid.Children.Insert(i, border);
                }
            }
        }
    }
}
