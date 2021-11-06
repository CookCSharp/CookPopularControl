using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CookPopularCSharpToolkit.Windows.Transitions
{
    /// <summary>
    /// ������ָ���µ����,ʹ���������ľ����ݵ����ݻ�����㡣
    /// ������������Ϊ��-1,0����ʹ���ݻõ�Ƭ����໬��
    /// </summary>
    public class TranslateTransition : TransitionBase
    {
        static TranslateTransition()
        {
            ClipToBoundsProperty.OverrideMetadata(typeof(TranslateTransition), new FrameworkPropertyMetadata(true));
        }

        public Duration Duration
        {
            get { return (Duration)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(Duration), typeof(TranslateTransition), new UIPropertyMetadata(Duration.Automatic));

        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof(Point), typeof(TranslateTransition), new UIPropertyMetadata(new Point()));

        public Point EndPoint
        {
            get { return (Point)GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); }
        }

        public static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register("EndPoint", typeof(Point), typeof(TranslateTransition), new UIPropertyMetadata(new Point()));

        protected internal override void BeginTransition(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            TranslateTransform tt = new TranslateTransform(StartPoint.X * transitionElement.ActualWidth, StartPoint.Y * transitionElement.ActualHeight);

            if (IsNewContentTopmost)
                newContent.RenderTransform = tt;
            else
                oldContent.RenderTransform = tt;

            DoubleAnimation da = new DoubleAnimation(EndPoint.X * transitionElement.ActualWidth, Duration);
            tt.BeginAnimation(TranslateTransform.XProperty, da);

            da.To = EndPoint.Y * transitionElement.ActualHeight;
            da.Completed += delegate
            {
                EndTransition(transitionElement, oldContent, newContent);
            };
            tt.BeginAnimation(TranslateTransform.YProperty, da);
        }

        protected override void OnTransitionEnded(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            newContent.ClearValue(ContentPresenter.RenderTransformProperty);
            oldContent.ClearValue(ContentPresenter.RenderTransformProperty);
        }
    }
}
