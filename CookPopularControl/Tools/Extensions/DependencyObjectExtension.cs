using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DependencyObjectExtension
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-28 14:37:13
 */
namespace CookPopularControl.Tools.Extensions
{
    public static class DependencyObjectExtension
    {
        //.Net通过"NotifyPropertyChange"主动触发通知
        //当子属性发生变化时，发送一个Changed通知，新旧值相同，并将IsASubPropertyChange设置为true
        //NotifyPropertyChange(new DependencyPropertyChangedEventArgs(dp, dp.GetMetadata(DependencyObjectType), GetValue(dp)));
        /// <summary>
        /// 调用示例：
        ///<![CDATA[InvokeInternal<DependencyObject>("NotifySubPropertyChange", new object[] { DpColorProperty });]]>
        /// </summary>

        /// <summary>
        /// 反射调用指定类型的Internal方法。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="caller"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object InvokeInternal<T>(this T caller, string method, object[] parameters)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic);
            return methodInfo?.Invoke(caller, parameters); 
        }

        public static IEnumerable<DependencyObject> VisualDepthFirstTraversal(this DependencyObject node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));

            yield return node;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(node); i++)
            {
                var child = VisualTreeHelper.GetChild(node, i);
                foreach (var descendant in child.VisualDepthFirstTraversal())
                {
                    yield return descendant;
                }
            }
        }

        public static IEnumerable<DependencyObject> VisualBreadthFirstTraversal(this DependencyObject node)
        {
            if (node is null) throw new ArgumentNullException(nameof(node));

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(node); i++)
            {
                var child = VisualTreeHelper.GetChild(node, i);
                yield return child;
            }

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(node); i++)
            {
                var child = VisualTreeHelper.GetChild(node, i);

                foreach (var descendant in child.VisualDepthFirstTraversal())
                {
                    yield return descendant;
                }
            }
        }

        public static bool IsAncestorOf(this DependencyObject parent, DependencyObject? node)
            => node != null && parent.VisualDepthFirstTraversal().Contains(node);


        public static IEnumerable<DependencyObject> GetVisualAncestry(this DependencyObject? leaf)
        {
            while (leaf is not null)
            {
                yield return leaf;
                leaf = leaf is Visual || leaf is Visual3D
                    ? VisualTreeHelper.GetParent(leaf)
                    : LogicalTreeHelper.GetParent(leaf);
            }
        }

        public static IEnumerable<DependencyObject?> GetLogicalAncestry(this DependencyObject leaf)
        {
            while (leaf is not null)
            {
                yield return leaf;
                leaf = LogicalTreeHelper.GetParent(leaf);
            }
        }

        public static bool IsDescendantOf(this DependencyObject? leaf, DependencyObject? ancestor)
        {
            DependencyObject? parent = null;
            foreach (var node in leaf.GetVisualAncestry())
            {
                if (Equals(node, ancestor))
                    return true;

                parent = node;
            }

            return parent?.GetLogicalAncestry().Contains(ancestor) == true;
        }
    }
}
