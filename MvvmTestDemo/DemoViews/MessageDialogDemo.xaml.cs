using CookPopularControl.Controls.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// MessageDialogDemo.xaml 的交互逻辑
    /// </summary>
    public partial class MessageDialogDemo : UserControl
    {
        public MessageDialogDemo()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var messageBoxResult = (sender as Button).Content switch
            {
                "ShowMessageDialogSuccess" => MessageDialog.ShowSuccess("写代码的厨子_Success", "Chance"),
                "ShowMessageDialogInfo" => MessageDialog.ShowInfo("写代码的厨子_Info", "Chance"),
                "ShowMessageDialogWarning" => MessageDialog.ShowWarning("写代码的厨子_Warning", "Chance"),
                "ShowMessageDialogError" => MessageDialog.ShowError("写代码的厨子_Error", "Chance"),
                "ShowMessageDialogFatal" => MessageDialog.ShowFatal("写代码的厨子_Fatal", "Chance"),
                "ShowMessageDialogQuestion" => MessageDialog.ShowQuestion("写代码的厨子_Quetion", "Chance"),
                "ShowMessageDialogCustom" => MessageDialog.Show("写代码的厨子_Custom", "Chance", MessageBoxButton.YesNoCancel, MessageBoxImage.None, MessageBoxResult.None),
                _ => throw new NotImplementedException(),
            };

            //Task.Run(() =>
            //{
            //    MessageDialog.ShowSuccess("写代码的厨子_Success", "Chance");
            //});
        }
    }
}
