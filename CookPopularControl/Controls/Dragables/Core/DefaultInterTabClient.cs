using CookPopularControl.Communal.Data;
using CookPopularControl.Communal.Interface;
using CookPopularControl.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DefaultInterTabClient
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 17:38:49
 */
namespace CookPopularControl.Controls.Dragables.Core
{
    public class DefaultInterTabClient : IInterTabClient
    {
        public virtual INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, DragableTabControl source)
        {
            if (source == null) throw new ArgumentNullException("source");
            var sourceWindow = Window.GetWindow(source);
            if (sourceWindow == null) throw new ApplicationException("Unable to ascertain source window.");
            var newWindow = (Window)Activator.CreateInstance(sourceWindow.GetType());

            newWindow.Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.DataBind);

            var newTabablzControl = newWindow.LogicalTreeDepthFirstTraversal().OfType<DragableTabControl>().FirstOrDefault();
            if (newTabablzControl == null) throw new ApplicationException("Unable to ascertain tab control.");

            if (newTabablzControl.ItemsSource == null)
                newTabablzControl.Items.Clear();

            return new NewTabHost<Window>(newWindow, newTabablzControl);
        }

        public virtual TabEmptiedResponse TabEmptiedHandler(DragableTabControl tabControl, Window window)
        {
            return TabEmptiedResponse.CloseWindowOrLayoutBranch;
        }
    }
}
