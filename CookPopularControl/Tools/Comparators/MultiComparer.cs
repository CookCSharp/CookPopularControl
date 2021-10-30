using System;
using System.Collections.Generic;
using System.Linq;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：MultiComparer
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 15:12:33
 */
namespace CookPopularControl.Tools.Comparators
{
    public class MultiComparer<TObject> : IComparer<TObject>
    {
        private readonly IList<FuncComparer<TObject>> _attributeComparers;

        private MultiComparer(FuncComparer<TObject> firstComparer)
        {
            _attributeComparers = new List<FuncComparer<TObject>>
            {
                firstComparer
            };
        }

        public static MultiComparer<TObject> Ascending<TAttribute>(Func<TObject, TAttribute> accessor) where TAttribute : IComparable
        {
            if (accessor == null) throw new ArgumentNullException("accessor");

            return new MultiComparer<TObject>(BuildAscendingComparer(accessor));
        }

        public static MultiComparer<TObject> Descending<TAttribute>(Func<TObject, TAttribute> accessor)
            where TAttribute : IComparable
        {
            if (accessor == null) throw new ArgumentNullException("accessor");

            return new MultiComparer<TObject>(BuildDescendingComparer(accessor));
        }

        public MultiComparer<TObject> ThenAscending<TAttribute>(Func<TObject, TAttribute> accessor)
            where TAttribute : IComparable
        {
            if (accessor == null) throw new ArgumentNullException("accessor");

            _attributeComparers.Add(BuildAscendingComparer(accessor));

            return this;
        }

        public MultiComparer<TObject> ThenDescending<TAttribute>(Func<TObject, TAttribute> accessor)
            where TAttribute : IComparable
        {
            if (accessor == null) throw new ArgumentNullException("accessor");

            _attributeComparers.Add(BuildDescendingComparer(accessor));

            return this;
        }

        public int Compare(TObject x, TObject y)
        {
            var nonEqual = _attributeComparers.Select(c => new { result = c.Compare(x, y) }).FirstOrDefault(a => a.result != 0);

            return nonEqual == null ? 0 : nonEqual.result;
        }

        private static FuncComparer<TObject> BuildAscendingComparer<TAttribute>(Func<TObject, TAttribute> accessor)
             where TAttribute : IComparable
        {
            //TODO handle ref types better
            return new FuncComparer<TObject>((x, y) => accessor(x).CompareTo(accessor(y)));

        }

        private static FuncComparer<TObject> BuildDescendingComparer<TAttribute>(Func<TObject, TAttribute> accessor)
            where TAttribute : IComparable
        {
            //TODO handle ref types better
            return new FuncComparer<TObject>((x, y) => accessor(y).CompareTo(accessor(x)));
        }
    }
}
