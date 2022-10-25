using CookPopularControl.Communal;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// ComboBoxDemo.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class ComboBoxDemo : UserControl
    {
        public ObservableCollection<object> Lists { get; set; }

        public ObservableCollection<SelectorItem> IconLists { get; set; }

        public ObservableCollection<SelectorItem> ImageLists { get; set; }

        public ComboBoxDemo()
        {
            InitializeComponent();

            this.DataContext = this;
            this.Loaded += ComboBoxDemo_Loaded;
        }

        private void ComboBoxDemo_Loaded(object sender, RoutedEventArgs e)
        {
            Lists = new ObservableCollection<object>() { "111", "222", "333" };
            IconLists = new ObservableCollection<SelectorItem>();
            ImageLists = new ObservableCollection<SelectorItem>();

            IconLists.Add(new SelectorItem { GeometryData = App.Current.Resources["EyeGeometry"] as Geometry, Content = "第一个Icon" });
            IconLists.Add(new SelectorItem { GeometryData = App.Current.Resources["LockGeometry"] as Geometry, Content = "第二个Icon" });
            IconLists.Add(new SelectorItem { GeometryData = App.Current.Resources["LeafGeometry"] as Geometry, Content = "第三个Icon" });

            //var source1 = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "tomcat.jpg", UriKind.Absolute));
            var source1 = new BitmapImage(new Uri("/MvvmTestDemo;component/Resources/Gif/tomcat.jpg", UriKind.Relative));
            var source2 = new BitmapImage(new Uri("/MvvmTestDemo;component/Resources/Gif/programmer.gif", UriKind.Relative));
            var source3 = new BitmapImage(new Uri("/MvvmTestDemo;component/Resources/Gif/timg.jpg", UriKind.Relative));
            ImageLists.Add(new SelectorItem { ImageSource = source1, Content = "第一张图片" });
            ImageLists.Add(new SelectorItem { ImageSource = source2, Content = "第二张图片" });
            ImageLists.Add(new SelectorItem { ImageSource = source3, Content = "第三张图片" });
        }

        private void ComboBox_IsItemChecked(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var comboBox = sender as ComboBox;
            var o = e.OriginalSource as ListBoxItem;
            var s = e.Source;
        }

        private void ComboBox_IsItemDelete(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var comboBox = sender as ComboBox;
            var o = e.OriginalSource as ListBoxItem;
            var s = e.Source;

            Lists.Remove(o.Content);
        }
    }
}
