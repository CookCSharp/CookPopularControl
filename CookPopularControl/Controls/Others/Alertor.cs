using CookPopularControl.Communal.Data.Enum;
using CookPopularControl.Tools.Boxes;
using CookPopularControl.Tools.Extensions.Colors;
using CookPopularControl.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：Alertor
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-10 09:43:57
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 警报器
    /// </summary>
    [TemplatePart(Name = ElementAlarm, Type = (typeof(Shape)))]
    public class Alertor : Control
    {
        private const string ElementAlarm = "PART_Alarm";
        private static readonly List<string> colors = new List<string>() { ResourceHelper.GetResource<Color>("PrimaryThemeColor").ToString(), "#32AA32", "#FFA500", "#FF0000", "#800000" };

        private Storyboard _storyboard;
        private Shape _alarm;

        /// <summary>
        /// 单次警报的时长
        /// </summary>
        /// <remarks>单位：s</remarks>
        public double Duration
        {
            get { return (double)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="Duration"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(double), typeof(Alertor), new PropertyMetadata(ValueBoxes.Double1Box));


        /// <summary>
        /// 解除警报
        /// </summary>
        public bool IsCancelAlarm
        {
            get { return (bool)GetValue(IsCancelAlarmProperty); }
            set { SetValue(IsCancelAlarmProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 提供<see cref="IsCancelAlarm"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsCancelAlarmProperty =
            DependencyProperty.Register("IsCancelAlarm", typeof(bool), typeof(Alertor), new PropertyMetadata(ValueBoxes.FalseBox, OnIsCancelAlarmChanged));

        private static void OnIsCancelAlarmChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is Alertor alertor)
            {
                if((bool)e.NewValue)
                {
                    alertor._storyboard.Pause();
                }
                else
                {
                    alertor._storyboard.Resume();
                }
            }
        }


        /// <summary>
        /// 警报器当前状态
        /// </summary>
        public AlertorState CurrentState
        {
            get { return (AlertorState)GetValue(CurrentStateProperty); }
            set { SetValue(CurrentStateProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="CurrentState"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty CurrentStateProperty =
            DependencyProperty.Register("CurrentState", typeof(AlertorState), typeof(Alertor), new PropertyMetadata(default(AlertorState), OnCurrentStatePropertyChanged));

        private static void OnCurrentStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Alertor alertor && !alertor.IsCancelAlarm)
            {
                if ((AlertorState)e.NewValue == AlertorState.Normal || (AlertorState)e.NewValue == AlertorState.Success)
                    alertor._alarm.Fill = ((Color)ColorConverter.ConvertFromString(colors[(int)e.NewValue])).ToBrushFromColor();
                else
                    alertor.StartAlarmAnimation(alertor, (int)e.NewValue);
                alertor.OnStateChanged((AlertorState)e.OldValue, (AlertorState)e.NewValue);
            }
        }

        private void StartAlarmAnimation(Alertor alertor, int index)
        {
            ColorAnimation colorAnimation = new ColorAnimation()
            {
                Duration = new Duration(TimeSpan.FromSeconds(alertor.Duration)),
                From = Colors.Transparent,
                To = (Color)ColorConverter.ConvertFromString(colors[index]),
                RepeatBehavior = RepeatBehavior.Forever,
            };
            Storyboard.SetTarget(colorAnimation, alertor._alarm);
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("(Shape.Fill).(SolidColorBrush.Color)"));
            alertor._storyboard.Children.Clear();
            alertor._storyboard.Children.Add(colorAnimation);
            alertor._storyboard.Begin();
        }

        protected virtual void OnStateChanged(AlertorState oldValue, AlertorState newValue)
        {
            RoutedPropertyChangedEventArgs<AlertorState> arg = new RoutedPropertyChangedEventArgs<AlertorState>(oldValue, newValue, StateChangedEvent);
            this.RaiseEvent(arg);
        }

        [Description("警报器状态变化时发生")]
        public event RoutedPropertyChangedEventHandler<AlertorState> StateChanged
        {
            add { this.AddHandler(StateChangedEvent, value); }
            remove { this.RemoveHandler(StateChangedEvent, value); }
        }
        /// <summary>
        /// <see cref="StateChangedEvent"/>标识警报器状态变化时的事件
        /// </summary>
        public static readonly RoutedEvent StateChangedEvent =
            EventManager.RegisterRoutedEvent("StateChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<AlertorState>), typeof(Alertor));


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _storyboard = new Storyboard();
            _alarm = GetTemplateChild(ElementAlarm) as Shape;
        }
    }
}
