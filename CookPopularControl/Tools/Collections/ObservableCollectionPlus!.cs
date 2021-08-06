using CookPopularControl.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ObservableCollectionPlus
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-05 11:54:23
 */
namespace CookPopularControl.Tools.Collections
{
    public class ObservableCollectionPlus<T> : ObservableCollection<T>
    {
        public ObservableCollectionPlus() : this(Enumerable.Empty<T>()) { }
#if WP7
        public ObservableCollectionPlus(IEnumerable<T> collection)
        {
            m_roCollection = new ReadOnlyObservableCollection<T>(this);
            m_lock = new WrappedLock(BeforeMultiUpdate, unlock);

            // yes, crazy events are fired here. Who cares. No one can be listening. :-)
            collection.ForEach(item => base.Add(item));
        }
#else
        public ObservableCollectionPlus(IEnumerable<T> collection): base(collection)
        {
            m_roCollection = new ReadOnlyObservableCollection<T>(this);
            m_lock = new WrappedLock(BeforeMultiUpdate, FinishMultiUpdate);
        }
#endif
        public IDisposable BeginMultiUpdate()
        {
            Contract.Ensures(Contract.Result<IDisposable>() != null);
            return m_lock.GetLock();
        }

        public ReadOnlyObservableCollection<T> ReadOnly
        {
            get
            {
                Contract.Ensures(Contract.Result<ReadOnlyObservableCollection<T>>() != null);
                return m_roCollection;
            }
        }

        /// <remarks>It's recommended that you use this method within BeginMultiUpdate</remarks>
        public void AddRange(IEnumerable<T> source)
        {
            Contract.Requires(source != null);
            AppendItems(source);
        }

        public void Reset(IEnumerable<T> source)
        {
            using (BeginMultiUpdate())
            {
                ClearItems();
                AppendItems(source);
            }
        }

        public void Sort(Func<T, T, int> comparer)
        {
            Contract.Requires(comparer != null);
            using (this.BeginMultiUpdate())
            {
                this.QuickSort(comparer);
            }
        }

        public void Sort(IComparer<T> comparer)
        {
            Contract.Requires(comparer != null);
            using (this.BeginMultiUpdate())
            {
                this.QuickSort(comparer);
            }
        }

        public void Sort(Comparison<T> comparison)
        {
            Contract.Requires(comparison != null);
            using (this.BeginMultiUpdate())
            {
                this.QuickSort(comparison);
            }
        }

        protected bool MultiUpdateActive { get { return m_lock.IsLocked; } }

        protected virtual void AppendItems(IEnumerable<T> source)
        {
            Contract.Requires(source != null);
            using (BeginMultiUpdate())
            {
                foreach (var item in source)
                {
                    InsertItem(this.Count, item);
                }
            }
        }

        protected virtual void BeforeMultiUpdate() { }

        protected virtual void AfterMultiUpdate() { }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (m_lock.IsLocked)
            {
                m_isChanged = true;
            }
            else
            {
                base.OnPropertyChanged(e);
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (m_lock.IsLocked)
            {
                m_isChanged = true;
            }
            else
            {
                base.OnCollectionChanged(e);
            }
        }

        private void FinishMultiUpdate()
        {
            AfterMultiUpdate();
            if (m_isChanged)
            {
                raiseReset();
                m_isChanged = false;
            }
        }

        private void raiseReset()
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(m_lock != null);
            Contract.Invariant(m_roCollection != null);
        }

        private bool m_isChanged;
        private readonly WrappedLock m_lock;
        private readonly ReadOnlyObservableCollection<T> m_roCollection;
    }
}
