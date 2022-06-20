using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：TimePicker
 * Author： Chance_写代码的厨子
 * Create Time：2021-07-27 17:19:34
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 时间选择器
    /// </summary>
    [TemplatePart(Name = ElementClock, Type = typeof(Clock))]
    public class TimePicker : Control
    {
        private const string ElementClock = "PART_Clock";
        private Clock _clock = default;

        public string CurrentTime
        {
            get { return (string)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="CurrentTime"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(string), typeof(TimePicker), new PropertyMetadata(DateTime.Now.ToString("HH:mm:ss")));


        public TimePicker()
        {
            CommandBindings.Add(new CommandBinding(Clock.ConfirmCommand, (s, e) => CurrentTime = _clock.CurrentTime));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _clock = GetTemplateChild(ElementClock) as Clock;
        }
    }
}
