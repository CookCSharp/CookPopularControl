using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;



/*
 * Description：PropertyItemEditorFactory 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-01-09 13:59:38
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */
namespace CookPopularControl.Controls
{
    public abstract class PropertyItemEditorFactory : DependencyObject
    {
        public virtual void SetBinding(DependencyObject element, PropertyItem propertyItem)
        {
            BindingOperations.SetBinding(element, GetDependencyProperty(),
                new Binding($"{propertyItem.PropertyName}")
                {
                    Source = propertyItem.PropertySource,
                    Mode = GetBindingMode(propertyItem),
                    UpdateSourceTrigger = GetUpdateSourceTrigger(propertyItem),
                    Converter = GetConverter(propertyItem)
                });
        }

        public virtual BindingMode GetBindingMode(PropertyItem propertyItem) => propertyItem.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay;

        public virtual UpdateSourceTrigger GetUpdateSourceTrigger(PropertyItem propertyItem) => UpdateSourceTrigger.PropertyChanged;

        protected virtual IValueConverter GetConverter(PropertyItem propertyItem) => null;

        public abstract FrameworkElement GetElement(PropertyItem propertyItem);

        public abstract DependencyProperty GetDependencyProperty();
    }
}
