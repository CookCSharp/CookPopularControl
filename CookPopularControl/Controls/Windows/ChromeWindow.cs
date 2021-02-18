using System.Windows;
using System.Windows.Input;


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
        public static readonly RoutedCommand ClosedWindowCommand = new RoutedCommand(nameof(ClosedWindowCommand), typeof(ChromeWindow));
        public static readonly RoutedCommand MaximizedWindowCommand = new RoutedCommand(nameof(MaximizedWindowCommand), typeof(ChromeWindow));
        public static readonly RoutedCommand NormalWindowCommand = new RoutedCommand(nameof(NormalWindowCommand), typeof(ChromeWindow));
        public static readonly RoutedCommand MinimizedWindowCommand = new RoutedCommand(nameof(MinimizedWindowCommand), typeof(ChromeWindow));

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
            CommandManager.RegisterClassCommandBinding(typeof(ChromeWindow), new CommandBinding(ClosedWindowCommand, Executed));
            CommandManager.RegisterClassCommandBinding(typeof(ChromeWindow), new CommandBinding(MaximizedWindowCommand, Executed));
            CommandManager.RegisterClassCommandBinding(typeof(ChromeWindow), new CommandBinding(NormalWindowCommand, Executed));
            CommandManager.RegisterClassCommandBinding(typeof(ChromeWindow), new CommandBinding(MinimizedWindowCommand, Executed));
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
                    window.IsShowBlock = true;
                }
                else if (e.Command == ClosedWindowCommand)
                {
                    window.Close();
                }
                else if (e.Command == MaximizedWindowCommand)
                {
                    window.WindowState = WindowState.Maximized;
                }
                else if (e.Command == NormalWindowCommand)
                {
                    window.WindowState = WindowState.Normal;
                }
                else if (e.Command == MinimizedWindowCommand)
                {
                    window.WindowState = WindowState.Minimized;
                }
            }
        }

        public bool IsShowBlock
        {
            get { return (bool)GetValue(IsShowBlockProperty); }
            set { SetValue(IsShowBlockProperty, value); }
        }

        /// <summary>
        /// 是否显示菜单栏
        /// <see cref="IsShowBlock"/>
        /// </summary>
        public static readonly DependencyProperty IsShowBlockProperty =
            DependencyProperty.Register("IsShowBlock", typeof(bool), typeof(ChromeWindow), new PropertyMetadata(default(bool)));
    }
}
