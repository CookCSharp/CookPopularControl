using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：IGeometrySourceParameters
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:20:06
 */
namespace CookPopularControl.Expression
{
    /// <summary>
    /// Provides an interface to describe the parameters of a Shape.
    /// </summary>
    /// <remarks>
    /// This interface is the data for communication between Shape and GeometrySource.
    /// Typically, a concrete implementation of IShape will implement this interface and pass it into
    /// GeometrySource.UpdateGeometry(), which will then consume the shape as a read-only data provider.
    /// </remarks>
    public interface IGeometrySourceParameters
    {
        Stretch Stretch { get; }

        Brush Stroke { get; }

        double StrokeThickness { get; }
    }
}
