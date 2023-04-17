using CookPopularControl.Controls;
using CookPopularCSharpToolkit.Communal;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.Win32;
using Prism.Commands;
using PropertyChanged;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;

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

        public string AutoText { get; set; } = "Cook:写代码的厨子&";

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
            //var s = num5.Value;
        }

        private int index = 1;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AutoText += $"Title{index}:Chance{index}&";
            index++;
            //AutoText = "Title1:写代码的厨子 Title2:Chance Title3:Cook";
        }

        private void Browser_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txt.Text = folderBrowserDialog.SelectedPath;
            }
        }
    }
}
