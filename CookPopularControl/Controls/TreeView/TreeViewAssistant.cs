using CookPopularControl.Tools.Boxes;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：TreeViewAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-10 10:09:03
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 提供<see cref="System.Windows.Controls.TreeView"/>的附加属性帮助类
    /// </summary>
    public class TreeViewAssistant
    {
        public static double GetExpandButtonSize(DependencyObject obj) => (double)obj.GetValue(ExpandButtonSizeProperty);
        public static void SetExpandButtonSize(DependencyObject obj, double value) => obj.SetValue(ExpandButtonSizeProperty, value);
        /// <summary>
        /// <see cref="ExpandButtonSizeProperty"/>标识展开按钮的大小
        /// </summary>
        public static readonly DependencyProperty ExpandButtonSizeProperty =
            DependencyProperty.RegisterAttached("ExpandButtonSize", typeof(double), typeof(TreeViewAssistant), new PropertyMetadata(ValueBoxes.Double10Box));


        public static Brush GetExpandButtonFill(DependencyObject obj) => (Brush)obj.GetValue(ExpandButtonFillProperty);
        public static void SetExpandButtonFill(DependencyObject obj, Brush value) => obj.SetValue(ExpandButtonFillProperty, value);
        /// <summary>
        /// <see cref="ExpandButtonFillProperty"/>标识展开按钮的填充颜色
        /// </summary>
        public static readonly DependencyProperty ExpandButtonFillProperty =
            DependencyProperty.RegisterAttached("ExpandButtonFill", typeof(Brush), typeof(TreeViewAssistant), new PropertyMetadata(default(Brush)));


        public static Geometry GetHeaderIconHasItems(DependencyObject obj) => (Geometry)obj.GetValue(HeaderIconHasItemsProperty);
        public static void SetHeaderIconHasItems(DependencyObject obj, Geometry value) => obj.SetValue(HeaderIconHasItemsProperty, value);
        /// <summary>
        /// <see cref="HeaderIconHasItemsProperty"/>标识<see cref="System.Windows.Controls.TreeView"/>有子项的标题头图标
        /// </summary>
        public static readonly DependencyProperty HeaderIconHasItemsProperty =
            DependencyProperty.RegisterAttached("HeaderIconHasItems", typeof(Geometry), typeof(TreeViewAssistant), new PropertyMetadata(Geometry.Empty));


        public static Geometry GetHeaderIconNoItems(DependencyObject obj) => (Geometry)obj.GetValue(HeaderIconNoItemsProperty);
        public static void SetHeaderIconNoItems(DependencyObject obj, Geometry value) => obj.SetValue(HeaderIconNoItemsProperty, value);
        /// <summary>
        /// <see cref="HeaderIconNoItemsProperty"/>标识<see cref="System.Windows.Controls.TreeView"/>无子项的标题头图标
        /// </summary>
        public static readonly DependencyProperty HeaderIconNoItemsProperty =
            DependencyProperty.RegisterAttached("HeaderIconNoItems", typeof(Geometry), typeof(TreeViewAssistant), new PropertyMetadata(Geometry.Empty));


        public static Brush GetHeaderIconFill(DependencyObject obj) => (Brush)obj.GetValue(HeaderIconFillProperty);
        public static void SetHeaderIconFill(DependencyObject obj, Brush value) => obj.SetValue(HeaderIconFillProperty, value);
        /// <summary>
        /// <see cref="HeaderIconFillProperty"/>标识<see cref="System.Windows.Controls.TreeView"/>的标题头填充颜色
        /// </summary>
        public static readonly DependencyProperty HeaderIconFillProperty =
            DependencyProperty.RegisterAttached("HeaderIconFill", typeof(Brush), typeof(TreeViewAssistant), new PropertyMetadata(default(Brush)));


        public static bool GetIsShowHeaderIcon(DependencyObject obj) => (bool)obj.GetValue(IsShowHeaderIconProperty);
        public static void SetIsShowHeaderIcon(DependencyObject obj, bool value) => obj.SetValue(IsShowHeaderIconProperty, value);
        /// <summary>
        /// <see cref="IsShowHeaderIconProperty"/>标识<see cref="System.Windows.Controls.TreeView"/>是否显示标题头
        /// </summary>
        public static readonly DependencyProperty IsShowHeaderIconProperty =
            DependencyProperty.RegisterAttached("IsShowHeaderIcon", typeof(bool), typeof(TreeViewAssistant), new PropertyMetadata(ValueBoxes.FalseBox));
    }
}
