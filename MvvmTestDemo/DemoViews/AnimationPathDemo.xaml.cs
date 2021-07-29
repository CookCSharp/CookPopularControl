using System;
using System.Windows;
using System.Windows.Controls;

namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// GifDemo.xaml 的交互逻辑
    /// </summary>
    public partial class AnimationPathDemo : UserControl
    {
        public AnimationPathDemo()
        {
            InitializeComponent();
        }

        private void GifBox_Completed(object sender, EventArgs e)
        {
            MessageBox.Show("123");
        }
    }
}
