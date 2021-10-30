using CookPopularControl.Communal.Data;
using CookPopularControl.Tools.Boxes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Badge
 * Author： Chance_写代码的厨子
 * Create Time：2021-10-21 18:28:57
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class Badge : ContentControl
    {
        /// <summary>
        /// 表示<see cref="Badge"/>的元素
        /// </summary>
        public object Element
        {
            get { return GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="Element"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register("Element", typeof(object), typeof(Badge), new PropertyMetadata(default, OnElementChanged));
        private static void OnElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Badge badge && badge.IsInitialized)
            {
                badge.OnElementChanged(e.OldValue, e.NewValue);
            }
        }


        public BadgeDirection Direction
        {
            get { return (BadgeDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        /// <summary>
        /// 提供<see cref="Direction"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(BadgeDirection), typeof(Badge), new PropertyMetadata(BadgeDirection.RightTop));


        public Thickness BadgeMargin
        {
            get => (Thickness)GetValue(BadgeMarginProperty);
            set => SetValue(BadgeMarginProperty, value);
        }
        /// <summary>
        /// 提供<see cref="BadgeMargin"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty BadgeMarginProperty =
            DependencyProperty.Register("BadgeMargin", typeof(Thickness), typeof(Badge), new PropertyMetadata(default(Thickness)));


        public bool IsShowBadge
        {
            get => (bool)GetValue(IsShowBadgeProperty);
            set => SetValue(IsShowBadgeProperty, ValueBoxes.BooleanBox(value));
        }
        /// <summary>
        /// 提供<see cref="IsShowBadge"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsShowBadgeProperty =
            DependencyProperty.Register("IsShowBadge", typeof(bool), typeof(Badge), new PropertyMetadata(ValueBoxes.TrueBox));



        public event EventHandler<RoutedPropertyChangedEventArgs<object>> ElementChanged
        {
            add { this.AddHandler(ElementChangedEvent, value); }
            remove { this.RemoveHandler(ElementChangedEvent, value); }
        }
        public static readonly RoutedEvent ElementChangedEvent =
            EventManager.RegisterRoutedEvent("ElementChanged", RoutingStrategy.Bubble, typeof(EventHandler<RoutedPropertyChangedEventArgs<object>>), typeof(Badge));

        protected virtual void OnElementChanged(object oldValue, object newValue)
        {
            RoutedPropertyChangedEventArgs<object> arg = new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, ElementChangedEvent);
            this.RaiseEvent(arg);
        }


        protected override Geometry GetLayoutClip(Size layoutSlotSize) => null;
    }
}
