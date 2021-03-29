using CookPopularControl.Communal.Data.Enum;
using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ComboBoxAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-23 18:42:49
 */
namespace CookPopularControl.Controls.ComboBox
{
    /// <summary>
    /// 表示<see cref="System.Windows.Controls.ComboBox"/>的附加属性帮助类
    /// </summary>
    [TemplatePart(Name = "PART_Border", Type = typeof(Border))]
    public class ComboBoxAssistant
    {
        private const string ComboBoxBorder = "PART_Border";

        public static Brush GetDropDownButtonFill(DependencyObject obj) => (Brush)obj.GetValue(DropDownButtonFillProperty);
        public static void SetDropDownButtonFill(DependencyObject obj, Brush value) => obj.SetValue(DropDownButtonFillProperty, value);
        /// <summary>
        /// 标识<see cref="DropDownButtonFillProperty"/>提供下拉按钮的背景色附加属性
        /// </summary>
        public static readonly DependencyProperty DropDownButtonFillProperty =
            DependencyProperty.RegisterAttached("DropDownButtonFill", typeof(Brush), typeof(ComboBoxAssistant), new PropertyMetadata(default(Brush)));

        public static bool GetIsClickDown(DependencyObject obj) => (bool)obj.GetValue(IsClickDownProperty);
        public static void SetIsClickDown(DependencyObject obj, bool value) => obj.SetValue(IsClickDownProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsClickDownProperty"/>标识点击<see cref="System.Windows.Controls.ComboBox"/>显示下拉列表
        /// </summary>
        public static readonly DependencyProperty IsClickDownProperty =
            DependencyProperty.RegisterAttached("IsClickDown", typeof(bool), typeof(ComboBoxAssistant),
                new PropertyMetadata(ValueBoxes.FalseBox, OnIsClickDownValueChanged));

        private static void OnIsClickDownValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var comboBox = d as System.Windows.Controls.ComboBox;
            if (comboBox != null)
            {
                if (comboBox.IsLoaded)
                    OpenPopup(comboBox);
                else
                    comboBox.Loaded += (s, e) => OpenPopup(comboBox);
            }
        }

        private static void OpenPopup(System.Windows.Controls.ComboBox comboBox)
        {
            var border = comboBox.Template.FindName(ComboBoxBorder, comboBox) as Border;
            if (border != null)
            {
                border.MouseLeftButtonDown += (s, e) =>  //控制下拉框开关 
                {
                    if (comboBox.IsDropDownOpen)
                        comboBox.IsDropDownOpen = false;
                    else
                        comboBox.IsDropDownOpen = true;
                };
            }
        }

        public static Effect GetComboBoxPopupListShadow(DependencyObject obj) => (Effect)obj.GetValue(ComboBoxPopupListShadowProperty);
        public static void SetComboBoxPopupListShadow(DependencyObject obj, Effect value) => obj.SetValue(ComboBoxPopupListShadowProperty, value);
        /// <summary>
        /// <see cref="ComboBoxPopupListShadowProperty"/>标识下拉列表阴影效果 
        /// </summary>
        public static readonly DependencyProperty ComboBoxPopupListShadowProperty =
            DependencyProperty.RegisterAttached("ComboBoxPopupListShadow", typeof(Effect), typeof(ComboBoxAssistant), new PropertyMetadata(default(Effect)));
    }
}
