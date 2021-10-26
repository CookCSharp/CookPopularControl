using CookPopularControl.Communal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Tag
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-08 09:04:09
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 可编辑标签
    /// </summary>
    /// <remarks>由<see cref="ContentPresenter"/>与<see cref="TextBox"/>组合而成</remarks>
    public class EditingTag : Control
    {
        /// <summary>
        /// 标签头
        /// </summary>
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        /// <summary>
        /// 表示<see cref="Header"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(EditingTag), new PropertyMetadata(default(object)));


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
            DependencyProperty.Register("HeaderWidth", typeof(GridLength), typeof(EditingTag), new PropertyMetadata(GridLength.Auto));


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
            DependencyProperty.Register("HeaderHorizontalAlignment", typeof(HorizontalAlignment), typeof(EditingTag), new PropertyMetadata(default(HorizontalAlignment)));


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
            DependencyProperty.Register("HeaderVerticalAlignment", typeof(VerticalAlignment), typeof(EditingTag), new PropertyMetadata(default(VerticalAlignment)));


        /// <summary>
        /// <see cref="EditingTag"/>内容类型，<see cref="EditorType"/>默认为<see cref="TextBox"/>
        /// </summary>
        public EditorType EditorType
        {
            get { return (EditorType)GetValue(EditorTypeProperty); }
            set { SetValue(EditorTypeProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="EditorType"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty EditorTypeProperty =
            DependencyProperty.Register("EditorType", typeof(EditorType), typeof(EditingTag), new PropertyMetadata(default(EditorType)));


        /// <summary>
        /// 标签内容
        /// </summary>
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        /// <summary>
        /// 表示<see cref="Content"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(EditingTag), new PropertyMetadata(default(object)));


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
            DependencyProperty.Register("HeaderMargin", typeof(Thickness), typeof(EditingTag), new PropertyMetadata(default(Thickness)));
    }
}
