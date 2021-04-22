using CookPopularControl.Tools.Boxes;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ChromeWindow 
 * Author： Chance_写代码的厨子
 * Create Time：2021-02-18 09:46:32
 */
namespace CookPopularControl.Controls.Windows
{
    /// <summary>
    /// 标准窗口<see cref="ChromeWindow"/>
    /// </summary>
    public class ChromeWindow : Window
    {
        public static readonly RoutedCommand SidebarPopupCommand = new RoutedCommand(nameof(SidebarPopupCommand), typeof(ChromeWindow));
        public static readonly RoutedCommand MoveWindowCommand = new RoutedCommand(nameof(MoveWindowCommand), typeof(ChromeWindow));
        //public static readonly RoutedCommand ClosedWindowCommand = new RoutedCommand(nameof(ClosedWindowCommand), typeof(ChromeWindow));
        //public static readonly RoutedCommand MaximizedWindowCommand = new RoutedCommand(nameof(MaximizedWindowCommand), typeof(ChromeWindow));
        //public static readonly RoutedCommand NormalWindowCommand = new RoutedCommand(nameof(NormalWindowCommand), typeof(ChromeWindow));
        //public static readonly RoutedCommand MinimizedWindowCommand = new RoutedCommand(nameof(MinimizedWindowCommand), typeof(ChromeWindow));

        static ChromeWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChromeWindow), new FrameworkPropertyMetadata(typeof(ChromeWindow)));

            InitCommand();
        }

        public ChromeWindow()
        {

        }

        static void InitCommand()
        {
            CommandManager.RegisterClassCommandBinding(typeof(ChromeWindow), new CommandBinding(SidebarPopupCommand, Executed));
            CommandManager.RegisterClassCommandBinding(typeof(ChromeWindow), new CommandBinding(MoveWindowCommand, Executed));
            CommandManager.RegisterClassCommandBinding(typeof(ChromeWindow), new CommandBinding(SystemCommands.CloseWindowCommand, Executed));
            CommandManager.RegisterClassCommandBinding(typeof(ChromeWindow), new CommandBinding(SystemCommands.MaximizeWindowCommand, Executed));
            CommandManager.RegisterClassCommandBinding(typeof(ChromeWindow), new CommandBinding(SystemCommands.RestoreWindowCommand, Executed));
            CommandManager.RegisterClassCommandBinding(typeof(ChromeWindow), new CommandBinding(SystemCommands.MinimizeWindowCommand, Executed));
            CommandManager.RegisterClassCommandBinding(typeof(ChromeWindow), new CommandBinding(SystemCommands.ShowSystemMenuCommand, Executed));
        }

        private static void Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is ChromeWindow window)
            {
                if (e.Command == MoveWindowCommand)
                {
                    window.DragMove();
                }
                else if (e.Command == SidebarPopupCommand)
                {
                    window.IsShowSideBar = true;
                }
                else if (e.Command == SystemCommands.CloseWindowCommand)
                {
                    window.Close();
                }
                else if (e.Command == SystemCommands.MaximizeWindowCommand)
                {
                    window.WindowState = WindowState.Maximized;
                }
                else if (e.Command == SystemCommands.RestoreWindowCommand)
                {
                    window.WindowState = WindowState.Normal;
                }
                else if (e.Command == SystemCommands.MinimizeWindowCommand)
                {
                    window.WindowState = WindowState.Minimized;
                }
                else if (e.Command == SystemCommands.ShowSystemMenuCommand)
                {
                    SystemCommands.ShowSystemMenu(window, default);
                }
            }
        }


        /// <summary>
        /// 是否全屏
        /// </summary>
        public bool IsFullScreen
        {
            get { return (bool)GetValue(IsFullScreenProperty); }
            set { SetValue(IsFullScreenProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="IsFullScreen"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsFullScreenProperty =
            DependencyProperty.Register("IsFullScreen", typeof(bool), typeof(ChromeWindow),
                new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(OnIsFullScreenValueChanged)));
        private static void OnIsFullScreenValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var win = d as ChromeWindow;
            if (win != null)
            {
                if ((bool)e.NewValue)
                {
                    win.WindowStyle = WindowStyle.None;
                    win.WindowState = WindowState.Maximized;
                    win.WindowState = WindowState.Minimized;
                    win.WindowState = WindowState.Maximized;
                }
                else
                    win.WindowStyle = WindowStyle.SingleBorderWindow;
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
            DependencyProperty.Register("IsShowSideBar", typeof(bool), typeof(ChromeWindow), new PropertyMetadata(ValueBoxes.FalseBox));


        /// <summary>
        /// 最小化、最大化、关闭操作按钮的宽度
        /// </summary>
        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="IconWidth"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register("IconWidth", typeof(double), typeof(ChromeWindow), new PropertyMetadata(ValueBoxes.Double30Box));


        /// <summary>
        /// 最小化、最大化、关闭操作按钮的高度
        /// </summary>
        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="IconHeight"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register("IconHeight", typeof(double), typeof(ChromeWindow), new PropertyMetadata(ValueBoxes.Double30Box));


        /// <summary>
        /// 窗体标题栏背景色
        /// </summary>
        public Brush ClientAeroCaptionBarBackground
        {
            get { return (Brush)GetValue(ClientAeroCaptionBarBackgroundProperty); }
            set { SetValue(ClientAeroCaptionBarBackgroundProperty, value); }
        }
        public static readonly DependencyProperty ClientAeroCaptionBarBackgroundProperty =
            DependencyProperty.Register("ClientAeroCaptionBarBackground", typeof(Brush), typeof(ChromeWindow), new PropertyMetadata(default(Brush)));


        /// <summary>
        /// 窗体标题栏文字颜色
        /// </summary>
        public Brush ClientAeroCaptionForeground
        {
            get { return (Brush)GetValue(ClientAeroCaptionForegroundProperty); }
            set { SetValue(ClientAeroCaptionForegroundProperty, value); }
        }   
        public static readonly DependencyProperty ClientAeroCaptionForegroundProperty =
            DependencyProperty.Register("ClientAeroCaptionForeground", typeof(Brush), typeof(ChromeWindow), new PropertyMetadata(default(Brush)));
    }
}
