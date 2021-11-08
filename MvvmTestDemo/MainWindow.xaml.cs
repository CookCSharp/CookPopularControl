using CookPopularControl.Windows;
using PropertyChanged;

namespace MvvmTestDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class MainWindow : SideBarWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            //this.Closing += (s, e) =>
            //{
            //    if (!IsOpenNotifyIconSwitch)
            //        Environment.Exit(0);
            //};
        }
    }
}
