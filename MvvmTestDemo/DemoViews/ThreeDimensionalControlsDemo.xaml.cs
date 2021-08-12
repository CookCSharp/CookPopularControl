using CookPopularControl.Communal.Data.Enum;
using CookPopularControl.Tools.Extensions;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// _3DControlsDemo.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class ThreeDimensionalControlsDemo : UserControl
    {
        private int stateIndex = 0;
        public IEnumerable<DiffuseMaterial> ImageMaterials { get; set; }

        public ThreeDimensionalControlsDemo()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            OnAlertor3DAutoChanged(3, typeof(AlertorState).GetEnumValues());
            //ImageMaterials = new ReadOnlyCollection<DiffuseMaterial>(GetImageFiles(ImageFilesPath));
        }

        private void OnAlertor3DAutoChanged(int sleep, Array array)
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3),
                IsEnabled = true,
            };
            timer.Tick += (s, e) =>
            {
                alertor3D.State = (AlertorState)array.GetValue(stateIndex);
                stateIndex++;
                if (stateIndex >= array.Length)
                    stateIndex = 0;
            };
            timer.Start();
        }

        private static readonly string SlnPath =  Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
        private static readonly string ImageFilesPath = SlnPath + "\\MvvmTestDemo\\Resources\\Effect";
        private ReadOnlyCollection<DiffuseMaterial> GetImageFiles(string path)
        {
            IList<string> files = Directory.GetFiles(path, "*.png").ToList();
            if (files.Count > 0)
            {
                return files.Select(file =>
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(file);
                    bitmapImage.DecodePixelWidth = 320;
                    bitmapImage.DecodePixelHeight = 240;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();

                    ImageBrush imageBrush = new ImageBrush(bitmapImage);
                    imageBrush.Stretch = Stretch.UniformToFill;
                    imageBrush.ViewportUnits = BrushMappingMode.Absolute;
                    imageBrush.Freeze();

                    return new DiffuseMaterial(imageBrush);
                }).ToReadOnlyCollection();
            }

            return default;
        }
    }
}
