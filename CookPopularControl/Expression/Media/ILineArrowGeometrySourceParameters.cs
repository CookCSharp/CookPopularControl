/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ILineArrowGeometrySourceParameters
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:19:27
 */
namespace CookPopularControl.Expression
{
    public interface ILineArrowGeometrySourceParameters : IGeometrySourceParameters
    {
        double BendAmount { get; }

        double ArrowSize { get; }

        ArrowType StartArrow { get; }

        ArrowType EndArrow { get; }

        CornerType StartCorner { get; }
    }
}
