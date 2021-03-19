using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：TextElementAttached
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-18 16:35:11
 */
namespace CookPopularControl.Communal.Attached  
{
    /// <summary>
    /// <see cref="TextElementAttached"/>表示可输入文本控件的附加属性基类
    /// </summary>
    /// <remarks>包括TextBox、TextBlock、RichTextBox、PasswordBox、ComboBox等</remarks>
    public class TextElementAttached
    {
        public static string GetPlaceHolder(DependencyObject obj) => (string)obj.GetValue(PlaceHolderProperty);
        public static void SetPlaceHolder(DependencyObject obj, string value) => obj.SetValue(PlaceHolderProperty, value);
        public static readonly DependencyProperty PlaceHolderProperty =
            DependencyProperty.RegisterAttached("PlaceHolder", typeof(string), typeof(TextElementAttached), new PropertyMetadata(default(string)));

        public static Brush GetPlaceHolderBrush(DependencyObject obj) => (Brush)obj.GetValue(PlaceHolderBrushProperty);
        public static void SetPlaceHolderBrush(DependencyObject obj, Brush value) => obj.SetValue(PlaceHolderBrushProperty, value);
        public static readonly DependencyProperty PlaceHolderBrushProperty =
            DependencyProperty.RegisterAttached("PlaceHolderBrush", typeof(Brush), typeof(TextElementAttached), new PropertyMetadata(default(Brush)));
    }
}
