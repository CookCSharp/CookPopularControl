using CookPopularControl.Tools.Boxes;
using CookPopularControl.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：NotifyIcon
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-30 16:41:35
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 任务栏图标
    /// </summary>
    public class TaskbarIcon : Hardcodet.Wpf.TaskbarNotification.TaskbarIcon
    {
        private static List<Window> PrepareWindows = new List<Window>();
        private DispatcherTimer timer;
        private ImageSource originalIcon;

        public static ICommand OpenApplicationCommand = new RoutedCommand(nameof(OpenApplicationCommand), typeof(TaskbarIcon));
        public static ICommand HideApplicationCommand = new RoutedCommand(nameof(HideApplicationCommand), typeof(TaskbarIcon));
        public static ICommand ExitApplicationCommand = new RoutedCommand(nameof(ExitApplicationCommand), typeof(TaskbarIcon));

        public TaskbarIcon()
        {
            foreach (Window win in Application.Current.Windows)
            {
                win.Loaded += (s, e) => PrepareWindows.Remove(win);
                win.Closing += (s, e) =>
                {
                    e.Cancel = true;
                    win.Hide();
                    if (!PrepareWindows.Contains(win))
                        PrepareWindows.Add(win);
                };
            }

            this.Loaded += (s, e) =>
            {
                BitmapImage tranparentIcon = new BitmapImage(new Uri("pack://application:,,,/CookPopularControl;component/Resources/Images/CookCSharpTransparent.ico", UriKind.Absolute));
                originalIcon = IconSource;
                timer = new DispatcherTimer(DispatcherPriority.Normal);
                timer.Interval = TimeSpan.FromMilliseconds(500);
                timer.Tick += (s, e) =>
                {
                    if (IconSource.Equals(originalIcon))
                        IconSource = tranparentIcon;
                    else
                        IconSource = originalIcon;
                };

                var firstWin = Window.GetWindow(this);
                if (firstWin != null)
                    PrepareWindows.Add(firstWin);
            };
        }

        static TaskbarIcon()
        {
            CommandManager.RegisterClassCommandBinding(typeof(FrameworkElement), new CommandBinding(OpenApplicationCommand, Excuted, (s, e) => e.CanExecute = true));
            CommandManager.RegisterClassCommandBinding(typeof(FrameworkElement), new CommandBinding(HideApplicationCommand, Excuted, (s, e) => e.CanExecute = true));
            CommandManager.RegisterClassCommandBinding(typeof(FrameworkElement), new CommandBinding(ExitApplicationCommand, Excuted, (s, e) => e.CanExecute = true));
        }

        private static void Excuted(object sender, ExecutedRoutedEventArgs e)
        {
            var taskbarIcon = sender as TaskbarIcon;
            if (e.Command == OpenApplicationCommand)
            {
                if (PrepareWindows.Count < 1) return;
                //设置主窗口为应用程序关闭的最后一个窗口
                Application.Current.MainWindow = PrepareWindows[PrepareWindows.Count - 1];
                Application.Current.MainWindow?.Activate();
                Application.Current.MainWindow?.Show();
                Application.Current.MainWindow?.SwitchToThisWindow();
            }
            else if (e.Command == HideApplicationCommand)
            {
                foreach (Window win in Application.Current.Windows)
                {
                    win?.Close();
                }
            }
            else if (e.Command == ExitApplicationCommand)
                Environment.Exit(0);
        }


        /// <summary>
        /// 程序启动后是否开启任务栏闪烁
        /// </summary>
        public bool IsStartTaskbarFlash
        {
            get { return (bool)GetValue(IsStartTaskbarFlashProperty); }
            set { SetValue(IsStartTaskbarFlashProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 提供<see cref="IsStartTaskbarFlash"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsStartTaskbarFlashProperty =
            DependencyProperty.Register("IsStartTaskbarFlash", typeof(bool), typeof(TaskbarIcon),
                new PropertyMetadata(ValueBoxes.FalseBox, OnTaskbarFlashChanged));

        private static void OnTaskbarFlashChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var taskbarIcon = d as TaskbarIcon;
            if (taskbarIcon != null)
            {
                if ((bool)e.NewValue)
                {
                    if (taskbarIcon.IsLoaded)
                    {
                        var win = Window.GetWindow(taskbarIcon);
                        win.SafeActivate();
                        win.FlashWindow(false);
                    }
                    else
                    {
                        taskbarIcon.Loaded += (s, e) =>
                        {
                            var win = Window.GetWindow(taskbarIcon);
                            win.SafeActivate();
                            win.FlashWindow(false);
                        };
                    }
                }
            }
        }


        /// <summary>
        /// 是否启用任务栏图标闪烁
        /// </summary>
        public bool IsStartTaskbarIconFlash
        {
            get { return (bool)GetValue(IsStartTaskbarIconFlashProperty); }
            set { SetValue(IsStartTaskbarIconFlashProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 提供<see cref="IsStartTaskbarIconFlash"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsStartTaskbarIconFlashProperty =
            DependencyProperty.Register("IsStartTaskbarIconFlash", typeof(bool), typeof(TaskbarIcon),
                new PropertyMetadata(ValueBoxes.FalseBox, OnTaskbarIconFlashPropertyChanged));

        private static void OnTaskbarIconFlashPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var taskbarIcon = d as TaskbarIcon;
            if (taskbarIcon != null)
            {
                if ((bool)e.NewValue)
                    taskbarIcon.timer.Start();
                else
                {
                    taskbarIcon.IconSource = taskbarIcon.originalIcon;
                    taskbarIcon.timer.Stop();
                }
            }
        }
    }
}
