using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-02-20 10:14:22
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 重叠面板
    /// </summary>
    /// <remarks>用于代替Grid</remarks>
    public class SimpleGrid : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            var size = new Size();

            foreach (UIElement child in InternalChildren)
            {
                if (child != null)
                {
                    child.Measure(availableSize);
                    size.Width = Math.Max(size.Width, child.DesiredSize.Width);
                    size.Height = Math.Max(size.Height, child.DesiredSize.Height);
                }
            }

            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                child?.Arrange(new Rect(finalSize));
            }

            return finalSize;
        }
    }
}
