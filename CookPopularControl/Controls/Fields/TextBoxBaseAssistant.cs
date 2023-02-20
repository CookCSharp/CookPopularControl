/*
 * Description：TextBoxBaseAssistant 
 * Author： Chance.Zheng
 * Create Time: 2023-02-16 14:09:44
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Forms.ComponentModel.Com2Interop;
using CookPopularCSharpToolkit.Communal;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
//using System.Runtime.Remoting.Contexts;
using System.Windows.Media;
using System.Windows.Markup;
using System.Windows.Data;
using CookPopularCSharpToolkit.Windows;
using System.Globalization;

namespace CookPopularControl.Controls
{
    [MarkupExtensionReturnType(typeof(Thickness))]
    [ValueConversion(typeof(Thickness), typeof(Thickness))]
    public class TextBlockEllipsisPaddingConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness thickness && int.TryParse(parameter?.ToString(), out int param))
            {
                return new Thickness(thickness.Left + param, thickness.Top + 1, thickness.Right, thickness.Bottom);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 用于指定省略号所在位置的枚举，你可以换成中文，不影响使用。
    /// </summary>
    public enum EllipsisPlacement
    {
        Left,
        Center,
        Right
    }


    /// <summary>
    /// 表示<see cref="TextBox"/>的附加属性类
    /// </summary>
    [TemplatePart(Name = ElementTextblock, Type = typeof(TextBlock))]
    public class TextBoxBaseAssistant
    {
        private const string ElementTextblock = "PART_Ellipsis";

        public static EllipsisPlacement GetEllipsisPlacement(DependencyObject obj) => (EllipsisPlacement)obj.GetValue(EllipsisPlacementProperty);
        public static void SetEllipsisPlacement(DependencyObject obj, EllipsisPlacement value) => obj.SetValue(EllipsisPlacementProperty, value);
        /// <summary>
        /// 省略号位置，左中右
        /// </summary>
        public static readonly DependencyProperty EllipsisPlacementProperty =
            DependencyProperty.RegisterAttached("EllipsisPlacement", typeof(EllipsisPlacement), typeof(TextBoxBaseAssistant), new PropertyMetadata(EllipsisPlacement.Right, OnValueChanged));


        public static bool GetIsEllipsisEnabled(DependencyObject obj) => (bool)obj.GetValue(IsEllipsisEnabledProperty);
        public static void SetIsEllipsisEnabled(DependencyObject obj, bool value) => obj.SetValue(IsEllipsisEnabledProperty, value);
        /// <summary>
        /// 是否启用省略号，如果启用了，就会截断文字，显示省略号。当文本获取焦点时，暂时失效
        /// </summary>
        public static readonly DependencyProperty IsEllipsisEnabledProperty =
            DependencyProperty.RegisterAttached("IsEllipsisEnabled", typeof(bool), typeof(TextBoxBaseAssistant), new PropertyMetadata(ValueBoxes.FalseBox, OnValueChanged));


        public static bool GetIsShowToolTip(DependencyObject obj) => (bool)obj.GetValue(IsShowToolTipProperty);
        public static void SetIsShowToolTip(DependencyObject obj, bool value) => obj.SetValue(IsShowToolTipProperty, value);
        /// <summary>
        /// 是否启用提示.如果是true,那么当文本被截断后，在控件的ToolTip属性内添加整个文本的内容;如果是false，那么控件的提示保持不变.
        /// </summary>
        public static readonly DependencyProperty IsShowToolTipProperty =
            DependencyProperty.RegisterAttached("IsShowToolTip", typeof(bool), typeof(TextBoxBaseAssistant), new PropertyMetadata(ValueBoxes.FalseBox, OnValueChanged));


        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs arg)
        {
            if (d is TextBox tb)
            {
                TextBlock? textBlock = null;
                if (tb.IsLoaded)
                    TextBoxLoaded();
                else
                    tb.Loaded += (s, e) => TextBoxLoaded();

                void TextBoxLoaded()
                {
                    textBlock = tb.Template.FindName(ElementTextblock, tb) as TextBlock;
                    if (arg.Property == EllipsisPlacementProperty)
                    {
                        if (!tb.IsFocused)
                        {
                            TextBoxLostFocus();
                        }
                        tb.LostFocus += (s, e) => TextBoxLostFocus();
                        tb.SizeChanged += (s, e) => TextBoxLostFocus();
                    }
                }

                void TextBoxLostFocus()
                {
                    if (textBlock != null)
                    {
                        if ((EllipsisPlacement)arg.NewValue == EllipsisPlacement.Right)
                        {
                            textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
                            textBlock.Text = tb.Text;
                        }
                        else
                        {
                            textBlock.TextTrimming = TextTrimming.None;
                            textBlock.ToolTip = tb.Text;
                            SetEllipsisForTextBlock(tb, textBlock, (EllipsisPlacement)arg.NewValue);
                        }
                    }
                }
            }
        }

        private static void SetEllipsisForTextBlock(TextBox tb, TextBlock textBlock, EllipsisPlacement placement)
        {
            Typeface typeface = new Typeface(tb.FontFamily, tb.FontStyle, tb.FontWeight, tb.FontStretch);
            FormattedText formattedText = new FormattedText(tb.Text,
                                                            System.Threading.Thread.CurrentThread.CurrentCulture,
                                                            tb.FlowDirection,
                                                            typeface,
                                                            tb.FontSize,
                                                            tb.Foreground);
            if (formattedText.Width + tb.Padding.Left > tb.ActualWidth - 1)
            {
                int p1, p2;
                string str1 = tb.Text;
                p1 = FindPosition(str1, typeface, tb, placement);
                char[] ret = str1.ToCharArray();
                string str2 = string.Concat<char>(ret.Reverse<char>());
                p2 = FindPosition(str2, typeface, tb, placement);
                string strOutput = string.Empty;
                if (placement == EllipsisPlacement.Center)
                    strOutput = tb.Text.Substring(0, p1) + "\u2026" + tb.Text.Substring(tb.Text.Length - p2, p2);
                else
                    strOutput = "\u2026" + tb.Text.Substring(tb.Text.Length - p2, p2);
                textBlock.Text = strOutput;
            }
            else
            {
                textBlock.Text = tb.Text;
            }
        }

        //利用二分法找到要截取的字符串
        private static int FindPosition(string str, Typeface typeface, TextBox tb, EllipsisPlacement placement)
        {
            int start = 0, end = str.Length - 1;
            while (start <= end)
            {
                int mid = (start + end) / 2;
                string strTemp = str.Substring(0, mid);
                FormattedText formattedText = new FormattedText(strTemp,
                                                                System.Threading.Thread.CurrentThread.CurrentCulture,
                                                                tb.FlowDirection,
                                                                typeface,
                                                                tb.FontSize,
                                                                tb.Foreground);

                FormattedText ellipsisFormattedText = new FormattedText("\u2026",
                                                                        System.Threading.Thread.CurrentThread.CurrentCulture,
                                                                        tb.FlowDirection,
                                                                        typeface,
                                                                        tb.FontSize,
                                                                        tb.Foreground);
                bool isValue = false;
                if (placement == EllipsisPlacement.Center)
                    isValue = formattedText.Width <= (tb.ActualWidth - tb.Padding.Left - 10) / 2;
                else
                    isValue = formattedText.Width <= tb.ActualWidth - tb.Padding.Left - ellipsisFormattedText.Width - 5;

                if (isValue)
                    start = mid + 1;
                else
                    end = mid - 1;
            }
            if (start == 0)
                return -1;
            else
                return start - 1;
        }
    }
}
