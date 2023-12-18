using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows;
using Microsoft.Xaml.Behaviors.Layout;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DialogBox
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-12 13:55:42
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 装饰器盒子
    /// </summary>
    public class DialogBox : ContentControl, IDialog
    {
        private AdornerContainer Container;
        private static readonly HashSet<FrameworkElement> DialogInstances = new HashSet<FrameworkElement>();
        public static readonly ICommand OpenDialogCommand = new RoutedCommand("OpenDialog", typeof(DialogBox));
        public static readonly ICommand CloseDialogCommand = new RoutedCommand("CloseDialog", typeof(DialogBox));

        public bool IsClosed { get; private set; }


        public static bool GetIsUseAnimation(DependencyObject obj) => (bool)obj.GetValue(IsUseAnimationProperty);
        public static void SetIsUseAnimation(DependencyObject obj, bool value) => obj.SetValue(IsUseAnimationProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsUseAnimationProperty"/>表示<see cref="DialogBox"/>是否启用动画的附加属性
        /// </summary>
        public static readonly DependencyProperty IsUseAnimationProperty =
            DependencyProperty.RegisterAttached("IsUseAnimation", typeof(bool), typeof(DialogBox), new PropertyMetadata(ValueBoxes.FalseBox));


        public static string GetMark(DependencyObject obj) => (string)obj.GetValue(MarkProperty);
        public static void SetMark(DependencyObject obj, string value) => obj.SetValue(MarkProperty, value);
        /// <summary>
        /// <see cref="MarkProperty"/>表示<see cref="DialogBox"/>的标识附加属性
        /// </summary>
        public static readonly DependencyProperty MarkProperty =
            DependencyProperty.RegisterAttached("Mark", typeof(string), typeof(DialogBox), new PropertyMetadata(default(string), OnMarkValueChanged));

        private static void OnMarkValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                var isExist = DialogInstances.Any(d => GetMark(d) == e.NewValue.ToString());
                if (!isExist)
                    DialogInstances.Add(element);
            }
        }


        static DialogBox()
        {
            CommandManager.RegisterClassCommandBinding(typeof(FrameworkElement), new CommandBinding(OpenDialogCommand, Executed, (s, e) => e.CanExecute = true));
            CommandManager.RegisterClassCommandBinding(typeof(DialogBox), new CommandBinding(CloseDialogCommand, Executed, (s, e) => e.CanExecute = true));
        }

        private static void Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialogBox = sender as DialogBox;
            if (e.Command == OpenDialogCommand && e.Parameter != null)
                Show(e.Parameter);
            else if (e.Command == CloseDialogCommand)
                dialogBox!.Close();
        }

        public static DialogBox Show<T>(string mark = "") where T : new() => Show(new T(), mark);

        public static DialogBox Show(object content, string mark = "")
        {
            if (content is FrameworkElement fe)
            {
                if (fe is UserControl uc && GetIsUseAnimation(fe))
                {
                    (uc.Content as FrameworkElement).Loaded += (s, e) =>
                    {
                        var ucContent = uc.Content as FrameworkElement;
                        var animation = AnimationHelper.CreateDoubleAnimation(0, ucContent.ActualWidth, 0.25);
                        ucContent.BeginAnimation(WidthProperty, animation);
                    };
                }

                //只有在content以外的区域DialogBox.MouseLeftButtonUp才生效
                fe.MouseLeftButtonDown += (s, e) => e.Handled = true;
                fe.MouseLeftButtonUp += (s, e) => e.Handled = true; 
            }

            DialogBox dialogBox = new DialogBox { Content = content, IsClosed = false };
            SetMark(dialogBox, mark);

            FrameworkElement element;
            AdornerDecorator adorner;

            if (string.IsNullOrEmpty(GetMark(dialogBox)))
            {
                element = WindowExtension.GetActiveWindow();
                adorner = element.GetVisualDescendants().OfType<AdornerDecorator>().FirstOrDefault();
            }
            else
            {
                element = DialogInstances.SingleOrDefault(d => GetMark(d) == mark);
                adorner = element is Window ? element.GetVisualDescendants().OfType<AdornerDecorator>().FirstOrDefault()
                                            : element.GetVisualDescendants().OfType<DialogBoxContainer>().FirstOrDefault();
            }

            //(content as FrameworkElement).Loaded += (s, e) =>
            //{
            //    (content as FrameworkElement).Width = element.ActualWidth;
            //    (content as FrameworkElement).Height = element.ActualHeight;
            //};

            if (adorner is not null)
            {
                var layer = adorner.AdornerLayer;
                if (layer is not null)
                {
                    var container = new AdornerContainer(layer) { Child = dialogBox };
                    dialogBox.Container = container;
                    layer.Add(container);
                }
            }

            return dialogBox;
        }

        public void Close()
        {
            var element = DialogInstances.SingleOrDefault(d => GetMark(d) == GetMark(this));
            if (string.IsNullOrEmpty(GetMark(this)))
                Close(WindowExtension.GetActiveWindow());
            else if (element != null && DialogInstances.Contains(element))
                Close(element);
        }

        private void Close(DependencyObject element)
        {
            if (element != null && Container != null)
            {
                var adorner = element.GetVisualDescendants().OfType<AdornerDecorator>().FirstOrDefault();
                if (adorner != null)
                {
                    IsClosed = true;
                    var layer = adorner.AdornerLayer;
                    layer?.Remove(Container);
                    DialogInstances.Remove(this);
                }
            }
        }
    }
}
