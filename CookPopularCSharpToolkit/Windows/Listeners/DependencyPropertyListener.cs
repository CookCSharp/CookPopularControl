using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Description：DependencyPropertyListener 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2021-11-25 11:17:35
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2021 All Rights Reserved.
 */
namespace CookPopularCSharpToolkit.Windows
{
    /// <summary>
    /// 通过<see cref="DependencyPropertyDescriptor"/>监听依赖属性
    /// </summary>
    public class DependencyPropertyListener
    {
        private static readonly object _instanceLock = new object();

        private static DependencyPropertyListener _instance;
        public static DependencyPropertyListener Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DependencyPropertyListener();
                        }
                    }
                }

                return _instance;
            }
        }

        private DependencyPropertyListener()
        {

        }


        public void MonitorDependencyProperty(DependencyProperty dependencyProperty, Type targetType, Delegate @delegate)
        {
            DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(dependencyProperty, targetType);
            descriptor.AddValueChanged(this, (s, e) => @delegate.DynamicInvoke());
        }
    }
}
