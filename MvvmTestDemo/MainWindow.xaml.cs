using CookPopularControl.Windows;
using CookPopularCSharpToolkit.Communal;
using PropertyChanged;
using System;
using System.Windows;

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
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var s = DemoMainWindow.Background;
            try
            {
                Logger.Debug("Debug测试测试");
                Logger.Info("Info测试测试");
                throw new Exception("错误测试");
            }
            catch (Exception ex)
            {
                Logger.Warn("Warning测试测试", ex);
                Logger.Error("Error测试测试", ex);
                Logger.Fatal("Fatal测试测试", ex);
            }
        }
    }
}
