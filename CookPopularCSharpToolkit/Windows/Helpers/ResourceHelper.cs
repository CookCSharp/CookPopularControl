using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ResourceHelper
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-13 20:24:26
 */
namespace CookPopularCSharpToolkit.Windows
{
    public class ResourceHelper
    {
        /// <summary>
        /// 根据Key获取资源
        /// </summary>
        /// <typeparam name="TResource"></typeparam>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public static TResource GetResource<TResource>(string resourceKey)
        {
            if (Application.Current.TryFindResource(resourceKey) is TResource resource)
                return resource;

            return default(TResource);
        }
    }
}
