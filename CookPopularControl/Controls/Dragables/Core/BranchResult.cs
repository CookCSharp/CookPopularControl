using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：BranchResult
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 16:26:14
 */
namespace CookPopularControl.Controls.Dragables.Core
{
    public class BranchResult
    {
        private readonly Branch _branch;
        private readonly DragableTabControl _tabablzControl;

        public BranchResult(Branch branch, DragableTabControl tabablzControl)
        {
            if (branch == null) throw new ArgumentNullException("branch");
            if (tabablzControl == null) throw new ArgumentNullException("tabablzControl");

            _branch = branch;
            _tabablzControl = tabablzControl;
        }

        /// <summary>
        /// The new branch.
        /// </summary>
        public Branch Branch
        {
            get { return _branch; }
        }

        /// <summary>
        /// The new tab control.
        /// </summary>
        public DragableTabControl TabablzControl
        {
            get { return _tabablzControl; }
        }
    }
}
