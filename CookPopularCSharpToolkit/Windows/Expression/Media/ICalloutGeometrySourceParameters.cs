using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ICalloutGeometrySourceParameters
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:22:07
 */
namespace CookPopularCSharpToolkit.Windows.Expression
{
    public interface ICalloutGeometrySourceParameters : IGeometrySourceParameters
    {
        CalloutStyle CalloutStyle { get; }

        Point AnchorPoint { get; }
    }
}
