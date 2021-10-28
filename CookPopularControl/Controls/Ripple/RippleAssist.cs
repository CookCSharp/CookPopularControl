using System.Windows;
using System.Windows.Media;

namespace CookPopularControl.Controls
{
    public static class RippleAssist
    {
        public static void SetClipToBounds(DependencyObject element, bool value) => element.SetValue(ClipToBoundsProperty, value);
        public static bool GetClipToBounds(DependencyObject element) => (bool)element.GetValue(ClipToBoundsProperty);
        public static readonly DependencyProperty ClipToBoundsProperty = DependencyProperty.RegisterAttached(
            "ClipToBounds", typeof(bool), typeof(RippleAssist), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Set to <c>true</c> to cause the ripple to originate from the centre of the 
        /// content.  Otherwise the effect will originate from the mouse down position.        
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetIsCentered(DependencyObject element, bool value) => element.SetValue(IsCenteredProperty, value);
        /// <summary>
        /// Set to <c>true</c> to cause the ripple to originate from the centre of the 
        /// content.  Otherwise the effect will originate from the mouse down position.        
        /// </summary>
        /// <param name="element"></param>        
        public static bool GetIsCentered(DependencyObject element) => (bool)element.GetValue(IsCenteredProperty);
        /// <summary>
        /// Set to <c>true</c> to cause the ripple to originate from the centre of the 
        /// content.  Otherwise the effect will originate from the mouse down position.        
        /// </summary>
        public static readonly DependencyProperty IsCenteredProperty = DependencyProperty.RegisterAttached(
            "IsCentered", typeof(bool), typeof(RippleAssist), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Set to <c>True</c> to disable ripple effect
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetIsDisabled(DependencyObject element, bool value) => element.SetValue(IsDisabledProperty, value);
        /// <summary>
        /// Set to <c>True</c> to disable ripple effect
        /// </summary>
        /// <param name="element"></param>        
        public static bool GetIsDisabled(DependencyObject element) => (bool)element.GetValue(IsDisabledProperty);
        /// <summary>
        /// Set to <c>True</c> to disable ripple effect
        /// </summary>
        public static readonly DependencyProperty IsDisabledProperty = DependencyProperty.RegisterAttached(
            "IsDisabled", typeof(bool), typeof(RippleAssist), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetRippleSizeMultiplier(DependencyObject element, double value) => element.SetValue(RippleSizeMultiplierProperty, value);
        public static double GetRippleSizeMultiplier(DependencyObject element) => (double)element.GetValue(RippleSizeMultiplierProperty);

        public static readonly DependencyProperty RippleSizeMultiplierProperty = DependencyProperty.RegisterAttached(
            "RippleSizeMultiplier", typeof(double), typeof(RippleAssist), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetFeedback(DependencyObject element, Brush value) => element.SetValue(FeedbackProperty, value);
        public static Brush GetFeedback(DependencyObject element) => (Brush)element.GetValue(FeedbackProperty);
        public static readonly DependencyProperty FeedbackProperty = DependencyProperty.RegisterAttached(
            "Feedback", typeof(Brush), typeof(RippleAssist), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender));

        public static void SetRippleOnTop(DependencyObject element, bool value) => element.SetValue(RippleOnTopProperty, value);
        public static bool GetRippleOnTop(DependencyObject element) => (bool)element.GetValue(RippleOnTopProperty);
        public static readonly DependencyProperty RippleOnTopProperty = DependencyProperty.RegisterAttached(
            "RippleOnTop", typeof(bool), typeof(RippleAssist),
            new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender));
    }
}