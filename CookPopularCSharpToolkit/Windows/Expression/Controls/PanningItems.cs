using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：PanningItems
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 18:05:31
 */
namespace CookPopularCSharpToolkit.Windows.Expression
{
    /// <summary>
    /// Provides an items control that displays one selected item, and allows panning between items using touch gestures.
    /// </summary>
    public class PanningItems : Selector
    {
        /// <summary>
        /// Gets or sets the orientation of items in the control.
        /// </summary>
        public Orientation ScrollDirection
        {
            get
            {
                return (Orientation)base.GetValue(PanningItems.ScrollDirectionProperty);
            }
            set
            {
                base.SetValue(PanningItems.ScrollDirectionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the flick tolerance.  This can be a value between 0 and 1.  
        /// It represents the percentage of the size of the PanningItems needed to be covered by the flick gesture to trigger an items change.
        /// </summary>
        public double FlickTolerance
        {
            get
            {
                return (double)base.GetValue(PanningItems.FlickToleranceProperty);
            }
            set
            {
                base.SetValue(PanningItems.FlickToleranceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the item before the selected item.
        /// </summary>
        public object PreviousItem
        {
            get
            {
                return base.GetValue(PanningItems.PreviousItemProperty);
            }
            set
            {
                base.SetValue(PanningItems.PreviousItemProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the item after the selected item.
        /// </summary>
        public object NextItem
        {
            get
            {
                return base.GetValue(PanningItems.NextItemProperty);
            }
            set
            {
                base.SetValue(PanningItems.NextItemProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets whether the contents of the items control will loop, so that the first item will follow the last item.
        /// </summary>
        public bool LoopContents
        {
            get
            {
                return (bool)base.GetValue(PanningItems.LoopContentsProperty);
            }
            set
            {
                base.SetValue(PanningItems.LoopContentsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value of the slider controlling the panning motion.
        /// </summary>
        public double SliderValue
        {
            get
            {
                return (double)base.GetValue(PanningItems.SliderValueProperty);
            }
            set
            {
                base.SetValue(PanningItems.SliderValueProperty, value);
            }
        }

        static PanningItems()
        {
            Selector.SelectedItemProperty.OverrideMetadata(typeof(PanningItems), new FrameworkPropertyMetadata(new PropertyChangedCallback(PanningItems.selectedItemChanged)));
            FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(Grid));
            frameworkElementFactory.SetValue(UIElement.ClipToBoundsProperty, true);
            FrameworkElementFactory frameworkElementFactory2 = new FrameworkElementFactory(typeof(ContentPresenter));
            Binding binding = new Binding();
            binding.RelativeSource = RelativeSource.TemplatedParent;
            binding.Path = new PropertyPath(PanningItems.PreviousItemProperty);
            frameworkElementFactory2.SetBinding(ContentPresenter.ContentProperty, binding);
            Binding binding2 = new Binding();
            binding2.RelativeSource = RelativeSource.TemplatedParent;
            binding2.Path = new PropertyPath(ItemsControl.ItemTemplateProperty);
            frameworkElementFactory2.SetBinding(ContentPresenter.ContentTemplateProperty, binding2);
            frameworkElementFactory2.Name = "Previous";
            frameworkElementFactory.AppendChild(frameworkElementFactory2);
            FrameworkElementFactory frameworkElementFactory3 = new FrameworkElementFactory(typeof(ContentPresenter));
            Binding binding3 = new Binding();
            binding3.RelativeSource = RelativeSource.TemplatedParent;
            binding3.Path = new PropertyPath(Selector.SelectedItemProperty);
            frameworkElementFactory3.SetBinding(ContentPresenter.ContentProperty, binding3);
            Binding binding4 = new Binding();
            binding4.RelativeSource = RelativeSource.TemplatedParent;
            binding4.Path = new PropertyPath(ItemsControl.ItemTemplateProperty);
            frameworkElementFactory3.SetBinding(ContentPresenter.ContentTemplateProperty, binding4);
            frameworkElementFactory3.Name = "Current";
            frameworkElementFactory.AppendChild(frameworkElementFactory3);
            FrameworkElementFactory frameworkElementFactory4 = new FrameworkElementFactory(typeof(ContentPresenter));
            Binding binding5 = new Binding();
            binding5.RelativeSource = RelativeSource.TemplatedParent;
            binding5.Path = new PropertyPath(PanningItems.NextItemProperty);
            frameworkElementFactory4.SetBinding(ContentPresenter.ContentProperty, binding5);
            Binding binding6 = new Binding();
            binding6.RelativeSource = RelativeSource.TemplatedParent;
            binding6.Path = new PropertyPath(ItemsControl.ItemTemplateProperty);
            frameworkElementFactory4.SetBinding(ContentPresenter.ContentTemplateProperty, binding6);
            frameworkElementFactory4.Name = "Next";
            frameworkElementFactory.AppendChild(frameworkElementFactory4);
            ControlTemplate controlTemplate = new ControlTemplate(typeof(PanningItems));
            controlTemplate.VisualTree = frameworkElementFactory;
            Style style = new Style(typeof(PanningItems));
            Setter item = new Setter(Control.TemplateProperty, controlTemplate);
            style.Setters.Add(item);
            style.Seal();
            FrameworkElement.StyleProperty.OverrideMetadata(typeof(PanningItems), new FrameworkPropertyMetadata(style));
        }

        /// <summary>
        /// The constructor for PanningItems.
        /// </summary>
        public PanningItems()
        {
            base.SelectedIndex = 0;
        }

        /// <summary>
        /// Called when the PanningItems template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.previousTransform = new TranslateTransform();
            this.previous = (ContentPresenter)base.Template.FindName("Previous", this);
            if (this.previous != null)
            {
                this.previous.Opacity = 0.0;
                this.previous.RenderTransform = this.previousTransform;
            }
            this.currentTransform = new TranslateTransform();
            this.current = (ContentPresenter)base.Template.FindName("Current", this);
            if (this.current != null)
            {
                this.current.RenderTransform = this.currentTransform;
            }
            this.nextTransform = new TranslateTransform();
            this.next = (ContentPresenter)base.Template.FindName("Next", this);
            if (this.next != null)
            {
                this.next.Opacity = 0.0;
                this.next.RenderTransform = this.nextTransform;
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.CaptureMouse();
            this.OnGestureDown(e.GetPosition(this));
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            this.OnGestureUp();
            base.ReleaseMouseCapture();
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.OnGestureMove(e.GetPosition(this));
            base.OnMouseMove(e);
        }

        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            this.OnGestureUp();
            base.OnLostMouseCapture(e);
        }

        protected override void OnTouchDown(TouchEventArgs e)
        {
            base.CaptureTouch(e.TouchDevice);
            this.OnGestureDown(e.GetTouchPoint(this).Position);
            base.OnTouchDown(e);
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {
            this.OnGestureUp();
            base.ReleaseTouchCapture(e.TouchDevice);
            base.OnTouchUp(e);
        }

        protected override void OnTouchMove(TouchEventArgs e)
        {
            this.OnGestureMove(e.GetTouchPoint(this).Position);
            base.OnTouchMove(e);
        }

        protected override void OnLostTouchCapture(TouchEventArgs e)
        {
            this.OnGestureUp();
            base.OnLostTouchCapture(e);
        }

        private static void flickToleranceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
        }

        private static object coerceFlickTolerance(DependencyObject sender, object value)
        {
            double val = (double)value;
            return Math.Max(Math.Min(1.0, val), 0.0);
        }

        private static void sliderValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PanningItems panningItems = (PanningItems)sender;
            if (panningItems.previous != null && panningItems.current != null && panningItems.next != null)
            {
                panningItems.previous.Opacity = 1.0;
                panningItems.next.Opacity = 1.0;
                if (panningItems.ScrollDirection == Orientation.Horizontal)
                {
                    panningItems.previousTransform.X = panningItems.current.ActualWidth * (panningItems.SliderValue - 1.0);
                    panningItems.currentTransform.X = panningItems.current.ActualWidth * panningItems.SliderValue;
                    panningItems.nextTransform.X = panningItems.current.ActualWidth * (panningItems.SliderValue + 1.0);
                    panningItems.previousTransform.Y = 0.0;
                    panningItems.currentTransform.Y = 0.0;
                    panningItems.nextTransform.Y = 0.0;
                    return;
                }
                panningItems.previousTransform.X = 0.0;
                panningItems.currentTransform.X = 0.0;
                panningItems.nextTransform.X = 0.0;
                panningItems.previousTransform.Y = panningItems.current.ActualHeight * (panningItems.SliderValue - 1.0);
                panningItems.currentTransform.Y = panningItems.current.ActualHeight * panningItems.SliderValue;
                panningItems.nextTransform.Y = panningItems.current.ActualHeight * (panningItems.SliderValue + 1.0);
            }
        }

        private static void selectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PanningItems panningItems = (PanningItems)sender;
            int selectedIndex = panningItems.SelectedIndex;
            if (selectedIndex == -1 || panningItems.Items.Count == 0)
            {
                panningItems.PreviousItem = null;
                panningItems.NextItem = null;
                return;
            }
            if (selectedIndex == 0)
            {
                if (panningItems.LoopContents)
                {
                    panningItems.PreviousItem = panningItems.Items[panningItems.Items.Count - 1];
                }
                else
                {
                    panningItems.PreviousItem = null;
                }
            }
            else
            {
                panningItems.PreviousItem = panningItems.Items[selectedIndex - 1];
            }
            if (selectedIndex != panningItems.Items.Count - 1)
            {
                panningItems.NextItem = panningItems.Items[selectedIndex + 1];
                return;
            }
            if (panningItems.LoopContents)
            {
                panningItems.NextItem = panningItems.Items[0];
                return;
            }
            panningItems.NextItem = null;
        }

        private void OnGestureDown(Point point)
        {
            this.touchDown = point;
            this.isDragging = true;
        }

        private void OnGestureUp()
        {
            if (this.isDragging)
            {
                this.AnimateSliderValueTo(0.0);
            }
            this.isDragging = false;
        }

        private void OnGestureMove(Point point)
        {
            if (this.isDragging)
            {
                Vector vector = point - this.touchDown;
                if (this.ScrollDirection == Orientation.Horizontal)
                {
                    this.SliderValue = vector.X / this.current.ActualWidth;
                }
                else
                {
                    this.SliderValue = vector.Y / this.current.ActualHeight;
                }
                if (Math.Abs(this.SliderValue) >= this.FlickTolerance)
                {
                    this.isDragging = false;
                    int num = base.SelectedIndex;
                    if (num != -1)
                    {
                        if (this.SliderValue > 0.0)
                        {
                            num--;
                            this.SliderValue -= 1.0;
                        }
                        else
                        {
                            num++;
                            this.SliderValue += 1.0;
                        }
                        num += base.Items.Count;
                        num %= base.Items.Count;
                        base.SelectedIndex = num;
                    }
                    this.AnimateSliderValueTo(0.0);
                }
            }
        }

        private void AnimateSliderValueTo(double target)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(target, new Duration(TimeSpan.FromSeconds(0.25)));
            doubleAnimation.FillBehavior = FillBehavior.Stop;
            doubleAnimation.Completed += delegate (object o, EventArgs e)
            {
                this.SliderValue = 0.0;
            };
            base.BeginAnimation(PanningItems.SliderValueProperty, doubleAnimation);
        }

        public static readonly DependencyProperty ScrollDirectionProperty = DependencyProperty.Register("ScrollDirection", typeof(Orientation), typeof(PanningItems), new PropertyMetadata(Orientation.Horizontal));

        public static readonly DependencyProperty FlickToleranceProperty = DependencyProperty.Register("FlickTolerance", typeof(double), typeof(PanningItems), new PropertyMetadata(0.25, new PropertyChangedCallback(PanningItems.flickToleranceChanged), new CoerceValueCallback(PanningItems.coerceFlickTolerance)));

        public static readonly DependencyProperty PreviousItemProperty = DependencyProperty.Register("PreviousItem", typeof(object), typeof(PanningItems), new PropertyMetadata(null));

        public static readonly DependencyProperty NextItemProperty = DependencyProperty.Register("NextItem", typeof(object), typeof(PanningItems), new PropertyMetadata(null));

        public static readonly DependencyProperty LoopContentsProperty = DependencyProperty.Register("LoopContents", typeof(bool), typeof(PanningItems), new PropertyMetadata(true));

        public static readonly DependencyProperty SliderValueProperty = DependencyProperty.Register("SliderValue", typeof(double), typeof(PanningItems), new PropertyMetadata(0.0, new PropertyChangedCallback(PanningItems.sliderValueChanged)));

        private Point touchDown;

        private bool isDragging;

        private TranslateTransform previousTransform;

        private TranslateTransform currentTransform;

        private TranslateTransform nextTransform;

        private ContentPresenter previous;

        private ContentPresenter current;

        private ContentPresenter next;
    }
}
