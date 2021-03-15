using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：MarkupExtenisonBase
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-11 13:51:52
 */
namespace CookPopularControl.Tools.Extensions.Markup
{
    /// <summary>
    /// 为可以由 .NET Framework XAML 服务及其他 XAML 读取器和 XAML 编写器支持的 XAML 标记扩展实现提供基类
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
