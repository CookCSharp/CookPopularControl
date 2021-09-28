using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：LabelAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-11 09:00:57
 */
namespace CookPopularControl.Controls.Labels
{
    /// <summary>
    /// 提供<see cref="Label"/>的附加属性类
    /// </summary>
    public class LabelAssistant
    {
        public static object GetHeader(DependencyObject obj) => (object)obj.GetValue(HeaderProperty);
        public static void SetHeader(DependencyObject obj, object value) => obj.SetValue(HeaderProperty, value);
        /// <summary>
        /// <see cref="HeaderProperty"/>标识标签头的附加属性
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.RegisterAttached("Header", typeof(object), typeof(LabelAssistant), new PropertyMetadata(default(object)));


        public static DataTemplate GetHeaderTemplate(DependencyObject obj) => (DataTemplate)obj.GetValue(HeaderTemplateProperty);
        public static void SetHeaderTemplate(DependencyObject obj, DataTemplate value) => obj.SetValue(HeaderTemplateProperty, value);
        /// <summary>
        /// <see cref="HeaderTemplateProperty"/>标识标签标头的数据模板
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.RegisterAttached("HeaderTemplate", typeof(DataTemplate), typeof(LabelAssistant), new PropertyMetadata(default(DataTemplate)));


        public static DataTemplateSelector GetHeaderTemplateSelector(DependencyObject obj) => (DataTemplateSelector)obj.GetValue(HeaderTemplateSelectorProperty);
        public static void SetHeaderTemplateSelector(DependencyObject obj, DataTemplateSelector value) => obj.SetValue(HeaderTemplateSelectorProperty, value);
        /// <summary>
        /// <see cref="HeaderTemplateSelectorProperty"/>标识标签标头的数据模板选择器
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateSelectorProperty =
            DependencyProperty.RegisterAttached("HeaderTemplateSelector", typeof(DataTemplateSelector), typeof(LabelAssistant), new PropertyMetadata(default(DataTemplateSelector)));


        public static string GetHeaderStringFormat(DependencyObject obj) => (string)obj.GetValue(HeaderStringFormatProperty);
        public static void SetHeaderStringFormat(DependencyObject obj, string value) => obj.SetValue(HeaderStringFormatProperty, value);
        /// <summary>
        /// <see cref="HeaderStringFormatProperty"/>标识标签标头的数据模板选择器
        /// </summary>
        public static readonly DependencyProperty HeaderStringFormatProperty =
            DependencyProperty.RegisterAttached("HeaderStringFormat", typeof(string), typeof(LabelAssistant), new PropertyMetadata(default(string)));

        public static CornerRadius GetCornerRadius(DependencyObject obj) => (CornerRadius)obj.GetValue(CornerRadiusProperty);
        public static void SetCornerRadius(DependencyObject obj, CornerRadius value) => obj.SetValue(CornerRadiusProperty, value);
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(LabelAssistant), new PropertyMetadata(ValueBoxes.CornerRadius0Box));
    }
}
