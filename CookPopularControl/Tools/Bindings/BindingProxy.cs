using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：BindingProxy
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-28 10:02:29
 */
namespace CookPopularControl.Tools.Bindings
{
    /// <summary>
    /// 绑定代理
    /// </summary>
    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore() => new BindingProxy();

        public object? Data
        {
            get { return (object?)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(default(object)));
    }
}
