using CookPopularControl.Communal.Data;
using CookPopularControl.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：BranchAccessor
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-12 08:42:23
 */
namespace CookPopularControl.Controls.Dragables.Core
{
    public class BranchAccessor
    {
        private readonly Branch _branch;
        private readonly BranchAccessor _firstItemBranchAccessor;
        private readonly BranchAccessor _secondItemBranchAccessor;
        private readonly DragableTabControl _firstItemTabableControl;
        private readonly DragableTabControl _secondItemTabableControl;

        public BranchAccessor(Branch branch)
        {
            if (branch == null) throw new ArgumentNullException("branch");

            _branch = branch;

            var firstChildBranch = branch.FirstItem as Branch;
            if (firstChildBranch != null)
                _firstItemBranchAccessor = new BranchAccessor(firstChildBranch);
            else
                _firstItemTabableControl = FindTabablzControl(branch.FirstItem, branch.FirstContentPresenter);

            var secondChildBranch = branch.SecondItem as Branch;
            if (secondChildBranch != null)
                _secondItemBranchAccessor = new BranchAccessor(secondChildBranch);
            else
                _secondItemTabableControl = FindTabablzControl(branch.SecondItem, branch.SecondContentPresenter);
        }

        private static DragableTabControl FindTabablzControl(object item, DependencyObject contentPresenter)
        {
            var result = item as DragableTabControl;
            return result ?? contentPresenter.GetVisualDescendantsAndSelf().OfType<DragableTabControl>().FirstOrDefault();
        }

        public Branch Branch
        {
            get { return _branch; }
        }

        public BranchAccessor FirstItemBranchAccessor
        {
            get { return _firstItemBranchAccessor; }
        }

        public BranchAccessor SecondItemBranchAccessor
        {
            get { return _secondItemBranchAccessor; }
        }

        public DragableTabControl FirstItemTabableControl
        {
            get { return _firstItemTabableControl; }
        }

        public DragableTabControl SecondItemTabableControl
        {
            get { return _secondItemTabableControl; }
        }

        /// <summary>
        /// Visits the content of the first or second side of a branch, according to its content type.  No more than one of the provided <see cref="Action"/>
        /// callbacks will be called.  
        /// </summary>
        /// <param name="childItem"></param>
        /// <param name="childBranchVisitor"></param>
        /// <param name="childTabablzControlVisitor"></param>
        /// <param name="childContentVisitor"></param>
        /// <returns></returns>
        public BranchAccessor Visit(BranchItem childItem,
            Action<BranchAccessor> childBranchVisitor = null,
            Action<DragableTabControl> childTabablzControlVisitor = null,
            Action<object> childContentVisitor = null)
        {
            Func<BranchAccessor> branchGetter;
            Func<DragableTabControl> tabGetter;
            Func<object> contentGetter;

            switch (childItem)
            {
                case BranchItem.First:
                    branchGetter = () => _firstItemBranchAccessor;
                    tabGetter = () => _firstItemTabableControl;
                    contentGetter = () => _branch.FirstItem;
                    break;
                case BranchItem.Second:
                    branchGetter = () => _secondItemBranchAccessor;
                    tabGetter = () => _secondItemTabableControl;
                    contentGetter = () => _branch.SecondItem;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("childItem");
            }

            var branchDescription = branchGetter();
            if (branchDescription != null)
            {
                if (childBranchVisitor != null)
                    childBranchVisitor(branchDescription);
                return this;
            }

            var tabablzControl = tabGetter();
            if (tabablzControl != null)
            {
                if (childTabablzControlVisitor != null)
                    childTabablzControlVisitor(tabablzControl);

                return this;
            }

            if (childContentVisitor == null) return this;

            var content = contentGetter();
            if (content != null)
                childContentVisitor(content);

            return this;
        }
    }
}
