using CookPopularControl.Communal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Finder
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-12 08:36:09
 */
namespace CookPopularControl.Controls.Dragables.Core
{
    internal static class Finder
    {
        internal static LocationReport Find(DragableTabControl tabablzControl)
        {
            if (tabablzControl == null) throw new ArgumentNullException("tabablzControl");

            var locationReportBuilder = new LocationReportBuilder(tabablzControl);

            foreach (var loadedInstance in Layout.GetLoadedInstances())
            {
                locationReportBuilder.CurrentLayout = loadedInstance;

                loadedInstance.Query().Visit(
                    locationReportBuilder,
                    BranchVisitor,
                    TabablzControlVisitor
                    );

                if (locationReportBuilder.IsFound)
                    break;
            }

            if (!locationReportBuilder.IsFound)
                throw new LocationReportException("Instance not within any layout.");

            return locationReportBuilder.ToLocationReport();
        }

        private static void BranchVisitor(LocationReportBuilder locationReportBuilder, BranchAccessor branchAccessor)
        {
            if (Equals(branchAccessor.FirstItemTabableControl, locationReportBuilder.TargetTabableControl))
                locationReportBuilder.MarkFound(branchAccessor.Branch, false);
            else if (Equals(branchAccessor.SecondItemTabableControl, locationReportBuilder.TargetTabableControl))
                locationReportBuilder.MarkFound(branchAccessor.Branch, true);
            else
            {
                branchAccessor.Visit(BranchItem.First, ba => BranchVisitor(locationReportBuilder, ba));
                if (locationReportBuilder.IsFound) return;
                branchAccessor.Visit(BranchItem.Second, ba => BranchVisitor(locationReportBuilder, ba));
            }
        }

        private static void TabablzControlVisitor(LocationReportBuilder locationReportBuilder, DragableTabControl tabableControl)
        {
            if (Equals(tabableControl, locationReportBuilder.TargetTabableControl))
                locationReportBuilder.MarkFound();
        }
    }
}
