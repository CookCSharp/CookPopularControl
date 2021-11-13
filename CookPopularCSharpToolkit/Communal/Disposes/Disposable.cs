using System;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Disposable
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 11:36:55
 */
namespace CookPopularCSharpToolkit.Communal
{
    /// <summary>
    /// 标准可释放对象
    /// </summary>
    internal static class Disposable
    {
        public static IDisposable Empty
        {
            get
            {
                return DefaultDisposable.Instance;
            }
        }

        public static IDisposable Create(Action dispose)
        {
            if (dispose == null)
                throw new ArgumentNullException("dispose");
            else
                return new AnonymousDisposable(dispose);
        }
    }
}
