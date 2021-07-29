using PropertyChanged;
using System.Windows;
using System.Windows.Controls;

namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// TextBoxDemo.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class FieldsDemo : UserControl
    {
        public string TextContent1 { get; set; }

        public string TextContent2 { get; set; }

        public double NumericValue { get; set; }

        public FieldsDemo()
        {
            InitializeComponent();
        }

        private void NumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
