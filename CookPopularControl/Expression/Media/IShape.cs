using System;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：IShape
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:17:08
 */
namespace CookPopularControl.Expression
{
    /// <summary>
    /// Provides the necessary interface to define a Shape.
    /// Both primitive and composite shapes need to match this interface, although they might derive from different types of FrameworkElement.
    /// </summary>
    public interface IShape
    {
        /// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> that specifies how to paint the interior of the shape.</summary>
        /// <returns>A <see cref="T:System.Windows.Media.Brush" /> that describes how the shape's interior is painted. The default is null.</returns>
        Brush Fill { get; set; }

        /// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> that specifies how the <see cref="T:System.Windows.Shapes.Shape" /> outline is painted.</summary>
        /// <returns>A <see cref="T:System.Windows.Media.Brush" /> that specifies how the <see cref="T:System.Windows.Shapes.Shape" /> outline is painted.</returns>
        Brush Stroke { get; set; }

        /// <summary>Gets or sets the width of the <see cref="T:System.Windows.Shapes.Shape" /> stroke outline. </summary>
        /// <returns>The width of the <see cref="T:System.Windows.Shapes.Shape" /> outline, in pixels.</returns>
        double StrokeThickness { get; set; }

        /// <summary>Gets or sets a <see cref="T:System.Windows.Media.Stretch" /> enumeration value that describes how the shape fills its allocated space.</summary>
        /// <returns>One of the <see cref="T:System.Windows.Media.Stretch" /> enumeration values. The default value at runtime depends on the type of <see cref="T:System.Windows.Shapes.Shape" />.</returns>
        Stretch Stretch { get; set; }

        /// <summary>
        /// Gets the rendered geometry presented by the rendering engine.
        /// </summary>
        Geometry RenderedGeometry { get; }

        /// <summary>
        /// Gets the margin between logical bounds and actual geometry bounds.
        /// This can be either positive (as in <see cref="T:Microsoft.Expression.Shapes.Arc" />) or negative (as in <see cref="T:Microsoft.Expression.Controls.Callout" />).
        /// </summary>
        Thickness GeometryMargin { get; }

        /// <summary>
        /// Invalidates the geometry for a <see cref="T:Microsoft.Expression.Media.IShape" />. After the invalidation, the <see cref="T:Microsoft.Expression.Media.IShape" /> will recompute the geometry, which will occur asynchronously.
        /// </summary>
        void InvalidateGeometry(InvalidateGeometryReasons reasons);

        /// <summary>
        /// Occurs when RenderedGeometry is changed.
        /// </summary>
        event EventHandler RenderedGeometryChanged;
    }
}
