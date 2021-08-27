using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：LocationReportBuilder
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-12 08:37:58
 */
namespace CookPopularControl.Controls.Dragables.Core
{
    internal class LocationReportBuilder
    {
        private readonly DragableTabControl _targetTabableControl;
        private Branch _branch;
        private bool _isSecondLeaf;
        private Layout _layout;

        public LocationReportBuilder(DragableTabControl targetTabableControl)
        {
            _targetTabableControl = targetTabableControl;
        }

        public DragableTabControl TargetTabableControl
        {
            get { return _targetTabableControl; }
        }

        public bool IsFound { get; private set; }

        public void MarkFound()
        {
            if (IsFound)
                throw new InvalidOperationException("Already found.");

            IsFound = true;

            _layout = CurrentLayout;
        }

        public void MarkFound(Branch branch, bool isSecondLeaf)
        {
            if (branch == null) throw new ArgumentNullException("branch");
            if (IsFound)
                throw new InvalidOperationException("Already found.");

            IsFound = true;

            _layout = CurrentLayout;
            _branch = branch;
            _isSecondLeaf = isSecondLeaf;
        }

        public Layout CurrentLayout { get; set; }

        public LocationReport ToLocationReport()
        {
            return new LocationReport(_targetTabableControl, _layout, _branch, _isSecondLeaf);
        }
    }
}
