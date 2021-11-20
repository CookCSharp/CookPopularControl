/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：IManualInterTabClient
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 17:50:32
 */
namespace CookPopularControl.Controls.Dragables
{
    public interface IManualInterTabClient : IInterTabClient
    {
        void Add(object item);
        void Remove(object item);
    }
}
