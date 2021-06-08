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
namespace CookPopularControl.Controls.GroupControls
{
    /// <summary>
    /// 可编辑标签
    /// </summary>
    /// <remarks>由<see cref="ContentPresenter"/>与<see cref="TextBox"/>组合而成</remarks>
    public class TagEditing : Control
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
            DependencyProperty.Register("Header", typeof(object), typeof(TagEditing), new PropertyMetadata(default(object)));


        /// <summary>
        /// 标签内容
        /// </summary>
        public string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        /// <summary>
        /// 表示<see cref="Content"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(string), typeof(TagEditing), new PropertyMetadata(default(string)));


        /// <summary>
        /// 标签头相对于内容的位置
        /// </summary>
        public Dock Position
        {
            get { return (Dock)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }
        /// <summary>
        /// 表示<see cref="Header"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Dock), typeof(TagEditing), new PropertyMetadata(default(Dock)));
    }
}
