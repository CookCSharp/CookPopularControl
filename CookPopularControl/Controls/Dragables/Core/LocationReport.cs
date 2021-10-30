using System;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：LocationReport
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 18:07:22
 */
namespace CookPopularControl.Controls.Dragables.Core
{
    /// <summary>
    /// Provides information about where a tab control is withing a layout structure.
    /// </summary>
    public class LocationReport
    {
        private readonly DragableTabControl _tabableControl;
        private readonly Layout _rootLayout;
        private readonly Branch _parentBranch;
        private readonly bool _isLeaf;
        private readonly bool _isSecondLeaf;

        //TODO I've internalised constructor for now, so I can come back and add Window without breaking.

        internal LocationReport(DragableTabControl tabableControl, Layout rootLayout) : this(tabableControl, rootLayout, null, false)
        { }

        internal LocationReport(DragableTabControl tabableControl, Layout rootLayout, Branch parentBranch, bool isSecondLeaf)
        {
            if (tabableControl == null) throw new ArgumentNullException("tabablzControl");
            if (rootLayout == null) throw new ArgumentNullException("rootLayout");

            _tabableControl = tabableControl;
            _rootLayout = rootLayout;
            _parentBranch = parentBranch;
            _isLeaf = _parentBranch != null;
            _isSecondLeaf = isSecondLeaf;
        }

        public DragableTabControl TabablzControl
        {
            get { return _tabableControl; }
        }

        public Layout RootLayout
        {
            get { return _rootLayout; }
        }

        /// <summary>
        /// Gets the parent branch if this is a leaf. If the <see cref="TabablzControl"/> is directly under the <see cref="RootLayout"/> will be <c>null</c>.
        /// </summary>
        public Branch ParentBranch
        {
            get { return _parentBranch; }
        }

        /// <summary>
        /// Idicates if this is a leaf in a branch. <c>True</c> if <see cref="ParentBranch"/> is not null.
        /// </summary>
        public bool IsLeaf
        {
            get { return _isLeaf; }
        }

        public bool IsSecondLeaf
        {
            get { return _isSecondLeaf; }
        }
    }
}
