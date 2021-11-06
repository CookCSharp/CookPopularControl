using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：GeometryEffect
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:27:14
 */
namespace CookPopularCSharpToolkit.Windows.Expression
{
    /// <summary>
    /// Provides the base class for GeometryEffect that transforms a geometry into another geometry.
    /// </summary>
    /// <remarks>
    /// This class provides the basic implementation of processing the rendered geometry of a IShape before it's passed to rendering.
    /// A typical implementation will extend the virtual function <see cref="F:ProcessGeometry" /> to transform the input geometry.
    /// <see cref="T:GeometryEffect" /> is typically attached to <see cref="T:IShape" /> as an attached property and activated when <see cref="T:IShape" /> geometry is updated.
    /// The <see cref="P:OutputGeometry" /> of a <see cref="T:GeometryEffect" /> will replace the rendered geometry in <see cref="T:IShape" />.
    /// </remarks>
    [TypeConverter(typeof(GeometryEffectConverter))]
    public abstract class GeometryEffect : Freezable
    {
        /// <summary>
        /// Gets the geometry effect as an attached property on a given dependency object.
        /// </summary>
        public static GeometryEffect GetGeometryEffect(DependencyObject obj)
        {
            return (GeometryEffect)obj.GetValue(GeometryEffect.GeometryEffectProperty);
        }

        /// <summary>
        /// Sets the geometry effect as an attached property on a given dependency object.
        /// </summary>
        public static void SetGeometryEffect(DependencyObject obj, GeometryEffect value)
        {
            obj.SetValue(GeometryEffect.GeometryEffectProperty, value);
        }

        private static void OnGeometryEffectChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            GeometryEffect? geometryEffect = e.OldValue as GeometryEffect;
            GeometryEffect? newValue = e.NewValue as GeometryEffect;
            if (geometryEffect != newValue)
            {
                if (geometryEffect != null && obj.Equals(geometryEffect.Parent))
                {
                    geometryEffect.Detach();
                }
                if (newValue != null)
                {
                    if (newValue.Parent != null)
                    {
                        obj.Dispatcher.BeginInvoke(new Action(delegate ()
                        {
                            GeometryEffect value = newValue.CloneCurrentValue();
                            obj.SetValue(GeometryEffect.GeometryEffectProperty, value);
                        }), DispatcherPriority.Send, null);
                        return;
                    }
                    newValue.Attach(obj);
                }
            }
        }

        /// <summary>
        /// Makes a deep copy of the <see cref="T:GeometryEffect" /> using its current values.
        /// </summary>
        public new GeometryEffect CloneCurrentValue()
        {
            return (GeometryEffect)base.CloneCurrentValue();
        }

        /// <summary>
        /// Makes a deep copy of the geometry effect. Implements CloneCurrentValue in Silverlight.
        /// </summary>
        /// <returns>A clone of the current instance of the geometry effect.</returns>
        protected abstract GeometryEffect DeepCopy();

        /// <summary>
        /// Tests if the given geometry effect is equivalent to the current instance.
        /// </summary>
        /// <param name="geometryEffect">A geometry effect to compare with.</param>
        /// <returns>Returns true when two effects render with the same appearance.</returns>
        public abstract bool Equals(GeometryEffect geometryEffect);

        /// <summary>
        /// The default geometry effect that only passes through the input geometry.
        /// </summary>
        public static GeometryEffect DefaultGeometryEffect
        {
            get
            {
                GeometryEffect result;
                if ((result = GeometryEffect.defaultGeometryEffect) == null)
                {
                    result = (GeometryEffect.defaultGeometryEffect = new GeometryEffect.NoGeometryEffect());
                }
                return result;
            }
        }

        /// <summary>
        /// Gets the output geometry of this geometry effect.
        /// </summary>
        public Geometry OutputGeometry
        {
            get
            {
                return this.cachedGeometry;
            }
        }

