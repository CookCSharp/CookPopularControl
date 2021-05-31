using CookPopularControl.Controls.DialogBox;
using MvvmTestDemo.UserControls;
using System.Windows;
using System.Windows.Controls;


namespace MvvmTestDemo.DemosView
{
    /// <summary>
    /// DialogBoxDemo.xaml 的交互逻辑
    /// </summary>
    public partial class DialogBoxDemo : UserControl
    {
        public DialogBoxDemo()
        {
            InitializeComponent();
        }

        private DialogBox dialogBox;
        private void ButtonDefault_Click(object sender, RoutedEventArgs e)
        {
            dialogBox = DialogBox.Show<AdornerDemo>();
        }

        private void ButtonInherit_Click(object sender, RoutedEventArgs e)
        {
            dialogBox = DialogBox.Show<AdornerDemo>("DialogBoxContainer");
        }
    }
}
