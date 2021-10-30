using CookPopularControl.Expression.Media;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Callout
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:59:07
 */
namespace CookPopularControl.Expression.Controls
{
    /// <summary>
    /// Renders a callout shape supporting several shapes combined with a callout arrow.
    /// </summary>
    public sealed class Callout : CompositeContentShape, ICalloutGeometrySourceParameters, IGeometrySourceParameters
    {
        /// <summary>
        /// Gets or sets the position of the callout relative to the top and left corner.
        /// </summary>
        public Point AnchorPoint
        {
            get
            {
                return (Point)base.GetValue(Callout.AnchorPointProperty);
            }
            set
            {
                base.SetValue(Callout.AnchorPointProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the callout style.
        /// </summary>
        public CalloutStyle CalloutStyle
        {
            get
            {
                return (CalloutStyle)base.GetValue(Callout.CalloutStyleProperty);
            }
            set
            {
                base.SetValue(Callout.CalloutStyleProperty, value);
            }
        }

        public Callout()
        {
            base.DefaultStyleKey = typeof(Callout);
        }

        protected override IGeometrySource CreateGeometrySource()
        {
            return new CalloutGeometrySource();
        }

        public static readonly DependencyProperty AnchorPointProperty = DependencyProperty.Register("AnchorPoint", typeof(Point), typeof(Callout), new DrawingPropertyMetadata(default(Point), DrawingPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty CalloutStyleProperty = DependencyProperty.Register("CalloutStyle", typeof(CalloutStyle), typeof(Callout), new DrawingPropertyMetadata(CalloutStyle.RoundedRectangle, DrawingPropertyMetadataOptions.AffectsRender));
    }
}
