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
    /// <remarks>
    /// 适用于控件自身是视觉树的根节点,即使通过RelativeSource.FindAncestor也找不到要传递的参数，例如ContextMenu
    /// 传递参数时:可以通过PlacementTarget解决。微软对PlacementTarget的解释是：获取或设置UIElement，当它打开时相对于它确定ContextMenu的位置
    /// <![CDATA[CommandParameter="{Binding RelativeSource={RelativeSource Mode=FindAncestor, AncestorType=ContextMenu}, Path=PlacementTarget}"]]>
    /// <![CDATA[CommandParameter="{Binding RelativeSource={RelativeSource Mode=FindAncestor, AncestorType=ContextMenu}, Path=PlacementTarget.SelectedItem}" ]]>
    /// </remarks>
    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore() => new BindingProxy();

        public object? Data
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(default(object)));
    }
}
