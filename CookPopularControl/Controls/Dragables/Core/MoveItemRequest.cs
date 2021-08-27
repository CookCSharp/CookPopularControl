using CookPopularControl.Communal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：MoveItemRequest
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 14:03:12
 */
namespace CookPopularControl.Controls.Dragables.Core
{
    public class MoveItemRequest
    {
        private readonly object _item;
        private readonly object _context;
        private readonly AddLocationHint _addLocationHint;

        public MoveItemRequest(object item, object context, AddLocationHint addLocationHint)
        {
            _item = item;
            _context = context;
            _addLocationHint = addLocationHint;
        }

        public object Item => _item;

        public object Context => _context;

        public AddLocationHint AddLocationHint => _addLocationHint;
    }
}
