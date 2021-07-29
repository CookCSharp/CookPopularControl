using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// MediaPlayerDemo.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class MediaPlayerDemo : UserControl
    {
        public List<Uri> MediaUris { get; set; }

        public MediaPlayerDemo()
        {
            InitializeComponent();

            this.DataContext = this;
            this.Loaded += MediaPlayerDemo_Loaded;
        }

        private void MediaPlayerDemo_Loaded(object sender, RoutedEventArgs e)
        {
            var path = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.Parent.Parent.FullName + "\\MvvmTestDemo\\Resources\\MediaPlayerVideo";
            MediaUris = new List<Uri>();

            MediaUris.Add(new Uri(path + "\\Video1.mp4"));
            MediaUris.Add(new Uri(path + "\\Video2.mp4"));
            MediaUris.Add(new Uri(path + "\\Video3.mp4"));

            mediaPlayer.CurrentUri = MediaUris[0];
        }
    }
}
