using CookPopularControl.Controls;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MvvmTestDemo.DemoViews
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
            var path = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.Parent.FullName + "\\MvvmTestDemo\\Resources\\CarouselViewImages";
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
