using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ActionDispose
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-05 16:14:06
 */
namespace CookPopularControl.Tools
{
    /// <summary>
    /// Provides a wrapper over <see cref="IDisposable"/> that
    /// invokes the provided delegate when <see cref="IDisposable.Dispose()"/>
    /// is called.
    /// </summary>
    /// <example>
    /// <code>
    /// 
    /// SqlConnection conn = new SqlConnection(connectionString);
    /// using(new ActionOnDispose(new Action(conn.Close))
    /// {
    ///     // Do work here...
    ///     // For cases where you want the connection closed
    ///     // but not disposed
    /// }
    /// </code>
    /// </example>
    public class ActionDispose : IDisposable
    {
        private Action m_unlockDelegate;

        /// <summary>
        /// Creats a new <see cref="ActionOnDispose"/>using the provided <see cref="Action"/>.
        /// </summary>
        /// <param name="unlockAction">
        ///     The <see cref="Action"/> to invoke when <see cref="Dispose"/> is called.
        /// </param>
        /// <exception cref="ArgumentNullException">if <paramref name="unlockAction"/> is null.</exception>
        public ActionDispose(Action unlockAction)
        {
            Contract.Requires(unlockAction != null);

            m_unlockDelegate = unlockAction;
        }

        /// <summary>
        /// Calls the provided Action if it has not been called;otherwise, throws an <see cref="Exception"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">If <see cref="Dispose()"/> has already been called.</exception>
        public void Dispose()
        {
            Action action = Interlocked.Exchange(ref m_unlockDelegate, null);
            if (action == null)
                throw new ObjectDisposedException("Dispose has already been called on this object.", default(Exception));
            action();
        }
    }
}
