using CookPopularControl.Communal.Data.Args;
using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ChromeWindow 
 * Author： Chance_写代码的厨子
 * Create Time：2021-02-18 09:46:32
 */
namespace CookPopularControl.Windows
{
    /// <summary>
    /// <see cref="SideBarWindow"/>表示具有侧边栏的窗体
    /// </summary>
    public class SideBarWindow : NormalWindow
    {
        public static readonly RoutedCommand SidebarPopupCommand = new RoutedCommand(nameof(SidebarPopupCommand), typeof(SideBarWindow));
        public static readonly RoutedCommand SettingCommand = new RoutedCommand(nameof(SettingCommand), typeof(SideBarWindow));
        public static readonly RoutedCommand MoveWindowCommand = new RoutedCommand(nameof(MoveWindowCommand), typeof(SideBarWindow));

        static SideBarWindow()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(SideBarWindow), new FrameworkPropertyMetadata(typeof(SideBarWindow)));
            StyleProperty.AddOwner(typeof(SideBarWindow), new FrameworkPropertyMetadata(default, (s, e) => ResourceHelper.GetResource<Style>("SideBarWindowStyle")));
            CommandManager.RegisterClassCommandBinding(typeof(SideBarWindow), new CommandBinding(SidebarPopupCommand, Executed));
            CommandManager.RegisterClassCommandBinding(typeof(SideBarWindow), new CommandBinding(SettingCommand, Executed));
            CommandManager.RegisterClassCommandBinding(typeof(SideBarWindow), new CommandBinding(MoveWindowCommand, Executed));
        }

        private static void Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is SideBarWindow window)
            {
                if (e.Command == MoveWindowCommand)
                {
                    window.DragMove();
                }
                else if (e.Command == SidebarPopupCommand)
                {
                    var s = window.IsShowSideBar;
                    window.OnIsShowSideBarChanged(window.IsShowSideBar);
                }
                else if (e.Command == SettingCommand)
                {
                    window.OnSettingClick(e.OriginalSource);
                }
            }
        }


        /// <summary>
        /// 是否显示侧边栏
        /// </summary>
        public bool IsShowSideBar
        {
            get { return (bool)GetValue(IsShowSideBarProperty); }
            set { SetValue(IsShowSideBarProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="IsShowSideBar"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsShowSideBarProperty =
            DependencyProperty.Register("IsShowSideBar", typeof(bool), typeof(SideBarWindow), new PropertyMetadata(ValueBoxes.FalseBox));



        public event RoutedPropertySingleEventHandler<bool> IsShowSideBarChanged
        {
            add { this.AddHandler(IsShowSideBarChangedEvent, value); }
            remove { this.RemoveHandler(IsShowSideBarChangedEvent, value); }
        }
        /// <summary>
        /// <see cref="IsShowSideBarChangedEvent"/>标识侧边栏是否打开事件
        /// </summary>
        public static readonly RoutedEvent IsShowSideBarChangedEvent =
            EventManager.RegisterRoutedEvent("IsShowSideBarChanged", RoutingStrategy.Bubble, typeof(RoutedPropertySingleEventHandler<bool>), typeof(SideBarWindow));

        protected virtual void OnIsShowSideBarChanged(bool value)
        {
            RoutedPropertySingleEventArgs<bool> arg = new RoutedPropertySingleEventArgs<bool>(value, IsShowSideBarChangedEvent);
            this.RaiseEvent(arg);
        }


        [Description("设置按钮点击时发生")]
        public event RoutedEventHandler SettingClick
        {
            add { this.AddHandler(SettingClickEvent, value); }
            remove { this.RemoveHandler(SettingClickEvent, value); }
        }
        /// <summary>
        /// <see cref="SettingClickEvent"/>标识设置按钮点击时事件
        /// </summary>
        public static readonly RoutedEvent SettingClickEvent =
            EventManager.RegisterRoutedEvent("SettingClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SideBarWindow));

        protected virtual void OnSettingClick(object source)
        {
            RoutedEventArgs arg = new RoutedEventArgs(SettingClickEvent, source);
            this.RaiseEvent(arg);
        }
    }
}
