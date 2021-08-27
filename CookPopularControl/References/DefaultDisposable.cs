using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DefaultDisposable
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 11:37:45
 */
namespace CookPopularControl.References
{
    /// <summary>
    /// 默认可释放对象
    /// </summary>
    internal sealed class DefaultDisposable : IDisposable
    {
        public static readonly DefaultDisposable Instance = new DefaultDisposable();

        static DefaultDisposable()
        {
        }

        private DefaultDisposable()
        {
        }

        public void Dispose()
        {
        }
    }
}
