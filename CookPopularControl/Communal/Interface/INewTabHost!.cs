using CookPopularControl.Controls.Dragables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：INewTabHost
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 17:35:45
 */
namespace CookPopularControl.Communal.Interface
{
    public interface INewTabHost<out TElement> where TElement : UIElement
    {
        TElement Container { get; }
        DragableTabControl DragableTabControl { get; }
    }
}
