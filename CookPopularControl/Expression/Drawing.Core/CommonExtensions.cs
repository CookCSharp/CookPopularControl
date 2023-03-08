using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CommonExtensions
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:41:52
 */
namespace CookPopularControl.Expression
{
    /// <summary>
    /// Extension methods that support non-geometry types.
    /// </summary>
    public static class CommonExtensions
    {
        /// <summary>
        /// Allows the application of an action delegate (often a very simple lambda) against an entire sequence.
        /// </summary>
        public static void ForEach(this IEnumerable items, Action<object> action)
        {
            foreach (object obj in items)
            {
                action(obj);
            }
        }

        /// <summary>
        /// Allows the application of an action delegate (often a very simple lambda) against an entire sequence.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T obj in items)
            {
                action(obj);
            }
        }

        /// <summary>
        /// Allows the application of an action delegate (often a very simple lambda) against an entire sequence with the index of each item.
        /// </summary>
        public static void ForEach<T>(this IList<T> list, Action<T, int> action)
        {
            for (int i = 0; i < list.Count; i++)
            {
                action(list[i], i);
            }
        }

        /// <summary>
        /// Ensures the count of a list to a given count. Creates with a given factory or removes items when necessary.
        /// If Input IList is a List, AddRange or RemoveRange is used when there's no factory.
        /// </summary>
        public static bool EnsureListCount<T>(this IList<T> list, int count, Func<T> factory = null)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (list.EnsureListCountAtLeast(count, factory))
            {
                return true;
            }
            if (list.Count > count)
            {
                List<T>? list2 = list as List<T>;
                if (list2 != null)
                {
                    list2.RemoveRange(count, list.Count - count);
                }
                else
                {
                    for (int i = list.Count - 1; i >= count; i--)
                    {
                        list.RemoveAt(i);
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ensures the count of a list is at least the given count. Creates with a given factory.
        /// </summary>
        public static bool EnsureListCountAtLeast<T>(this IList<T> list, int count, Func<T> factory = null)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (list.Count < count)
            {
                List<T>? list2 = list as List<T>;
                if (list2 != null && factory == null)
                {
                    list2.AddRange(new T[count - list.Count]);
                }
                else
                {
                    for (int i = list.Count; i < count; i++)
                    {
                        list.Add((factory == null) ? default(T) : factory());
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add a range of items to the end of a collection.
        /// If a collection is a list, List.AddRange is used.
        /// </summary>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> newItems)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            List<T>? list = collection as List<T>;
            if (list != null)
            {
                list.AddRange(newItems);
                return;
            }
            foreach (T item in newItems)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Gets the last item of a given list.
        /// </summary>
        public static T Last<T>(this IList<T> list)
        {
            return list[list.Count - 1];
        }

        /// <summary>
        /// Removes the last item from the given list.
        /// </summary>
        public static void RemoveLast<T>(this IList<T> list)
        {
            list.RemoveAt(list.Count - 1);
        }

        /// <summary>
        /// Makes a copy of obj and all it's public properties, including all collection properties.
        /// </summary>
        public static T DeepCopy<T>(this T obj) where T : class
        {
            if (obj == null)
            {
                return default(T);
            }
            Type type = obj.GetType();
            if (type.IsValueType || type == typeof(string))
            {
                return obj;
            }
            if (typeof(IList).IsAssignableFrom(type))
            {
                IList collection = (IList)Activator.CreateInstance(type);
                ((IList)obj).ForEach(delegate (object o)
                {
                    collection.Add(o.DeepCopy<object>());
                });
                return (T)collection;
            }
            if (type.IsClass)
            {
                object obj2 = Activator.CreateInstance(obj.GetType());
                PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo propertyInfo in properties)
                {
                    if (propertyInfo.CanRead && propertyInfo.CanWrite)
                    {
                        object value = propertyInfo.GetValue(obj, null);
                        object value2 = propertyInfo.GetValue(obj2, null);
                        if (value != value2)
                        {
                            propertyInfo.SetValue(obj2, value.DeepCopy<object>(), null);
                        }
                    }
                }
                return (T)obj2;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the value if different. Avoids setting a local value if possible.
        /// Returns true when the value has been changed.
        /// </summary>
        public static bool SetIfDifferent(this DependencyObject dependencyObject, DependencyProperty dependencyProperty, object value)
        {
            object value2 = dependencyObject.GetValue(dependencyProperty);
            if (!object.Equals(value2, value))
            {
                dependencyObject.SetValue(dependencyProperty, value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clears the dependency property when it is locally set on the given dependency object.
        /// Returns false if the dependeny property is not locally set.
        /// </summary>
        public static bool ClearIfSet(this DependencyObject dependencyObject, DependencyProperty dependencyProperty)
        {
            object obj = dependencyObject.ReadLocalValue(dependencyProperty);
            if (obj != DependencyProperty.UnsetValue)
            {
                dependencyObject.ClearValue(dependencyProperty);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Finds all visual descendants of a given type and condition using breadth-first search.
        /// </summary>
        public static IEnumerable<T> FindVisualDesendent<T>(this DependencyObject parent, Func<T, bool> condition) where T : DependencyObject
        {
            Queue<DependencyObject> queue = new Queue<DependencyObject>();
            parent.GetVisualChildren().ForEach(delegate (DependencyObject child)
            {
                queue.Enqueue(child);
            });
            while (queue.Count > 0)
            {
                DependencyObject next = queue.Dequeue();
                next.GetVisualChildren().ForEach(delegate (DependencyObject child)
                {
                    queue.Enqueue(child);
                });
                T? candidate = next as T;
                if (candidate != null && condition(candidate))
                {
                    yield return candidate;
                }
            }
            yield break;
        }

        /// <summary>
        /// Gets all visual children in IEnumerable.
        /// </summary>
        public static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject parent)
        {
            int N = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < N; i++)
            {
                yield return VisualTreeHelper.GetChild(parent, i);
            }
            yield break;
        }
    }
}
