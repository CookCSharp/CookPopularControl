using CookPopularCSharpToolkit.Communal;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：TabControl
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-31 10:40:10
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 提供<see cref="TabControl"/>中<see cref="TabItem"/>的附加属性类
    /// </summary>
    [TemplatePart(Name = ElementTabPanel, Type = typeof(TabPanel))]
    [TemplatePart(Name = ElementClearButton, Type = typeof(System.Windows.Controls.Button))]
    public class TabControlAssistant
    {
        private const string ElementTabPanel = "HeaderPanel";
        private const string ElementClearButton = "PART_CloseButton";

        public static bool GetIsAddClearButton(DependencyObject obj) => (bool)obj.GetValue(IsAddClearButtonProperty);
        public static void SetIsAddClearButton(DependencyObject obj, bool value) => obj.SetValue(IsAddClearButtonProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsAddClearButtonProperty"/>标识是否增加删除按钮
        /// </summary>
        public static readonly DependencyProperty IsAddClearButtonProperty =
            DependencyProperty.RegisterAttached("IsAddClearButton", typeof(bool), typeof(TabControlAssistant), new PropertyMetadata(ValueBoxes.FalseBox, OnIsAddClearButtonChanged));

        private static void OnIsAddClearButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabItem tabItem)
            {
                tabItem.Loaded += (s, e) => ClearHandler(tabItem);
            }
        }

        private static void ClearHandler(TabItem tabItem)
        {
            var parentElement = ItemsControl.ItemsControlFromItemContainer(tabItem) as System.Windows.Controls.TabControl;
            if (parentElement != null)
            {
                var value = GetIsAddClearButton(parentElement);
                var clearButton = tabItem.Template.FindName(ElementClearButton, tabItem) as System.Windows.Controls.Button;
                if (clearButton != null)
                {
                    RoutedEventHandler handler = (s, e) =>
                    {
                        //var item = parentElement.ItemContainerGenerator.ContainerFromItem(tabItem);
                        var item = parentElement.ItemContainerGenerator.ItemFromContainer(tabItem);
                        var list = GetActualList(parentElement);
                        list?.Remove(item);
                    };

                    if (value)
                        clearButton.Click += handler;
                    else
                        clearButton.Click -= handler;
                }
            }
        }

        private static IList GetActualList(ItemsControl itemsControl)
        {
            IList? list;
            if (itemsControl.ItemsSource != null)
            {
                list = itemsControl.ItemsSource as IList;
            }
            else
            {
                list = itemsControl.Items;
            }

            return list;
        }
    }
}
