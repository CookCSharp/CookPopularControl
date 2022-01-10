using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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
using CookPopularControl.Controls;
using PropertyChanged;


namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// PropertyGridDemo.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class PropertyGridDemo : UserControl
    {
        public PropertyGridDemoModel DemoModel { get; set; }

        public PropertyGridDemo()
        {
            InitializeComponent();

            var s = typeof(PropertyGridDemoModel);
            var a = s.GetProperty("Integer").GetCustomAttributes();

            DemoModel = new PropertyGridDemoModel
            {
                String = "写代码的厨子",
                Integer = 100,
                Double = 23.5,
                Boolean = false,
                Enum = Country.China,
                Lists = new List<string>() { "Chance1","Chance2","Chance3"},
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Stretch,
                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MvvmTestDemo;component/Resources/Gif/tomcat.jpg")),
            };      
        }
    }


    public class PropertyGridDemoModel
    {
        [Category("Category1")]
        [Description("这是String")]
        public string String { get; set; }

        [Category("Category1")]
        [Description("这是Int")]
        [NumberRange(1,110)]
        public int Integer { get; set; }

        [Category("Category1")]
        [Description("这是Double")]
        [NumberRange(1.1, 110.5)]
        public double Double { get; set; }

        [Category("Category1")]
        [Description("这是一个布尔型")]
        public bool Boolean { get; set; }

        [Category("Category2")]
        [Description("这是Enum")]
        public Country Enum { get; set; }

        [Category("Category2")]
        [Description("这是IEnumerable")]
        [Index(2)]
        public IList<string> Lists { get; set; }

        [Description("这是图片")]
        public ImageSource ImageSource { get; set; }

        public HorizontalAlignment HorizontalAlignment { get; set; }

        public VerticalAlignment VerticalAlignment { get; set; }
    }

    public enum Country
    {
        China,
        Japan,
        Italy,
        USA,
        UK
    }
}
