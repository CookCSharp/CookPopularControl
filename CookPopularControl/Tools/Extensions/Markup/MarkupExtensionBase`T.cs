using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：MarkupExtensionBase_T
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-11 14:00:53
 */
namespace CookPopularControl.Tools.Extensions.Markup
{
    public abstract class MarkupExtensionBase<T> : MarkupExtensionBase
    {
        private readonly object parameter;
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached("Value", typeof(T), typeof(MarkupExtensionBase), new PropertyMetadata(default(T)));

        protected MarkupExtensionBase(object _parameter)
        {
            this.parameter = _parameter;
        }

        /// <summary>
        /// 获取绑定目标的属性值
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected bool TryGetValue(IServiceProvider serviceProvider, out T value)
        {
            if (parameter is BindingBase binding)
            {
                if (serviceProvider is IProvideValueTarget provideValueTarget)
                {
                    if (provideValueTarget.TargetObject is DependencyObject target)
                    {
                        try
                        {
                            BindingOperations.SetBinding(target, ValueProperty, binding);
                            value = (T)target.GetValue(ValueProperty);

                            return true;
                        }
                        catch (Exception)
                        {
                            value = default(T);
                            return false;
                        }
                        finally
                        {
                            BindingOperations.ClearBinding(target, ValueProperty);
                        }
                    }
                }
            }

            value = default(T);
            return false;
        }
    }
}
