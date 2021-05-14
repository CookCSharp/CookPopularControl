using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DispatcherHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-12 11:22:37
 */
namespace CookPopularControl.Tools.Helpers
{
    public class DispatcherHelper
    {
        /// <summary>
        /// 循环执行<see cref="Dispatcher"/>
        /// </summary>
        public static void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            DispatcherOperation exitOperation = Dispatcher.CurrentDispatcher.InvokeAsync(new Action(() => frame.Continue = false), DispatcherPriority.Background);
            //DispatcherOperation exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(t => (t as DispatcherFrame)!.Continue = false), frame);
            Dispatcher.PushFrame(frame);

            if (exitOperation.Status != DispatcherOperationStatus.Executing)
            {
                exitOperation.Abort();
            }
        }
    }
}
