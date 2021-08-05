using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：MetadataAttribute
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-05 11:14:52
 */
namespace CookPopularControl.Tools.Attributes
{
    /// <summary>
    /// 指定为某个类型的导出提供元数据的自定义特性
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ControlMetadataAttribute : ExportAttribute
    {
        public ControlMetadataAttribute(string name) : this(name, string.Empty) { }
        public ControlMetadataAttribute(string name, string description) : base(ControlContractName, typeof(FrameworkElement))
        {
            Name = name;
            Description = description;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }

        public const string ControlContractName = "CookPopularControl";
    }
}
