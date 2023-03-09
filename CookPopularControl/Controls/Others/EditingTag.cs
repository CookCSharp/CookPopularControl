using CookPopularControl.Communal.Data;
using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows;
using System;
using System.ComponentModel;
using System.Globalization;
//using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：EditingTag
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-08 09:04:09
 */
namespace CookPopularControl.Controls
{
    [MarkupExtensionReturnType(typeof(Visibility))]
    [ValueConversion(typeof(EditorType), typeof(Visibility))]
    public class EditorTypeToVisibilityConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Equals(parameter))
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 可编辑标签元素的类型
    /// </summary>
    internal enum EditorType
    {
        TextBox,
        TextBlock,
        NumericUpDown,
        Other,
    }

    public enum HeaderAligment
    {
        Left,
        Top,
    }

    /// <summary>
    /// 可编辑标签
    /// </summary>
    /// <remarks>由两个<see cref="ContentControl"/>组合而成的标签控件</remarks>
    [TemplatePart(Name = ElementFrameworkElement, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ElementTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = ElementNumericUpDown, Type = typeof(NumericUpDown))]
    [DefaultProperty("Content")]
    [ContentProperty("Content")]
    [Localizability(LocalizationCategory.None)]
    public class EditingTag : HeaderedContentControl
    {
        private const string ElementFrameworkElement = "PART_Content";
        private const string ElementTextBox = "PART_TextBox";
        private const string ElementNumericUpDown = "PART_NumericUpDown";


        private bool IsInDesignMode
        {
            get
            {
                return (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
            }
        }


        /// <summary>
        /// <see cref="EditingTag"/>内容类型，<see cref="EditorType"/>默认为<see cref="TextBox"/>
        /// </summary>
        internal EditorType EditorType
        {
            get { return (EditorType)GetValue(EditorTypeProperty); }
            set { SetValue(EditorTypeProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="EditorType"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty EditorTypeProperty =
            DependencyProperty.Register("EditorType", typeof(EditorType), typeof(EditingTag), new PropertyMetadata(default(EditorType), OnPropertiesChanged));


        /// <summary>
        /// 标头位置，上或者左
        /// </summary>
        public HeaderAligment Aligment
        {
            get => (HeaderAligment)GetValue(AligmentProperty);
            set => SetValue(AligmentProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Aligment"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty AligmentProperty =
            DependencyProperty.Register("Aligment", typeof(HeaderAligment), typeof(EditingTag), new PropertyMetadata(default(HeaderAligment)));


        /// <summary>
        /// 标签头的宽度
        /// </summary>
        public GridLength HeaderWidth
        {
            get => (GridLength)GetValue(HeaderWidthProperty);
            set => SetValue(HeaderWidthProperty, value);
        }
        /// <summary>
        /// 提供<see cref="HeaderWidth"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty HeaderWidthProperty =
            DependencyProperty.Register("HeaderWidth", typeof(GridLength), typeof(EditingTag), new PropertyMetadata(GridLength.Auto, OnPropertiesChanged));


        /// <summary>
        /// 标签头的高度
        /// </summary>
        public GridLength HeaderHeight
        {
            get => (GridLength)GetValue(HeaderHeightProperty);
            set => SetValue(HeaderHeightProperty, value);
        }
        /// <summary>
        /// 提供<see cref="HeaderHeight"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty HeaderHeightProperty =
            DependencyProperty.Register("HeaderHeight", typeof(GridLength), typeof(EditingTag), new PropertyMetadata(GridLength.Auto, OnPropertiesChanged));


        /// <summary>
        /// 标签头的水平定位
        /// </summary>
        public HorizontalAlignment HeaderHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(HeaderHorizontalAlignmentProperty);
            set => SetValue(HeaderHorizontalAlignmentProperty, value);
        }
        /// <summary>
        /// 提供<see cref="HeaderHorizontalAlignment"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty HeaderHorizontalAlignmentProperty =
            DependencyProperty.Register("HeaderHorizontalAlignment", typeof(HorizontalAlignment), typeof(EditingTag), new PropertyMetadata(HorizontalAlignment.Right, OnPropertiesChanged));


        /// <summary>
        /// 标签头的垂直定位
        /// </summary>
        public VerticalAlignment HeaderVerticalAlignment
        {
            get => (VerticalAlignment)GetValue(HeaderVerticalAlignmentProperty);
            set => SetValue(HeaderVerticalAlignmentProperty, value);
        }
        /// <summary>
        /// 提供<see cref="HeaderVerticalAlignment"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty HeaderVerticalAlignmentProperty =
            DependencyProperty.Register("HeaderVerticalAlignment", typeof(VerticalAlignment), typeof(EditingTag), new PropertyMetadata(default(VerticalAlignment), OnPropertiesChanged));


        /// <summary>
        /// 标签头与内容间距
        /// </summary>
        public Thickness HeaderMargin
        {
            get { return (Thickness)GetValue(HeaderMarginProperty); }
            set { SetValue(HeaderMarginProperty, value); }
        }
        /// <summary>
        /// 表示<see cref="HeaderMargin"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty HeaderMarginProperty =
            DependencyProperty.Register("HeaderMargin", typeof(Thickness), typeof(EditingTag), new PropertyMetadata(new Thickness(0, 0, 6, 0), OnPropertiesChanged));


        private static void OnPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EditingTag editing)
            {
                if (!DesignerProperties.GetIsInDesignMode(editing))
                {
                    d.SetValue(e.Property, e.NewValue);
                }
            }
        }
    }


    /// <summary>
    /// 可编辑标签的附加属性类
    /// </summary>
    public class EditingTagAssistant
    {
        public static double GetNumericMinimum(DependencyObject obj) => (double)obj.GetValue(NumericMinimumProperty);
        public static void SetNumericMinimum(DependencyObject obj, double value) => obj.SetValue(NumericMinimumProperty, value);
        public static readonly DependencyProperty NumericMinimumProperty =
            DependencyProperty.RegisterAttached("NumericMinimum", typeof(double), typeof(EditingTagAssistant), new PropertyMetadata(ValueBoxes.Double0Box));


        public static double GetNumericaMaximum(DependencyObject obj) => (double)obj.GetValue(NumericMaximumProperty);
        public static void SetNumericMaximum(DependencyObject obj, double value) => obj.SetValue(NumericMaximumProperty, value);
        public static readonly DependencyProperty NumericMaximumProperty =
            DependencyProperty.RegisterAttached("NumericMaximum", typeof(double), typeof(EditingTagAssistant), new PropertyMetadata(double.MaxValue));
    }
}
