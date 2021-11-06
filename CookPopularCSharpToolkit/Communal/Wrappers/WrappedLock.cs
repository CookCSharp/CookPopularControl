using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：WrappedLock
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-05 11:56:52
 */
namespace CookPopularCSharpToolkit.Communal
{
    public class WrappedLock : WrappedLock<object>
    {
        public WrappedLock(Action actionOnLock, Action actionOnUnlock) : base(WrapMaybeNull(actionOnLock), WrapMaybeNull(actionOnUnlock)) { }

        public IDisposable GetLock()
        {
            return GetLock(null);
        }

        private static Action<object> WrapMaybeNull(Action action)
        {
            return (param) =>
            {
                if (action != null) { action(); }
            };
        }
    }

    public class WrappedLock<T>
    {
        public WrappedLock(Action<T> actionOnLock, Action<T> actionOnUnlock)
        {
            m_actionOnLock = actionOnLock ?? new Action<T>((param) => { });
            m_actionOnUnlock = actionOnUnlock ?? new Action<T>((param) => { });
        }

        public IDisposable GetLock(T param)
        {
            Contract.Ensures(Contract.Result<IDisposable>() != null);
            if (m_stack.Count == 0)
            {
                m_actionOnLock(param);
            }

            var g = Guid.NewGuid();
            m_stack.Push(g);
            return new ActionDispose(() => Unlock(g, param));
        }

        public bool IsLocked { get { return m_stack.Count > 0; } }

        private void Unlock(Guid g, T param)
        {
            if (m_stack.Count > 0 && m_stack.Peek() == g)
            {
                m_stack.Pop();

                if (m_stack.Count == 0)
                {
                    m_actionOnUnlock(param);
                }
            }
            else
            {
                throw new InvalidOperationException("Unlock happened in the wrong order or at a weird time or too many times");
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(m_stack != null);
            Contract.Invariant(m_actionOnLock != null);
            Contract.Invariant(m_actionOnUnlock != null);
        }

        private readonly Stack<Guid> m_stack = new Stack<Guid>();
        private readonly Action<T> m_actionOnUnlock;
        private readonly Action<T> m_actionOnLock;
    }
}
