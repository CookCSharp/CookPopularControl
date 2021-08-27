using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ItemsControlExtension
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 14:29:18
 */
namespace CookPopularControl.Tools.Extensions.Controls
{
    public static class ItemsControlExtension
    {
        public static IEnumerable<TContainer> Containers<TContainer>(this ItemsControl itemsControl) where TContainer : class
        {
#if NET40
            var fieldInfo = typeof(ItemContainerGenerator).GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
            var list = (IList)fieldInfo.GetValue(itemsControl.ItemContainerGenerator);            
            for (var i = 0; i < list.Count; i++)
#else
            for (var i = 0; i < itemsControl.ItemContainerGenerator.Items.Count; i++)
#endif
            {
                var container = itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as TContainer;
                if (container != null)
                    yield return container;
            }
        }
    }
}
