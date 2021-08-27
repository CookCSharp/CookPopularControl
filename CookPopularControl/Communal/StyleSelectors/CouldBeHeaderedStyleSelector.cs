using CookPopularControl.Controls.Dragables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CouldBeHeaderedStyleSelector
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 16:05:06
 */
namespace CookPopularControl.Communal.StyleSelectors
{
    public class CouldBeHeaderedStyleSelector : StyleSelector
    {
        public Style NonHeaderedStyle { get; set; }

        public Style HeaderedStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            return container is HeaderedDragableItem || container is HeaderedContentControl
                   ? HeaderedStyle
                   : NonHeaderedStyle;
        }
    }
}
