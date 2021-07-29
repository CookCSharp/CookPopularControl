using CookPopularControl.Controls.Groups;
using CookPopularControl.Tools.Helpers;
using PropertyChanged;
using System.Windows;
using System.Windows.Controls;

namespace MvvmTestDemo.DemoViews
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
