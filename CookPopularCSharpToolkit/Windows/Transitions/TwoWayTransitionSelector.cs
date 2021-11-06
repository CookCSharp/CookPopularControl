using System.Windows;

namespace CookPopularCSharpToolkit.Windows.Transitions
{
    public enum TransitionDirection
    {
        Forward,
        Backward,
    }

    /// <summary>
    /// 根据方向属性选择前向和后向转换
    /// </summary>
    public class TwoWayTransitionSelector : TransitionSelector
    {
        public TwoWayTransitionSelector() { }

        public TransitionBase Forward
        {
            get { return (TransitionBase)GetValue(ForwardProperty); }
            set { SetValue(ForwardProperty, value); }
        }
        public static readonly DependencyProperty ForwardProperty =
            DependencyProperty.Register("Forward", typeof(TransitionBase), typeof(TwoWayTransitionSelector), new UIPropertyMetadata(null));

        public TransitionBase Backward
        {
            get { return (TransitionBase)GetValue(BackwardProperty); }
            set { SetValue(BackwardProperty, value); }
        }

        public static readonly DependencyProperty BackwardProperty =
            DependencyProperty.Register("Backward", typeof(TransitionBase), typeof(TwoWayTransitionSelector), new UIPropertyMetadata(null));

        public TransitionDirection Direction
        {
            get { return (TransitionDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(TransitionDirection), typeof(TwoWayTransitionSelector), new UIPropertyMetadata(TransitionDirection.Forward));

        public override TransitionBase SelectTransition(object oldContent, object newContent)
        {
            return Direction == TransitionDirection.Forward ? Forward : Backward;
        }
    }
}
