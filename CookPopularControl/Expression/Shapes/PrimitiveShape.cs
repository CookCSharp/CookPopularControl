using CookPopularControl.Expression.Drawing.Core;
using CookPopularControl.Expression.Media;
using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：PrimitiveShape
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:01:50
 */
namespace CookPopularControl.Expression.Shapes
{
	/// <summary>
	/// Platform-neutral implementation of Shape deriving from WPF:Shape or SL:Path.
	/// </summary>
	/// <summary>
	/// Provides the WPF implementation of Shape that derives from the platform Shape.
	/// </summary>
	public abstract class PrimitiveShape : Shape, IGeometrySourceParameters, IShape
	{
		/// <summary>
		/// Occurs when RenderedGeometry is changed.
		/// </summary>
		public event EventHandler RenderedGeometryChanged;

		/// <summary>
		/// Extends how the shape is drawn with creating geometry source.
		/// </summary>
		protected abstract IGeometrySource CreateGeometrySource();

		private IGeometrySource GeometrySource
		{
			get
			{
				IGeometrySource result;
				if ((result = this.geometrySource) == null)
				{
					result = (this.geometrySource = this.CreateGeometrySource());
				}
				return result;
			}
		}

		/// <summary>
		/// Invalidates the geometry for a <see cref="T:Microsoft.Expression.Media.IShape" />. After the invalidation, the <see cref="T:Microsoft.Expression.Media.IShape" /> will recompute the geometry, which will occur asynchronously.
		/// </summary>
		public void InvalidateGeometry(InvalidateGeometryReasons reasons)
		{
			if (this.GeometrySource.InvalidateGeometry(reasons))
			{
				base.InvalidateArrange();
			}
		}

		/// <summary>Provides the behavior for the Measure portion of Silverlight layout pass. Classes can override this method to define their own Measure pass behavior.</summary>
		/// <returns>The size that this object determines it requires during layout, based on its calculations of child object allotted sizes, or possibly on other considerations such as fixed container size.</returns>
		/// <param name="availableSize">The available size that this object can provide to child objects. Infinity (<see cref="F:System.Double.PositiveInfinity" />) can be specified as a value to indicate that the object will size to whatever content is available.</param>
		/// <remarks>
		/// In WPF, measure override works from Shape.DefiningGeometry which is not always as expected
		/// see bug 99497 for details where WPF is not having correct measure by default.
		///
		/// In Silverlight, measure override on Path does not work the same as primitive shape works.
		///
		/// We should return the smallest size this shape can correctly render without clipping.
		/// By default a shape can render as small as a dot, therefore returning the strokethickness.
		/// </remarks>
		protected override Size MeasureOverride(Size availableSize)
		{
			return new Size(base.StrokeThickness, base.StrokeThickness);
		}

		/// <summary>Provides the behavior for the Arrange portion of Silverlight layout pass. Classes can override this method to define their own Arrange pass behavior.</summary>
		/// <returns>The actual size used once the element is arranged in layout.</returns>
		/// <param name="finalSize">The final area within the parent that this object should use to arrange itself and its children.</param>
		/// <remarks> <see cref="T:Microsoft.Expression.Shapes.PrimitiveShape" />  will recompute the Geometry when it's invalidated and update the RenderedGeometry and GeometryMargin.</remarks>
		protected override Size ArrangeOverride(Size finalSize)
		{
			if (this.GeometrySource.UpdateGeometry(this, finalSize.Bounds()))
			{
				this.RealizeGeometry();
			}
			base.ArrangeOverride(finalSize);
			return finalSize;
		}

		private void RealizeGeometry()
		{
			if (this.RenderedGeometryChanged != null)
			{
				this.RenderedGeometryChanged(this, EventArgs.Empty);
			}
		}

		protected sealed override Geometry DefiningGeometry
		{
			get
			{
				return this.GeometrySource.Geometry ?? Geometry.Empty;
			}
		}

		/// <summary>
		/// Gets the margin between logical bounds and actual geometry bounds.
		/// This can be either positive (as in <see cref="T:Microsoft.Expression.Shapes.Arc" />) or negative (as in <see cref="T:Microsoft.Expression.Controls.Callout" />).
		/// </summary>
		public Thickness GeometryMargin
		{
			get
			{
				if (this.RenderedGeometry == null)
				{
					return default(Thickness);
				}
				return this.GeometrySource.LogicalBounds.Subtract(this.RenderedGeometry.Bounds);
			}
		}

		static PrimitiveShape()
		{
			Shape.StretchProperty.OverrideMetadata(typeof(PrimitiveShape), new DrawingPropertyMetadata(Stretch.Fill, DrawingPropertyMetadataOptions.AffectsRender));
			Shape.StrokeThicknessProperty.OverrideMetadata(typeof(PrimitiveShape), new DrawingPropertyMetadata(ValueBoxes.Double1Box, DrawingPropertyMetadataOptions.AffectsRender));
		}

		Stretch IGeometrySourceParameters.Stretch => Stretch;

		Brush IGeometrySourceParametersStroke => Stroke;

		double IGeometrySourceParameters.StrokeThickness => StrokeThickness;


        Brush IShape.Fill
        {
			get => Fill;
			set => Fill = value;
        }

		Brush IShape.Stroke
		{
			get => Stroke;
			set => Stroke = value;
		}

		double IShape.StrokeThickness
		{
			get => StrokeThickness;
			set => StrokeThickness = value;
		}

		Stretch IShape.Stretch
		{
			get => Stretch;
			set => Stretch = value;
		}

        private IGeometrySource geometrySource;
	}
}
