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

namespace MvvmTestDemo.UserControls
{
    /// <summary>
    /// OverViewDemo.xaml 的交互逻辑
    /// </summary>
    public partial class OverViewDemo : UserControl
    {
        public OverViewDemo()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //var s = e.Source;
            //var s1 = e.OriginalSource;
            //var tree = sender as TreeView;
        }

        private void TreeView_Selected(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as TreeViewItem;
            //var s = item.Items.Count;
            //System.Diagnostics.Debug.WriteLine("111");
        }

        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("222");
        }

        private void TreeView_Expanded(object sender, RoutedEventArgs e)
        {

        }
    }
}
