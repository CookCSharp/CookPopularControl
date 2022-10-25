using CookPopularCSharpToolkit.Communal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


/*
 * Description：AutoGroupingText 
 * Author： Chance.Zheng
 * Create Time: 2022-06-20 20:24:53
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2020-2022 All Rights Reserved.
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 文本自动分组控件
    /// </summary>
    /// <remarks>
    /// 将文本按照某个符号自动分组
    /// </remarks>
    [TemplatePart(Name = ElementPanel, Type = typeof(Panel))]
    public class AutoGroupingText : Control
    {
        private const string ElementPanel = "PART_Panel";
        private StackPanel _panel;


        /// <summary>
        /// 标头后所带符号,形如"Title:"
        /// </summary>
        public string HeaderSymbol
        {
            get => (string)GetValue(HeaderSymbolProperty);
            set => SetValue(HeaderSymbolProperty, value);
        }
        /// <summary>
        /// 提供<see cref="HeaderSymbol"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty HeaderSymbolProperty =
            DependencyProperty.Register("HeaderSymbol", typeof(string), typeof(AutoGroupingText), new PropertyMetadata(":", OnPropertiesChanged));


        /// <summary>
        /// 分割符号
        /// </summary>
        public char SplitSymbol
        {
            get => (char)GetValue(SplitSymbolProperty);
            set => SetValue(SplitSymbolProperty, value);
        }
        /// <summary>
        /// 提供<see cref="SplitSymbol"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty SplitSymbolProperty =
            DependencyProperty.Register("SplitSymbol", typeof(char), typeof(AutoGroupingText), new PropertyMetadata((char)0x20, OnPropertiesChanged));


        /// <summary>
        /// 文本，格式形如"Title1:Content1 Title2:Content2"
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Text"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(AutoGroupingText), new PropertyMetadata(default(string), OnPropertiesChanged));


        public GridLength HeaderWidth
        {
            get => (GridLength)GetValue(HeaderWidthProperty);
            set => SetValue(HeaderWidthProperty, value);
        }
        /// <summary>
        /// 提供<see cref="HeaderWidth"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty HeaderWidthProperty =
            DependencyProperty.Register("HeaderWidth", typeof(GridLength), typeof(AutoGroupingText), new PropertyMetadata(GridLength.Auto, OnPropertiesChanged));


        /// <summary>
        /// 子项每行的高度
        /// </summary>
        public double ItemHeight
        {
            get => (double)GetValue(ItemHeightProperty);
            set => SetValue(ItemHeightProperty, value);
        }
        /// <summary>
        /// 提供<see cref="ItemHeight"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight", typeof(double), typeof(AutoGroupingText), new PropertyMetadata(ValueBoxes.Double20Box, OnPropertiesChanged));


        private static void OnPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AutoGroupingText ctl)
            {
                if (ctl.IsLoaded)
                {
                    RefreshChildren(ctl, ctl.Text);
                }
                else
                {
                    ctl.Loaded += (s, arg) =>
                    {
                        RefreshChildren(ctl, ctl.Text);
                    };
                }
            }
        }

        private static void RefreshChildren(AutoGroupingText ctl, string txt)
        {
            if (txt == null) return;
            if (ctl._panel == null) return;
            ctl._panel.Children.Clear();
            var arrayContent = txt.Split(ctl.SplitSymbol);
            if (arrayContent.Length <= 0)
                return;

            for (int i = 0; i < arrayContent.Length; i++)
            {
                var group = arrayContent[i].Split(':');
                //if (group.Length != 2)
                //    throw new ArgumentException($"{e.NewValue} is out of format, please add ':' for the divide ");
                if (group.Length == 2)
                {
                    EditingTag tag = new EditingTag
                    {
                        Height = ctl.ItemHeight,
                        Foreground = ctl.Foreground,
                        Header = group[0] + ctl.HeaderSymbol,
                        HeaderWidth = ctl.HeaderWidth,
                        HeaderHorizontalAlignment = ctl.HorizontalContentAlignment,
                        HeaderVerticalAlignment = ctl.VerticalContentAlignment,
                        Content = group[1],
                        EditorType = EditorType.TextBlock
                    };
                    ctl._panel.Children.Add(tag);
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _panel = GetTemplateChild(ElementPanel) as StackPanel;
        }
    }
}
