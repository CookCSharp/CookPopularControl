using System.Windows;


/*
 * Description：PropertyNumberEditor
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-01-09 14:08:38
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */
namespace CookPopularControl.Controls
{
    public class PropertyNumberEditor : PropertyItemEditorFactory
    {
        public PropertyNumberEditor()
        {

        }

        public PropertyNumberEditor(double minimum, double maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public double Minimum { get; set; }

        public double Maximum { get; set; }

        public override FrameworkElement GetElement(PropertyItem propertyItem) => new NumericUpDown
        {
            IsReadOnly = propertyItem.IsReadOnly,
            Minimum = Minimum,
            Maximum = Maximum
        };

        public override DependencyProperty GetDependencyProperty() => NumericUpDown.ValueProperty;
    }
}
