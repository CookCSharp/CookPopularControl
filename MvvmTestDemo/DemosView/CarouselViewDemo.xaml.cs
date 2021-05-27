using System;
using System.Collections.Generic;
using System.IO;
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
using CookPopularControl.Controls.CarouselView;
using PropertyChanged;

namespace MvvmTestDemo.DemosView
{
    /// <summary>
    /// CarouselViewDemo.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class CarouselViewDemo : UserControl
    {
        public List<ImageItemInfo> ImageLists { get; set; }

        public CarouselViewDemo()
        {
            InitializeComponent();

            ImageLists = new List<ImageItemInfo>();
            this.DataContext = this;

            this.Loaded += CarouselViewDemo_Loaded;
            this.Unloaded += CarouselViewDemo_UnLoaded;
        }

        private void CarouselViewDemo_Loaded(object sender, RoutedEventArgs e)
        {
            var path = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.Parent.Parent.FullName + "\\MvvmTestDemo\\Resources\\CarouselViewImages";
            ImageLists.Add(new ImageItemInfo { ImgUri = path + "\\1.jpg" });
            ImageLists.Add(new ImageItemInfo { ImgUri = path + "\\2.jpg" });
            ImageLists.Add(new ImageItemInfo { ImgUri = path + "\\3.jpg" });
            ImageLists.Add(new ImageItemInfo { ImgUri = path + "\\4.jpg" });
            ImageLists.Add(new ImageItemInfo { ImgUri = path + "\\5.jpg" });
            ImageLists.Add(new ImageItemInfo { ImgUri = path + "\\6.jpg" });
            ImageLists.Add(new ImageItemInfo { ImgUri = path + "\\7.jpg" });
            ImageLists.Add(new ImageItemInfo { ImgUri = path + "\\8.jpg" });
            ImageLists.Add(new ImageItemInfo { ImgUri = path + "\\9.jpg" });
        }

        private void CarouselViewDemo_UnLoaded(object sender, RoutedEventArgs e)
        {
            ImageLists.Clear();
        }
    }
}
