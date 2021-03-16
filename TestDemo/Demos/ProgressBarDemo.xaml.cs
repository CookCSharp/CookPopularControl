using System;
using System.Collections.Generic;
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
    /// ProgressBarDemo.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class ProgressBarDemo : UserControl
    {
        public double Value { get; set; }

        public PathGeometry PathGeometry { get; set; }

        public ProgressBarDemo()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Value = 0;
            await Task.Run(() =>
            {
                for (int i = 1; i <= 100; i++)
                {
                    Value = i;
                    System.Threading.Thread.Sleep(50);
                }
            });
        }
    }
}
