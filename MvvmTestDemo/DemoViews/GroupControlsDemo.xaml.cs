using CookPopularControl.Controls;
using CookPopularCSharpToolkit.Communal;
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

        private void SearchBar_StartSearch(object sender, RoutedEventArgs e)
        {
            //StreamResourceInfo sri = Application.GetResourceStream(new Uri("/Resources/Cursors/myCursor.cur", UriKind.Relative));
            //(sender as SearchBar).Cursor = new Cursor(sri.Stream);
            (sender as SearchBar).Cursor = CursorHelper.ConvertToCursor(customCursor, new Point(0.5, 0.5));
        }

        private void SearchBar_ContentChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
