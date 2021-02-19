using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-02-19 16:05:08
 */
namespace CookPopularControl.Tools.Boxes
{
    /// <summary>
    /// 装箱后的值类型（用于提高效率）
    /// </summary>
    internal static class ValueBoxes
    {
        internal static readonly object VisibleBox = Visibility.Visible;
        internal static readonly object HiddenBox = Visibility.Hidden;
        internal static readonly object CollapsedBox = Visibility.Collapsed;
        internal static object Box(this Visibility visibility)
        {
            object v;
            switch (visibility)
            {
                case Visibility.Visible:
                    v = VisibleBox;
                    break;
                case Visibility.Hidden:
                    v = HiddenBox;
                    break;
                case Visibility.Collapsed:
                    v = CollapsedBox;
                    break;
                default:
                    v = default;
                    break;
            }

            return v;
        }

        internal static readonly object TrueBox = true;
        internal static readonly object FalseBox = false;
        internal static object Box(this bool value) => value ? TrueBox : FalseBox;

        internal static readonly object Double30Box = 30.0;
    }
}