        static GeometryEffect()
        {
            DrawingPropertyMetadata.DrawingPropertyChanged += delegate (object sender, DrawingPropertyChangedEventArgs args)
            {
                GeometryEffect? geometryEffect = sender as GeometryEffect;
                if (geometryEffect != null && args.Metadata.AffectsRender)
                {
                    geometryEffect.InvalidateGeometry(InvalidateGeometryReasons.PropertyChanged);
                }
            };
        }

        /// <summary>
        /// Invalidates the geometry effect without actually computing the geometry.
        /// Notifies all parent shapes or effects to invalidate accordingly.
        /// </summary>
        public bool InvalidateGeometry(InvalidateGeometryReasons reasons)
        {
            if (!this.effectInvalidated)
            {
                this.effectInvalidated = true;
                if (reasons != InvalidateGeometryReasons.ParentInvalidated)
                {
                    GeometryEffect.InvalidateParent(this.Parent);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes the geometry effect on a given input geometry.
        /// Stores the result in GeometryEffect.OutputGeometry.
        /// </summary>
        /// <returns>Returns false if nothing has been changed.</returns>
        public bool ProcessGeometry(Geometry input)
        {
            bool flag = false;
            if (this.effectInvalidated)
            {
                flag |= this.UpdateCachedGeometry(input);
                this.effectInvalidated = false;
            }
            return flag;
        }

        /// <summary>
        /// Extends the way of updating cachedGeometry based on a given input geometry.
        /// </summary>
        protected abstract bool UpdateCachedGeometry(Geometry input);

        /// <summary>
        /// Parent can be either IShape or GeometryEffectGroup.
        /// </summary>
        protected internal DependencyObject Parent { get; private set; }

        /// <summary>
        /// Notified when detached from a parent chain.
        /// </summary>
        protected internal virtual void Detach()
        {
            this.effectInvalidated = true;
            this.cachedGeometry = null;
            if (this.Parent != null)
            {
                GeometryEffect.InvalidateParent(this.Parent);
                this.Parent = null;
            }
        }

        /// <summary>
        /// Notified when attached to a parent chain.
        /// </summary>
        protected internal virtual void Attach(DependencyObject obj)
        {
            if (this.Parent != null)
            {
                this.Detach();
            }
            this.effectInvalidated = true;
            this.cachedGeometry = null;
            if (GeometryEffect.InvalidateParent(obj))
            {
                this.Parent = obj;
            }
        }

        /// <summary>
        /// Invalidates the geometry on a given dependency object when
        /// the object is a valid parent type (IShape or GeometryEffect).
        /// </summary>
        private static bool InvalidateParent(DependencyObject parent)
        {
            IShape shape = parent as IShape;
            if (shape != null)
            {
                shape.InvalidateGeometry(InvalidateGeometryReasons.ChildInvalidated);
                return true;
            }
            GeometryEffect geometryEffect = parent as GeometryEffect;
            if (geometryEffect != null)
            {
                geometryEffect.InvalidateGeometry(InvalidateGeometryReasons.ChildInvalidated);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Implement the Freezable in WPF.
        /// </summary>
        protected override Freezable CreateInstanceCore()
        {
            Type type = base.GetType();
            return (Freezable)Activator.CreateInstance(type);
        }

        public static readonly DependencyProperty GeometryEffectProperty = DependencyProperty.RegisterAttached("GeometryEffect", typeof(GeometryEffect), typeof(GeometryEffect), new DrawingPropertyMetadata(GeometryEffect.DefaultGeometryEffect, DrawingPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(GeometryEffect.OnGeometryEffectChanged)));

        private static GeometryEffect defaultGeometryEffect;

        /// <summary>
        /// Specifics the geometry from the previous geometry effect process.
        /// </summary>
        protected Geometry cachedGeometry;

        private bool effectInvalidated;

        private class NoGeometryEffect : GeometryEffect
        {
            protected override bool UpdateCachedGeometry(Geometry input)
            {
                this.cachedGeometry = input;
                return false;
            }

            protected override GeometryEffect DeepCopy()
            {
                return new GeometryEffect.NoGeometryEffect();
            }

            public override bool Equals(GeometryEffect geometryEffect)
            {
                return geometryEffect == null || geometryEffect is GeometryEffect.NoGeometryEffect;
            }
        }
    }
}
