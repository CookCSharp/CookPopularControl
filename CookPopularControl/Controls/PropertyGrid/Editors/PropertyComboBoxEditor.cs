using CookPopularCSharpToolkit.Windows;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls.Primitives;


/*
 * Description：PropertyComboBoxEditor 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-01-09 14:04:38
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */
namespace CookPopularControl.Controls
{
    public class PropertyComboBoxEditor : PropertyItemEditorFactory
    {
        public int SelectedIndex { get; private set; }

        public PropertyComboBoxEditor(int selectedIndex)
        {
            SelectedIndex = selectedIndex;
        }

        public override FrameworkElement GetElement(PropertyItem propertyItem) => new System.Windows.Controls.ComboBox
        {
            IsEnabled = !propertyItem.IsReadOnly,
            SelectedIndex = SelectedIndex,
            ItemsSource = propertyItem.PropertyType.BaseType == typeof(Enum) ? Enum.GetValues(propertyItem.PropertyType) : propertyItem.Value as IEnumerable,
        };

        public override DependencyProperty GetDependencyProperty() => Selector.SelectedItemProperty;
    }
}
