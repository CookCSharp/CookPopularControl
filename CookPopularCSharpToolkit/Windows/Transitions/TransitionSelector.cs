using System.Windows;

namespace CookPopularCSharpToolkit.Windows.Transitions
{
    /// <summary>
    /// ������ھ��������в�ͬ��ת��,����SelectTransition�����Է��ص�Ӧ�õ�ת��
    /// </summary>
    public class TransitionSelector : DependencyObject
    {
        public virtual TransitionBase SelectTransition(object oldContent, object newContent)
        {
            return null;
        }
    }
}
