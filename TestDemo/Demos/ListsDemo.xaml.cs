using CookPopularControl.Communal.Attached;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TestDemo.Demos
{
    /// <summary>
    /// ListsDemo.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class ListsDemo : UserControl
    {
        public ObservableCollection<object> Lists { get; set; }

        public ObservableCollection<SelectorItem> IconLists { get; set; }

        public ObservableCollection<SelectorItem> ImageLists { get; set; }

        public ObservableCollection<PersonInfo> PersonInfos { get; set; }

        public ListsDemo()
        {
            InitializeComponent();

            this.DataContext = this;
            this.Loaded += ListBoxDemo_Loaded;
        }

        private void ListBoxDemo_Loaded(object sender, RoutedEventArgs e)
        {
            Lists = new ObservableCollection<object>() { "Lori", "Chance", "写代码的厨子" };
            IconLists = new ObservableCollection<SelectorItem>();
            ImageLists = new ObservableCollection<SelectorItem>();

            IconLists.Add(new SelectorItem { GeometryData = App.Current.Resources["EyeGeometry"] as Geometry, Content = "第一个Iccon" });
            IconLists.Add(new SelectorItem { GeometryData = App.Current.Resources["LockGeometry"] as Geometry, Content = "第二个Iccon" });
            IconLists.Add(new SelectorItem { GeometryData = App.Current.Resources["LeafGeometry"] as Geometry, Content = "第三个Iccon" });

            var source1 = new BitmapImage(new Uri("/TestDemo;component/Resources/Gif/tomcat.jpg", UriKind.Relative));
            var source2 = new BitmapImage(new Uri("/TestDemo;component/Resources/Gif/programmer.gif", UriKind.Relative));
            var source3 = new BitmapImage(new Uri("/TestDemo;component/Resources/Gif/timg.jpg", UriKind.Relative));
            ImageLists.Add(new SelectorItem { ImageSource = source1, Content = "第一张图片" });
            ImageLists.Add(new SelectorItem { ImageSource = source2, Content = "第二张图片" });
            ImageLists.Add(new SelectorItem { ImageSource = source3, Content = "第三张图片" });

            PersonInfos = new ObservableCollection<PersonInfo>();
            PersonInfos.Add(new PersonInfo { Name = "Lori", Age = 18, Description = "Teacher" });
            PersonInfos.Add(new PersonInfo { Name = "Chance", Age = 28, Description = "Cook" });
            PersonInfos.Add(new PersonInfo { Name = "写代码的厨子", Age = 38, Description = "Cook With Programmer" });
        }

        private void ListBox_IsItemDelete(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var o = e.OriginalSource as ListBoxItem;
            Lists.Remove(o.Content);
        }
    }

    public class PersonInfo
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public string Description { get; set; }

    }
}
