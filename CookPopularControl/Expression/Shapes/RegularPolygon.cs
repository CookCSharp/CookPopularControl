using System.Windows;
using System.Windows.Media;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：RegularPolygon
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:03:49
 */
namespace CookPopularControl.Expression
{
    /// <summary>
    /// Renders a regular polygon shape or corresponding star shape with variable number of points.
    /// </summary>
    public sealed class RegularPolygon : PrimitiveShape, IPolygonGeometrySourceParameters, IGeometrySourceParameters
    {
        /// <summary>
        /// Gets or sets the number of points of the <see cref="T:Microsoft.Expression.Shapes.RegularPolygon" />.
        /// </summary>
        public double PointCount
        {
            get
            {
                return (double)base.GetValue(RegularPolygon.PointCountProperty);
            }
            set
            {
                base.SetValue(RegularPolygon.PointCountProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the the distance between the center and the innermost point.
        /// </summary>
        /// <value>The distance between the center and the innermost point.</value>
        public double InnerRadius
        {
            get
            {
                return (double)base.GetValue(RegularPolygon.InnerRadiusProperty);
            }
            set
            {
                base.SetValue(RegularPolygon.InnerRadiusProperty, value);
            }
        }

        protected override IGeometrySource CreateGeometrySource()
        {
            return new PolygonGeometrySource();
        }

        Stretch IGeometrySourceParameters.Stretch => Stretch;

        Brush IGeometrySourceParameters.Stroke => Stroke;

        double IGeometrySourceParameters.StrokeThickness => StrokeThickness;

        public static readonly DependencyProperty PointCountProperty = DependencyProperty.Register("PointCount", typeof(double), typeof(RegularPolygon), new DrawingPropertyMetadata(6.0, DrawingPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register("InnerRadius", typeof(double), typeof(RegularPolygon), new DrawingPropertyMetadata(1.0, DrawingPropertyMetadataOptions.AffectsRender));
    }
}

