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
        /// 当从TransitionPresenter的Visual树中删除元素时调用
        /// </summary>
        protected internal virtual void BeginTransition(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            EndTransition(transitionElement, oldContent, newContent);
        }

        /// <summary>
        /// 转换完成时调用此方法
        /// </summary>
        protected void EndTransition(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
            OnTransitionEnded(transitionElement, oldContent, newContent);

            transitionElement.OnTransitionCompleted();
        }

        /// <summary>
        /// 在过渡结束时执行清理
        /// </summary>
        protected virtual void OnTransitionEnded(TransitionPresenter transitionElement, ContentPresenter oldContent, ContentPresenter newContent)
        {
        }

        /// <summary>
        /// 返回元素的克隆并将其隐藏在主树中
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
