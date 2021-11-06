using CookPopularCSharpToolkit.Communal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CookPopularCSharpToolkit.Windows.Transitions
{
    public class TransitionBase : DependencyObject
    {
        public bool ClipToBounds
        {
            get { return (bool)GetValue(ClipToBoundsProperty); }
            set { SetValue(ClipToBoundsProperty, ValueBoxes.BooleanBox(value)); }
        }
        public static readonly DependencyProperty ClipToBoundsProperty =
            DependencyProperty.Register("ClipToBounds", typeof(bool), typeof(TransitionBase), new UIPropertyMetadata(ValueBoxes.FalseBox));

        public bool IsNewContentTopmost
        {
            get { return (bool)GetValue(IsNewContentTopmostProperty); }
            set { SetValue(IsNewContentTopmostProperty, ValueBoxes.BooleanBox(value)); }
        }

        public static readonly DependencyProperty IsNewContentTopmostProperty =
            DependencyProperty.Register("IsNewContentTopmost", typeof(bool), typeof(TransitionBase), new UIPropertyMetadata(ValueBoxes.TrueBox));

        /// <summary>
        /// ����TransitionPresenter��Visual����ɾ��Ԫ��ʱ����
        /// </summary>
        protected internal virtual void BeginTransition(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            EndTransition(transitionElement, oldContent, newContent);
        }

        /// <summary>
        /// ת�����ʱ���ô˷���
        /// </summary>
        protected void EndTransition(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            OnTransitionEnded(transitionElement, oldContent, newContent);

            transitionElement.OnTransitionCompleted();
        }

        /// <summary>
        /// �ڹ��ɽ���ʱִ������
        /// </summary>
        protected virtual void OnTransitionEnded(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
        }

        /// <summary>
        /// ����Ԫ�صĿ�¡������������������
        /// </summary>
        protected static Brush CreateBrush(ContentPresenter content)
        {
            ((Decorator)content.Parent).Visibility = Visibility.Hidden;

            VisualBrush brush = new VisualBrush(content);
            brush.ViewportUnits = BrushMappingMode.Absolute;
            RenderOptions.SetCachingHint(brush, CachingHint.Cache);
            RenderOptions.SetCacheInvalidationThresholdMinimum(brush, 40);
            return brush;
        }
    }
}
