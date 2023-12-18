using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：VisualTreeHelperExtension
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-12 14:51:17
 */
namespace CookPopularCSharpToolkit.Windows
{
    public static class VisualTreeHelperExtension
    {
        /// <summary>
        /// Get the visual tree ancestors of an element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The visual tree ancestors of the element.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        public static IEnumerable<DependencyObject> GetVisualAncestors(this DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return GetVisualAncestorsAndSelfIterator(element).Skip(1);
        }

        /// <summary>
        /// Get the visual tree ancestors of an element and the element itself.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The visual tree ancestors of an element and the element itself.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        public static IEnumerable<DependencyObject> GetVisualAncestorsAndSelf(this DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return GetVisualAncestorsAndSelfIterator(element);
        }

        /// <summary>
        /// Get the visual tree ancestors of an element and the element itself.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The visual tree ancestors of an element and the element itself.
        /// </returns>
        private static IEnumerable<DependencyObject> GetVisualAncestorsAndSelfIterator(DependencyObject element)
        {
            Debug.Assert(element != null, "element should not be null!");

            for (DependencyObject obj = element!; obj != null; obj = VisualTreeHelper.GetParent(obj))
            {
                yield return obj;
            }
        }

        /// <summary>
        /// Get the visual tree children of an element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The visual tree children of an element.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        public static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return GetVisualChildrenAndSelfIterator(element).Skip(1);
        }

        /// <summary>
        /// Get the visual tree children of an element and the element itself.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The visual tree children of an element and the element itself.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        public static IEnumerable<DependencyObject> GetVisualChildrenAndSelf(this DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return GetVisualChildrenAndSelfIterator(element);
        }

        /// <summary>
        /// Get the visual tree children of an element and the element itself.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The visual tree children of an element and the element itself.
        /// </returns>
        private static IEnumerable<DependencyObject> GetVisualChildrenAndSelfIterator(this DependencyObject element)
        {
            Debug.Assert(element != null, "element should not be null!");

            yield return element;

            int count = VisualTreeHelper.GetChildrenCount(element);
            for (int i = 0; i < count; i++)
            {
                yield return VisualTreeHelper.GetChild(element, i);
            }
        }

        /// <summary>
        /// Get the visual tree descendants of an element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The visual tree descendants of an element.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        public static IEnumerable<DependencyObject> GetVisualDescendants(this DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return GetVisualDescendantsAndSelfIterator(element).Skip(1);
        }

        /// <summary>
        /// Get the visual tree descendants of an element and the element
        /// itself.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The visual tree descendants of an element and the element itself.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        public static IEnumerable<DependencyObject> GetVisualDescendantsAndSelf(this DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return GetVisualDescendantsAndSelfIterator(element);
        }

        /// <summary>
        /// Get the visual tree descendants of an element and the element
        /// itself.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The visual tree descendants of an element and the element itself.
        /// </returns>
        private static IEnumerable<DependencyObject> GetVisualDescendantsAndSelfIterator(DependencyObject element)
        {
            Debug.Assert(element != null, "element should not be null!");

            Queue<DependencyObject> remaining = new Queue<DependencyObject>();
            remaining.Enqueue(element!);

            while (remaining.Count > 0)
            {
                DependencyObject obj = remaining.Dequeue();
                yield return obj;

                foreach (DependencyObject child in obj.GetVisualChildren())
                {
                    remaining.Enqueue(child);
                }
            }
        }

        /// <summary>
        /// Get the visual tree siblings of an element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The visual tree siblings of an element.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        public static IEnumerable<DependencyObject> GetVisualSiblings(this DependencyObject element)
        {
            return element.GetVisualSiblingsAndSelf().Where(p => p != element);
        }

        /// <summary>
        /// Get the visual tree siblings of an element and the element itself.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The visual tree siblings of an element and the element itself.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        public static IEnumerable<DependencyObject> GetVisualSiblingsAndSelf(this DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            DependencyObject parent = VisualTreeHelper.GetParent(element);
            return parent == null ? Enumerable.Empty<DependencyObject>() : parent.GetVisualChildren();
        }




        /// <summary>
        /// Get the bounds of an element relative to another element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="otherElement">
        /// The element relative to the other element.
        /// </param>
        /// <returns>
        /// The bounds of the element relative to another element, or null if
        /// the elements are not related.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="otherElement"/> is null.
        /// </exception>
        public static Rect? GetBoundsRelativeTo(this FrameworkElement element, UIElement otherElement)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            else if (otherElement == null)
            {
                throw new ArgumentNullException("otherElement");
            }

            try
            {
#pragma warning disable IDE0018 // 内联变量声明
                Point origin, bottom;
#pragma warning restore IDE0018 // 内联变量声明
                GeneralTransform transform = element.TransformToVisual(otherElement);
                if (transform != null &&
                    transform.TryTransform(new Point(), out origin) &&
                    transform.TryTransform(new Point(element.ActualWidth, element.ActualHeight), out bottom))
                {
                    return new Rect(origin, bottom);
                }
            }
            catch (ArgumentException)
            {
                // Ignore any exceptions thrown while trying to transform
            }

            return null;
        }

        /// <summary>
        /// Perform an action when the element's LayoutUpdated event fires.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="action">The action to perform.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="element"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="action"/> is null.
        /// </exception>
        public static void InvokeOnLayoutUpdated(this FrameworkElement element, Action action)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            else if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            // Create an event handler that unhooks itself before calling the
            // action and then attach it to the LayoutUpdated event.
