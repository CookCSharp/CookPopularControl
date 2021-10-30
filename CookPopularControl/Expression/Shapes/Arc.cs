using CookPopularControl.Expression.Media;
using CookPopularControl.Tools.Boxes;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Arc
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 16:54:37
 */
namespace CookPopularControl.Expression.Shapes
{
    /// <summary>
    /// Renders an arc shape supporting Arc, Ring, and Pie mode controlled by ArcThickness.
    /// </summary>
    public sealed class Arc : PrimitiveShape, IArcGeometrySourceParameters, IGeometrySourceParameters
    {
        /// <summary>
        /// Gets or sets the start angle.
        /// </summary>
        /// <value>The start angle in degrees. Zero degrees is pointing up.</value>
        public double StartAngle
        {
            get
            {
                return (double)base.GetValue(Arc.StartAngleProperty);
            }
            set
            {
                base.SetValue(Arc.StartAngleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the end angle.
        /// </summary>
        /// <value>The end angle in degrees. Zero degrees is pointing up.</value>
        public double EndAngle
        {
            get
            {
                return (double)base.GetValue(Arc.EndAngleProperty);
            }
            set
            {
                base.SetValue(Arc.EndAngleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the arc thickness.
        /// </summary>
        /// <value>The arc thickness in pixels or percentage depending on "ArcThicknessUnit".</value>
        public double ArcThickness
        {
            get
            {
                return (double)base.GetValue(Arc.ArcThicknessProperty);
            }
            set
            {
                base.SetValue(Arc.ArcThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the arc thickness unit.
        /// </summary>
        /// <value>The arc thickness unit in pixels or percentage.</value>
        public UnitType ArcThicknessUnit
        {
            get
            {
                return (UnitType)base.GetValue(Arc.ArcThicknessUnitProperty);
            }
            set
            {
                base.SetValue(Arc.ArcThicknessUnitProperty, value);
            }
        }


        protected override IGeometrySource CreateGeometrySource()
        {
            return new ArcGeometrySource();
        }

        Stretch IGeometrySourceParameters.Stretch => Stretch;

        Brush IGeometrySourceParameters.Stroke => Stroke;

        double IGeometrySourceParameters.StrokeThickness => StrokeThickness;


        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register("StartAngle", typeof(double), typeof(Arc), new DrawingPropertyMetadata(ValueBoxes.Double0Box, DrawingPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register("EndAngle", typeof(double), typeof(Arc), new DrawingPropertyMetadata(90.0, DrawingPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ArcThicknessProperty = DependencyProperty.Register("ArcThickness", typeof(double), typeof(Arc), new DrawingPropertyMetadata(ValueBoxes.Double0Box, DrawingPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ArcThicknessUnitProperty = DependencyProperty.Register("ArcThicknessUnit", typeof(UnitType), typeof(Arc), new DrawingPropertyMetadata(UnitType.Pixel, DrawingPropertyMetadataOptions.AffectsRender));
    }
}
