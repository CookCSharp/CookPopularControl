using CookPopularControl.Communal.Data;
using CookPopularControl.Controls;
using CookPopularControl.Controls.Windows.Printers;
using CookPopularControl.Windows;
using CookPopularCSharpToolkit.Windows;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using MvvmTestDemo.DemoDragables;
using MvvmTestDemo.DemoViewModels;
using MvvmTestDemo.UserControls;
using MvvmTestDemo.Windows;
using PropertyChanged;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// WindowsDemo.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class WindowsDemo : UserControl
    {
        public string Text { get; set; }
        public WindowsDemo()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            object p = btn.Name switch
            {
                "PrintPreviewWindow" => Show<PrintPreviewWindow>(),
                "DragableWindows" => Show<QuickStartWindow>(),
                "NoneTitleBarWindow" => Show<NoneWindow>(),
                "FixedSizeWindow" => Show<FixedSizeWindow>(),
                "DialogWindow" => DialogWindowShow(),
                "DemoDialogWindow" => Show<DemoDialogWindow>(),
                "ToastMessage" => ShowToast(),
                _ => throw new NotImplementedException(),
            };
        }

        private async Task<string> DialogWindowShow()
        {
            Text = await CookPopularControl.Windows.DialogWindow.Show<Adorner>()
                .Initialize<AdornerViewModel>(vm => { vm.Message = Text; vm.Result = Text; })
                .GetResultAsync<string>();

            return Text;
        }

        private object ShowToast()
        {
            ToastMessage.ShowWarning("123123123");
            return null;
        }

        private object Show<T>() where T : Window, new()
        {
            var win = new T();
            win.Show();

            return default;
        }
    }
}
