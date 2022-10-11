using CookPopularControl.Controls;
using CookPopularControl.Themes.CookColors;
using CookPopularControl.Windows;
using CookPopularCSharpToolkit.Communal;
using MvvmTestDemo.Commumal;
using MvvmTestDemo.UserControls;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;



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
        public class ThemeModel
        {
            public Brush Brush
            {
                get; set;
            }

            public bool IsShow
            {
                get; set;
            }
        }

        private static readonly Brush[] BrushLists = new Brush[8] { Brushes.DodgerBlue, Brushes.Red, Brushes.Orange, Brushes.Yellow, Brushes.Green, Brushes.Aqua, Brushes.Blue, Brushes.Purple };
        private const string MainWindowBubbleMessageToken = "MainWindowToken";
        private ThemeProvider _themeProvider;
        private OverViewDemo _overView = new OverViewDemo();
        private HashSet<string> _demoFiles;


        public ObservableCollection<string> ControlNamesList { get; set; }
        public IList<ThemeModel> ThemeBrushs { get; set; }
        public int SelectedThemeIndex { get; set; }
        public object ControlContent { get; set; }
        public int DemoViewsSelectedIndex { get; set; }
        public bool IsOpenNotifyIconSwitch { get; set; }
        public ICommand ShowSideBarCommand { get; set; }
        public ICommand SettingClickCommand { get; set; }
        public ICommand HomePageCommand { get; set; }
        public ICommand SwitchAppThemeCommand { get; set; }
        public ICommand DemoViewsSelectedCommand { get; set; }
        public ICommand ViewSizeChangedCommand { get; set; }

        public MainWindowViewModel()
        {
            InitCommand();

            _themeProvider = new ThemeProvider();
            _demoFiles = new HashSet<string>();
            ControlNamesList = new ObservableCollection<string>();
        }

        private void InitCommand()
        {
            ViewLoadedCommand = new DelegateCommand(OnLoaded);
            ViewSizeChangedCommand = new DelegateCommand(ViewSizeChanged);
            WindowClosingCommand = new DelegateCommand(OnWindowClosing);

            ShowSideBarCommand = new DelegateCommand(OnShowSideBar);
            SettingClickCommand = new DelegateCommand(OnSettingClick);
            HomePageCommand = new DelegateCommand(OnHomePage);
            SwitchAppThemeCommand = new DelegateCommand(OnSwitchAppTheme);
            DemoViewsSelectedCommand = new DelegateCommand(OnDemoViewsSelected);
        }

        private void OnLoaded()
        {
            SetControlsList();
            GetThemes();
            OnHomePage();
        }

        private void ViewSizeChanged()
        {

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
            var demoFiles = Directory.GetFiles(@"D:\WPFSourceCode\CookPopularControl\MvvmTestDemo\DemoViews", "*.xaml").ToList();
            //var demoFiles = Directory.GetFiles(demoPath + "\\MvvmTestDemo\\DemoViews", "*.xaml").ToList();

            foreach (var file in demoFiles)
            {
                _demoFiles.Add(file);
            }

            foreach (var file in _demoFiles)
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

        private void GetThemes()
        {
            ThemeBrushs = new List<ThemeModel>();
            for (int i = 0; i < BrushLists.Length; i++)
            {
                ThemeBrushs.Add(new ThemeModel { Brush = BrushLists[i] });
            }
        }

        private void OnShowSideBar()
        {

        }

        private void OnSettingClick()
        {
            BubbleMessage.ShowInfo("打开了设置", MainWindowBubbleMessageToken);

            //MessageDialog.Show("打开了设置");
        }

        private void OnHomePage()
        {
            DemoViewsSelectedIndex = -1;
            ControlContent = _overView;
        }

        private void OnSwitchAppTheme()
        {
            var colorName = BrushLists[SelectedThemeIndex].ToColorFromBrush().GetColorName();
            if (colorName == null)
                colorName = "DodgerBlue";

            _themeProvider.SetAppTheme(colorName, 2);
        }

        private void OnDemoViewsSelected()
        {
            if (DemoViewsSelectedIndex >= 0)
                ControlContent = ObjectFactory.ResolveIntance(ControlNamesList[DemoViewsSelectedIndex]);
        }
    }
}
