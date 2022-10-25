using CookPopularCSharpToolkit.Communal;
using System.Windows;
using System.Windows.Controls.Primitives;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ClockAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-07-27 18:21:55
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 时钟辅助类
    /// </summary>
    public class ClockAssistant
    {
        private const string ElementClock = "PART_Clock";
        private const string ElementUniformGrid = "PART_UniformGrid";

        public static bool GetIsAddConfirmButton(DependencyObject obj) => (bool)obj.GetValue(IsAddConfirmButtonProperty);
        public static void SetIsAddConfirmButton(DependencyObject obj, bool value) => obj.SetValue(IsAddConfirmButtonProperty, value);
        /// <summary>
        /// <see cref="IsAddConfirmButtonProperty"/>标识是否增加确定按钮
        /// </summary>
        public static readonly DependencyProperty IsAddConfirmButtonProperty =
            DependencyProperty.RegisterAttached("IsAddConfirmButton", typeof(bool), typeof(ClockAssistant), new PropertyMetadata(ValueBoxes.FalseBox, OnIsAddConfirmButtonChanged));

        private static void OnIsAddConfirmButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimePicker timePicker)
            {
                timePicker.Loaded += (s, e) =>
                {
                    var clock = timePicker.Template.FindName(ElementClock, timePicker) as Clock;
                    if (clock != null)
                    {
                        clock.Loaded += (s, e) =>
                        {
                            var uniformGrid = clock.Template.FindName(ElementUniformGrid, clock) as UniformGrid;
                            if (uniformGrid != null)
                                uniformGrid.Columns = 4;
                        };
                    }
                };
            }
        }
    }
}
