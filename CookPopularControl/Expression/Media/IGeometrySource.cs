using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：IGeometrySource
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:21:22
 */
namespace CookPopularControl.Expression
{
    /// <summary>
    /// Provides an interface to describe the source of a geometry.
    /// </summary>
    /// <remarks>
    /// This interface is designed to expose the geometry source in a non-generic way.
    /// Typical implementation should subclass GeometrySource instead of implementing this interface directly.
    /// </remarks>
    public interface IGeometrySource
    {
        /// <summary>
        /// Notifies that the geometry has been invalidated because of external changes.
        /// </summary>
        /// <remarks>
        /// Geometry is typically invalidated when parameters are changed.
        /// If any geometry has been invalidated externally, the geometry will be recomputed even if the layout bounds change.
        /// </remarks>
        bool InvalidateGeometry(InvalidateGeometryReasons reasons);

        /// <summary>
        /// Update the geometry using the given parameters and the layout bounds.
        /// Returns false if nothing has been updated.
        /// </summary>
        bool UpdateGeometry(IGeometrySourceParameters parameters, Rect layoutBounds);

        /// <summary>
        /// Gets or sets the resulting geometry after the latest UpdateGeometry().
        /// </summary>
        Geometry Geometry { get; }

        /// <summary>
        /// Gets the bounding box where the geometry should stretch to.
        /// The actual geometry might be smaller or larger than this.
        /// <see cref="P:Microsoft.Expression.Media.IGeometrySource.LogicalBounds" /> should already take stroke thickness and stretch into consideration.
        /// </summary>
        Rect LogicalBounds { get; }

        /// <summary>
        /// Gets the actual bounds of FrameworkElement.
        /// <see cref="P:Microsoft.Expression.Media.IGeometrySource.LayoutBounds" /> includes logical bounds, stretch, and stroke thickness.
        /// </summary>
        Rect LayoutBounds { get; }
    }
}
