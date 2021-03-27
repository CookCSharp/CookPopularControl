using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using ThreadState = System.Threading.ThreadState;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ScrollBarAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-26 13:22:25
 */
namespace CookPopularControl.Controls.ScrollControls
{
    /// <summary>
    /// 提供<see cref="ScrollBar"/>的附加属性基类
    /// </summary>
    public class ScrollBarAssistant
    {
        public static bool GetIsScrollBarRepeatButton(DependencyObject obj) => (bool)obj.GetValue(IsScrollBarRepeatButtonProperty);
        public static void SetIsScrollBarRepeatButton(DependencyObject obj, bool value) => obj.SetValue(IsScrollBarRepeatButtonProperty, value);
        /// <summary>
        /// <see cref="IsScrollBarRepeatButtonProperty"/>标识<see cref="RepeatButton"/>是否显示，默认True
        /// </summary>
        public static readonly DependencyProperty IsScrollBarRepeatButtonProperty =
            DependencyProperty.RegisterAttached("IsScrollBarRepeatButton", typeof(bool), typeof(ScrollBarAssistant), new PropertyMetadata(ValueBoxes.TrueBox));

        public static Thickness GetThumbInsideMargin(DependencyObject obj) => (Thickness)obj.GetValue(ThumbInsideMarginProperty);
        public static void SetThumbInsideMargin(DependencyObject obj, Thickness value) => obj.SetValue(ThumbInsideMarginProperty, value);
        /// <summary>
        /// <see cref="ThumbInsideMarginProperty"/>标识<see cref="Thumb"/>在<see cref="Track"/>中移动时距离边界的位置
        /// </summary>
        public static readonly DependencyProperty ThumbInsideMarginProperty =
            DependencyProperty.RegisterAttached("ThumbInsideMargin", typeof(Thickness), typeof(ScrollBarAssistant), new PropertyMetadata(default(Thickness)));



        public static bool GetIsExecuteCommand(DependencyObject obj) => (bool)obj.GetValue(IsExecuteCommandProperty);
        public static void SetIsExecuteCommand(DependencyObject obj, bool value) => obj.SetValue(IsExecuteCommandProperty, value);
        public static readonly DependencyProperty IsExecuteCommandProperty =
            DependencyProperty.RegisterAttached("IsExecuteCommand", typeof(bool), typeof(ScrollBarAssistant),
                new PropertyMetadata(ValueBoxes.FalseBox, Changed));

        private static void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var btn = d as RepeatButton;

            //if(d is ScrollBar sb)
            //{
            //    MoveToPosition(sb);
            //}
            if (d is RepeatButton btn)
            {
                if (btn.IsPressed)
                {
                    if (storyboard == null)
                        storyboard = new Storyboard();
                    var sb = btn.TemplatedParent as ScrollBar;
                    var sv = sb.TemplatedParent as ScrollViewer;
                    sv.ScrollChanged += Sv_ScrollChanged;
                    btn.PreviewMouseLeftButtonDown += Btn_PreviewMouseLeftButtonDown;
                    btn.PreviewMouseLeftButtonUp += Btn_PreviewMouseLeftButtonUp;
                    MoveToPosition(sv, sb, btn);
                }
            }
        }

        private static int i;
        private static Storyboard storyboard;
        private static void Sv_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            Trace.WriteLine(i++);
        }

        private static void MoveToPosition(ScrollViewer sv, ScrollBar sb, RepeatButton btn)
        {
            double fromvalue = 0;
            double tovalue = 0;
            switch (btn.Name)
            {
                case "LineDownButton":
                    fromvalue = sv.VerticalOffset;
                    tovalue = sv.ExtentHeight - sv.ViewportHeight;
                    break;
                case "LineUpButton":
                    fromvalue = sv.VerticalOffset;
                    tovalue = 0;
                    break;
                case "LineLeftButton":
                    fromvalue = sv.HorizontalOffset;
                    tovalue = 0;
                    break;
                case "LineRightButton":
                    fromvalue = sv.HorizontalOffset;
                    tovalue = sv.ExtentWidth - sv.ViewportWidth;
                    break;
                default:
                    break;
            }

            //if (storyboard == null) storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = fromvalue;
            doubleAnimation.To = tovalue;
            doubleAnimation.Duration = TimeSpan.FromSeconds(3);
            doubleAnimation.FillBehavior = FillBehavior.Stop;

            doubleAnimation.Completed += (s, e) =>
            {
                //sb.Value = value;
                //sv.ScrollToVerticalOffset(value);
            };
            var h = sv.ExtentHeight;
            var w = sv.ExtentWidth;
            var sw = sv.ViewportWidth;
            var sh = sv.ViewportHeight;
            var v = sv.ContentVerticalOffset;
            var hv = sv.ContentHorizontalOffset;
            var hoff = sv.HorizontalOffset;
            var voff = sv.VerticalOffset;

            Storyboard.SetTarget(doubleAnimation, sb);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Value"));
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
            //sb.BeginAnimation(ScrollBar.ValueProperty, doubleAnimation);          
        }

        private static void Btn_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //var btn = sender as RepeatButton;
            //var sb = btn.TemplatedParent as ScrollBar;
            //var sv = sb.TemplatedParent as ScrollViewer;
            //MoveToPosition(sv,sb,btn);
            storyboard.Resume();
        }

        private static void Btn_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //storyboard.Pause();
            storyboard.Pause();
            //storyboard = null;
        }
    }
}
