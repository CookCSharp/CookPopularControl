using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;



/*
 * Description：CollectionDeepObserver<T>
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2021-12-11 21:18:13
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2021 All Rights Reserved.
 */
namespace CookPopularCSharpToolkit.Communal
{
    /// <summary>
    /// A helper class that tracks both, <see cref="INotifyCollectionChanged.CollectionChanged"/> and 
    /// <see cref="INotifyPropertyChanged.PropertyChanged"/> events.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionDeepObserver<T>
    {
        private readonly NotifyCollectionChangedEventHandler _onCollectionChanged;
        private readonly PropertyChangedEventHandler _onItemPropertyChanged;
        private readonly HashSet<INotifyPropertyChanged> _itemsListening = new();

        /// <summary>
        /// The check i notify property changed
        /// </summary>
        protected bool checkINotifyPropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionDeepObserver{T}"/> class.
        /// </summary>
        /// <param name="onCollectionChanged">The on collection changed handler.</param>
        /// <param name="onItemPropertyChanged">The on item property changed handler.</param>
        /// <param name="checkINotifyPropertyChanged">if true, it will force the check to verify if the type {T} implements INotifyPropertyChanged.</param>
        public CollectionDeepObserver(
            NotifyCollectionChangedEventHandler onCollectionChanged,
            PropertyChangedEventHandler onItemPropertyChanged,
            bool? checkINotifyPropertyChanged = null)
        {
            _onCollectionChanged = onCollectionChanged;
            _onItemPropertyChanged = onItemPropertyChanged;

            if (checkINotifyPropertyChanged is not null)
            {
                this.checkINotifyPropertyChanged = checkINotifyPropertyChanged.Value;
                return;
            }

            this.checkINotifyPropertyChanged = typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(T));
        }

        /// <summary>
        /// Initializes the listeners.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public void Initialize(IEnumerable<T>? instance)
        {
            if (instance is null) return;

            if (instance is INotifyCollectionChanged incc)
            {
                incc.CollectionChanged += OnCollectionChanged;
            }

            if (checkINotifyPropertyChanged)
                foreach (var item in GetINotifyPropertyChangedItems(instance))
                    item.PropertyChanged += _onItemPropertyChanged;
        }

        /// <summary>
        /// Disposes the listeners.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public void Dispose(IEnumerable<T>? instance)
        {
            if (instance is null) return;

            if (instance is INotifyCollectionChanged incc)
            {
                incc.CollectionChanged -= OnCollectionChanged;
            }

            if (checkINotifyPropertyChanged)
                foreach (var item in GetINotifyPropertyChangedItems(instance))
                    item.PropertyChanged -= _onItemPropertyChanged;
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (checkINotifyPropertyChanged)
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var item in GetINotifyPropertyChangedItems(e.NewItems))
                        {
                            item.PropertyChanged += _onItemPropertyChanged;
                            _ = _itemsListening.Add(item);
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var item in GetINotifyPropertyChangedItems(e.OldItems))
                        {
                            item.PropertyChanged -= _onItemPropertyChanged;
                            _ = _itemsListening.Remove(item);
                        }
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        foreach (var item in GetINotifyPropertyChangedItems(e.NewItems))
                        {
                            item.PropertyChanged += _onItemPropertyChanged;
                            _ = _itemsListening.Add(item);
                        }
                        foreach (var item in GetINotifyPropertyChangedItems(e.OldItems))
                        {
                            item.PropertyChanged -= _onItemPropertyChanged;
                            _ = _itemsListening.Remove(item);
                        }
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        foreach (var item in _itemsListening)
                        {
                            item.PropertyChanged -= _onItemPropertyChanged;
                        }
                        _itemsListening.Clear();
                        if (sender is not IEnumerable<T> s) break;
                        foreach (var item in GetINotifyPropertyChangedItems(s))
                        {
                            item.PropertyChanged += _onItemPropertyChanged;
                            _ = _itemsListening.Remove(item);
                        }
                        break;
                    case NotifyCollectionChangedAction.Move:
                        // ignored.
                        break;
                    default:
                        break;
                }

            _onCollectionChanged(sender, e);
        }

        private static IEnumerable<INotifyPropertyChanged> GetINotifyPropertyChangedItems(IEnumerable? source)
        {
            if (source is null) yield break;

            foreach (var item in source)
            {
                if (item is not INotifyPropertyChanged inpc) continue;
                yield return inpc;
            }
        }
    }
}
