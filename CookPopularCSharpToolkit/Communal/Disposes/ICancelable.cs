using System;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ICancelable
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 10:43:55
 */
namespace CookPopularCSharpToolkit.Communal
{
    internal interface ICancelable : IDisposable
    {
        bool IsDisposed { get; }
    }
}
