using CookPopularControl.Controls.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MvvmTestDemo.DemosView
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
