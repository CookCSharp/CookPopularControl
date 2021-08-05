using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：IComparers
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-05 16:32:33
 */
namespace CookPopularControl.Tools
{
    public class FuncComparer<T> : IComparer<T>
    {
        public FuncComparer(Func<T, T, int> func)
        {
            Contract.Requires(func != null);
            m_func = func;
        }

        public int Compare(T x, T y)
        {
            return m_func(x, y);
        }

        private readonly Func<T, T, int> m_func;
    }

    public class ComparisonComparer<T> : IComparer<T>
    {
        public ComparisonComparer(Comparison<T> func)
        {
            Contract.Requires(func != null);
            m_func = func;
        }

        public int Compare(T x, T y)
        {
            return m_func(x, y);
        }

        private readonly Comparison<T> m_func;
    }

    public class FuncEqualityComparer<T> : IEqualityComparer<T>
    {
        public FuncEqualityComparer(Func<T, T, bool> func)
        {
            Contract.Requires(func != null);
            m_func = func;
        }
        public bool Equals(T x, T y)
        {
            return m_func(x, y);
        }

        public int GetHashCode(T obj)
        {
            return 0; // this is on purpose. Should only use function...not short-cut by hashcode compare
        }

        [ContractInvariantMethod]
        void ObjectInvariant()
        {
            Contract.Invariant(m_func != null);
        }

        private readonly Func<T, T, bool> m_func;
    }
}
