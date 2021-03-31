﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：GridViewColumThumb
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-30 10:52:48
 */
namespace CookPopularControl.Controls.Thumb
{
    /// <summary>
    /// GridViewColumThumb
    /// </summary>
    public class GridViewColumThumb : System.Windows.Controls.Primitives.Thumb
    {
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.SizeWE;
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            Mouse.OverrideCursor = null;
        }
    }
}