/*
 * Description：DialogExtensions 
 * Author： Chance.Zheng
 * Create Time: 2023-03-08 11:48:52
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2023 All Rights Reserved.
 */


using CookPopularCSharpToolkit.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;


namespace CookPopularControl.Controls
{
    public interface IDialogResultable<T>
    {
        T Result { get; set; }

        Action CloseAction { get; set; }
    }

    public static class DialogExtensions
    {
        public static IDialog Initialize<TViewModel>(this IDialog dialog, Action<TViewModel> action)
        {
            action?.Invoke(dialog.GetViewModel<TViewModel>());

            return dialog;
        }

        public static TViewModel GetViewModel<TViewModel>(this IDialog dialog)
        {
            if (dialog is not ContentControl control)
                throw new InvalidOperationException("The dialog is not subclass of the ContentControl. ");

            if (control.Content is not FrameworkElement frameworkElement)
                throw new InvalidOperationException("The dialog's content is not subclass of the FrameworkElement. ");

            if (frameworkElement.DataContext is not TViewModel viewModel)
                throw new InvalidOperationException($"The viewmodel of the dialog's content is not the {typeof(TViewModel)} type or its subclass. ");

            return viewModel;
        }

        public static Task<TResult> GetResultAsync<TResult>(this IDialog dialog)
        {
            var tcs = new TaskCompletionSource<TResult>();

            try
            {
                if (dialog.IsClosed)
                {
                    SetResult();
                }
                else
                {
                    (dialog as FrameworkElement).Unloaded += OnUnloaded;

                    var dialogBox = dialog as DialogBox;
                    if (dialogBox != null)
                        dialog.GetViewModel<IDialogResultable<TResult>>().CloseAction = dialogBox.Close;

                    var win = dialog as Windows.DialogWindow;
                    if (win != null)
                    {
                        dialog.GetViewModel<IDialogResultable<TResult>>().CloseAction = win.CloseWindow;
                    }
                }
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            return tcs.Task;

            void OnUnloaded(object sender, RoutedEventArgs args)
            {
                (dialog as FrameworkElement).Unloaded -= OnUnloaded;
                SetResult();
            }

            void SetResult()
            {
                try
                {
                    if ((dialog is Windows.DialogWindow win && win.IsConfirm) || dialog is DialogBox)
                        tcs.TrySetResult(dialog.GetViewModel<IDialogResultable<TResult>>().Result);
                }
                catch (Exception e)
                {
                    tcs.TrySetException(e);
                }
            }
        }
    }
}