#pragma warning disable IDE0039 // 使用本地函数
            EventHandler? handler = null;
            handler = (s, e) =>
            {
                element.LayoutUpdated -= handler;
                action();
            };
            element.LayoutUpdated += handler;
#pragma warning restore IDE0039 // 使用本地函数
        }

        /// <summary>
        /// Retrieves all the logical children of a framework element using a
        /// breadth-first search. For performance reasons this method manually
        /// manages the stack instead of using recursion.
        /// </summary>
        /// <param name="parent">The parent framework element.</param>
        /// <returns>The logical children of the framework element.</returns>
        internal static IEnumerable<FrameworkElement> GetLogicalChildren(this FrameworkElement parent)
        {
            Debug.Assert(parent != null, "The parent cannot be null.");

            Popup? popup = parent as Popup;
            if (popup != null)
            {
                FrameworkElement? popupChild = popup.Child as FrameworkElement;
                if (popupChild != null)
                {
                    yield return popupChild;
                }
            }

            // If control is an items control return all children using the
            // Item container generator.
            ItemsControl? itemsControl = parent as ItemsControl;
            if (itemsControl != null)
            {
                foreach (FrameworkElement logicalChild in Enumerable.Range(0, itemsControl.Items.Count)
                                                                    .Select(index => itemsControl.ItemContainerGenerator.ContainerFromIndex(index))
                                                                    .OfType<FrameworkElement>())
                {
                    yield return logicalChild;
                }
            }

            string parentName = parent.Name;
            Queue<FrameworkElement> queue = new Queue<FrameworkElement>(parent.GetVisualChildren().OfType<FrameworkElement>());

            while (queue.Count > 0)
            {
                FrameworkElement element = queue.Dequeue();
                if (element.Parent == parent || element is UserControl)
                {
                    yield return element;
                }
                else
                {
                    foreach (FrameworkElement visualChild in element.GetVisualChildren().OfType<FrameworkElement>())
                    {
                        queue.Enqueue(visualChild);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves all the logical descendents of a framework element using a
        /// breadth-first search. For performance reasons this method manually
        /// manages the stack instead of using recursion.
        /// </summary>
        /// <param name="parent">The parent framework element.</param>
        /// <returns>The logical children of the framework element.</returns>
        internal static IEnumerable<FrameworkElement> GetLogicalDescendents(this FrameworkElement parent)
        {
            Debug.Assert(parent != null, "The parent cannot be null.");

            return FunctionalProgramming.TraverseBreadthFirst(parent!, node => node.GetLogicalChildren(), node => true);
        }
    }


    /// <summary>
    /// Collection of functions for functional programming tasks.
    /// </summary>
    public static class FunctionalProgramming
    {
        /// <summary>
        /// Traverses a tree by accepting an initial value and a function that
        /// retrieves the child nodes of a node.
        /// </summary>
        /// <typeparam name="T">The type of the stream.</typeparam>
        /// <param name="initialNode">The initial node.</param>
        /// <param name="getChildNodes">A function that retrieves the child
        /// nodes of a node.</param>
        /// <param name="traversePredicate">A predicate that evaluates a node
        /// and returns a value indicating whether that node and it's children
        /// should be traversed.</param>
        /// <returns>A stream of nodes.</returns>
        internal static IEnumerable<T> TraverseBreadthFirst<T>(T initialNode, Func<T, IEnumerable<T>> getChildNodes, Func<T, bool> traversePredicate)
        {
            Queue<T> queue = new Queue<T>();
            queue.Enqueue(initialNode);

            while (queue.Count > 0)
            {
                T node = queue.Dequeue();
                if (traversePredicate(node))
                {
                    yield return node;
                    IEnumerable<T> childNodes = getChildNodes(node);
                    foreach (T childNode in childNodes)
                    {
                        queue.Enqueue(childNode);
                    }
                }
            }
        }
    }
}
