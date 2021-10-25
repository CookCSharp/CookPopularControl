using CookPopularControl.Tools.Boxes;
using CookPopularControl.Tools.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using static CookPopularControl.Tools.Interop.NativeMethods;
using OriginPopup = System.Windows.Controls.Primitives.Popup;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ExtendPopup
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-27 13:11:46
 */
namespace CookPopularControl.Controls.Popup
{
    /// <summary>
    /// 提供<see cref="OriginPopup"/>的扩展控件
    /// </summary>
    /// <remarks>可跟随父元素移动</remarks>
    public class ExtendPopup : OriginPopup
    {
        static ExtendPopup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendPopup), new FrameworkPropertyMetadata(typeof(ExtendPopup)));
        }

        /// <summary>
        /// 是否置前
        /// </summary>
        public bool IsTopMost
        {
            get { return (bool)GetValue(IsTopMostProperty); }
            set { SetValue(IsTopMostProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// <see cref="IsTopMostProperty"/>提供<see cref="IsTopMost"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsTopMostProperty =
            DependencyProperty.Register("IsTopMost", typeof(bool), typeof(ExtendPopup),
                new PropertyMetadata(ValueBoxes.FalseBox, OnIsTopMostChanged));
        private static void OnIsTopMostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ExtendPopup)?.UpdatePopup();
        }

        /// <summary>
        /// 是否跟随父元素移动
        /// </summary>
        public bool IsPositionUpdate
        {
            get { return (bool)GetValue(IsPositionUpdateProperty); }
            set { SetValue(IsPositionUpdateProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// <see cref="IsPositionUpdateProperty"/>提供<see cref="IsPositionUpdate"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsPositionUpdateProperty =
            DependencyProperty.Register("IsPositionUpdate", typeof(bool), typeof(ExtendPopup),
                new PropertyMetadata(ValueBoxes.FalseBox, IsPositionUpdateChanged));
        private static void IsPositionUpdateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ExtendPopup)?.ExtendPopup_Loaded(d, null);
        }

        /// <summary>
        /// 鼠标左键按下是否关闭<see cref="ExtendPopup"/>
        /// </summary>
        public bool IsMouseLeftButtonDownClosed
        {
            get { return (bool)GetValue(IsMouseLeftButtonDownClosedProperty); }
            set { SetValue(IsMouseLeftButtonDownClosedProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// <see cref="IsMouseLeftButtonDownClosedProperty"/>提供<see cref="IsMouseLeftButtonDownClosed"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsMouseLeftButtonDownClosedProperty =
            DependencyProperty.Register("IsMouseLeftButtonDownClosed", typeof(bool), typeof(ExtendPopup), new PropertyMetadata(ValueBoxes.FalseBox));


        public ExtendPopup()
        {
            this.Loaded += ExtendPopup_Loaded;
            this.Unloaded += ExtendPopup_Unloaded;
        }

        private void ExtendPopup_Loaded(object sender, RoutedEventArgs e)
        {
            ExtendPopup? pop = sender as ExtendPopup;
            if (pop == null) return;
            var win = Window.GetWindow(pop);
            if (win == null) return;
            win.LocationChanged -= HostWindow_PositionChanged;
            win.SizeChanged -= HostWindow_PositionChanged;
            pop.SizeChanged -= HostWindow_PositionChanged;
            win.StateChanged -= HostWindow_StateChanged;
            win.Activated -= HostWindow_Activated;
            win.Activated += HostWindow_Activated;
            win.Deactivated -= HostWindow_Deactivated;
            win.Deactivated += HostWindow_Deactivated;

            if (IsPositionUpdate)
            {
                win.LocationChanged += HostWindow_PositionChanged;
                win.SizeChanged += HostWindow_PositionChanged;
                pop.SizeChanged += HostWindow_PositionChanged;
                win.StateChanged += HostWindow_StateChanged;
            }
        }

        private void ExtendPopup_Unloaded(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(sender as FrameworkElement);
            if (PlacementTarget is FrameworkElement target)
            {
                target.SizeChanged -= HostWindow_PositionChanged;
            }
            if (win != null)
            {
                win.LocationChanged -= HostWindow_PositionChanged;
                win.SizeChanged -= HostWindow_PositionChanged;
                win.StateChanged -= HostWindow_StateChanged;
                win.Activated -= HostWindow_Activated;
                win.Deactivated -= HostWindow_Deactivated;
            }
            Unloaded -= ExtendPopup_Unloaded;
        }

        private void HostWindow_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                var method = typeof(OriginPopup).GetMethod("UpdatePosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (IsOpen)
                    method.Invoke(this, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void HostWindow_StateChanged(object sender, EventArgs e)
        {
            var win = Window.GetWindow(sender as FrameworkElement);
            if (win != null && win.WindowState != WindowState.Minimized)
            {
                var target = PlacementTarget as FrameworkElement;
                var holder = target != null ? target.DataContext as AdornedElementPlaceholder : null;
                if (holder != null && holder.AdornedElement != null)
                {
                    PopupAnimation = System.Windows.Controls.Primitives.PopupAnimation.None;
                    IsOpen = false;
                    var errorTemplate = holder.AdornedElement.GetValue(Validation.ErrorTemplateProperty);
                    holder.AdornedElement.SetValue(Validation.ErrorTemplateProperty, null);
                    holder.AdornedElement.SetValue(Validation.ErrorTemplateProperty, errorTemplate);
                }
            }
        }

        private void HostWindow_Activated(object sender, EventArgs e) => UpdatePopup(true);

        private void HostWindow_Deactivated(object sender, EventArgs e) =>UpdatePopup(false);

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (IsMouseLeftButtonDownClosed)
                IsOpen = false;

            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnOpened(EventArgs e)
        {
            UpdatePopup();
            
            base.OnOpened(e);
        }

        private void UpdatePopup(bool isTop = false)
        {
            if (!IsOpen) return;

            if (Child is null) return;

            var handSource = (HwndSource)PresentationSource.FromVisual(Child);
            if (handSource is null) return;

            isTop &= IsTopMost;
            var intPtr = handSource.Handle;
            if (!NativeMethods.GetWindowRect(intPtr, out RECT rect)) return;

            NativeMethods.SetWindowPos(intPtr, new IntPtr(isTop ? -1 : -2), rect.Left, rect.Top, (int)Width, (int)Height, SWP.TOPMOST);
        }
    }
}
