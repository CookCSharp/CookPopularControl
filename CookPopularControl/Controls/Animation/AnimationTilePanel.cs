using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：AnimationTilePanel
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-05 09:08:31
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 标识类似瓦片的动画面板
    /// </summary>
    public class AnimationTilePanel : AnimationPanel
    {
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="ItemWidth"/>的附加属性
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.RegisterAttached("ItemWidth", typeof(double), typeof(AnimationTilePanel),
                new FrameworkPropertyMetadata(ValueBoxes.Double30Box, FrameworkPropertyMetadataOptions.AffectsMeasure), value => OnValidateValue(value, 0, double.PositiveInfinity, false, false));


        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="ItemHeight"/>的附加属性
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.RegisterAttached("ItemHeight", typeof(double), typeof(AnimationTilePanel),
                new FrameworkPropertyMetadata(ValueBoxes.Double30Box, FrameworkPropertyMetadataOptions.AffectsMeasure), value => OnValidateValue(value, 0, double.PositiveInfinity, false, false));


        private bool m_AppliedTemplate;
        private bool m_ArrangedOnce;
        private DoubleAnimation m_itemOpacityAnimation;

        protected override Size MeasureOverride(Size availableSize)
        {
            OnPreApplyTemplate();

            Size theChildSize = GetItemSize();

            foreach (UIElement child in Children)
            {
                child.Measure(theChildSize);
            }

            int childrenPerRow;

            // Figure out how many children fit on each row
            if (availableSize.Width == Double.PositiveInfinity)
            {
                childrenPerRow = this.Children.Count;
            }
            else
            {
                childrenPerRow = Math.Max(1, (int)Math.Floor(availableSize.Width / this.ItemWidth));
            }

            // Calculate the width and height this results in
            double width = childrenPerRow * this.ItemWidth;
            double height = this.ItemHeight * (Math.Floor((double)this.Children.Count / childrenPerRow) + 1);
            height = (height.IsValid()) ? height : 0;
            return new Size(width, height);
        }

        //[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void OnPreApplyTemplate()
        {
            if (!m_AppliedTemplate)
            {
                m_AppliedTemplate = true;

                DependencyObject source = base.TemplatedParent;
                if (source is ItemsPresenter)
                {
                    source = VisualTreeHelperExtension.GetVisualAncestorsAndSelf(source).FirstOrDefault();
                }

                if (source != null)
                {
                    BindToParentItemsControl(ItemHeightProperty, source);
                    BindToParentItemsControl(ItemWidthProperty, source);
                }
            }
        }

        private void BindToParentItemsControl(DependencyProperty property, DependencyObject source)
        {
            if (DependencyPropertyHelper.GetValueSource(this, property).BaseValueSource == BaseValueSource.Default)
            {
                Binding binding = new Binding();
                binding.Source = source;
                binding.Path = new PropertyPath(property);
                base.SetBinding(property, binding);
            }
        }


        protected override sealed Size ArrangeOverride(Size finalSize)
        {
            // Calculate how many children fit on each row
            int childrenPerRow = Math.Max(1, (int)Math.Floor(finalSize.Width / this.ItemWidth));
            Size theChildSize = GetItemSize();

            for (int i = 0; i < this.Children.Count; i++)
            {
                // Figure out where the child goes
                Point newOffset = CalculateChildOffset(i, childrenPerRow, this.ItemWidth, this.ItemHeight, finalSize.Width, this.Children.Count);
                ArrangeChild(Children[i], new Rect(newOffset, theChildSize));
            }

            m_ArrangedOnce = true;
            return finalSize;
        }

        //Given a child index, child size and children per row, figure out where the child goes
        private static Point CalculateChildOffset(int index, int childrenPerRow, double itemWidth, double itemHeight, double panelWidth, int totalChildren)
        {
            double fudge = 0;
            if (totalChildren > childrenPerRow)
            {
                fudge = (panelWidth - childrenPerRow * itemWidth) / childrenPerRow;
                //Debug.Assert(fudge >= 0);
            }

            int row = index / childrenPerRow;
            int column = index % childrenPerRow;
            return new Point(0.5 * fudge + column * (itemWidth + fudge), row * itemHeight);
        }


        protected override Point ProcessNewChild(UIElement child, Rect providedBounds)
        {
            var startLocation = providedBounds.Location;
            if (m_ArrangedOnce)
            {
                if (m_itemOpacityAnimation == null)
                {
                    m_itemOpacityAnimation = new DoubleAnimation()
                    {
                        From = 0,
                        Duration = new Duration(TimeSpan.FromSeconds(0.5))
                    };
                    m_itemOpacityAnimation.Freeze();
                }

                child.BeginAnimation(UIElement.OpacityProperty, m_itemOpacityAnimation);
                startLocation -= new Vector(providedBounds.Width, 0);
            }
            return startLocation;
        }

        private Size GetItemSize() => new Size(ItemWidth, ItemHeight);
    }
}
