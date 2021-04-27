using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CookPopularControl.Controls.CarouselView
{
    /// <summary>
    /// ImageAnimation.xaml 的交互逻辑
    /// </summary>
    public partial class ImageAnimation : UserControl
    {
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(ImageAnimation), new UIPropertyMetadata(0.0));

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(ImageAnimation), new UIPropertyMetadata(0.0));

        public double ScaleX
        {
            get { return (double)GetValue(ScaleXProperty); }
            set { SetValue(ScaleXProperty, value); }
        }
        public static readonly DependencyProperty ScaleXProperty =
            DependencyProperty.Register("ScaleX", typeof(double), typeof(ImageAnimation), new UIPropertyMetadata(1.0));

        public double ScaleY
        {
            get { return (double)GetValue(ScaleYProperty); }
            set { SetValue(ScaleYProperty, value); }
        }
        public static readonly DependencyProperty ScaleYProperty =
            DependencyProperty.Register("ScaleY", typeof(double), typeof(ImageAnimation), new UIPropertyMetadata(1.0));

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(ImageAnimation), new PropertyMetadata(0.0));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageAnimation), new PropertyMetadata(default(ImageSource)));

        public string ImageTagName
        {
            get { return (string)GetValue(ImageTagNameProperty); }
            set { SetValue(ImageTagNameProperty, value); }
        }
        public static readonly DependencyProperty ImageTagNameProperty =
            DependencyProperty.Register("ImageTagName", typeof(string), typeof(ImageAnimation), new PropertyMetadata(default(string)));

        public string WholeScenePath
        {
            get { return (string)GetValue(WholeScenePathProperty); }
            set { SetValue(WholeScenePathProperty, value); }
        }
        public static readonly DependencyProperty WholeScenePathProperty =
            DependencyProperty.Register("WholeScenePath", typeof(string), typeof(ImageAnimation), new PropertyMetadata(default(string)));


        public ImageAnimation()
        {
            InitializeComponent();

            this.DataContext = this;
            this.Loaded += ImageAnimation_Loaded;
        }

        private void ImageAnimation_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
