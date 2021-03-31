using CookPopularControl.Tools.Boxes;
using CookPopularControl.Tools.Extensions.Images;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brush = System.Windows.Media.Brush;
using Image = System.Windows.Controls.Image;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：QRCodeControl
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-30 20:03:45
 */
namespace CookPopularControl.Controls.QRCode
{
    /// <summary>
    /// 二维码控件
    /// </summary>
    [TemplatePart(Name = CodeImage, Type = typeof(Image))]
    public class QRCodeControl : Control
    {
        private const string CodeImage = "PART_QRCodeImage";
        public static readonly ICommand RefreshQrCodeCommand = new RoutedCommand("RefreshQrCode", typeof(QRCodeControl));

        static QRCodeControl()
        {
            ForegroundProperty.AddOwner(typeof(QRCodeControl), new FrameworkPropertyMetadata(default(System.Windows.Media.Brush), FrameworkPropertyMetadataOptions.AffectsRender, GetChangedQRCodeImage));
            BackgroundProperty.AddOwner(typeof(QRCodeControl), new FrameworkPropertyMetadata(default(System.Windows.Media.Brush), FrameworkPropertyMetadataOptions.AffectsRender, GetChangedQRCodeImage));

            CommandManager.RegisterClassCommandBinding(typeof(QRCodeControl), new CommandBinding(RefreshQrCodeCommand, Executed));
        }

        private async static void Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var qrCode = sender as QRCodeControl;
            if (qrCode != null)
            {
                qrCode.OnIsRefreshChanged(false, true);
                qrCode.IsShowRefreshIcon = false;
                await Task.Delay(TimeSpan.FromSeconds(qrCode.Duration));
                qrCode.IsShowRefreshIcon = true;
            }
        }

