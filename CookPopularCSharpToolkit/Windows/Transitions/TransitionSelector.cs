using System.Windows;

namespace CookPopularCSharpToolkit.Windows.Transitions
{
    /// <summary>
    /// 允许基于旧内容运行不同的转换,覆盖SelectTransition方法以返回到应用的转换
    /// </summary>
    public class TransitionSelector : DependencyObject
    {
        public virtual TransitionBase SelectTransition(object oldContent, object newContent)
        {
            return null;
        }
    }
}
