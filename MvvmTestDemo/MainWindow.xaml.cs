using CookPopularControl.Communal.Data.Args;
using CookPopularControl.Controls.Windows;
using CookPopularControl.Tools.Helpers;
using MvvmTestDemo.Commumal;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MvvmTestDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class MainWindow : SideBarWindow
    {
        public List<string> ControlNamesList { get; set; }
        public object ControlContent { get; set; }
        public int DemosViewelectedIndex { get; set; } = -1;
        public bool IsOpenNotifyIconSwitch { get; set; }

        private HashSet<string> DemoFiles;


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            this.Closing += (s, e) =>
            {
                if (!IsOpenNotifyIconSwitch)
                    Environment.Exit(0);
            };

            DemoFiles = new HashSet<string>();
            ControlNamesList = new List<string>();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Microsoft.Expression.Drawing.dll
            //System.Windows.Threading.Dispatcher.PushFrame
            SetControlsList();
        }

        private void SetControlsList()
        {
            //new Window() { Content = "123213", Name = "test1" }.Show();
            //new Window() { Content = "456456", Name = "test2" }.Show();

            var basePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var demoPath = Directory.GetParent(basePath).Parent.Parent.Parent.Parent.FullName;
            var demoFiles = Directory.GetFiles(demoPath + "\\MvvmTestDemo\\DemosView", "*.xaml").ToList();

            foreach (var file in demoFiles)
            {
                DemoFiles.Add(file);
            }

            foreach (var file in DemoFiles)
            {
                var fileName = System.IO.Path.GetFileName(file);
                ControlNamesList.Add(fileName.Replace("Demo.xaml", ""));
            }

            var currentAssemblyName = this.GetType().Assembly.GetName().Name;
            foreach (var className in ControlNamesList)
            {
                var instance = ObjectFactory.CreateInstanceInActivator($"{className}Demo");
                //var instance = ObjectCreateHelper.CreateInstanceInClassName($"{currentAssemblyName}.DemosView", $"{className}Demo");
                ObjectFactory.Register(className, instance);
            }

            DemosViewelectedIndex = 0;
            //CollectionViewSource.GetDefaultView(viewmodel);// 返回给定源的默认视图。
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ControlContent = ObjectFactory.ResolveIntance(ControlNamesList[DemosViewelectedIndex]);
        }

        private void DemoMainWindow_IsShowSideBarChanged(object sender, RoutedPropertySingleEventArgs<bool> e)
        {
            if ((bool)e.Value)
                listBox.BeginAnimation(ListBox.WidthProperty, AnimationHelper.CreateDoubleAnimation(0, 150, 0.5));
            else
                listBox.BeginAnimation(ListBox.WidthProperty, AnimationHelper.CreateDoubleAnimation(150, 0, 0.5));
        }

        private void DemoMainWindow_SettingClick(object sender, RoutedEventArgs e)
        {
            MessageDialog.Show("打开了设置");
        }
    }
}
