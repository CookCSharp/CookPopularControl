using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ValidationExceptionBehavior
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-30 22:36:25
 */
namespace CookPopularCSharpToolkit.Windows
{
    public interface IValidationExceptionHandle
    {
        /// <summary>
        /// 是否有错误
        /// </summary>
        public bool HasValidationError { get; set; }
    }

    /// <summary>
    /// 验证行为类
    /// </summary>
    public class ValidationExceptionBehavior : Behavior<FrameworkElement>
    {
        //错误计数器
        private int validationExceptionCount = 0;

        protected override void OnAttached()
        {
            this.AssociatedObject.AddHandler(Validation.ErrorEvent, new EventHandler<ValidationErrorEventArgs>(OnOccuredValidationError));
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        private IValidationExceptionHandle GetValidationHandle()
        {
            if (this.AssociatedObject.DataContext is IValidationExceptionHandle)
                return AssociatedObject.DataContext as IValidationExceptionHandle;

            return null;
        }

        /// <summary>
        /// 验证ValidationError事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOccuredValidationError(object sender, ValidationErrorEventArgs e)
        {
            try
            {
                var handle = GetValidationHandle();
                var element = e.OriginalSource as FrameworkElement;
                if (handle == null || element == null)
                    return;

                if (e.Action == ValidationErrorEventAction.Added)
                    validationExceptionCount++;
                else if (e.Action == ValidationErrorEventAction.Removed)
                    validationExceptionCount--;

                handle.HasValidationError = validationExceptionCount != 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
