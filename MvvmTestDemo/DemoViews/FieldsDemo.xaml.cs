using Prism.Commands;
using PropertyChanged;
using System;
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

        public DelegateCommand<object> Sure123Command => new Lazy<DelegateCommand<object>>(() => new DelegateCommand<object>(OnSure123)).Value;

        private void OnSure123(object obj)
        {
         
        }

        public FieldsDemo()
        {
            InitializeComponent();
        }

        private void NumericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var s = num5.Value;
        }
    }
}
