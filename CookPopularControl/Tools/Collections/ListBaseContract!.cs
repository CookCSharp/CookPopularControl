using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ListBaseContract_
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-05 16:55:36
 */
namespace CookPopularControl.Tools.Collections
{
    [ContractClassFor(typeof(ListBase<>))]
    public abstract class ListBaseContract<T> : ListBase<T>
    {
        protected override T GetItem(int index)
        {
            Contract.Requires(index >= 0);
            Contract.Requires(index < Count);
            return default(T);
        }

        public override int Count
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                return default(int);
            }
        }
    }
}
