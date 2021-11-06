using CookPopularCSharpToolkit.Communal;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CookPopularCSharpToolkit.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：AnimationPanel
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-05 09:16:28
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 标识动画面板布局的抽象基类
    /// </summary>
    public abstract class AnimationPanel : Panel
    {
        /// <summary>
        /// 表示阻尼大小
        /// </summary>
        public double Dampening
        {
            get { return (double)GetValue(DampeningProperty); }
            set { SetValue(DampeningProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="Dampening"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DampeningProperty =
            DependencyProperty.Register("Dampening", typeof(double), typeof(AnimationPanel),
                new FrameworkPropertyMetadata(0.2, FrameworkPropertyMetadataOptions.None), value => OnValidateValue(value, 0, 1, false, false));

        /// <summary>
        /// 表示吸附力大小
        /// </summary>
        public double Attraction
        {
            get { return (double)GetValue(AttractionProperty); }
            set { SetValue(AttractionProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="Attraction"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty AttractionProperty =
            DependencyProperty.Register("Attraction", typeof(double), typeof(AnimationPanel),
                new FrameworkPropertyMetadata(ValueBoxes.Double3Box, FrameworkPropertyMetadataOptions.None), value => OnValidateValue(value, 0, double.PositiveInfinity, false, false));

        /// <summary>
        /// 表示变化大小
        /// </summary>
        public double Variation
        {
            get { return (double)GetValue(VariationProperty); }
            set { SetValue(VariationProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="Variation"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty VariationProperty =
            DependencyProperty.Register("Variation", typeof(double), typeof(AnimationPanel),
                new FrameworkPropertyMetadata(ValueBoxes.Double1Box, FrameworkPropertyMetadataOptions.None), value => OnValidateValue(value, 0, 1, true, true));

        private static readonly DependencyProperty DataProperty = DependencyProperty.RegisterAttached("Data", typeof(AnimatingPanelItemData), typeof(AnimationTilePanel));

        protected static bool OnValidateValue(object objValue, double minValue, double maxValue, bool includeMin, bool includeMax)
        {
            Contract.Requires(!double.IsNaN(minValue));
            Contract.Requires(!double.IsNaN(maxValue));
            Contract.Requires(maxValue >= minValue);

            double value = (double)objValue;

            if (includeMin)
            {
                if (value < minValue)
                {
                    return false;
                }
            }
            else
            {
                if (value <= minValue)
                {
                    return false;
                }
            }
            if (includeMax)
            {
                if (value > maxValue)
                {
                    return false;
                }
            }
            else
            {
                if (value >= maxValue)
                {
                    return false;
                }
            }

            return true;
        }


        private const double c_diff = 0.1;
        private const double c_terminalVelocity = 10000;

        protected AnimationPanel()
        {
            this.Loaded += delegate (object sender, RoutedEventArgs e)
            {
                CompositionTarget.Rendering += CompositionTarget_Rendering;
            };

            this.Unloaded += delegate (object sender, RoutedEventArgs e)
            {
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
            };

            //m_listener.Rendering += CompositionTarget_Rendering;
            //m_listener.WireParentLoadedUnloaded(this);
        }

        protected void ArrangeChild(UIElement child, Rect bounds)
        {
            //m_listener.StartListening();

            AnimatingPanelItemData data = (AnimatingPanelItemData)child.GetValue(DataProperty);
            if (data == null)
            {
                data = new AnimatingPanelItemData();
                child.SetValue(DataProperty, data);
                Debug.Assert(child.RenderTransform == Transform.Identity);
                child.RenderTransform = data.Transform;

                data.Current = ProcessNewChild(child, bounds);
            }
            Debug.Assert(child.RenderTransform == data.Transform);

            //set the location attached DP
            data.Target = bounds.Location;

            child.Arrange(bounds);
        }

        protected virtual Point ProcessNewChild(UIElement child, Rect providedBounds)
        {
            return providedBounds.Location;
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            double dampening = this.Dampening;
            double attractionFactor = this.Attraction * 0.01;
            double variation = this.Variation;

            bool shouldChange = false;
            for (int i = 0; i < Children.Count; i++)
            {
                shouldChange = UpdateChildData((AnimatingPanelItemData)Children[i].GetValue(DataProperty), dampening, attractionFactor, variation) || shouldChange;
            }

            if (!shouldChange)
            {
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
                CompositionTarget.Rendering += CompositionTarget_Rendering;
                //m_listener.StopListening();
            }
        }

        private bool UpdateChildData(AnimatingPanelItemData data, double dampening, double attractionFactor, double variation)
        {
            if (data == null)
            {
                return false;
            }
            else
            {
                Debug.Assert(dampening > 0 && dampening < 1);
                Debug.Assert(attractionFactor > 0 && !double.IsInfinity(attractionFactor));

                attractionFactor *= 1 + (variation * data.RandomSeed - 0.5);

                Point newLocation;
                Vector newVelocity;

                bool anythingChanged = AnimationHelper.Animate(data.Current, data.LocationVelocity, data.Target, attractionFactor, dampening, c_terminalVelocity, c_diff, c_diff, out newLocation, out newVelocity);

                data.Current = newLocation;
                data.LocationVelocity = newVelocity;

                var transformVector = data.Current - data.Target;
                Contract.Requires(data.Transform != null);
                data.Transform.X = transformVector.X;
                data.Transform.Y = transformVector.Y;

                return anythingChanged;
            }
        }

        private class AnimatingPanelItemData
        {
            public Point Target;
            public Point Current;
            public Vector LocationVelocity;
            public readonly double RandomSeed = RandomHelper.Rnd.NextDouble();
            public readonly TranslateTransform Transform = new TranslateTransform();
        }

        private readonly CompositionTargetRenderingListener m_listener = new CompositionTargetRenderingListener();
    }
}
