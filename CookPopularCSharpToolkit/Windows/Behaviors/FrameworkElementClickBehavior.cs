using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;


/*
 * Description：FrameworkElementClickBehavior 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-06-06 19:59:03
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */
namespace CookPopularCSharpToolkit.Windows.Behaviors
{
    public class FrameworkElementClickBehavior : Behavior<FrameworkElement>
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer(DispatcherPriority.Normal);

        public FrameworkElementClickBehavior()
        {
            _timer.Interval = TimeSpan.FromSeconds(0.2);
            _timer.Tick += Timer_Tick;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            ClickCommand?.Execute(null);
        }

        public ICommand ClickCommand
        {
            get => (ICommand)GetValue(ClickCommandPropery);
            set => SetValue(ClickCommandPropery, value);
        }
        public static readonly DependencyProperty ClickCommandPropery =
             DependencyProperty.Register(nameof(ClickCommand), typeof(ICommand), typeof(FrameworkElementClickBehavior));

        public ICommand DoubleClickCommand
        {
            get => (ICommand)GetValue(DoubleClickCommandPropery);
            set => SetValue(DoubleClickCommandPropery, value);
        }
        public static readonly DependencyProperty DoubleClickCommandPropery =
            DependencyProperty.Register(nameof(DoubleClickCommand), typeof(ICommand), typeof(FrameworkElementClickBehavior));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.Unloaded += AssociatedObject_Unloaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
            AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObject_PreviewMouseLeftButtonDown;
        }

        private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                _timer.Stop();
                DoubleClickCommand?.Execute(null);
            }
            else
            {
                _timer.Start();
            }
        }
    }
}
