using CookPopularControl.Windows;
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
                "ShowMessageDialogSuccess" => MessageDialog.ShowSuccess("MessageDialogTextBlockStyleMessageDialogTextBlockStyleMessageDialogTextBlockStyleMessageDialogTextBlockStyle写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子_I写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子_Info_Info写代码的厨子_Info写代码的厨子_Info写代码的厨子_Infonfo顶顶顶大大大大大大顶顶顶顶滴滴答答而温热我热温热温热微软广泛大概豆腐干梵蒂冈地方官地方官地方官法国地方官梵蒂冈豆腐干豆腐干", "Test"),//MessageDialog.ShowSuccess("写代码的厨子_Success", "Chance"),
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

        private void ButtonTest_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog.ShowInfo("写代码的厨子_Info", "Chance");
            MessageDialog.ShowWarning("写代码的厨子_Waning", "Chance");

            MessageBox.Show("123");
        }
    }
}
