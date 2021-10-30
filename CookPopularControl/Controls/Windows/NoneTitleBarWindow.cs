using CookPopularControl.Tools.Helpers;
using System.Windows;
using System.Windows.Input;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：NoneTitleBarWindow
 * Author： Chance_写代码的厨子
 * Create Time：2021-09-27 16:38:22
 */
namespace CookPopularControl.Windows
{
    /// <summary>
    /// <see cref="NoneTitleBarWindow"/>标识没有标题的窗体
    /// </summary>
    public class NoneTitleBarWindow : NormalWindow
    {
        public static readonly RoutedCommand MoveWindowCommand = new RoutedCommand(nameof(MoveWindowCommand), typeof(NoneTitleBarWindow));
        
        static NoneTitleBarWindow()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(NoneTitleBarWindow), new FrameworkPropertyMetadata(typeof(NoneTitleBarWindow)));
            StyleProperty.AddOwner(typeof(NoneTitleBarWindow), new FrameworkPropertyMetadata(default, (s, e) => ResourceHelper.GetResource<Style>("NoneTitleBarWindowStyle")));
            CommandManager.RegisterClassCommandBinding(typeof(NoneTitleBarWindow), new CommandBinding(MoveWindowCommand, Executed));
        }

        private static void Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is NoneTitleBarWindow window)
            {
                if (e.Command == MoveWindowCommand)
                {
                    window.DragMove();
                }
            }
        }
    }
}
