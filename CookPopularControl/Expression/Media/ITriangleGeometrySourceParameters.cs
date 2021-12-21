using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Description：ITriangleGeometrySourceParameters 
 * Author： Chance(a cook of write code)
 * Company: NCATest
 * Create Time：2021-11-28 15:47:57
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) NCATest 2021 All Rights Reserved.
 */
namespace CookPopularControl.Expression
{
    public interface ITriangleGeometrySourceParameters : IGeometrySourceParameters
    {
        double FirstSide { get; }

        double SecondSide { get; }

        double ThirdSide { get; }

        double Angle { get; }
    }
}
