using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;



/*
 * Description：PropertyItem 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-01-07 14:49:32
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */
namespace CookPopularControl.Controls
{
    /***
     * 可参考System.ComponentModel的一些特性
     * */

    /// <summary>
    /// 表示<see cref="PropertyGrid"/>中一个子项
    /// </summary>
    public class PropertyItem : ListBoxItem
    {
        /// <summary>
        /// 属性绑定源
        /// </summary>
        internal object PropertySource { get; set; }

        /// <summary>
        /// 指定某一属性或事件是否应在“属性窗口中显示”
        /// </summary>
        public bool IsBrowsable
        {
            get => (bool)GetValue(IsBrowsableProperty);
            set => SetValue(IsBrowsableProperty, ValueBoxes.BooleanBox(value));
        }
        /// <summary>
        /// 提供<see cref="IsBrowsable"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsBrowsableProperty =
            DependencyProperty.Register("IsBrowsable", typeof(bool), typeof(PropertyItem), new PropertyMetadata(ValueBoxes.TrueBox));


        /// <summary>
        /// 指定该属性是只读属性还是读/写属性
        /// </summary>
        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, ValueBoxes.BooleanBox(value));
        }
        /// <summary>
        /// 提供<see cref="IsReadOnly"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(PropertyItem), new PropertyMetadata(ValueBoxes.FalseBox));


        /// <summary>
        /// 指定当属性或事件显示再一个设置为“按分类顺序”模式的<see cref="PropertyGrid"/>控件中时，
        /// 用于对属性或事件分组的类别名称
        /// </summary>
        public string Category
        {
            get => (string)GetValue(CategoryProperty);
            set => SetValue(CategoryProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Category"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("Category", typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));


        /// <summary>
        /// 指定属性或事件的描述
        /// </summary>
        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Description"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));


        /// <summary>
        /// 指定属性、事件或不采用任何参数的公共void方法的显示名称
        /// </summary>
        public string DisplayName
        {
            get => (string)GetValue(DisplayNameProperty);
            set => SetValue(DisplayNameProperty, value);
        }
        /// <summary>
        /// 提供<see cref="DisplayName"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty =
            DependencyProperty.Register("DisplayName", typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));


        /// <summary>
        /// 指定属性类型
        /// </summary>
        public Type PropertyType
        {
            get => (Type)GetValue(PropertyTypeProperty);
            set => SetValue(PropertyTypeProperty, value);
        }
        /// <summary>
        /// 提供<see cref="PropertyType"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty PropertyTypeProperty =
            DependencyProperty.Register("PropertyType", typeof(Type), typeof(PropertyItem), new PropertyMetadata(default(Type)));


        /// <summary>
        /// 指定属性名
        /// </summary>
        public string PropertyName
        {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }
        /// <summary>
        /// 提供<see cref="PropertyName"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(string), typeof(PropertyItem), new PropertyMetadata(default(string)));


        /// <summary>
        /// 属性值
        /// </summary>
        public object Value
        {
            get => (object)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Value"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(PropertyItem), new PropertyMetadata(default(object)));


        /// <summary>
        /// 属性默认值
        /// </summary>
        public object DefaultValue
        {
            get => (object)GetValue(DefaultValueProperty);
            set => SetValue(DefaultValueProperty, value);
        }
        /// <summary>
        /// 提供<see cref="DefaultValue"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DefaultValueProperty =
            DependencyProperty.Register("DefaultValue", typeof(object), typeof(PropertyItem), new PropertyMetadata(default(object)));


        /// <summary>
        /// 承接属性的元素
        /// </summary>
        public FrameworkElement Element
        {
            get => (FrameworkElement)GetValue(ElementProperty);
            set => SetValue(ElementProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Element"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register("Element", typeof(FrameworkElement), typeof(PropertyItem), new PropertyMetadata(default(FrameworkElement)));


        /// <summary>
        /// 属性编辑器工厂
        /// </summary>
        public PropertyItemEditorFactory EditorFactory
        {
            get => (PropertyItemEditorFactory)GetValue(EditorFactoryProperty);
            set => SetValue(EditorFactoryProperty, value);
        }
        /// <summary>
        /// 提供<see cref="EditorFactory"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty EditorFactoryProperty =
            DependencyProperty.Register("EditorFactory", typeof(PropertyItemEditorFactory), typeof(PropertyItem), new PropertyMetadata(default(PropertyItemEditorFactory)));


        public static bool GetIsItemSelected(DependencyObject obj) => (bool)obj.GetValue(IsItemSelectedProperty);
        public static void SetIsItemSelected(DependencyObject obj, bool value) => obj.SetValue(IsItemSelectedProperty, value);
        public static readonly DependencyProperty IsItemSelectedProperty =
            DependencyProperty.RegisterAttached("IsItemSelected", typeof(bool), typeof(PropertyItem), new PropertyMetadata(ValueBoxes.FalseBox, OnIsItemSelectedChanged));

        private static void OnIsItemSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is PropertyItem item && (bool)e.NewValue)
            { 
                var propertyGrid = item.GetVisualAncestors().OfType<PropertyGrid>().FirstOrDefault();
                propertyGrid.PropertyTitle = item.DisplayName;
                propertyGrid.PropertyDescription = item.Description;
            }
        }
    }
}
