using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：HeaderedDragablzItem
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 16:01:39
 */
namespace CookPopularControl.Controls.Dragables
{
    public class HeaderedDragableItem : DragableItem
    {
        static HeaderedDragableItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HeaderedDragableItem), new FrameworkPropertyMetadata(typeof(HeaderedDragableItem)));
        }

        public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register(
            "HeaderContent", typeof(object), typeof(HeaderedDragableItem), new PropertyMetadata(default(object)));

        public object HeaderContent
        {
            get { return (object)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        public static readonly DependencyProperty HeaderContentStringFormatProperty = DependencyProperty.Register(
            "HeaderContentStringFormat", typeof(string), typeof(HeaderedDragableItem), new PropertyMetadata(default(string)));

        public string HeaderContentStringFormat
        {
            get { return (string)GetValue(HeaderContentStringFormatProperty); }
            set { SetValue(HeaderContentStringFormatProperty, value); }
        }

        public static readonly DependencyProperty HeaderContentTemplateProperty = DependencyProperty.Register(
            "HeaderContentTemplate", typeof(DataTemplate), typeof(HeaderedDragableItem), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate HeaderContentTemplate
        {
            get { return (DataTemplate)GetValue(HeaderContentTemplateProperty); }
            set { SetValue(HeaderContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderContentTemplateSelectorProperty = DependencyProperty.Register(
            "HeaderContentTemplateSelector", typeof(DataTemplateSelector), typeof(HeaderedDragableItem), new PropertyMetadata(default(DataTemplateSelector)));

        public DataTemplateSelector HeaderContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(HeaderContentTemplateSelectorProperty); }
            set { SetValue(HeaderContentTemplateSelectorProperty, value); }
        }
    }
}
