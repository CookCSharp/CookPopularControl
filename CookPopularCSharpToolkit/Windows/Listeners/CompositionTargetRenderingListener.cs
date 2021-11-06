using CookPopularCSharpToolkit.Communal;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：CompositionTargetRenderingListener
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-06 11:57:14
 */
namespace CookPopularCSharpToolkit.Windows
{
    public class CompositionTargetRenderingListener : IDisposable
    {
        public CompositionTargetRenderingListener() { }

        public void StartListening()
        {
            requireAccessAndNotDisposed();

            if (!m_isListening)
            {
                IsListening = true;
                CompositionTarget.Rendering += compositionTarget_Rendering;
            }
        }

        public void StopListening()
        {
            requireAccessAndNotDisposed();

            if (m_isListening)
            {
                IsListening = false;
                CompositionTarget.Rendering -= compositionTarget_Rendering;
            }
        }

#if !WP7
        public void WireParentLoadedUnloaded(FrameworkElement parent)
        {
            Contract.Requires(parent != null);
            requireAccessAndNotDisposed();

            parent.Loaded += delegate (object sender, RoutedEventArgs e)
            {
                this.StartListening();
            };

            parent.Unloaded += delegate (object sender, RoutedEventArgs e)
            {
                this.StopListening();
            };
        }
#endif

        public bool IsListening
        {
            get { return m_isListening; }
            private set
            {
                if (value != m_isListening)
                {
                    m_isListening = value;
                    OnIsListeneningChanged(EventArgs.Empty);
                }
            }
        }

        public bool IsDisposed
        {
            get
            {
                return m_disposed;
            }
        }

        public event EventHandler Rendering;

        protected virtual void OnRendering(EventArgs args)
        {
            requireAccessAndNotDisposed();

            EventHandler handler = Rendering;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public event EventHandler IsListeningChanged;

        protected virtual void OnIsListeneningChanged(EventArgs args)
        {
            var handler = IsListeningChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void Dispose()
        {
            requireAccessAndNotDisposed();
            StopListening();

            Rendering.GetInvocationList().ForEach(d => Rendering -= (EventHandler)d);

            m_disposed = true;
        }


        [DebuggerStepThrough]
        private void requireAccessAndNotDisposed()
        {
            if (m_disposed)
                throw new ObjectDisposedException("This object has been disposed", default(Exception));
        }

        private void compositionTarget_Rendering(object sender, EventArgs e)
        {
            OnRendering(e);
        }

        private bool m_isListening;
        private bool m_disposed;
    }
}
