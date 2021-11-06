using CookPopularCSharpToolkit.Communal;
using System.ComponentModel;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：PasswordBoxAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-18 16:15:12
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 提供<see cref="System.Windows.Controls.PasswordBox"/>的附加属性类
    /// </summary>
    public class PasswordBoxAssistant
    {
        public static bool GetIsShowIcon(DependencyObject obj) => (bool)obj.GetValue(IsShowIconProperty);
        public static void SetIsShowIcon(DependencyObject obj, bool value) => obj.SetValue(IsShowIconProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsShowIconProperty"/>表示图标是否可见
        /// </summary>
        public static readonly DependencyProperty IsShowIconProperty =
            DependencyProperty.RegisterAttached("IsShowIcon", typeof(bool), typeof(PasswordBoxAssistant), new PropertyMetadata(ValueBoxes.FalseBox));

        public static bool GetIsShowPassword(DependencyObject obj) => (bool)obj.GetValue(IsShowPasswordProperty);
        public static void SetIsShowPassword(DependencyObject obj, bool value) => obj.SetValue(IsShowPasswordProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsShowPasswordProperty"/>表示密码是否可见
        /// </summary>
        [Description("备用")]
        public static readonly DependencyProperty IsShowPasswordProperty =
            DependencyProperty.RegisterAttached("IsShowPassword", typeof(bool), typeof(PasswordBoxAssistant), new PropertyMetadata(ValueBoxes.FalseBox));

        public static string GetPassword(DependencyObject obj) => (string)obj.GetValue(PasswordProperty);
        public static void SetPassword(DependencyObject obj, string value) => obj.SetValue(PasswordProperty, value);
        /// <summary>
        /// <see cref="PasswordProperty"/>表示可绑定的Password
        /// </summary>
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxAssistant), new PropertyMetadata(string.Empty, OnPasswordValueChanged));

        private static void OnPasswordValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pwdBox = d as System.Windows.Controls.PasswordBox;
            string password = (string)e.NewValue;
            if (pwdBox != null && pwdBox.Password != password) //需判断后给password赋值，否则会造成PasswordBox输入时光标位置错误
            {
                pwdBox.Password = password;
            }
        }

        public static bool GetIsTrigger(DependencyObject obj) => (bool)obj.GetValue(IsTriggerProperty);
        public static void SetIsTrigger(DependencyObject obj, bool value) => obj.SetValue(IsTriggerProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsTriggerProperty"/>表示是否触发PasswordChanged事件
        /// </summary>
        public static readonly DependencyProperty IsTriggerProperty =
            DependencyProperty.RegisterAttached("IsTrigger", typeof(bool), typeof(PasswordBoxAssistant),
                new PropertyMetadata(ValueBoxes.FalseBox, OnIsTriggerValueChanged));

        private static void OnIsTriggerValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pwd = d as System.Windows.Controls.PasswordBox;
            if (pwd == null) return;

            //PasswordBoxBehavior behavior = new PasswordBoxBehavior();
            //behavior.Attach(pwd);

            pwd.PasswordChanged += Pwd_PasswordChanged;
            if ((bool)e.OldValue)
            {
                pwd.PasswordChanged -= Pwd_PasswordChanged;
            }
            if ((bool)e.NewValue)
            {
                pwd.PasswordChanged += Pwd_PasswordChanged;
            }
        }

        private static void Pwd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pwd = sender as System.Windows.Controls.PasswordBox;
            SetPassword(pwd!, pwd.Password);
        }
    }
}