        [Description("刷新事件")]
        public event RoutedEventHandler IsRefreshChanged
        {
            add { this.AddHandler(IsRefreshChangedEvent, value); }
            remove { this.RemoveHandler(IsRefreshChangedEvent, value); }
        }
        /// <summary>
        /// <see cref="IsRefreshChangedEvent"/>标识刷新的路由事件
        /// </summary>
        public static readonly RoutedEvent IsRefreshChangedEvent =
            EventManager.RegisterRoutedEvent("IsRefreshChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(QRCodeControl));
        private void OnIsRefreshChanged(bool oldValue, bool newValue)
        {
            RoutedPropertyChangedEventArgs<bool> arg = new RoutedPropertyChangedEventArgs<bool>(oldValue, newValue, IsRefreshChangedEvent);
            this.RaiseEvent(arg);
        }


        /// <summary>
        /// 是否显示刷新按钮        
        /// </summary>
        public bool IsShowRefreshIcon
        {
            get { return (bool)GetValue(IsShowRefreshIconProperty); }
            set { SetValue(IsShowRefreshIconProperty, ValueBoxes.BooleanBox(value)); }
        }
        public static readonly DependencyProperty IsShowRefreshIconProperty =
            DependencyProperty.Register("IsShowRefreshIcon", typeof(bool), typeof(QRCodeControl), new PropertyMetadata(ValueBoxes.FalseBox));

        /// <summary>
        /// 刷新按钮颜色
        /// </summary>
        public Brush RefreshButtonFill
        {
            get { return (Brush)GetValue(RefreshButtonFillProperty); }
            set { SetValue(RefreshButtonFillProperty, value); }
        }
        /// <summary>
        /// <see cref="RefreshButtonFillProperty"/>标识<see cref="RefreshButtonFill"/>的附加属性
        /// </summary>
        public static readonly DependencyProperty RefreshButtonFillProperty =
            DependencyProperty.Register("RefreshButtonFill", typeof(Brush), typeof(QRCodeControl), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 二维码持续时长
        /// </summary>
        /// <remarks>可设置无线等待<see cref="Timeout.InfiniteTimeSpan"/>,单位：s</remarks>
        public double Duration
        {
            get { return (double)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        /// <summary>
        /// <see cref="DurationProperty"/>标识<see cref="Duration"/>的附加属性
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(double), typeof(QRCodeControl), new PropertyMetadata(ValueBoxes.Double5Box));

        /// <summary>
        /// 二维码内容
        /// </summary>
        public string QrCodeContent
        {
            get { return (string)GetValue(QrCodeContentProperty); }
            set { SetValue(QrCodeContentProperty, value); }
        }
        /// <summary>
        /// <see cref="QrCodeContentProperty"/>标识<see cref="QrCodeContent"/>的附加属性
        /// </summary>
        public static readonly DependencyProperty QrCodeContentProperty =
            DependencyProperty.Register("QrCodeContent", typeof(string), typeof(QRCodeControl), new PropertyMetadata("**我是写代码的厨子**", GetChangedQRCodeImage));

        /// <summary>
        /// 二维码图标
        /// </summary>
        public BitmapSource QrCodeIcon
        {
            get { return (BitmapSource)GetValue(QrCodeIconProperty); }
            set { SetValue(QrCodeIconProperty, value); }
        }
        /// <summary>
        /// <see cref="QrCodeIconProperty"/>标识<see cref="QrCodeIcon"/>的附加属性
        /// </summary>
        public static readonly DependencyProperty QrCodeIconProperty =
            DependencyProperty.Register("QrCodeIcon", typeof(BitmapSource), typeof(QRCodeControl), new PropertyMetadata(GetChangedQRCodeImage));

        /// <summary>
        /// 图标百分比大小
        /// </summary>
        /// <remarks>0~30</remarks>
        public int QrCodeIconSizePercent
        {
            get { return (int)GetValue(QrCodeIconSizePercentProperty); }
            set { SetValue(QrCodeIconSizePercentProperty, value); }
        }
        /// <summary>
        /// <see cref="QrCodeIconSizePercentProperty"/>标识<see cref="QrCodeIconSizePercent"/>的附加属性
        /// </summary>
        public static readonly DependencyProperty QrCodeIconSizePercentProperty =
            DependencyProperty.Register("QrCodeIconSizePercent", typeof(int), typeof(QRCodeControl), new PropertyMetadata(ValueBoxes.Inter15Box, GetChangedQRCodeImage));

        /// <summary>
        /// 图标外边框Border宽度
        /// </summary>
        public int QrCodeIconBorderWidth
        {
            get { return (int)GetValue(QrCodeIconBorderWidthProperty); }
            set { SetValue(QrCodeIconBorderWidthProperty, value); }
        }
        /// <summary>
        /// <see cref="QrCodeIconBorderWidthProperty"/>标识<see cref="QrCodeIconBorderWidth"/>的附加属性
        /// </summary>
        public static readonly DependencyProperty QrCodeIconBorderWidthProperty =
            DependencyProperty.Register("QrCodeIconBorderWidth", typeof(int), typeof(QRCodeControl), new PropertyMetadata(ValueBoxes.Inter5Box, GetChangedQRCodeImage));

        /// <summary>
        /// 二维码像素
        /// </summary>
        public int QrCodePixelsPerModule
        {
            get { return (int)GetValue(QrCodePixelsPerModuleProperty); }
            set { SetValue(QrCodePixelsPerModuleProperty, value); }
        }
        /// <summary>
        /// <see cref="QrCodePixelsPerModuleProperty"/>标识<see cref="QrCodePixelsPerModule"/>的附加属性
        /// </summary>
        public static readonly DependencyProperty QrCodePixelsPerModuleProperty =
            DependencyProperty.Register("QrCodePixelsPerModule", typeof(int), typeof(QRCodeControl),
                new FrameworkPropertyMetadata(ValueBoxes.Inter30Box, FrameworkPropertyMetadataOptions.AffectsRender, GetChangedQRCodeImage));

        //属性更改时
        private static void GetChangedQRCodeImage(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var qrCodeControl = d as QRCodeControl;
                if (qrCodeControl.QrCodeIconSizePercent < 0)
                    qrCodeControl.QrCodeIconSizePercent = 0;
                if (qrCodeControl.QrCodeIconSizePercent > 30)
                    qrCodeControl.QrCodeIconSizePercent = 30;
                if (qrCodeControl.QrCodeIconBorderWidth <= 0)
                    qrCodeControl.QrCodeIconBorderWidth = 1;
                if (qrCodeControl.QrCodePixelsPerModule <= 0)
                    qrCodeControl.QrCodePixelsPerModule = 1;
                if (qrCodeControl.QrCodePixelsPerModule > 200)
                    qrCodeControl.QrCodePixelsPerModule = 200;

                //if (e.Property == ForegroundProperty)
                //    qrCodeControl.SetValue(ForegroundProperty, e.NewValue);
                //if (e.Property == BackgroundProperty)
                //    qrCodeControl.SetValue(BackgroundProperty, e.NewValue);

                if (qrCodeControl.IsLoaded)
                    SetQRCodeImage(qrCodeControl);
                else
                    qrCodeControl.Loaded += (s, e) => SetQRCodeImage(qrCodeControl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置QRCodeImage
        /// </summary>
        /// <param name="qr"></param>
        private static void SetQRCodeImage(QRCodeControl qr)
        {
            var image = qr.GetTemplateChild(CodeImage) as Image;
            image.Source = qr.GetQRCodeImage(qr);
        }

        private ImageSource GetQRCodeImage(QRCodeControl qrCodeControl)
        {
            if (qrCodeControl.Foreground is null) qrCodeControl.Foreground = Application.Current.Resources["PrimaryForeground"] as SolidColorBrush;
            if (qrCodeControl.Background is null) qrCodeControl.Background = Application.Current.Resources["PrimaryColorTheme"] as SolidColorBrush;

            System.Drawing.ColorConverter colorConverter = new System.Drawing.ColorConverter();
            using (QRCodeGenerator qrCodeGenerator = new QRCodeGenerator())
            {
                using (QRCodeData data = qrCodeGenerator.CreateQrCode(qrCodeControl.QrCodeContent ?? string.Empty, QRCodeGenerator.ECCLevel.Q))
                {
                    using (QRCoder.QRCode qrCode = new QRCoder.QRCode(data))
                    {
                        Bitmap codeImage = qrCode.GetGraphic(
                            qrCodeControl.QrCodePixelsPerModule,
                            (System.Drawing.Color)(colorConverter.ConvertFromString(qrCodeControl.Foreground.ToString())),
                            (System.Drawing.Color)(colorConverter.ConvertFromString(qrCodeControl.Background.ToString())),
                            //System.Drawing.Color.FromName(((SolidColorBrush)qrCodeControl.Background).Color.ToString()),
                            ImageBitmapExtension.ToBitmap(qrCodeControl.QrCodeIcon),
                            qrCodeControl.QrCodeIconSizePercent,
                            qrCodeControl.QrCodeIconBorderWidth);

                        codeImage.Save(AppDomain.CurrentDomain.BaseDirectory + "QRCode.png", ImageFormat.Png);
                        return ImageBitmapExtension.ToImageSource(codeImage);
                    }
                }
            }
        }
    }
}
