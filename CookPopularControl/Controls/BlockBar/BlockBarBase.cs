using CookPopularCSharpToolkit.Communal;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：BlockBarBase
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-06 14:37:47
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 表示块状的基类
    /// </summary>
    public abstract class BlockBarBase : FrameworkElement
    {
        private Pen _borderPen;

        /// <summary>
        /// 块状数量
        /// </summary>
        public int BlockCount
        {
            get { return (int)GetValue(BlockCountProperty); }
            set { SetValue(BlockCountProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="BlockCount"/>依赖属性
        /// </summary>
        public static readonly DependencyProperty BlockCountProperty =
            DependencyProperty.Register("BlockCount", typeof(int), typeof(BlockBarBase),
                new FrameworkPropertyMetadata(ValueBoxes.Inter5Box, FrameworkPropertyMetadataOptions.AffectsRender, null, new CoerceValueCallback(CoerceBlockCount)));

        private static object CoerceBlockCount(DependencyObject d, object baseValue)
        {
            int input = (int)baseValue;

            if (input < 1)
            {
                return 1;
            }
            else
            {
                return input;
            }
        }


        /// <summary>
        /// 当前值
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="Value"/>依赖属性
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(BlockBarBase),
                new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.AffectsRender, null, new CoerceValueCallback(CoerceValue)));

        private static object CoerceValue(DependencyObject d, object baseValue)
        {
            double input = (double)baseValue;
            if (input < 0 || double.IsNaN(input))
            {
                return 0;
            }
            else if (input > 1)
            {
                return 1;
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// 相邻块状之间的间距
        /// </summary>
        public double BlockMargin
        {
            get { return (double)GetValue(BlockMarginProperty); }
            set { SetValue(BlockMarginProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="BlockMargin"/>依赖属性
        /// </summary>
        public static readonly DependencyProperty BlockMarginProperty =
            DependencyProperty.Register("BlockMargin", typeof(double), typeof(BlockBarBase),
                new FrameworkPropertyMetadata((double)0, FrameworkPropertyMetadataOptions.AffectsRender, null, new CoerceValueCallback(CoerceBlockMargin)));

        private static object CoerceBlockMargin(DependencyObject d, object baseValue)
        {
            double input = (double)baseValue;
            if (input < 0 || double.IsNaN(input) || double.IsInfinity(input))
            {
                return 0;
            }
            else
            {
                return input;
            }
        }


        protected Pen BorderBen
        {
            get
            {
                if (_borderPen == null || _borderPen.Brush != Foreground)
                {
                    _borderPen = new Pen(Foreground, 4);
                    _borderPen.Freeze();
                }
                return _borderPen;
            }
        }


        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }
        public static readonly DependencyProperty ForegroundProperty = Control.ForegroundProperty.AddOwner(typeof(BlockBarBase), new FrameworkPropertyMetadata(Brushes.Navy, FrameworkPropertyMetadataOptions.AffectsRender));


        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        public static readonly DependencyProperty BackgroundProperty = Control.BackgroundProperty.AddOwner(typeof(BlockBarBase), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));


        static BlockBarBase()
        {
            BlockBarBase.MinHeightProperty.OverrideMetadata(typeof(BlockBarBase), new FrameworkPropertyMetadata((double)10));
            BlockBarBase.MinWidthProperty.OverrideMetadata(typeof(BlockBarBase), new FrameworkPropertyMetadata((double)10));
            BlockBarBase.ClipToBoundsProperty.OverrideMetadata(typeof(BlockBarBase), new FrameworkPropertyMetadata(true));
        }

        protected virtual int GetThreshold(double value, int blockCount)
        {
            //Contract.Requires<ArgumentOutOfRangeException>(value >= 0 && value <= 1);
            //Contract.Requires<ArgumentOutOfRangeException>(blockCount > 0);

            int blockNumber = Math.Min((int)(value * (blockCount + 1)), blockCount);

            //Debug.Assert(blockNumber <= blockCount && blockNumber >= 0);

            return blockNumber;
        }
    }
}
