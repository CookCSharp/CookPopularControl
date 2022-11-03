using CookPopularControl.DialogBox;
using MvvmTestDemo.UserControls;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace MvvmTestDemo.DemoViews
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
            var win = App.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            var adornerDemo = new Adorner { Width = win.ActualWidth, Height = win.ActualHeight };
            dialogBox = DialogBox.Show(adornerDemo);
            dialogBox.MouseLeftButtonUp += (s, e) => dialogBox.Close();
        }

        private void ButtonInherit_Click(object sender, RoutedEventArgs e)
        {
            dialogBox = DialogBox.Show<Adorner>("DialogBoxContainer");
        }
    }
}
