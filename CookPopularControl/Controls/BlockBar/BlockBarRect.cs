using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：BlockBarRec
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-06 15:06:09
 */
namespace CookPopularControl.Controls.BlockBar
{
    /// <summary>
    /// 表示矩形块状
    /// </summary>
    public class BlockBarRect : BlockBarBase
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Contract.Requires(!RenderSize.IsEmpty, "RenderSize");
            Contract.Requires(BlockCount > 0, "BlockCount");

            double width = (RenderSize.Width - (BlockCount - 1) * BlockMargin - BorderBen.Thickness) / BlockCount;
            double height = RenderSize.Height - BorderBen.Thickness;
            if (width <= 0 || height <= 0) return;

            for (int i = 0; i < BlockCount; i++)
            {
                Contract.Requires(BlockCount > i, "BlockNumber");

                double left = BorderBen.Thickness / 2 + (width + BlockMargin) * i;
                var rect = new Rect(left, BorderBen.Thickness / 2, width, height);
                if (!rect.IsEmpty)
                {
                    int threshold = GetThreshold(Value, BlockCount);
                    drawingContext.DrawRectangle((i < threshold) ? Foreground : Background, BorderBen, rect);
                }
            }
        }
    }
}
