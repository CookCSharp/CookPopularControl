﻿using CookPopularCSharpToolkit.Communal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;



/*
 * Description：UniformStackPanel 
 * Author： Chance_写代码的厨子
 * Company: NCATest
 * Create Time：2021-11-14 02:28:30
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) NCATest 2021 All Rights Reserved.
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 按照水平或垂直方向等份布局的面板
    /// </summary>
    public class UniformStackPanel : Panel
    {
        /// <summary>
        /// 如果值为true,子项之间、子项与边界之间间距相等
        /// 如果为false，仅子项之间间距相等，子项与边界之间间距肯能不相等
        /// </summary>
        public bool IsUniformSpace
        {
            get { return (bool)GetValue(IsUniformSpaceProperty); }
            set { SetValue(IsUniformSpaceProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="IsUniformSpace"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsUniformSpaceProperty =
            DependencyProperty.Register("IsUniformSpace", typeof(bool), typeof(UniformStackPanel), new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.AffectsMeasure));


        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Orientation"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(UniformStackPanel), new FrameworkPropertyMetadata(Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsMeasure));


        public HorizontalAlignment ItemsHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(ItemsHorizontalAlignmentProperty);
            set => SetValue(ItemsHorizontalAlignmentProperty, value);
        }
        /// <summary>
        /// 提供<see cref="ItemsHorizontalAlignment"/>的依赖属性
        /// </summary>
        /// <remarks>当<see cref="Orientation"/>为<see cref="Orientation.Vertical"/>使用</remarks>
        public static readonly DependencyProperty ItemsHorizontalAlignmentProperty =
            DependencyProperty.Register("ItemsHorizontalAlignment", typeof(HorizontalAlignment), typeof(UniformStackPanel),
                new FrameworkPropertyMetadata(HorizontalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsMeasure));


        public VerticalAlignment ItemsVerticalAlignment
        {
            get => (VerticalAlignment)GetValue(ItemsVerticalAlignmentProperty);
            set => SetValue(ItemsVerticalAlignmentProperty, value);
        }
        /// <summary>
        /// 提供<see cref="ItemsVerticalAlignment"/>的依赖属性
        /// </summary>
        /// <remarks>当<see cref="Orientation"/>为<see cref="Orientation.Horizontal"/>使用</remarks>
        public static readonly DependencyProperty ItemsVerticalAlignmentProperty =
            DependencyProperty.Register("ItemsVerticalAlignment", typeof(VerticalAlignment), typeof(UniformStackPanel),
                new FrameworkPropertyMetadata(VerticalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsMeasure));


        protected override Size MeasureOverride(Size availableSize)
        {
            Size childSize;

            if (Orientation == Orientation.Horizontal)
                childSize = new Size(availableSize.Width / InternalChildren.Count, availableSize.Height);
            else
                childSize = new Size(availableSize.Width, availableSize.Height / InternalChildren.Count);

            foreach (UIElement element in InternalChildren)
            {
                element.Measure(childSize);
            }

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int index = 0;
            double maxDistanceX = finalSize.Width / InternalChildren.Count;
            double maxDistanceY = finalSize.Height / InternalChildren.Count;
            double elementsWidth = 0, elementsHeight = 0;
            double distanceX = 0, distanceY = 0;

            foreach (UIElement element in InternalChildren)
            {
                elementsWidth += element.DesiredSize.Width;
                elementsHeight += element.DesiredSize.Height;
            }

            distanceX = (finalSize.Width - elementsWidth) / (InternalChildren.Count + 1);
            distanceY = (finalSize.Height - elementsHeight) / (InternalChildren.Count + 1);

            foreach (UIElement element in InternalChildren)
            {
                Point location;
                double childWidth, childHeight;
                double locationX, locationY;

                if (IsUniformSpace)
                {
                    if (Orientation == Orientation.Horizontal)
                    {
                        childWidth = element.DesiredSize.Width;
                        childHeight = element.DesiredSize.Height;
                        if (ItemsVerticalAlignment == VerticalAlignment.Top)
                            locationY = 0;
                        else if (ItemsVerticalAlignment == VerticalAlignment.Bottom)
                            locationY = finalSize.Height - childHeight;
                        else
                            locationY = finalSize.Height / 2D - childHeight / 2D;
                        location = new Point(index * childWidth + (index + 1) * distanceX, locationY);
                    }
                    else
                    {
                        childWidth = element.DesiredSize.Width;
                        childHeight = element.DesiredSize.Height;
                        if (ItemsHorizontalAlignment == HorizontalAlignment.Left)
                            locationX = 0;
                        else if (ItemsHorizontalAlignment == HorizontalAlignment.Right)
                            locationX = finalSize.Width - childWidth;
                        else
                            locationX = finalSize.Width / 2D - childWidth / 2D;
                        location = new Point(locationX, index * childHeight + (index + 1) * distanceY);
                    }
                }
                else
                {
                    if (Orientation == Orientation.Horizontal)
                    {
                        childWidth = element.DesiredSize.Width;
                        childHeight = element.DesiredSize.Height;
                        if (ItemsVerticalAlignment == VerticalAlignment.Top)
                            locationY = 0;
                        else if (ItemsVerticalAlignment == VerticalAlignment.Bottom)
                            locationY = finalSize.Height - childHeight;
                        else
                            locationY = finalSize.Height / 2D - childHeight / 2D;
                        location = new Point(maxDistanceX * index + (maxDistanceX - childWidth) / 2, locationY);
                    }
                    else
                    {
                        childWidth = element.DesiredSize.Width;
                        childHeight = element.DesiredSize.Height;
                        if (ItemsHorizontalAlignment == HorizontalAlignment.Left)
                            locationX = 0;
                        else if (ItemsHorizontalAlignment == HorizontalAlignment.Right)
                            locationX = finalSize.Width - childWidth;
                        else
                            locationX = finalSize.Width / 2D - childWidth / 2D;
                        location = new Point(locationX, maxDistanceY * index + (maxDistanceY - childHeight) / 2);
                    }
                }

                var childSize = new Size(childWidth, childHeight);
                element?.Arrange(new Rect(location, childSize));

                index += 1;
            }

            return finalSize;
        }
    }
}
