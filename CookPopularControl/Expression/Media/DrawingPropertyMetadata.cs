using System;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DrawingPropertyMetadata
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 17:30:09
 */
namespace CookPopularControl.Expression.Media
{
    /// <summary>
    /// Unifies the interface of PropertyMetadata in WPF and Silverlight.
    /// Provides the necessary notification about render, arrange, or measure.
    /// </summary>
    public class DrawingPropertyMetadata : FrameworkPropertyMetadata
    {
        public DrawingPropertyMetadata(object defaultValue) : this(defaultValue, DrawingPropertyMetadataOptions.None, null)
        {
        }

        public DrawingPropertyMetadata(PropertyChangedCallback propertyChangedCallback) : this(DependencyProperty.UnsetValue, DrawingPropertyMetadataOptions.None, propertyChangedCallback)
        {
        }

        public DrawingPropertyMetadata(object defaultValue, DrawingPropertyMetadataOptions options) : this(defaultValue, options, null)
        {
        }

        public DrawingPropertyMetadata(object defaultValue, DrawingPropertyMetadataOptions options, PropertyChangedCallback propertyChangedCallback) : base(defaultValue, (FrameworkPropertyMetadataOptions)options, DrawingPropertyMetadata.AttachCallback(defaultValue, options, propertyChangedCallback))
        {
        }

        public static event EventHandler<DrawingPropertyChangedEventArgs> DrawingPropertyChanged;

        /// <summary>
        /// This private Ctor should only be used by AttachCallback.
        /// </summary>
        private DrawingPropertyMetadata(DrawingPropertyMetadataOptions options, object defaultValue) : base(defaultValue, (FrameworkPropertyMetadataOptions)options)
        {
        }

        /// <summary>
        /// Chain InternalCallback() to attach the instance of DrawingPropertyMetadata on property callback.
        /// In Silverlight, the property metadata is thrown away after setting. Use callback to remember it.
        /// </summary>
        private static PropertyChangedCallback AttachCallback(object defaultValue, DrawingPropertyMetadataOptions options, PropertyChangedCallback propertyChangedCallback)
        {
            return new PropertyChangedCallback(new DrawingPropertyMetadata(options, defaultValue)
            {
                options = options,
                propertyChangedCallback = propertyChangedCallback
            }.InternalCallback);
        }

        /// <summary>
        /// Before chaining the original callback, trigger DrawingPropertyChangedEvent.
        /// </summary>
        private void InternalCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (DrawingPropertyMetadata.DrawingPropertyChanged != null)
            {
                DrawingPropertyMetadata.DrawingPropertyChanged(sender, new DrawingPropertyChangedEventArgs
                {
                    Metadata = this,
                    IsAnimated = DependencyPropertyHelper.GetValueSource(sender, e.Property).IsAnimated
                });
            }
            if (this.propertyChangedCallback != null)
            {
                this.propertyChangedCallback(sender, e);
            }
        }

        static DrawingPropertyMetadata()
        {
            DrawingPropertyMetadata.DrawingPropertyChanged += delegate (object sender, DrawingPropertyChangedEventArgs args)
            {
                IShape? shape = sender as IShape;
                if (shape != null && args.Metadata.AffectsRender)
                {
                    InvalidateGeometryReasons invalidateGeometryReasons = InvalidateGeometryReasons.PropertyChanged;
                    if (args.IsAnimated)
                    {
                        invalidateGeometryReasons |= InvalidateGeometryReasons.IsAnimated;
                    }
                    shape.InvalidateGeometry(invalidateGeometryReasons);
                }
            };
        }

        private DrawingPropertyMetadataOptions options;

        private PropertyChangedCallback propertyChangedCallback;
    }
}
