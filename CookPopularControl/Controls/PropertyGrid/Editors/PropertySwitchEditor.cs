using CookPopularCSharpToolkit.Windows;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;


/*
 * Description：PropertySwitchEditor 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-01-09 14:15:38
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */
namespace CookPopularControl.Controls
{
    public class PropertySwitchEditor : PropertyItemEditorFactory
    {
        public override FrameworkElement GetElement(PropertyItem propertyItem) => new SwitchButton
        {
            IsEnabled = !propertyItem.IsReadOnly,
            HorizontalAlignment = HorizontalAlignment.Left,
            SwicthCloseBackground = ResourceHelper.GetResource<Brush>("ControlMouseOverBackground"),
            SwitchOpenBackground = ResourceHelper.GetResource<Brush>("ControlPressBackground"),
        };

        public override DependencyProperty GetDependencyProperty() => ToggleButton.IsCheckedProperty;
    }
}
