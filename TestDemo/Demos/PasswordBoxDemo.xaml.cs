using CookPopularControl.Communal.Attached;
using CookPopularControl.Controls.PasswordBox;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
using PropertyChanged;

namespace TestDemo.Demos
{
    /// <summary>
    /// PasswordBoxDemo.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class PasswordBoxDemo : UserControl
    {
        public PasswordBoxDemo()
        {
            InitializeComponent();
        }

        public string PasswordContent { get; set; }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            var pwdAttached1 = PasswordBoxAssistant.GetPassword(Pwd1);
            var pwd1 = Pwd1.Password;
            var bol1 = pwdAttached1.Equals(pwd1);

            var pwdAttached2 = PasswordBoxAssistant.GetPassword(Pwd2);
            var pwd2 = Pwd2.Password;
            var bol2 = pwdAttached2.Equals(pwd2);

            var pwdAttached3 = PasswordBoxAssistant.GetPassword(Pwd3);
            var pwd3 = Pwd3.Password;
            var bol3 = pwdAttached3.Equals(pwd3);
        }

        private void Pwd1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pwd = (sender as PasswordBox).Password;
        }

        private void Pwd2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pwd = (sender as PasswordBox).Password;
        }
        

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            var s = PasswordContent;
        }
    }
}
