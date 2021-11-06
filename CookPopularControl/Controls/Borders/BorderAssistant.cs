using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：BorderAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-11 14:51:09
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 提供<see cref="Border"/>附加属性类
    /// </summary>
    public class BorderAssistant
    {
        public static bool GetIsCircular(DependencyObject obj) => (bool)obj.GetValue(IsCircularProperty);
        public static void SetIsCircular(DependencyObject obj, bool value) => obj.SetValue(IsCircularProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsCircularProperty"/>标识是否满圆角
        /// </summary>
        public static readonly DependencyProperty IsCircularProperty =
            DependencyProperty.RegisterAttached("IsCircular", typeof(bool), typeof(BorderAssistant), new PropertyMetadata(ValueBoxes.FalseBox, OnIsCircularChanged));

        private static void OnIsCircularChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Border border)
            {
                if ((bool)e.NewValue)
                {
                    var binding = new MultiBinding { Converter = new BorderCircularToCornerRadiusConverter() };
                    binding.Bindings.Add(new Binding(FrameworkElement.ActualWidthProperty.Name) { Source = border });
                    binding.Bindings.Add(new Binding(FrameworkElement.ActualHeightProperty.Name) { Source = border });
                    border.SetBinding(Border.CornerRadiusProperty, binding);
                }
                else
                {
                    BindingOperations.ClearBinding(border, FrameworkElement.ActualWidthProperty);
                    BindingOperations.ClearBinding(border, FrameworkElement.ActualHeightProperty);
                    BindingOperations.ClearBinding(border, Border.CornerRadiusProperty);
                }
            }
        }
    }
}
