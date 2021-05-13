using CookPopularControl.Controls.GroupControls;
using CookPopularControl.Tools.Helpers;
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
using System.Windows.Resources;
using System.Windows.Shapes;
using PropertyChanged;

namespace TestDemo.Demos
{
    /// <summary>
    /// GroupControls.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class GroupControlsDemo : UserControl
    {
        public string SearchContent { get; set; }

        public GroupControlsDemo()
        {
            InitializeComponent();           
        }

        private void SearchControl_StartSearch(object sender, RoutedEventArgs e)
        {
            //StreamResourceInfo sri = Application.GetResourceStream(new Uri("/Resources/Cursors/myCursor.cur", UriKind.Relative));
            //(sender as SearchControl).Cursor = new Cursor(sri.Stream);
            (sender as SearchControl).Cursor = CursorHelper.ConvertToCursor(customCursor, new Point(0.5, 0.5));
        }

        private void SearchControl_ContentChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
