using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：OnceTaskScheduler
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-13 19:39:33
 */
namespace CookPopularControl.Tools.Windows.Tasks
{
    /// <summary>
    /// 每个<see cref="Task"/>代表一个<see cref="Thread"/>
    /// </summary>
    /// <remarks><see cref="Task"/>是基于<see cref="ThreadPool"/>基础上的封装</remarks>
    public class OnceTaskScheduler : TaskScheduler
    {
        protected override void QueueTask(Task task)
        {
            new Thread(() => TryExecuteTask(task)).Start();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            throw new NotImplementedException();
        }
    }
}
