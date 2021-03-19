using CookPopularControl.Controls.PasswordBox;
using Microsoft.Xaml.Behaviors;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：PasswordBoxBehavior
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-19 10:23:46
 */
namespace CookPopularControl.Communal.Behaviors
{
    /// <summary>
    /// 提供<see cref="PasswordBox"/>输入密码时的光标行为
    /// </summary>
    public class PasswordBoxBehavior: Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
        }

        private void AssociatedObject_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            var pwdBox = sender as PasswordBox;
            string password = PasswordBoxAssistant.GetPassword(pwdBox!);
            if(pwdBox != null && pwdBox.Password != password)
            {
                PasswordBoxAssistant.SetPassword(pwdBox, pwdBox.Password);
            }

            //SetCursorPosition(AssociatedObject, AssociatedObject.Password.Length, 0);
        }

        /// <summary>
        /// 设置光标焦点
        /// </summary>
        /// <param name="pwd">控件</param>
        /// <param name="start">起始位置</param>
        /// <param name="length">长度</param>
        private void SetCursorPosition(PasswordBox pwd, int start, int length)
        {
            pwd.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(pwd, new object[] { start, length });
        }
    }
}
