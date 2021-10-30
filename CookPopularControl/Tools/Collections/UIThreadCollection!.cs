using CookPopularControl.Communal.ViewModel;
using CookPopularControl.Tools.Extensions;
using CookPopularControl.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Windows.Input;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：UIThreadCollection
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-05 11:44:54
 */
namespace CookPopularControl.Tools.Collections
{
    /// <summary>
    /// 仅通过一个线程来确保此类成员的线程安全,任何跨线程变化会导致不期望的行为
    /// </summary>
    public class UIThreadCollection<T> : ReadOnlyObservableCollection<T>
    {
        public static UIThreadCollection<T> Create(IList<T> source, int initialCount, int minCount, int maxCount)
        {
            Contract.Requires(source != null);
            Contract.Requires(source.Count > 0);
            Contract.Requires(initialCount >= 0);
            Contract.Requires(minCount >= 0);
            Contract.Requires(minCount <= initialCount);
            Contract.Requires(initialCount <= maxCount);

            var sourceItems = source.ToReadOnlyCollection();

            var observableCollection = new ObservableCollectionPlus<T>();

            for (int i = 0; i < initialCount; i++)
            {
                observableCollection.Add(sourceItems[i % sourceItems.Count]);
            }

            return new UIThreadCollection<T>(sourceItems, observableCollection, minCount, maxCount, initialCount);
        }

        public void Add()
        {
            if (CanAddOrInsert)
            {
                m_activeItems.Add(m_sourceItems.Random());
            }
        }

        public void Remove()
        {
            if (CanRemove)
            {
                int targetIndex = m_random.Next(m_activeItems.Count);

                m_activeItems.RemoveAt(targetIndex);
            }
        }

        public void Move()
        {
            if (CanMove)
            {
                int startIndex = m_random.Next(m_activeItems.Count);
                int endIndex;
                do
                {
                    endIndex = m_random.Next(m_activeItems.Count);
                }
                while (endIndex == startIndex);

                m_activeItems.Move(startIndex, endIndex);
            }
        }

        public void Change()
        {
            if (CanChange)
            {
                int targetIndex = m_random.Next(m_activeItems.Count);

                m_activeItems[targetIndex] = m_sourceItems.Random();
            }
        }

        public void Insert()
        {
            if (CanAddOrInsert)
            {
                int targetIndex = m_random.Next(m_activeItems.Count + 1);

                m_activeItems.Insert(targetIndex, m_sourceItems.Random());
            }
        }

        public void Reset()
        {
            // This is essential. '==' doesn't work against an arbitrary 'T'
            // And item.Equals will blow up if any item is null
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;

            for (int i = 0; i < m_initialCount; i++)
            {
                int j = i % m_sourceItems.Count;

                if (i < m_activeItems.Count) // just re-shuffle
                {
                    if (comparer.Equals(m_activeItems[i], m_sourceItems[j]))
                    {
                        // This item is already what it should be
                    }
                    else // Go look for an active item that matches
                    {
                        bool found = false;
                        for (int k = i + 1; k < m_activeItems.Count; k++)
                        {
                            if (comparer.Equals(m_activeItems[k], m_sourceItems[j]))
                            {
                                m_activeItems.Move(k, i);
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            m_activeItems[i] = m_sourceItems[j];
                        }
                    }
                }
                else
                {
                    // need to add new ones
                    m_activeItems.Add(m_sourceItems[j]);
                }
            }

            while (m_activeItems.Count > m_initialCount)
            {
                // need to remove extras
                m_activeItems.RemoveAt(m_activeItems.Count - 1);
            }
        }

        public ICommand AddCommand { get { return m_addCommand; } }
        public ICommand RemoveCommand { get { return m_removeCommand; } }
        public ICommand MoveCommand { get { return m_moveCommand; } }
        public ICommand ChangeCommand { get { return m_changeCommand; } }
        public ICommand InsertCommand { get { return m_insertCommand; } }
        public ICommand ResetCommand { get { return m_resetCommand; } }


        private UIThreadCollection(ReadOnlyCollection<T> sourceItems, ObservableCollectionPlus<T> activeItems, int minCount, int maxCount, int initialCount) : base(activeItems)
        {
            Contract.Requires(activeItems != null);
            Contract.Requires(sourceItems != null);
            Contract.Requires(sourceItems.Count > 0);

            m_minCount = minCount;
            m_maxCount = maxCount;
            m_initialCount = initialCount;

            m_activeItems = activeItems;
            m_sourceItems = sourceItems;

            m_addCommand = new DelegateCommand(Add, () => CanAddOrInsert);
            m_insertCommand = new DelegateCommand(Insert, () => CanAddOrInsert);
            m_removeCommand = new DelegateCommand(Remove, () => CanRemove);
            m_moveCommand = new DelegateCommand(Move, () => CanMove);
            m_changeCommand = new DelegateCommand(Change, () => CanChange);
            m_resetCommand = new DelegateCommand(Reset);

            activeItems.CollectionChanged += (sender, e) =>
            {
                m_removeCommand.CanExecute(null);
                m_addCommand.CanExecute(null);
                m_moveCommand.CanExecute(null);
                m_changeCommand.CanExecute(null);
                m_insertCommand.CanExecute(null);
            };
        }

        private bool CanAddOrInsert { get { return m_activeItems.Count < m_maxCount; } }

        private bool CanRemove { get { return m_activeItems.Count > m_minCount; } }

        private bool CanMove { get { return m_activeItems.Count > 1; } }

        private bool CanChange { get { return m_activeItems.Count > 0; } }

        [ContractInvariantMethod]
        void ObjectInvariant()
        {
            Contract.Invariant(m_random != null);
            Contract.Invariant(m_sourceItems != null);
            Contract.Invariant(m_sourceItems.Count > 0);
            Contract.Invariant(m_activeItems != null);
            Contract.Invariant(m_addCommand != null);
            Contract.Invariant(m_removeCommand != null);
            Contract.Invariant(m_moveCommand != null);
            Contract.Invariant(m_changeCommand != null);
            Contract.Invariant(m_resetCommand != null);
            Contract.Invariant(m_insertCommand != null);
        }

        private readonly DelegateCommand m_addCommand, m_removeCommand, m_moveCommand, m_changeCommand, m_insertCommand, m_resetCommand;
        private readonly int m_minCount, m_maxCount, m_initialCount;
        private readonly Random m_random = RandomHelper.Rnd;
        private readonly ObservableCollectionPlus<T> m_activeItems;
        private readonly ReadOnlyCollection<T> m_sourceItems;
    }
}
