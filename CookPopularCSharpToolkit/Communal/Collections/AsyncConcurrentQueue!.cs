/*
 * Description：AsyncConcurrentQueue_ 
 * Author： Chance.Zheng
 * Create Time: 2022-11-03 17:17:37
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2020-2022 All Rights Reserved.
 */


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace CookPopularCSharpToolkit.Communal
{
    public class AsyncConcurrentQueue<T>
    {
        /// <summary>
        /// Backing queue
        /// </summary>
        private readonly ConcurrentQueue<T> queue = new ConcurrentQueue<T>();

        /// <summary>
        /// Wake up any pending dequeue task
        /// </summary>
        private TaskCompletionSource<bool> dequeueTask;
        private SemaphoreSlim @dequeueTaskLock = new SemaphoreSlim(1);
        private CancellationToken taskCancellationToken;

        internal AsyncConcurrentQueue(CancellationToken taskCancellationToken)
        {
            this.taskCancellationToken = taskCancellationToken;
        }

        /// <summary>
        /// Supports multi-threaded producers
        /// </summary>
        /// <param name="value"></param>
        internal void Enqueue(T value)
        {
            queue.Enqueue(value);

            dequeueTaskLock.Wait();
            dequeueTask?.TrySetResult(true);
            dequeueTaskLock.Release();
        }

        /// <summary>
        /// Assumes a single-threaded consumer!
        /// </summary>
        /// <returns></returns>
        internal async Task<T> DequeueAsync()
        {
            T result;
            queue.TryDequeue(out result);

            if (result != null)
            {
                return result;
            }

            await dequeueTaskLock.WaitAsync();
            dequeueTask = new TaskCompletionSource<bool>();
            dequeueTaskLock.Release();

            taskCancellationToken.Register(() => dequeueTask.TrySetCanceled());
            await dequeueTask.Task;

            queue.TryDequeue(out result);
            return result;
        }
    }
}
