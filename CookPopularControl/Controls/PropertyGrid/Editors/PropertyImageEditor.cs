using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;


/*
 * Description：PropertyTimerPickerEditor
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-01-09 14:10:38
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */
namespace CookPopularControl.Controls
{
    public class PropertyImageEditor : PropertyItemEditorFactory
    {
        public override FrameworkElement GetElement(PropertyItem propertyItem) => new Image
        {
            IsEnabled = !propertyItem.IsReadOnly,
            Width = 60,
            Height = 60,
            Stretch = Stretch.Uniform,
            StretchDirection = StretchDirection.Both,
            HorizontalAlignment = HorizontalAlignment.Left
        };

        public override BindingMode GetBindingMode(PropertyItem propertyItem) => BindingMode.OneWay;

        public override DependencyProperty GetDependencyProperty() => Image.SourceProperty;
    }
}
