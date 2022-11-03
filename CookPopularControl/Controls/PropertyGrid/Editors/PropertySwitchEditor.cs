using CookPopularCSharpToolkit.Windows;
using System.ComponentModel;
using System.Drawing.Imaging;
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
        private static PropertyItem _propertyItem = new PropertyItem();
        private SwitchButton _switchButton = new SwitchButton
        {
            IsEnabled = !_propertyItem.IsReadOnly,
            HorizontalAlignment = HorizontalAlignment.Left,
            SwicthCloseBackground = ResourceHelper.GetResource<Brush>("UnEnabledBrush"),
            SwitchOpenBackground = ResourceHelper.GetResource<Brush>("ControlPressBackground"),
        };

        public PropertySwitchEditor()
        {
            Themes.ThemeProvider.ThemeChanged += (s, e) =>
            {
                _switchButton.SwitchOpenBackground = e.ThemeDictionary["ControlPressBackground"] as Brush;
            };
        }

        public override FrameworkElement GetElement(PropertyItem propertyItem)
        {
            _propertyItem = propertyItem;

            return _switchButton;
        }

        public override DependencyProperty GetDependencyProperty() => ToggleButton.IsCheckedProperty;
    }
}
