using CookPopularControl.Communal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：LayoutAccessor
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-12 08:40:20
 */
namespace CookPopularControl.Controls.Dragables.Core
{
    /// <summary>
    /// Provides information about the <see cref="Layout"/> instance.
    /// </summary>
    public class LayoutAccessor
    {
        private readonly Layout _layout;
        private readonly BranchAccessor _branchAccessor;
        private readonly DragableTabControl _dragableTabControl;

        public LayoutAccessor(Layout layout)
        {
            if (layout == null) throw new ArgumentNullException("layout");

            _layout = layout;

            var branch = Layout.Content as Branch;
            if (branch != null)
                _branchAccessor = new BranchAccessor(branch);
            else
                _dragableTabControl = Layout.Content as DragableTabControl;
        }

        public Layout Layout
        {
            get { return _layout; }
        }

        public IEnumerable<DragableItem> FloatingItems
        {
            get { return _layout.FloatingDragablzItems(); }
        }

        /// <summary>
        /// <see cref="BranchAccessor"/> and <see cref="TabablzControl"/> are mutually exclusive, according to whether the layout has been split, or just contains a tab control.
        /// </summary>
        public BranchAccessor BranchAccessor
        {
            get { return _branchAccessor; }
        }

        /// <summary>
        /// <see cref="BranchAccessor"/> and <see cref="TabablzControl"/> are mutually exclusive, according to whether the layout has been split, or just contains a tab control.
        /// </summary>
        public DragableTabControl DragableTabControl
        {
            get { return _dragableTabControl; }
        }

        /// <summary>
        /// Visits the content of the layout, according to its content type.  No more than one of the provided <see cref="Action"/>
        /// callbacks will be called.  
        /// </summary>        
        public LayoutAccessor Visit(
            Action<BranchAccessor> branchVisitor = null,
            Action<DragableTabControl> tabablzControlVisitor = null,
            Action<object> contentVisitor = null)
        {
            if (_branchAccessor != null)
            {
                if (branchVisitor != null)
                {
                    branchVisitor(_branchAccessor);
                }

                return this;
            }

            if (_dragableTabControl != null)
            {
                if (tabablzControlVisitor != null)
                    tabablzControlVisitor(_dragableTabControl);

                return this;
            }

            if (_layout.Content != null && contentVisitor != null)
                contentVisitor(_layout.Content);

            return this;
        }

        /// <summary>
        /// Gets all the Tabablz controls in a Layout, regardless of location.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DragableTabControl> TabablzControls()
        {
            var tabablzControls = new List<DragableTabControl>();
            this.Visit(tabablzControls, BranchAccessorVisitor, TabablzControlVisitor);
            return tabablzControls;
        }

        private static void TabablzControlVisitor(IList<DragableTabControl> resultSet, DragableTabControl tabablzControl)
        {
            resultSet.Add(tabablzControl);
        }

        private static void BranchAccessorVisitor(IList<DragableTabControl> resultSet, BranchAccessor branchAccessor)
        {
            branchAccessor
                .Visit(resultSet, BranchItem.First, BranchAccessorVisitor, TabablzControlVisitor)
                .Visit(resultSet, BranchItem.Second, BranchAccessorVisitor, TabablzControlVisitor);
        }
    }
}
