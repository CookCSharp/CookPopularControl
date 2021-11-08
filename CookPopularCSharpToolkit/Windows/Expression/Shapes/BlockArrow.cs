using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：BlockArrow
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:05:36
 */
namespace CookPopularCSharpToolkit.Windows.Expression
{
    /// <summary>
    /// Renders a block arrow shape that supports resizable arrow head and body.
    /// </summary>
    public sealed class BlockArrow : PrimitiveShape, IBlockArrowGeometrySourceParameters, IGeometrySourceParameters
    {
        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        /// <value>The orientation where the arrow is pointing to.</value>
        public ArrowOrientation Orientation
        {
            get
            {
                return (ArrowOrientation)base.GetValue(BlockArrow.OrientationProperty);
            }
            set
            {
                base.SetValue(BlockArrow.OrientationProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the arrow head angle.
        /// </summary>
        /// <value>The arrow head angle in degrees.</value>
        public double ArrowheadAngle
        {
            get
            {
                return (double)base.GetValue(BlockArrow.ArrowheadAngleProperty);
            }
            set
            {
                base.SetValue(BlockArrow.ArrowheadAngleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the size of the arrow body.
        /// </summary>
        /// <value>The size of the arrow body in pixels.</value>
        public double ArrowBodySize
        {
            get
            {
                return (double)base.GetValue(BlockArrow.ArrowBodySizeProperty);
            }
            set
            {
                base.SetValue(BlockArrow.ArrowBodySizeProperty, value);
            }
        }

        protected override IGeometrySource CreateGeometrySource()
        {
            return new BlockArrowGeometrySource();
        }

        Stretch IGeometrySourceParameters.Stretch => Stretch;

        Brush IGeometrySourceParameters.Stroke => Stroke;

        double IGeometrySourceParameters.StrokeThickness => StrokeThickness;


        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(ArrowOrientation), typeof(BlockArrow), new DrawingPropertyMetadata(ArrowOrientation.Right, DrawingPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ArrowheadAngleProperty = DependencyProperty.Register("ArrowheadAngle", typeof(double), typeof(BlockArrow), new DrawingPropertyMetadata(90.0, DrawingPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ArrowBodySizeProperty = DependencyProperty.Register("ArrowBodySize", typeof(double), typeof(BlockArrow), new DrawingPropertyMetadata(0.5, DrawingPropertyMetadataOptions.AffectsRender));
    }
}
