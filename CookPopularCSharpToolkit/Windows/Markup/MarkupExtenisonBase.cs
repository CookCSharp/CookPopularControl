using System;
using System.Windows.Markup;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：MarkupExtenisonBase
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-11 13:51:52
 */
namespace CookPopularCSharpToolkit.Windows
{
    /// <summary>
    /// 为可以由.NET Framework XAML服务及其他XAML读取器和XAML编写器支持的XAML标记扩展实现提供基类
    /// </summary>
    public abstract class MarkupExtensionBase : MarkupExtension
    {
        protected MarkupExtensionBase()
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
