using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;



/*
 * Description：ElementClickHelper 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-01-24 14:05:57
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */
namespace CookPopularCSharpToolkit.Windows
{
    public static class ElementClickHelper
    {
        private static void SetInputInfo(DependencyObject element, ElementInfo value) => element.SetValue(InputInfoProperty, value);
        private static ElementInfo GetInputInfo(DependencyObject element) => (ElementInfo)element.GetValue(InputInfoProperty);
        private static readonly DependencyProperty InputInfoProperty = DependencyProperty.RegisterAttached("InputInfo", typeof(ElementInfo), typeof(ElementClickHelper), new PropertyMetadata(default(ElementInfo)));


        /// <summary>
        /// 将 MouseDown MouseMove MouseUp 封装为点击事件
        /// </summary>
        /// <param name="element">要被附加的元素</param>
        /// <param name="clickEventHandler">点击的事件</param>
        /// <param name="dragStarted">因为拖动而结束点击时触发</param>
        public static void AttachMouseDownMoveUpToClick(UIElement element, EventHandler clickEventHandler, EventHandler dragStarted = null)
        {
            var elementInfo = GetOrCreateInputInfo(element);
            elementInfo.ClickEventHandler += clickEventHandler;
            elementInfo.DragStarted += dragStarted;

            element.MouseDown -= Element_MouseDown;
            element.MouseDown += Element_MouseDown;
            element.MouseMove -= Element_MouseMove;
            element.MouseMove += Element_MouseMove;
            element.MouseUp -= Element_MouseUp;
            element.MouseUp += Element_MouseUp;
            element.LostMouseCapture -= Element_LostMouseCapture;
            element.LostMouseCapture += Element_LostMouseCapture;
        }

        /// <summary>
        /// 去掉对 <paramref name="element" /> 的点击时间的监听
        /// </summary>
        /// <param name="element"></param>
        /// <param name="clickEventHandler">点击的事件</param>
        /// <param name="dragStarted">因为拖动而结束点击时触发的事件</param>
        public static void DetachMouseDownMoveUpToClick(UIElement element, EventHandler clickEventHandler, EventHandler dragStarted = null)
        {
            var elementInfo = GetInputInfo(element);
            if (elementInfo == null) return;

            elementInfo.ClickEventHandler -= clickEventHandler;
            elementInfo.DragStarted -= dragStarted;

            if (elementInfo.IsEmpty())
            {
                element.ClearValue(InputInfoProperty);
                element.MouseDown -= Element_MouseDown;
                element.MouseMove -= Element_MouseMove;
                element.MouseUp -= Element_MouseUp;
                element.LostMouseCapture -= Element_LostMouseCapture;
            }
        }

        private static void Element_LostMouseCapture(object sender, MouseEventArgs e)
        {
            var element = (UIElement)sender;
            GetInputInfo(element)?.LostCapture();
        }

        private static void Element_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var element = (UIElement)sender;
            GetInputInfo(element)?.Up(e.GetPosition(element));
        }

        private static void Element_MouseMove(object sender, MouseEventArgs e)
        {
            var element = (UIElement)sender;
            GetInputInfo(element)?.Move(e.GetPosition(element));
        }

        private static void Element_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = (UIElement)sender;
            GetInputInfo(element)?.Down(e.GetPosition(element));
        }

        private static ElementInfo GetOrCreateInputInfo(UIElement element)
        {
            var inputInfo = GetInputInfo(element);
            if (inputInfo == null)
            {
                inputInfo = new ElementInfo();
                SetInputInfo(element, inputInfo);
            }

            return inputInfo;
        }

        private class ElementInfo
        {
            private const double ToleranceSquared = 0.01;

            private Point _downedPosition;

            private bool _isClick;

            public event EventHandler ClickEventHandler;

            public event EventHandler DragStarted;

            public void Down(Point position)
            {
                _downedPosition = position;
                _isClick = true;
            }

            public void Move(Point position)
            {
                if (!_isClick) return;

                if ((position - _downedPosition).LengthSquared > ToleranceSquared)
                {
                    _isClick = false;
                    DragStarted?.Invoke(null, EventArgs.Empty);
                }
            }

            public void Up(Point position)
            {
                _isClick = _isClick && (position - _downedPosition).LengthSquared <= ToleranceSquared;

                if (!_isClick) return;

                ClickEventHandler?.Invoke(null, EventArgs.Empty);

                _isClick = false;
            }

            public void LostCapture() => _isClick = false;

            public bool IsEmpty() => ClickEventHandler is null && DragStarted is null;
        }
    }
}
