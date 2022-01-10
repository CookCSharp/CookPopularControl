using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Description：PropertyButtonEditor 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-01-10 11:34:20
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */
namespace CookPopularControl.Controls
{
    public class PropertyButtonEditor : PropertyItemEditorFactory
    {
        public override FrameworkElement GetElement(PropertyItem propertyItem) => new System.Windows.Controls.Button
        {
            IsEnabled = !propertyItem.IsReadOnly,
        };

        public override DependencyProperty GetDependencyProperty() => System.Windows.Controls.Button.ContentProperty;
    }
}
