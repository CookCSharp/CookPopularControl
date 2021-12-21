using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;



/*
 * Description：ToastMessage 
 * Author： Chance(a cook of write code)
 * Company: NCATest
 * Create Time：2021-11-25 17:32:33
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) NCATest 2021 All Rights Reserved.
 */
namespace CookPopularControl.Windows
{
    /// <summary>
    /// 表示类似Android的Toast消息框
    /// </summary>
    [TemplatePart(Name = ElementBorder, Type = (typeof(UIElement)))]
    public class ToastMessage : NormalWindow
    {
        /// <summary>
        /// <see cref="ToastMessage"/>的消息图标
        /// </summary>
        public Geometry ToastMessageIcon
        {
            get => (Geometry)GetValue(ToastMessageIconProperty);
            set => SetValue(ToastMessageIconProperty, value);
        }
        /// <summary>
        /// 提供<see cref="ToastMessageIcon"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ToastMessageIconProperty =
            DependencyProperty.Register("ToastMessageIcon", typeof(Geometry), typeof(ToastMessage), new PropertyMetadata(Geometry.Empty));


        /// <summary>
        /// <see cref="ToastMessage"/>的消息图标画刷
        /// </summary>
        public Brush ToastMessageIconBrush
        {
            get => (Brush)GetValue(ToastMessageIconBrushProperty);
            set => SetValue(ToastMessageIconBrushProperty, value);
        }
        /// <summary>
        /// 提供<see cref="ToastMessageIconBrush"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ToastMessageIconBrushProperty =
            DependencyProperty.Register("ToastMessageIconBrush", typeof(Brush), typeof(ToastMessage), new PropertyMetadata(default(Brush)));




        private const string ElementBorder = "RootBorder";
        private UIElement _border;

        static ToastMessage()
        {
            StyleProperty.OverrideMetadata(typeof(ToastMessage), new FrameworkPropertyMetadata(default, (s, e) => ResourceHelper.GetResource<Style>("ToastMessageStyle")));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _border = GetTemplateChild(ElementBorder) as UIElement;
        }

        /// <summary>
        /// 显示<see cref="ToastMessage"/>
        /// </summary>
        /// <param name="content">内容</param>
        public static void ShowWarning(object content)
        {
            ToastMessageInfo messageInfo = new ToastMessageInfo()
            {
                Content = content,
                MessageIcon = ResourceHelper.GetResource<Geometry>("WarningGeometry"),
                MessageIconBrush = ResourceHelper.GetResource<Brush>("MessageDialogWarningBrush"),
            };
            InternalShow(messageInfo);
        }

        private static void InternalShow(ToastMessageInfo messageInfo)
        {
            ToastMessage toastMessage = new ToastMessage()
            {
                Content = messageInfo.Content,
                Owner = WindowExtension.GetActiveWindow(),
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ToastMessageIcon = messageInfo.MessageIcon,
                ToastMessageIconBrush = messageInfo.MessageIconBrush,
            };

            toastMessage.Show();
            toastMessage.SetTimer(toastMessage, messageInfo.Duration, messageInfo.ShowType);
        }

        private void SetTimer(ToastMessage toast, double duration, ToastMessageShowType showType)
        {
            var timer = new DispatcherTimer(DispatcherPriority.Normal, Dispatcher);
            timer.Interval = TimeSpan.FromSeconds(duration);
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                timer = null;

                CloseAnimation(toast, showType);
            };
            timer.Start();
        }

        private void CloseAnimation(ToastMessage toast, ToastMessageShowType showType)
        {
            switch (showType)
            {
                case ToastMessageShowType.None:
                    toast.Close();
                    break;
                case ToastMessageShowType.Fade:
                    var fadeAnimation = AnimationHelper.CreateDoubleAnimation(0);
                    fadeAnimation.Completed += Animation_Completed;
                    toast.BeginAnimation(OpacityProperty, fadeAnimation);
                    break;
                case ToastMessageShowType.Scroll:
                    var scale = new ScaleTransform() { ScaleX = 1, ScaleY = 1 };
                    toast.RenderTransform = scale;
                    toast.RenderTransformOrigin = new Point(0.5, 0.5);
                    var animation1 = AnimationHelper.CreateDoubleAnimation(0);
                    var animation2 = AnimationHelper.CreateDoubleAnimation(0);
                    animation1.Completed += Animation_Completed;
                    animation2.Completed += Animation_Completed;
                    scale.BeginAnimation(ScaleTransform.ScaleXProperty, animation1);
                    scale.BeginAnimation(ScaleTransform.ScaleYProperty, animation2);
                    break;
                case ToastMessageShowType.Rotate:
                    var rotate = new RotateTransform() { Angle = 0 };
                    toast.RenderTransform = rotate;
                    toast.RenderTransformOrigin = new Point(0.5, 0.5);
                    var rotateAnimation = AnimationHelper.CreateDoubleAnimation(360D);
                    rotateAnimation.Completed += Animation_Completed;
                    rotate.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
                    break;
                default:
                    break;
            }
        }

        private void Animation_Completed(object sender, EventArgs e) => Close();
    }


    public class ToastMessageInfo
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// 消息图标
        /// </summary>
        public Geometry MessageIcon { get; set; }

        /// <summary>
        /// 消息图标颜色
        /// </summary>
        public Brush MessageIconBrush { get; set; }

        /// <summary>
        /// 动画显示类型
        /// </summary>
        public ToastMessageShowType ShowType { get; set; } = ToastMessageShowType.Fade;

        /// <summary>
        /// 是否显示关闭按钮
        /// </summary>
        public bool IsShowCloseButton { get; set; } = true;

        /// <summary>
        /// 消息是否自动关闭
        /// </summary>
        public bool IsAutoClose { get; set; } = true;

        /// <summary>
        /// 消息持续时间
        /// </summary>
        /// <remarks>单位:s</remarks>
        public double Duration { get; set; } = 3;

        /// <summary>
        /// 消息关闭前触发的方法
        /// </summary>
        public Action<bool> ActionBeforeClose { get; set; }
    }

    /// <summary>
    /// <see cref="ToastMessage"/>动画显示类型
    /// </summary>
    public enum ToastMessageShowType
    {
        /// <summary>
        /// 无动画显示
        /// </summary>
        None,
        /// <summary>
        /// 渐变动画显示
        /// </summary>
        Fade,
        /// <summary>
        /// 缩放动画显示
        /// </summary>
        Scroll,
        /// <summary>
        /// 旋转动画显示
        /// </summary>
        Rotate,
    }
}
