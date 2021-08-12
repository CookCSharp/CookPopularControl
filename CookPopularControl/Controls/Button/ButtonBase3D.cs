using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ButtonBase3D
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-10 14:44:41
 */
namespace CookPopularControl.Controls.Button
{
    public class ButtonBase3D : UIElement3D
    {
        public static readonly RoutedEvent ClickEvent = ButtonBase.ClickEvent.AddOwner(typeof(ButtonBase3D));

        public event RoutedEventHandler Click
        {
            add { base.AddHandler(ClickEvent, value); }
            remove { base.AddHandler(ClickEvent, value); }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            OnClick();
            e.Handled = true;
        }

        private void OnClick()
        {
            RoutedEventArgs args = new RoutedEventArgs(ClickEvent,this);
            base.RaiseEvent(args);
        }
    }
}
