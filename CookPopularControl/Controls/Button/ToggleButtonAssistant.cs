using CookPopularCSharpToolkit.Communal;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ToggleButtonAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-28 16:35:56
 */
namespace CookPopularControl.Controls
{
    public class ToggleButtonAssistant
    {
        public static Geometry GetUnCheckedGeometry(DependencyObject obj) => (Geometry)obj.GetValue(UnCheckedGeometryProperty);
        public static void SetUnCheckedGeometry(DependencyObject obj, Geometry value) => obj.SetValue(UnCheckedGeometryProperty, value);
        /// <summary>
        /// <see cref="OnCheckedElementProperty"/>表示未选中时的图标
        /// </summary>
        public static readonly DependencyProperty UnCheckedGeometryProperty =
            DependencyProperty.RegisterAttached("UnCheckedGeometry", typeof(Geometry), typeof(ToggleButtonAssistant), new PropertyMetadata());


        public static Geometry GetOnCheckedGeometry(DependencyObject obj) => (Geometry)obj.GetValue(OnCheckedGeometryProperty);
        public static void SetOnCheckedGeometry(DependencyObject obj, Geometry value) => obj.SetValue(OnCheckedGeometryProperty, value);
        /// <summary>
        /// <see cref="OnCheckedGeometryProperty"/>表示选中时的图标
        /// </summary>
        public static readonly DependencyProperty OnCheckedGeometryProperty =
            DependencyProperty.RegisterAttached("OnCheckedGeometry", typeof(Geometry), typeof(ToggleButtonAssistant), new PropertyMetadata());


        public static object GetOnCheckedElement(DependencyObject obj) => obj.GetValue(OnCheckedElementProperty);
        public static void SetOnCheckedElement(DependencyObject obj, object value) => obj.SetValue(OnCheckedElementProperty, value);
        /// <summary>
        /// <see cref="OnCheckedElementProperty"/>表示翻转的Content
        /// </summary>
        public static readonly DependencyProperty OnCheckedElementProperty =
            DependencyProperty.RegisterAttached("OnCheckedElement", typeof(object), typeof(ToggleButtonAssistant), new PropertyMetadata());


        public static bool GetIsAddCheckedAnimation(DependencyObject obj) => (bool)obj.GetValue(IsAddCheckedAnimationProperty);
        public static void SetIsAddCheckedAnimation(DependencyObject obj, bool value) => obj.SetValue(IsAddCheckedAnimationProperty, value);
        /// <summary>
        /// <see cref="OnCheckedElementProperty"/>表示点击时是否增加动画
        /// </summary>
        public static readonly DependencyProperty IsAddCheckedAnimationProperty =
            DependencyProperty.RegisterAttached("IsAddCheckedAnimation", typeof(bool), typeof(ToggleButtonAssistant), new PropertyMetadata(ValueBoxes.FalseBox));
    }
}
