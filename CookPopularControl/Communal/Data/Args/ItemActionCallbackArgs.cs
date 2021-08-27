using CookPopularControl.Controls.Dragables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ItemActionCallbackArgs
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 15:52:46
 */
namespace CookPopularControl.Communal.Data
{
    public delegate void ItemActionCallback(ItemActionCallbackArgs<DragableTabControl> args);

    public class ItemActionCallbackArgs<TOwner> where TOwner : FrameworkElement
    {
        private readonly Window _window;
        private readonly TOwner _owner;
        private readonly DragableItem _dragableItem;

        public ItemActionCallbackArgs(Window window, TOwner owner, DragableItem dragableItem)
        {
            if (window == null) throw new ArgumentNullException("window");
            if (owner == null) throw new ArgumentNullException("owner");
            if (dragableItem == null) throw new ArgumentNullException("dragablzItem");

            _window = window;
            _owner = owner;
            _dragableItem = dragableItem;
        }

        public Window Window
        {
            get { return _window; }
        }

        public TOwner Owner
        {
            get { return _owner; }
        }

        public DragableItem DragablzItem
        {
            get { return _dragableItem; }
        }

        public bool IsCancelled { get; private set; }

        public void Cancel()
        {
            IsCancelled = true;
        }
    }
}
