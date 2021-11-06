﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：SortHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-05 16:23:58
 */
namespace CookPopularCSharpToolkit.Communal
{
    public static class SortHelper
    {
        public static bool QuickSort<T>(this IList<T> list, Func<T, T, int> comparer)
        {
            Contract.Requires(list != null);
            Contract.Requires(comparer != null);
            return list.QuickSort(comparer.ToComparer());
        }

        public static bool QuickSort<T>(this IList<T> list, Comparison<T> comparison)
        {
            Contract.Requires(list != null);
            Contract.Requires(comparison != null);
            return list.QuickSort(comparison.ToComparer());
        }

        public static bool QuickSort<T>(this IList<T> list)
        {
            Contract.Requires(list != null);
            return list.QuickSort(Comparer<T>.Default);
        }

        public static bool QuickSort<T>(this IList<T> list, IComparer<T> comparer)
        {
            Contract.Requires(list != null);
            Contract.Requires(comparer != null);

            if (list.Count > 1)
            {
                try
                {
                    return QuickSort(list, 0, list.Count - 1, comparer);
                }
                catch (IndexOutOfRangeException ioore)
                {
                    throw new ArgumentException("Bogus IComparer", ioore);
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException("IComparer Failed", exception);
                }
            }
            return false;
        }

        private static bool QuickSort<T>(IList<T> keys, int left, int right, IComparer<T> comparer)
        {
            Debug.Assert(comparer != null);
            Debug.Assert(keys != null);
            Debug.Assert(left >= 0);
            Debug.Assert(left < keys.Count);
            Debug.Assert(right >= 0);
            Debug.Assert(right < keys.Count);

            bool change = false;
            do
            {
                int a = left;
                int b = right;
                int num3 = a + ((b - a) >> 1);
                change = SwapIfGreaterWithItems(keys, comparer, a, num3) || change;
                change = SwapIfGreaterWithItems(keys, comparer, a, b) || change;
                change = SwapIfGreaterWithItems(keys, comparer, num3, b) || change;
                T y = keys[num3];
                do
                {
                    while (comparer.Compare(keys[a], y) < 0)
                    {
                        a++;
                    }
                    while (comparer.Compare(y, keys[b]) < 0)
                    {
                        b--;
                    }
                    if (a > b)
                    {
                        break;
                    }
                    if (a < b)
                    {
                        T local2 = keys[a];
                        keys[a] = keys[b];
                        keys[b] = local2;
                        change = true;
                    }
                    a++;
                    b--;
                }
                while (a <= b);
                if ((b - left) <= (right - a))
                {
                    if (left < b)
                    {
                        change = QuickSort(keys, left, b, comparer) || change;
                    }
                    left = a;
                }
                else
                {
                    if (a < right)
                    {
                        change = QuickSort(keys, a, right, comparer) || change;
                    }
                    right = b;
                }
            }
            while (left < right);

            return change;
        }

        private static bool SwapIfGreaterWithItems<T>(IList<T> keys, IComparer<T> comparer, int a, int b)
        {
            Contract.Requires(comparer != null);
            Contract.Requires(keys != null);
            Contract.Requires(a >= 0);
            Contract.Requires(a < keys.Count);
            Contract.Requires(a >= 0);
            Contract.Requires(b < keys.Count);

            if ((a != b) && (comparer.Compare(keys[a], keys[b]) > 0))
            {
                T local = keys[a];
                keys[a] = keys[b];
                keys[b] = local;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
