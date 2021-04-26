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

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Microsoft.Expression.Drawing.dll

            var basePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var demoPath = Directory.GetParent(basePath).Parent.Parent.Parent.Parent.Parent.FullName;
            var demoFiles = Directory.GetFiles(demoPath + "\\TestDemo\\Demos", "*.xaml").ToList();

            DemoFiles = new HashSet<string>();
            foreach (var file in demoFiles)
            {
                DemoFiles.Add(file);
            }

            ControlNamesList = new List<string>();
            foreach (var file in DemoFiles)
            {
                var fileName = System.IO.Path.GetFileName(file);
                ControlNamesList.Add(fileName.Replace("Demo.xaml", ""));
            }

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

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            var className = listView.SelectedItem.ToString().Insert(listView.SelectedItem.ToString().Length, "Demo");
            ControlContent = ClassFactory.GetSpecificClass(listView.SelectedIndex);
            //ControlContent = Assembly.GetExecutingAssembly().CreateInstance($"TestDemo.Demos.{className}") as UserControl;
            //XmlReader xr = new XmlTextReader(DemoFiles.FirstOrDefault(f => f.Contains(className)));
            //Content = XamlReader.Load(xr) as DependencyObject;
        }
    }
}
