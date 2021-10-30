using Microsoft.Xaml.Behaviors.Layout;
using System.Windows;
using System.Windows.Documents;
using OriginAdorner = System.Windows.Documents.Adorner;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：AdornerBase
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-12 11:13:11
 */
namespace CookPopularControl.Controls.Adorner
{
    /// <summary>
    /// 将装饰器绑定到元素的基类
    /// </summary>
    public class AdornerBase : OriginAdorner
    {
        public AdornerBase(UIElement adornerElement) : base(adornerElement)
        {

        }

        public void AddAdornerElement()
        {
            var layer = AdornerLayer.GetAdornerLayer(AdornedElement);
            layer?.Add(new AdornerContainer(AdornedElement));
        }
    }
}
