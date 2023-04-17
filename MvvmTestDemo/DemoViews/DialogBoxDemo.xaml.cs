using CookPopularControl.Controls;
using MvvmTestDemo.DemoViewModels;
using MvvmTestDemo.UserControls;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PropertyChanged;


namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// DialogBoxDemo.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class DialogBoxDemo : UserControl
    {
        public string Text { get; set; }

        public DialogBoxDemo()
        {
            InitializeComponent();
        }

        private DialogBox dialogBox;
        private async void ButtonDefault_Click(object sender, RoutedEventArgs e)
        {
            //var win = App.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            //var adornerDemo = new Adorner { Width = win.ActualWidth, Height = win.ActualHeight};
            ////dialogBox = DialogBox.Show(adornerDemo);
            //dialogBox = DialogBox.Show<Adorner>();
            //dialogBox.MouseLeftButtonUp += (s, e) => dialogBox.Close();

            Text = await DialogBox.Show<Adorner>().Initialize<AdornerViewModel>(vm => { vm.Message = Text; vm.Result = Text; }).GetResultAsync<string>();
        }

        private void ButtonInherit_Click(object sender, RoutedEventArgs e)
        {           
            dialogBox = DialogBox.Show<Adorner>("DialogBoxContainer");
        }
    }
}
