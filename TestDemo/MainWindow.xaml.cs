using CookPopularControl.Controls.DialogBox;
using CookPopularControl.Controls.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
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
using TestDemo.Demos;
using PropertyChanged;
using System.IO;
using System.Xml;
using System.Windows.Markup;
using System.Reflection;
using CookPopularControl.Communal.Attached;
using CookPopularControl.Tools.Helpers;
using System.Windows.Media.Animation;

namespace TestDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class MainWindow : Window
    {
        public List<string> ControlNamesList { get; set; }
        public object ControlContent { get; set; }

        private HashSet<string> DemoFiles;


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            DemoFiles = new HashSet<string>();

            ControlNamesList = new List<string>();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Microsoft.Expression.Drawing.dll
            //new Window() { Content = "123213",Name="test1" }.Show();
            //new Window() { Content = "456456",Name="test2" }.Show();

            var basePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var demoPath = Directory.GetParent(basePath).Parent.Parent.Parent.Parent.Parent.FullName;
            var demoFiles = Directory.GetFiles(demoPath + "\\TestDemo\\Demos", "*.xaml").ToList();

            foreach (var file in demoFiles)
            {
                DemoFiles.Add(file);
            }

            foreach (var file in DemoFiles)
            {
                var fileName = System.IO.Path.GetFileName(file);
                ControlNamesList.Add(fileName.Replace("Demo.xaml", ""));
            }

            ControlNamesList.RemoveRange(0, 2);
            listBox.Width = 0;
            ControlContent = ClassFactory.GetSpecificClass(0);
            //CollectionViewSource.GetDefaultView()

            //using (FileStream fs = new FileStream(@"D:\WPFSourceCode\CookPopularControl\Output\bin\AnyCPU\Debug\net461\1.xaml", FileMode.Open, FileAccess.Read))
            //{
            //    ControlContent = XamlReader.Load(fs) as UIElement;
            //}

            //Uri uri = new Uri("/Demos/ButtonDemo.xaml", UriKind.Relative);
            //Stream stream = Application.GetResourceStream(uri).Stream;
            //var element = XamlReader.Load(stream) as FrameworkElement;
            //ControlContent = element;

            //XmlReader xr = new XmlTextReader("<local:ButtonDemo />");
            //ControlContent = XamlReader.Load(xr) as UserControl;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ControlContent = ClassFactory.GetSpecificClass(listBox.SelectedIndex);
            //var className = listBox.SelectedItem.ToString().Insert(listBox.SelectedItem.ToString().Length, "Demo");
            //ControlContent = Assembly.GetExecutingAssembly().CreateInstance($"TestDemo.Demos.{className}") as UserControl;
            //XmlReader xr = new XmlTextReader(DemoFiles.FirstOrDefault(f => f.Contains(className)));
            //Content = XamlReader.Load(xr) as DependencyObject;
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            FrameworkElementBaseAttached.SetIconGeometry(sender as FrameworkElement, ResourceHelper.GetResource<Geometry>("LeftTriangleGeometry"));
            listBox.BeginAnimation(ListBox.WidthProperty, CreateAnimation(0, 150));
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            FrameworkElementBaseAttached.SetIconGeometry(sender as FrameworkElement, ResourceHelper.GetResource<Geometry>("RightTriangleGeometry"));
            listBox.BeginAnimation(ListBox.WidthProperty, CreateAnimation(150, 0));
        }

        private DoubleAnimation CreateAnimation(double from, double to)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = from;
            animation.To = to;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.7));

            return animation;
        }
    }
}
