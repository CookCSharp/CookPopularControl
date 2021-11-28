using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：LineArrow
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 18:04:11
 */
namespace CookPopularControl.Expression
{
    /// <summary>
    /// Renders a bent line segment with optional arrow heads on both ends.
    /// </summary>
    public sealed class LineArrow : CompositeShape, ILineArrowGeometrySourceParameters, IGeometrySourceParameters
    {
        protected override IGeometrySource CreateGeometrySource()
        {
            return new LineArrowGeometrySource();
        }

        /// <summary>
        /// Gets or sets the amount of bend for the arrow.
        /// </summary>
        /// <value>The bend amount between 0 and 1.</value>
        public double BendAmount
        {
            get
            {
                return (double)base.GetValue(LineArrow.BendAmountProperty);
            }
            set
            {
                base.SetValue(LineArrow.BendAmountProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets how the arrow head is rendered at the start of the line.
        /// </summary>
        public ArrowType StartArrow
        {
            get
            {
                return (ArrowType)base.GetValue(LineArrow.StartArrowProperty);
            }
            set
            {
                base.SetValue(LineArrow.StartArrowProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets how the arrow head is rendered at the end of the line.
        /// </summary>
        public ArrowType EndArrow
        {
            get
            {
                return (ArrowType)base.GetValue(LineArrow.EndArrowProperty);
            }
            set
            {
                base.SetValue(LineArrow.EndArrowProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets from which corner to start drawing the arrow.
        /// </summary>
        public CornerType StartCorner
        {
            get
            {
                return (CornerType)base.GetValue(LineArrow.StartCornerProperty);
            }
            set
            {
                base.SetValue(LineArrow.StartCornerProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the length of the arrow in pixels.
        /// </summary>
        public double ArrowSize
        {
            get
            {
                return (double)base.GetValue(LineArrow.ArrowSizeProperty);
            }
            set
            {
                base.SetValue(LineArrow.ArrowSizeProperty, value);
            }
        }

        public LineArrow()
        {
            base.DefaultStyleKey = typeof(LineArrow);
        }

        /// <summary>Provides the behavior for the Measure pass of Silverlight layout. Classes can override this method to define their own Measure pass behavior.</summary>
        /// <returns>The size that this object determines it requires during layout, based on its calculations of child object allotted sizes, or possibly on other considerations such as fixed container size.</returns>
        /// <param name="availableSize">The available size that this object can give to child objects. Infinity (<see cref="F:System.Double.PositiveInfinity" />) can be specified as a value to indicate that the object will size to whatever content is available.</param>
        /// <remarks>
        /// A default <see cref="T:LineArrow" /> can render at anysize.
        /// The <see cref="P:RenderedGeometry" /> will stretch to the layout boundary and render to the outside if necessary.
        /// </remarks>
        protected override Size MeasureOverride(Size availableSize)
        {
            return base.MeasureOverride(new Size(0.0, 0.0));
        }

        public static readonly DependencyProperty BendAmountProperty = DependencyProperty.Register("BendAmount", typeof(double), typeof(LineArrow), new DrawingPropertyMetadata(0.5, DrawingPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StartArrowProperty = DependencyProperty.Register("StartArrow", typeof(ArrowType), typeof(LineArrow), new DrawingPropertyMetadata(ArrowType.NoArrow, DrawingPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty EndArrowProperty = DependencyProperty.Register("EndArrow", typeof(ArrowType), typeof(LineArrow), new DrawingPropertyMetadata(ArrowType.Arrow, DrawingPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StartCornerProperty = DependencyProperty.Register("StartCorner", typeof(CornerType), typeof(LineArrow), new DrawingPropertyMetadata(CornerType.TopLeft, DrawingPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ArrowSizeProperty = DependencyProperty.Register("ArrowSize", typeof(double), typeof(LineArrow), new DrawingPropertyMetadata(10.0, DrawingPropertyMetadataOptions.AffectsRender));
    }
}
