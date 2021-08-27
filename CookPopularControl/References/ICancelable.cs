using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ICancelable
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 10:43:55
 */
namespace CookPopularControl.References
{
    internal interface ICancelable : IDisposable
    {
        bool IsDisposed { get; }
    }
}
