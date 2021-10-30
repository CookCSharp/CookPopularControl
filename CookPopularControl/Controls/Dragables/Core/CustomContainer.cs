using System;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CustomContainer
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 11:51:45
 */
namespace CookPopularControl.Controls.Dragables.Core
{
    internal class CustomContainer
    {
        private readonly Func<DragableItem> _getContainerForItemOverride;
        private readonly Action<DependencyObject, object> _prepareContainerForItemOverride;
        private readonly Action<DependencyObject, object> _clearingContainerForItemOverride;

        public CustomContainer(Func<DragableItem> getContainerForItemOverride = null, Action<DependencyObject, object> prepareContainerForItemOverride = null, Action<DependencyObject, object> clearingContainerForItemOverride = null)
        {
            _getContainerForItemOverride = getContainerForItemOverride;
            _prepareContainerForItemOverride = prepareContainerForItemOverride;
            _clearingContainerForItemOverride = clearingContainerForItemOverride;
        }

        public Func<DragableItem> GetContainerForItemOverride
        {
            get { return _getContainerForItemOverride; }
        }

        public Action<DependencyObject, object> PrepareContainerForItemOverride
        {
            get { return _prepareContainerForItemOverride; }
        }

        public Action<DependencyObject, object> ClearingContainerForItemOverride
        {
            get { return _clearingContainerForItemOverride; }
        }
    }
}
