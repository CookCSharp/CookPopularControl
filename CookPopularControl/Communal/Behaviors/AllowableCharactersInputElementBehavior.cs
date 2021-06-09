using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：AllowableCharactersInputElementBehavior
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-09 19:03:03
 */
namespace CookPopularControl.Communal.Behaviors
{
    /// <summary>
    /// 使用正则表达式对输入控件的字符串进行的过滤行为
    /// </summary>
    public class AllowableCharactersInputElementBehavior<TInputElement> where TInputElement : FrameworkElement
    {
        public static string GetRegularExpressionProperty(DependencyObject obj) => (string)obj.GetValue(RegularExpressionPropertyProperty);
        public static void SetRegularExpressionProperty(DependencyObject obj, string value) => obj.SetValue(RegularExpressionPropertyProperty, value);
        /// <summary>
        /// 正则表达式
        /// </summary>
        /// <remarks>用来对字符串进行过滤</remarks>
        public static readonly DependencyProperty RegularExpressionPropertyProperty =
            DependencyProperty.RegisterAttached("RegularExpressionProperty", typeof(string), typeof(AllowableCharactersInputElementBehavior<TInputElement>), new PropertyMetadata(".*", OnRegularExpressionPropertyChanged));


        private static void OnRegularExpressionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = d as TInputElement;  //如果存在以避免内存泄漏，则删除处理程序                
            uiElement.PreviewTextInput -= UIElement_PreviewTextInput;

            var value = e.NewValue as string;
            if (value != null)
            {
                uiElement.PreviewTextInput += UIElement_PreviewTextInput;

                DataObject.AddPastingHandler(uiElement, OnPaste);
            }
        }

        private static void UIElement_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var RegularExpression = AllowableCharactersInputElementBehavior<TInputElement>.GetRegularExpressionProperty((sender as TInputElement)!);

            e.Handled = !IsValid(e.Text, false, RegularExpression);
        }

        /// <summary>
        /// 检查粘贴的字符串是否符合正则表达式，不符合则取消输入
        /// </summary>
        private static void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                var RegularExpression = AllowableCharactersInputElementBehavior<TInputElement>.GetRegularExpressionProperty((sender as TInputElement)!);

                string text = Convert.ToString(e.DataObject.GetData(DataFormats.Text));

                if (!IsValid(text, true, RegularExpression))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        /// <summary>
        /// 确定字符串是否符合正则表达式。
        /// </summary>
        private static bool IsValid(string newText, bool paste, string regularExpression)
        {
            return Regex.IsMatch(newText, regularExpression);
        }
    }
}
