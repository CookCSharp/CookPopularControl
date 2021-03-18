using CookPopularControl.Communal.Attached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：SwitchControlAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-17 17:51:01
 */
namespace CookPopularControl.Controls.CheckBox
{
    /// <summary>
    /// 表示开关控件
    /// </summary>
    [TemplatePart(Name = "PART_CheckFlag", Type = typeof(Border))]
    public class SwitchControl : System.Windows.Controls.CheckBox
    {
        private double sliderw; //滑动距离
        private Border borderCheckFlag; //滑动的控件
        private TranslateTransform translate; //位移变换

        static SwitchControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SwitchControl), new FrameworkPropertyMetadata(typeof(SwitchControl), FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnValueChanged)));
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Console.WriteLine("写代码的厨子。。。");
        }

        public SwitchControl()
        {
            this.Loaded += SwitchControl_Loaded;
        }

        private void SwitchControl_Loaded(object sender, RoutedEventArgs e)
        {
            sliderw = this.Width - this.Height;
            FrameworkElementBaseAttached.SetCornerRadius(this, new CornerRadius(this.Height / 2));

            borderCheckFlag = GetTemplateChild("PART_CheckFlag") as Border;
            translate = new TranslateTransform();
            borderCheckFlag.RenderTransform = translate;

            this.SizeChanged += SwitchControl_SizeChanged;
        }

        private void SwitchControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            sliderw = this.Width - this.Height;
            FrameworkElementBaseAttached.SetCornerRadius(this, new CornerRadius(this.Height / 2));
            if (this.IsChecked == true)
            {
                OnChecked(e);
            }
        }

        /// <summary>
        /// 文本
        /// </summary>
        internal string SwitchContent
        {
            get { return (string)GetValue(SwitchContentProperty); }
            set { SetValue(SwitchContentProperty, value); }
        }
        internal static readonly DependencyProperty SwitchContentProperty =
            DependencyProperty.Register("SwitchContent", typeof(string), typeof(SwitchControl), new PropertyMetadata());

        /// <summary>
        /// 打开的背景颜色
        /// </summary>
        public Brush SwitchOpenBackground
        {
            get { return (Brush)GetValue(SwitchOpenBackgroundProperty); }
            set { SetValue(SwitchOpenBackgroundProperty, value); }
        }
        public static readonly DependencyProperty SwitchOpenBackgroundProperty =
            DependencyProperty.Register("SwitchOpenBackground", typeof(Brush), typeof(SwitchControl), new PropertyMetadata());

        /// <summary>
        /// 关闭的背景颜色
        /// </summary>
        public Brush SwicthCloseBackground
        {
            get { return (Brush)GetValue(SwicthCloseBackgroundProperty); }
            set { SetValue(SwicthCloseBackgroundProperty, value); }
        }
        public static readonly DependencyProperty SwicthCloseBackgroundProperty =
            DependencyProperty.Register("SwicthCloseBackground", typeof(Brush), typeof(SwitchControl), new PropertyMetadata());


        //选中
        protected override void OnChecked(RoutedEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            Duration duration = new Duration(TimeSpan.FromMilliseconds(200));
            translate.X = 0;
            doubleAnimation.To = sliderw;
            doubleAnimation.Duration = duration;
            translate.BeginAnimation(TranslateTransform.XProperty, doubleAnimation);
        }

        //未选中
        protected override void OnUnchecked(RoutedEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            Duration duration = new Duration(TimeSpan.FromMilliseconds(200));
            translate.X = sliderw;
            doubleAnimation.To = 0;
            doubleAnimation.Duration = duration;
            translate.BeginAnimation(TranslateTransform.XProperty, doubleAnimation);
        }
    }
}
