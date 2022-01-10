using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;



/*
 * Description：Triangle 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2021-11-28 15:46:55
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2021 All Rights Reserved.
 */
namespace CookPopularControl.Expression
{
    public class Triangle : PrimitiveShape, ITriangleGeometrySourceParameters, IGeometrySourceParameters
    {
        public double FirstSide
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

        public double SecondSide
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

        public double ThirdSide
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

        public double Angle
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

        protected override IGeometrySource CreateGeometrySource()
        {
            return new TriangleGeometrySource();
        }

        Stretch IGeometrySourceParameters.Stretch => Stretch;

        Brush IGeometrySourceParameters.Stroke => Stroke;

        double IGeometrySourceParameters.StrokeThickness => StrokeThickness;

        public static readonly DependencyProperty FirstSideProperty = DependencyProperty.Register("FirstSide", typeof(double), typeof(Triangle), new DrawingPropertyMetadata(5, DrawingPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty SecondSideProperty = DependencyProperty.Register("SecondSide", typeof(double), typeof(Triangle), new DrawingPropertyMetadata(5, DrawingPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty ThirdSideProperty = DependencyProperty.Register("ThirdSide", typeof(double), typeof(Triangle), new DrawingPropertyMetadata(5, DrawingPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double), typeof(Triangle), new DrawingPropertyMetadata(90, DrawingPropertyMetadataOptions.AffectsRender));
    }
}
