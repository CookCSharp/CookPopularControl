using CookPopularControl.Expression.Drawing.Core;
using CookPopularControl.Expression.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CompositeShape
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 18:02:15
 */
namespace CookPopularControl.Expression.Controls
{
	/// <summary>
	/// Provides a base class of a composite shape that derives from <see cref="T:System.Windows.Controls.Control" /> and implements <see cref="T:Microsoft.Expression.Media.IShape" />.
	/// </summary>
	/// <remarks>
	/// <see cref="T:Microsoft.Expression.Controls.CompositeShape" /> implements <see cref="T:Microsoft.Expression.Media.IShape" /> interface,
	/// and supports rendering a geometry similar to <see cref="T:System.Windows.Shapes.Shape" />, but the geometry can be rendered outside the layout boundary.
	///
	/// A typical implementation has a customized default template in generic.xaml which template-binds most shape properties to a <see cref="T:System.Windows.Shapes.Path" />.
	/// It should also extend the <see cref="P:GeometrySource" /> property to customize the appearance of the <see cref="T:System.Windows.Shapes.Path" />.
	/// </remarks>
	public abstract class CompositeShape : Control, IGeometrySourceParameters, IShape
	{
		private Path PartPath { get; set; }

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.PartPath = this.FindVisualDesendent((Path child) => child.Name == "PART_Path").FirstOrDefault<Path>();
			this.GeometrySource.InvalidateGeometry(InvalidateGeometryReasons.TemplateChanged);
		}

