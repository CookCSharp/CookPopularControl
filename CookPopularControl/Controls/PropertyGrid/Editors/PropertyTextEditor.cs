using System.Windows;


/*
 * Description：PropertyTextEditor
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-01-09 14:06:38
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */
namespace CookPopularControl.Controls
{
    public class PropertyTextEditor : PropertyItemEditorFactory
    {
        public bool IsReadOnly { get; set; }

        public PropertyTextEditor(bool isReadOnly= false)
        {
            IsReadOnly = isReadOnly;
        }

        public override FrameworkElement GetElement(PropertyItem propertyItem) => new System.Windows.Controls.TextBox
        {
            IsReadOnly = IsReadOnly
        };

        public override DependencyProperty GetDependencyProperty() => System.Windows.Controls.TextBox.TextProperty;
    }
}
