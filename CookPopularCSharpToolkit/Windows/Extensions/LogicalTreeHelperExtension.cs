using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：LogicalTreeHelperExtension
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 16:15:14
 */
namespace CookPopularCSharpToolkit.Windows
{
    public static class LogicalTreeHelperExtension
    {
        public static IEnumerable<object> LogicalTreeDepthFirstTraversal(this DependencyObject node)
        {
            if (node == null) yield break;
            yield return node;

            foreach (var child in LogicalTreeHelper.GetChildren(node).OfType<DependencyObject>()
                .SelectMany(depObj => depObj.LogicalTreeDepthFirstTraversal()))
                yield return child;
        }

        /// <summary>
        /// Yields the logical ancestory (including the starting point).
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        public static IEnumerable<DependencyObject> LogicalTreeAncestory(this DependencyObject dependencyObject)
        {
            if (dependencyObject == null) throw new ArgumentNullException("dependencyObject");

            while (dependencyObject != null)
            {
                yield return dependencyObject;
                dependencyObject = LogicalTreeHelper.GetParent(dependencyObject);
            }
        }
    }
}
