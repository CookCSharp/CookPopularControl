using CookPopularControl.Tools.Extensions;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：NotifyIcon
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-30 16:41:35
 */
namespace CookPopularControl.Controls.Notify
{
    /// <summary>
    /// 任务栏图标
    /// </summary>
    public class TaskbarIcon : Hardcodet.Wpf.TaskbarNotification.TaskbarIcon
    {
        private static List<Window> PrepareWindows = new List<Window>();
        public static ICommand OpenApplicationCommand = new RoutedCommand(nameof(OpenApplicationCommand), typeof(TaskbarIcon));
        public static ICommand HideApplicationCommand = new RoutedCommand(nameof(HideApplicationCommand), typeof(TaskbarIcon));
        public static ICommand ExitApplicationCommand = new RoutedCommand(nameof(ExitApplicationCommand), typeof(TaskbarIcon));

        public TaskbarIcon()
        {
            foreach (Window win in Application.Current.Windows)
            {
                win.Loaded += (s, e) => PrepareWindows.Remove(win);
                win.Closing += (s, e) => { e.Cancel = true; win.Hide(); PrepareWindows.Add(win); };
            }
        }

        static TaskbarIcon()
        {
            CommandManager.RegisterClassCommandBinding(typeof(FrameworkElement), new CommandBinding(OpenApplicationCommand, Excuted, (s, e) => e.CanExecute = true));
            CommandManager.RegisterClassCommandBinding(typeof(FrameworkElement), new CommandBinding(HideApplicationCommand, Excuted, (s, e) => e.CanExecute = true));
            CommandManager.RegisterClassCommandBinding(typeof(FrameworkElement), new CommandBinding(ExitApplicationCommand, Excuted, (s, e) => e.CanExecute = true));
        }

        private static void Excuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == OpenApplicationCommand)
            {
                if (PrepareWindows.Count < 1) return;
                //设置主窗口为应用程序关闭的最后一个窗口
                Application.Current.MainWindow = PrepareWindows[PrepareWindows.Count - 1];
                Application.Current.MainWindow?.Show();
                Application.Current.MainWindow?.Activate();
            }
            else if (e.Command == HideApplicationCommand)
            {
                foreach (Window win in Application.Current.Windows)
                {
                    win?.Hide();
                }
            }
            else if (e.Command == ExitApplicationCommand)
                Application.Current.Shutdown();
        }
    }
}