		/// <summary>
		/// Gets or sets the <see cref="T:System.Windows.Media.Brush" /> that specifies how to paint the interior of the shape.
		/// </summary>
		/// <returns>A <see cref="T:System.Windows.Media.Brush" /> that describes how the shape's interior is painted.</returns>
		public Brush Fill
		{
			get
			{
				return (Brush)base.GetValue(CompositeShape.FillProperty);
			}
			set
			{
				base.SetValue(CompositeShape.FillProperty, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> that specifies how the <see cref="T:System.Windows.Shapes.Shape" /> outline is painted.</summary>
		/// <returns>A <see cref="T:System.Windows.Media.Brush" /> that specifies how the <see cref="T:System.Windows.Shapes.Shape" /> outline is painted.</returns>
		public Brush Stroke
		{
			get
			{
				return (Brush)base.GetValue(CompositeShape.StrokeProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeProperty, value);
			}
		}

		/// <summary>Gets or sets the width of the <see cref="T:System.Windows.Shapes.Shape" /> stroke outline. </summary>
		/// <returns>The width of the <see cref="T:System.Windows.Shapes.Shape" /> outline, in pixels.</returns>
		public double StrokeThickness
		{
			get
			{
				return (double)base.GetValue(CompositeShape.StrokeThicknessProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeThicknessProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.Stretch" /> enumeration value that describes how the shape fills its allocated space.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Media.Stretch" /> enumeration values.</returns>
		public Stretch Stretch
		{
			get
			{
				return (Stretch)base.GetValue(CompositeShape.StretchProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StretchProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.PenLineCap" /> enumeration value that describes the <see cref="T:System.Windows.Shapes.Shape" /> at the start of a <see cref="P:System.Windows.Shapes.Shape.Stroke" />. </summary>
		/// <returns>A value of the <see cref="T:System.Windows.Media.PenLineCap" /> enumeration that specifies the shape at the start of a <see cref="P:System.Windows.Shapes.Shape.Stroke" />.</returns>
		public PenLineCap StrokeStartLineCap
		{
			get
			{
				return (PenLineCap)base.GetValue(CompositeShape.StrokeStartLineCapProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeStartLineCapProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.PenLineCap" /> enumeration value that describes the <see cref="T:System.Windows.Shapes.Shape" /> at the end of a line. </summary>
		/// <returns>One of the enumeration values for <see cref="T:System.Windows.Media.PenLineCap" />. </returns>
		public PenLineCap StrokeEndLineCap
		{
			get
			{
				return (PenLineCap)base.GetValue(CompositeShape.StrokeEndLineCapProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeEndLineCapProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.PenLineJoin" /> enumeration value that specifies the type of join that is used at the vertices of a <see cref="T:System.Windows.Shapes.Shape" />.</summary>
		/// <returns>A value of the <see cref="T:System.Windows.Media.PenLineJoin" /> enumeration that specifies the join appearance. </returns>
		public PenLineJoin StrokeLineJoin
		{
			get
			{
				return (PenLineJoin)base.GetValue(CompositeShape.StrokeLineJoinProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeLineJoinProperty, value);
			}
		}

		/// <summary>Gets or sets a limit on the ratio of the miter length to half the <see cref="P:System.Windows.Shapes.Shape.StrokeThickness" /> of a <see cref="T:System.Windows.Shapes.Shape" /> element. </summary>
		/// <returns>The limit on the ratio of the miter length to the <see cref="P:System.Windows.Shapes.Shape.StrokeThickness" /> of a <see cref="T:System.Windows.Shapes.Shape" /> element. This value is always a positive number that is greater than or equal to 1.</returns>
		public double StrokeMiterLimit
		{
			get
			{
				return (double)base.GetValue(CompositeShape.StrokeMiterLimitProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeMiterLimitProperty, value);
			}
		}

		/// <summary>Gets or sets a collection of <see cref="T:System.Double" /> values that indicate the pattern of dashes and gaps that is used to outline shapes. </summary>
		/// <returns>A collection of <see cref="T:System.Double" /> values that specify the pattern of dashes and gaps. </returns>
		public DoubleCollection StrokeDashArray
		{
			get
			{
				return (DoubleCollection)base.GetValue(CompositeShape.StrokeDashArrayProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeDashArrayProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.PenLineCap" /> enumeration value that specifies how the ends of a dash are drawn. </summary>
		/// <returns>One of the enumeration values for <see cref="T:System.Windows.Media.PenLineCap" />. The default is <see cref="F:System.Windows.Media.PenLineCap.Flat" />. </returns>
		public PenLineCap StrokeDashCap
		{
			get
			{
				return (PenLineCap)base.GetValue(CompositeShape.StrokeDashCapProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeDashCapProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Double" /> that specifies the distance within the dash pattern where a dash begins.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the distance within the dash pattern where a dash begins. The default value is 0.</returns>
		public double StrokeDashOffset
		{
			get
			{
				return (double)base.GetValue(CompositeShape.StrokeDashOffsetProperty);
			}
			set
			{
				base.SetValue(CompositeShape.StrokeDashOffsetProperty, value);
			}
		}

		/// <summary>
		/// Gets the rendered geometry presented by the rendering engine.
		/// </summary>
		public Geometry RenderedGeometry
		{
			get
			{
				return this.GeometrySource.Geometry;
			}
		}

		/// <summary>
		/// Gets the margin between the logical bounds and the actual geometry bounds.
		/// This can be either positive (as in <see cref="T:Microsoft.Expression.Shapes.Arc" />) or negative (as in <see cref="T:Microsoft.Expression.Controls.Callout" />).
		/// </summary>
		public Thickness GeometryMargin
		{
			get
			{
				if (this.PartPath == null || this.PartPath.Data == null)
				{
					return default(Thickness);
				}
				return this.GeometrySource.LogicalBounds.Subtract(this.PartPath.Data.Bounds);
			}
		}

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

		/// <summary>Provides the behavior for the Arrange portion of a Silverlight layout pass. Classes can override this method to define their own Arrange pass behavior.</summary>
		/// <returns>The actual size used once the element is arranged in layout.</returns>
		/// <param name="finalSize">The final area within the parent that this object should use to arrange itself and its children.</param>
		/// <remarks> <see cref="T:Microsoft.Expression.Controls.CompositeShape" />  will recompute the Geometry when it's invalidated and update the RenderedGeometry and GeometryMargin.</remarks>
		protected override Size ArrangeOverride(Size finalSize)
		{
			if (this.GeometrySource.UpdateGeometry(this, finalSize.Bounds()))
			{
				this.RealizeGeometry();
			}
			return base.ArrangeOverride(finalSize);
		}

		private void RealizeGeometry()
		{
			if (this.PartPath != null)
			{
				this.PartPath.Data = this.RenderedGeometry.CloneCurrentValue();
				this.PartPath.Margin = this.GeometryMargin;
			}
			if (this.RenderedGeometryChanged != null)
			{
				this.RenderedGeometryChanged(this, EventArgs.Empty);
			}
		}

		public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(CompositeShape), new PropertyMetadata(null));

		public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(CompositeShape), new PropertyMetadata(null));

		public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(CompositeShape), new DrawingPropertyMetadata(1.0, DrawingPropertyMetadataOptions.AffectsRender));

		public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(CompositeShape), new DrawingPropertyMetadata(Stretch.Fill, DrawingPropertyMetadataOptions.AffectsRender));

		public static readonly DependencyProperty StrokeStartLineCapProperty = DependencyProperty.Register("StrokeStartLineCap", typeof(PenLineCap), typeof(CompositeShape), new PropertyMetadata(PenLineCap.Flat));

		public static readonly DependencyProperty StrokeEndLineCapProperty = DependencyProperty.Register("StrokeEndLineCap", typeof(PenLineCap), typeof(CompositeShape), new PropertyMetadata(PenLineCap.Flat));

		public static readonly DependencyProperty StrokeLineJoinProperty = DependencyProperty.Register("StrokeLineJoin", typeof(PenLineJoin), typeof(CompositeShape), new PropertyMetadata(PenLineJoin.Miter));

		public static readonly DependencyProperty StrokeMiterLimitProperty = DependencyProperty.Register("StrokeMiterLimit", typeof(double), typeof(CompositeShape), new PropertyMetadata(10.0));

		public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register("StrokeDashArray", typeof(DoubleCollection), typeof(CompositeShape), new PropertyMetadata(null));

		public static readonly DependencyProperty StrokeDashCapProperty = DependencyProperty.Register("StrokeDashCap", typeof(PenLineCap), typeof(CompositeShape), new PropertyMetadata(PenLineCap.Flat));

		public static readonly DependencyProperty StrokeDashOffsetProperty = DependencyProperty.Register("StrokeDashOffset", typeof(double), typeof(CompositeShape), new PropertyMetadata(0.0));
		
		private IGeometrySource geometrySource;
	}
}
