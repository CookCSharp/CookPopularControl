using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：BlockBarCircle
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-06 14:47:58
 */
namespace CookPopularControl.Controls.BlockBar
{
    /// <summary>
    /// 表示圆形块
    /// </summary>
    public class BlockBarCircle : BlockBarBase
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Size effectiveRenderSize = new Size(this.RenderSize.Width - BorderBen.Thickness, this.RenderSize.Height - BorderBen.Thickness);

            double circleDiameter = (effectiveRenderSize.Width - (BlockCount - 1) * BlockMargin) / BlockCount;
            if (circleDiameter > effectiveRenderSize.Height)
            {
                circleDiameter = effectiveRenderSize.Height;
            }

            //double startLeft = penThickness / 2 + effectiveRenderSize.Width - (this.BlockCount * circleDiameter + (this.BlockCount - 1) * BlockMargin);
            //double startTop = penThickness / 2 + (effectiveRenderSize.Height - circleDiameter) / 2;

            double circleRadius = circleDiameter / 2;
            Point center = new Point();

            int threshHold = GetThreshold(this.Value, this.BlockCount);

            for (int i = 0; i < this.BlockCount; i++)
            {
                double startLeft = BorderBen.Thickness / 2 + (circleDiameter + BlockMargin) * i;
                double startTop = BorderBen.Thickness / 2 + (effectiveRenderSize.Height - circleDiameter) / 2;
                center.X = startLeft + circleRadius;
                center.Y = startTop + circleRadius;

                //var brushToUse = ((this.BlockCount - (i + 1)) < threshHold) ? Foreground : Background;
                var brushToUse = (i < threshHold) ? Foreground : Background;
                drawingContext.DrawEllipse(brushToUse, BorderBen, center, circleRadius, circleRadius);

                startLeft += circleDiameter + BlockMargin;
            }
        }
    }
}
