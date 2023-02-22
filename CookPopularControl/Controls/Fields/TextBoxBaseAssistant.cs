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
using System.Windows.Controls.Primitives;
using CookPopularControl.Communal;
using System.Windows.Interop;
using System.Windows.Automation.Provider;
using System.Windows.Media.TextFormatting;

namespace CookPopularControl.Controls
{
    [MarkupExtensionReturnType(typeof(string))]
    [ValueConversion(typeof(string), typeof(string))]
    internal class TextTrimmingConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text && int.TryParse(parameter?.ToString(), out int param))
            {
                return SetEllipsisForText(default(TextBoxBase), text, EllipsisPlacement.Center);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static string SetEllipsisForText(TextBoxBase textBox, string text, EllipsisPlacement placement)
        {
            string ellipsisText = string.Empty;
            Typeface typeface = new Typeface(textBox.FontFamily, textBox.FontStyle, textBox.FontWeight, textBox.FontStretch);
#pragma warning disable CS0618 // 类型或成员已过时
            FormattedText formattedText = new FormattedText(text,
                                                            System.Threading.Thread.CurrentThread.CurrentCulture,
                                                            textBox.FlowDirection,
                                                            typeface,
                                                            textBox.FontSize,
                                                            textBox.Foreground,
                                                            null,
                                                            TextFormattingMode.Ideal);
#pragma warning restore CS0618 // 类型或成员已过时
#pragma warning disable CS0618 // 类型或成员已过时
            FormattedText ellipsisFormattedText = new FormattedText("\u2026",
                                                                    System.Threading.Thread.CurrentThread.CurrentCulture,
                                                                    textBox.FlowDirection,
                                                                    typeface,
                                                                    textBox.FontSize,
                                                                    textBox.Foreground,
                                                                    null,
                                                                    TextFormattingMode.Ideal);
#pragma warning restore CS0618 // 类型或成员已过时

            double clearButtonWidth = 0;
            if (TextElementAttached.GetIsAddClearButton(textBox))
                clearButtonWidth = FrameworkElementBaseAttached.GetIconWidth(textBox) + FrameworkElementBaseAttached.GetIconMargin(textBox).Right;

            if (formattedText.Width + textBox.Padding.Left + clearButtonWidth + ellipsisFormattedText.Width > textBox.ActualWidth - 1)
            {
                int p1, p2;
                string str1 = text;
                p1 = FindPosition(str1, typeface, textBox, placement);
                char[] ret = str1.ToCharArray();
                string str2 = string.Concat<char>(ret.Reverse<char>());
                p2 = FindPosition(str2, typeface, textBox, placement);
                string strOutput = string.Empty;
                switch (placement)
                {
                    case EllipsisPlacement.Left:
                        strOutput = "\u2026" + text.Substring(text.Length - p2, p2);
                        break;
                    case EllipsisPlacement.Center:
                        strOutput = text.Substring(0, p1) + "\u2026" + text.Substring(text.Length - p2, p2);
                        break;
                    case EllipsisPlacement.Right:
                        strOutput = text.Substring(0, p1) + "\u2026";
                        break;
                    default:
                        break;
                }
                ellipsisText = strOutput;
            }
            else
            {
                ellipsisText = text;
            }

            return ellipsisText;
        }


        //利用二分法找到要截取的字符串
        private static int FindPosition(string str, Typeface typeface, TextBoxBase tb, EllipsisPlacement placement)
        {
            int start = 0, end = str.Length - 1;
            while (start <= end)
            {
                int mid = (start + end) / 2;
                string strTemp = str.Substring(0, mid);
#pragma warning disable CS0618 // 类型或成员已过时
                FormattedText formattedText = new FormattedText(strTemp,
                                                                System.Threading.Thread.CurrentThread.CurrentCulture,
                                                                tb.FlowDirection,
                                                                typeface,
                                                                tb.FontSize,
                                                                tb.Foreground,
                                                                null,
                                                                TextFormattingMode.Ideal);
#pragma warning restore CS0618 // 类型或成员已过时
#pragma warning disable CS0618 // 类型或成员已过时
                FormattedText ellipsisFormattedText = new FormattedText("\u2026",
                                                                        System.Threading.Thread.CurrentThread.CurrentCulture,
                                                                        tb.FlowDirection,
                                                                        typeface,
                                                                        tb.FontSize,
                                                                        tb.Foreground,
                                                                        null,
                                                                        TextFormattingMode.Ideal);
#pragma warning restore CS0618 // 类型或成员已过时
                double clearButtonWidth = 0;
                bool isValue = false;

                if (TextElementAttached.GetIsAddClearButton(tb))
                    clearButtonWidth = FrameworkElementBaseAttached.GetIconWidth(tb) + FrameworkElementBaseAttached.GetIconMargin(tb).Right;

                switch (placement)
                {
                    case EllipsisPlacement.Left:
                        isValue = formattedText.Width <= tb.ActualWidth - tb.Padding.Left - clearButtonWidth - ellipsisFormattedText.Width - 4;
                        break;
                    case EllipsisPlacement.Center:
                        isValue = formattedText.Width <= (tb.ActualWidth - tb.Padding.Left - clearButtonWidth - ellipsisFormattedText.Width) / 2 - 4;
                        break;
                    case EllipsisPlacement.Right:
                        isValue = formattedText.Width <= tb.ActualWidth - tb.Padding.Left - clearButtonWidth - ellipsisFormattedText.Width - 4;
                        break;
                    default:
                        break;
                }

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


    /// <summary>
    /// 用于指定省略号所在位置的枚举
    /// </summary>
    public enum EllipsisPlacement
    {
        Left,
        Center,
        Right
    }


    /// <summary>
    /// 表示<see cref="TextBoxBase"/>的附加属性类
    /// </summary>
    [TemplatePart(Name = ElementTextBlock, Type = typeof(TextBlock))]
    [TemplatePart(Name = ElementScrollViewer, Type = typeof(ScrollViewer))]
    public class TextBoxBaseAssistant
    {
        private const string ElementTextBlock = "PART_Trimming";
        private const string ElementScrollViewer = "PART_ContentHost";
        private const int ReserveWidth = 0;
        private static readonly CultureInfo CurrentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;


        internal static bool GetIsFirstChecked(DependencyObject obj) => (bool)obj.GetValue(IsFirstCheckedProperty);
        internal static void SetIsFirstChecked(DependencyObject obj, bool value) => obj.SetValue(IsFirstCheckedProperty, ValueBoxes.BooleanBox(value));
        internal static readonly DependencyProperty IsFirstCheckedProperty =
            DependencyProperty.RegisterAttached("IsFirstChecked", typeof(bool), typeof(TextBoxBaseAssistant), new PropertyMetadata(ValueBoxes.FalseBox, OnValueChanged));


        internal static bool GetIsGotFocus(DependencyObject obj) => (bool)obj.GetValue(IsGotFocusProperty);
        internal static void SetIsGotFocus(DependencyObject obj, bool value) => obj.SetValue(IsGotFocusProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// 是否启用省略号，如果启用了，就会截断文字，显示省略号。当文本获取焦点时，暂时失效
        /// </summary>
        internal static readonly DependencyProperty IsGotFocusProperty =
            DependencyProperty.RegisterAttached("IsGotFocus", typeof(bool), typeof(TextBoxBaseAssistant), new PropertyMetadata(ValueBoxes.FalseBox, OnValueChanged));


        public static bool GetIsTrimming(DependencyObject obj) => (bool)obj.GetValue(IsTrimmingProperty);
        public static void SetIsTrimming(DependencyObject obj, bool value) => obj.SetValue(IsTrimmingProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsTrimmingProperty"/>表示是否裁剪文本
        /// </summary>
        public static readonly DependencyProperty IsTrimmingProperty =
            DependencyProperty.RegisterAttached("IsTrimming", typeof(bool), typeof(TextBoxBaseAssistant), new PropertyMetadata(ValueBoxes.FalseBox, OnValueChanged));


        public static EllipsisPlacement GetEllipsisPlacement(DependencyObject obj) => (EllipsisPlacement)obj.GetValue(EllipsisPlacementProperty);
        public static void SetEllipsisPlacement(DependencyObject obj, EllipsisPlacement value) => obj.SetValue(EllipsisPlacementProperty, value);
        /// <summary>
        /// <see cref="EllipsisPlacementProperty"/>表示省略号位置，左中右
        /// </summary>
        public static readonly DependencyProperty EllipsisPlacementProperty =
            DependencyProperty.RegisterAttached("EllipsisPlacement", typeof(EllipsisPlacement), typeof(TextBoxBaseAssistant), new PropertyMetadata(EllipsisPlacement.Left, OnValueChanged));


        public static bool GetIsShowToolTip(DependencyObject obj) => (bool)obj.GetValue(IsShowToolTipProperty);
        public static void SetIsShowToolTip(DependencyObject obj, bool value) => obj.SetValue(IsShowToolTipProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// 是否启用提示.如果是true,那么当文本被截断后，在控件的ToolTip属性内添加整个文本的内容;如果是false，那么控件的提示保持不变.
        /// </summary>
        public static readonly DependencyProperty IsShowToolTipProperty =
            DependencyProperty.RegisterAttached("IsShowToolTip", typeof(bool), typeof(TextBoxBaseAssistant), new PropertyMetadata(ValueBoxes.FalseBox, OnValueChanged));


        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBoxBase textBoxBase)
            {
                if (textBoxBase.IsLoaded)
                {
                    OnTextBoxLoaded();
                }
                else
                {
                    textBoxBase.Loaded += (s, e) => OnTextBoxLoaded();
                }

                void OnTextBoxLoaded()
                {
                    var textBlock = textBoxBase.Template.FindName(ElementTextBlock, textBoxBase) as TextBlock;

                    if (!textBoxBase.IsFocused)
                    {
                        if (GetIsTrimming(textBoxBase) && textBlock != null)
                        {
                            if (TextElementAttached.GetIsTextClear(textBoxBase))
                                textBlock.Text = string.Empty;
                            else
                                textBlock.Text = SetEllipsisForText(textBoxBase, (textBoxBase as TextBox)?.Text, GetEllipsisPlacement(textBoxBase));
                        }
                    }
                    else
                    {
                        //这里由于textBoxView的默认Margin="2,0"
                        //故要么在此处置为default,textBoxView.Margin = default(Thickness);
                        //要么将textBlock的margin设置为Margin="2,0"
                        var scrollViewer = textBoxBase.Template.FindName(ElementScrollViewer, textBoxBase) as ScrollViewer;
                        if (scrollViewer != null)
                        {
                            var textBoxView = scrollViewer.Content as FrameworkElement;
                        }
                    }

                    CheckShowToolTip();

                    textBoxBase.TextChanged += (s, e) => CheckShowToolTip();
                }

                void CheckShowToolTip()
                {
                    if (GetIsShowToolTip(textBoxBase) && CheckTextIsLongger(textBoxBase, (textBoxBase as TextBox)?.Text))
                    {
                        textBoxBase.ToolTip = (textBoxBase as TextBox)?.Text;
                    }
                    else
                    {
                        textBoxBase.ToolTip = null;
                    }
                }
            }
        }

        private static bool CheckTextIsLongger(TextBoxBase control, string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            Typeface typeface = new Typeface(control.FontFamily, control.FontStyle, control.FontWeight, control.FontStretch);
#pragma warning disable CS0618 // 类型或成员已过时
            FormattedText formattedText = new FormattedText(text, CurrentCulture, control.FlowDirection, typeface, control.FontSize, control.Foreground);
#pragma warning restore CS0618 // 类型或成员已过时

            double clearButtonWidth = 0;
            if (TextElementAttached.GetIsAddClearButton(control))
                clearButtonWidth = FrameworkElementBaseAttached.GetIconWidth(control) + FrameworkElementBaseAttached.GetIconMargin(control).Right;

            if (formattedText.Width + control.Padding.Left + clearButtonWidth >= control.ActualWidth)
                return true;
            else
                return false;
        }

        private static string SetEllipsisForText(TextBoxBase control, string text, EllipsisPlacement placement)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            string ellipsisText = string.Empty;
            Typeface typeface = new Typeface(control.FontFamily, control.FontStyle, control.FontWeight, control.FontStretch);
#pragma warning disable CS0618 // 类型或成员已过时
            FormattedText formattedText = new FormattedText(text, CurrentCulture, control.FlowDirection, typeface, control.FontSize, control.Foreground);
#pragma warning restore CS0618 // 类型或成员已过时
#pragma warning disable CS0618 // 类型或成员已过时
            FormattedText ellipsisFormattedText = new FormattedText("\u2026", CurrentCulture, control.FlowDirection, typeface, control.FontSize, control.Foreground);
#pragma warning restore CS0618 // 类型或成员已过时

            double clearButtonWidth = 0;
            if (TextElementAttached.GetIsAddClearButton(control))
                clearButtonWidth = FrameworkElementBaseAttached.GetIconWidth(control) + FrameworkElementBaseAttached.GetIconMargin(control).Right;

            if (formattedText.Width + control.Padding.Left + clearButtonWidth + ellipsisFormattedText.Width > control.ActualWidth)
            {
                int p1, p2;
                string str1 = text;
                p1 = FindPosition(control, str1, typeface, clearButtonWidth, placement);
                char[] ret = str1.ToCharArray();
                string str2 = string.Concat<char>(ret.Reverse<char>());
                p2 = FindPosition(control, str2, typeface, clearButtonWidth, placement);
                string strOutput = string.Empty;
                switch (placement)
                {
                    case EllipsisPlacement.Left:
                        strOutput = "\u2026" + text.Substring(text.Length - p2, p2);
                        break;
                    case EllipsisPlacement.Center:
                        strOutput = text.Substring(0, p1) + "\u2026" + text.Substring(text.Length - p2, p2);
                        break;
                    case EllipsisPlacement.Right:
                        strOutput = text.Substring(0, p1) + "\u2026";
                        break;
                    default:
                        break;
                }
                ellipsisText = strOutput;
            }
            else
            {
                ellipsisText = text;
            }

            return ellipsisText;
        }

        //利用二分法找到要截取的字符串
        private static int FindPosition(TextBoxBase control, string str, Typeface typeface, double clearButtonWidth, EllipsisPlacement placement)
        {
            int start = 0, end = str.Length - 1;
            while (start <= end)
            {
                int mid = (start + end) / 2;
                string strTemp = str.Substring(0, mid);
#pragma warning disable CS0618 // 类型或成员已过时
                FormattedText formattedText = new FormattedText(strTemp, CurrentCulture, control.FlowDirection, typeface, control.FontSize, control.Foreground);
#pragma warning restore CS0618 // 类型或成员已过时
#pragma warning disable CS0618 // 类型或成员已过时
                FormattedText ellipsisFormattedText = new FormattedText("\u2026", CurrentCulture, control.FlowDirection, typeface, control.FontSize, control.Foreground);
#pragma warning restore CS0618 // 类型或成员已过时

                bool isValue = false;
                switch (placement)
                {
                    case EllipsisPlacement.Left:
                        isValue = formattedText.Width <= control.ActualWidth - control.Padding.Left - clearButtonWidth - ellipsisFormattedText.Width - ReserveWidth;
                        break;
                    case EllipsisPlacement.Center:
                        isValue = formattedText.Width <= (control.ActualWidth - control.Padding.Left - clearButtonWidth - ellipsisFormattedText.Width - ReserveWidth) / 2;
                        break;
                    case EllipsisPlacement.Right:
                        isValue = formattedText.Width <= control.ActualWidth - control.Padding.Left - clearButtonWidth - ellipsisFormattedText.Width - ReserveWidth;
                        break;
                    default:
                        break;
                }

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
