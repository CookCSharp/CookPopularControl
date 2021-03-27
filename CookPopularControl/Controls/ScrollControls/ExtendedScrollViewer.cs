using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ExtendedScrollViewer
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-26 22:27:17
 */
namespace CookPopularControl.Controls.ScrollControls
{
    /// <summary>
    /// 解决滚动条劫持(scroll-wheel-hijack)的问题
    /// </summary>
    public class ExtendedScrollViewer : ScrollViewer
    {
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (ViewportHeight + VerticalOffset >= ExtentHeight && e.Delta <= 0)
                return;
            if (VerticalOffset == 0 && e.Delta >= 0)
                return;

            base.OnMouseWheel(e);
        }
    }
}
