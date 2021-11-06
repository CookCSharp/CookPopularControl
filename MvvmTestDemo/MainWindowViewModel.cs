using CookPopularControl.Controls;
using CookPopularControl.Controls.Windows;
using MvvmTestDemo.Commumal;
using MvvmTestDemo.UserControls;
using Prism.Commands;
using Prism.Common;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：MainViewModel
 * Author： Chance_写代码的厨子
 * Create Time：2021-07-29 15:43:35
 */
namespace MvvmTestDemo
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const string MainWindowBubbleMessageToken = "MainWindowToken";
        private OverViewDemo OverView = new OverViewDemo();
        private HashSet<string> DemoFiles;

        public ObservableCollection<string> ControlNamesList { get; set; }
        public object ControlContent { get; set; }
        public int DemoViewsSelectedIndex { get; set; }
        public bool IsOpenNotifyIconSwitch { get; set; }
        public ICommand ShowSideBarCommand { get; set; }
        public ICommand SettingClickCommand { get; set; }
        public ICommand HomePageCommand { get; set; }
        public ICommand DemoViewsSelectedCommand { get; set; }
        public ICommand ViewSizeChangedCommand { get; set; }

        public MainWindowViewModel()
        {
            InitCommand();

            DemoFiles = new HashSet<string>();
            ControlNamesList = new ObservableCollection<string>();
        }

        private void InitCommand()
        {
            ViewLoadedCommand = new DelegateCommand(OnLoaded);
            WindowClosingCommand = new DelegateCommand(OnWindowClosing);

            ShowSideBarCommand = new DelegateCommand(OnShowSideBar);
            SettingClickCommand = new DelegateCommand(OnSettingClick);
            HomePageCommand = new DelegateCommand(OnHomePage);
            DemoViewsSelectedCommand = new DelegateCommand(OnDemoViewsSelected);
            ViewSizeChangedCommand = new DelegateCommand(ViewSizeChanged);
        }

        private void ViewSizeChanged()
        {
           
        }

        private void OnLoaded()
        {
            SetControlsList();
            OnHomePage();
        }

        private void OnWindowClosing()
        {
            if (!IsOpenNotifyIconSwitch)
                Environment.Exit(0);
        }

        private void SetControlsList()
        {
            //new Window() { Content = "123213", Name = "test1" }.Show();
            //new Window() { Content = "456456", Name = "test2" }.Show();

            var basePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var demoPath = Directory.GetParent(basePath).Parent.Parent.Parent.Parent.FullName;
            var demoFiles = Directory.GetFiles(demoPath + "\\MvvmTestDemo\\DemoViews", "*.xaml").ToList();

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
                //var instance = ObjectCreateHelper.CreateInstanceInClassName($"{currentAssemblyName}.DemoViews", $"{className}Demo");
                ObjectFactory.Register(className, instance);
            }

            //CollectionViewSource.GetDefaultView(viewmodel);// 返回给定源的默认视图。
        }

        private void OnDemoViewsSelected()
        {
            if (DemoViewsSelectedIndex >= 0)
                ControlContent = ObjectFactory.ResolveIntance(ControlNamesList[DemoViewsSelectedIndex]);
        }

        private void OnSettingClick()
        {
            BubbleMessage.ShowInfo("打开了设置", MainWindowBubbleMessageToken);

            //MessageDialog.Show("打开了设置");
        }

        private void OnHomePage()
        {
            DemoViewsSelectedIndex = -1;
            ControlContent = OverView;
        }

        private void OnShowSideBar()
        {

        }
    }
}
