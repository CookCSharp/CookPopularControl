using CookPopularCSharpToolkit.Communal;
using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Layout;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ColorPicker
 * Author： Chance_写代码的厨子
 * Create Time：2021-06-04 15:06:25
 */


/****
 * 摘自知乎（https://www.zhihu.com/question/265265004）
 * HSL介绍如下：
 * Hue 叫色相，表示了颜色在色环上的角度。纯红色位于 0 度，纯绿色位于 120 度，纯蓝色位于 240 度。具体计算时，角度主要由 RGB 中最大的那个决定，由次大的那个进行修正。
 * Lightness 叫亮度，具体计算公式是 RGB 中最大值与最小值的平均值。
 * Saturation 叫饱和度，表示的是 RGB 三个值的对比有多强烈。其算式中分子 C 是 RGB 中最大值与最小值的差值，但当 L 特别大或特别小的时候，C 的范围有限，为了把它归一化到 [0,1]，又除了个与亮度有关的分母。
 * 当我们已知 R、G、B 的值时，我们可以将其映射到 色相E、亮度L 和饱和度S(windows颜色选择器工具中色相、饱和度和亮度的范围都是 0~240)。其映射公式如下：
 * ①.色相 E （范围 [0°,360°)）分成 6 段讨论：
 * 1) R≥G≥B 时， E = 0°  + 60°·(G-B)/(R-B)
 * 2) G≥R≥B 时， E = 120°- 60°·(R-B)/(G-B)
 * 3) G≥B≥R 时， E = 120°+ 60°·(B-R)/(G-R)
 * 4) B≥G≥R 时， E = 240°- 60°·(G-R)/(B-R)
 * 5) B≥R≥G 时， E = 240°+ 60°·(R-G)/(B-G)
 * 6) R≥B≥G 时， E = 360°- 60°·(B-G)/(R-G)
 * ②.亮度 L （范围 [0,1]）
 * L = 1/2 * [max(r,g,b) + min(r,g,b)] / M 
 * ③.饱和度 S （范围 [0,1]）
 * S = [max(r,g,b) - min(r,g,b)] / [M - |max(r,g,b) + min(r,g,b) - M|]
 * 其中 M 为 RGB 的理论最大值，一般为 255
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 颜色选择器
    /// </summary>
    /// <remarks>参考<see cref="System.Windows.Forms.ColorDialog"/></remarks>
    [TemplatePart(Name = ElementColorPanel, Type = typeof(Border))]
    [TemplatePart(Name = ElementBorderPicker, Type = typeof(Border))]
    [TemplatePart(Name = ElementBorderDrag, Type = typeof(Border))]
    [TemplatePart(Name = ElementSliderOpacity, Type = typeof(System.Windows.Controls.Slider))]
    [TemplatePart(Name = ElementSliderColor, Type = typeof(System.Windows.Controls.Slider))]
    public class ColorPicker : Control, IDisposable
    {
        private const string ElementColorPanel = "PART_ColorPanel";
        private const string ElementBorderPicker = "PART_BorderPicker";
        private const string ElementBorderDrag = "PART_BorderDrag";
        private const string ElementSliderOpacity = "PART_SliderOpacity";
        private const string ElementSliderColor = "PART_SliderColor";

        /// <summary>
        ///     颜色选取面板宽度
        /// </summary>
        private const double ColorPanelWidth = 300;

        /// <summary>
        ///     颜色选取面板高度
        /// </summary>
        private const double ColorPanelHeight = 150;

        private Border _colorPanel;
        private Border _borderPicker;
        private Border _borderDrag;
        private System.Windows.Controls.Slider _sliderOpacity;
        private System.Windows.Controls.Slider _sliderColor;

        /// <summary>
        /// 是否在拖动小球
        /// </summary>
        private bool _isOnDragging;
        /// <summary>
        /// 是否需要更新小球位置
        /// </summary>
        private bool _isNeedUpdatePicker = true;

        /// <summary>
        /// 颜色选择面板<see cref="ElementColorPanel"/>中颜色选择器<see cref="ElementBorderPicker"/>的坐标位置
        /// </summary>
        private Point _pickerCurrentPosition = new Point(1, 0);

        private TranslateTransform _pickerTranslateTransform;

        private bool _surpressPropertyChanged;

        private bool disposedValue;

        /// <summary>
        /// 选中的颜色(包含透明度)
        /// </summary>
        public SolidColorBrush SelectedBrush
        {
            get { return (SolidColorBrush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }
        /// <summary>
        /// 表示<see cref="SelectedBrush"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush", typeof(SolidColorBrush), typeof(ColorPicker), new PropertyMetadata(Brushes.Red, OnSelectedBrushChanged));

        private static void OnSelectedBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPicker picker)
            {
                var color = ((SolidColorBrush)e.NewValue).Color;
                picker.UpdateHexColor(color);
                picker.OnSelectedColorChanged((Brush)e.OldValue, (Brush)e.NewValue);
            }
        }


        /// <summary>
        /// 选中色(不包含透明度)
        /// </summary>
        /// <remarks>透明度100%</remarks>
        internal SolidColorBrush SelectedBrushWithoutOpacity
        {
            get { return (SolidColorBrush)GetValue(SelectedBrushWithoutOpacityProperty); }
            set { SetValue(SelectedBrushWithoutOpacityProperty, value); }
        }
        /// <summary>
        /// 表示<see cref="SelectedBrushWithoutOpacity"/>的依赖属性
        /// </summary>
        internal static readonly DependencyProperty SelectedBrushWithoutOpacityProperty =
            DependencyProperty.Register("SelectedBrushWithoutOpacity", typeof(SolidColorBrush), typeof(ColorPicker), new PropertyMetadata(Brushes.White));


        /// <summary>
        /// 颜色选择器面板的主背景色
        /// </summary>
        /// <remarks><see cref="ElementColorPanel"/></remarks>
        internal SolidColorBrush ColorPanelBackground
        {
            get { return (SolidColorBrush)GetValue(ColorPanelBackgroundProperty); }
            set { SetValue(ColorPanelBackgroundProperty, value); }
        }
        /// <summary>
        /// 表示<see cref="ColorPanelBackground"/>的依赖属性
        /// </summary>
        internal static readonly DependencyProperty ColorPanelBackgroundProperty =
            DependencyProperty.Register("ColorPanelBackground", typeof(SolidColorBrush), typeof(ColorPicker), new PropertyMetadata(Brushes.Red));


        /// <summary>
        /// 十六进制颜色表示
        /// </summary>
        public string HexColor
        {
            get { return (string)GetValue(HexColorProperty); }
            set { SetValue(HexColorProperty, value); }
        }
        /// <summary>
        /// 表示<see cref="HexColor"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty HexColorProperty =
            DependencyProperty.Register("HexColor", typeof(string), typeof(ColorPicker), new PropertyMetadata("#FFFF0000"));


        /// <summary>
        /// 透明度
        /// </summary>
        internal int ChannelA
        {
            get { return (int)GetValue(ChannelAProperty); }
            set { SetValue(ChannelAProperty, value); }
        }
        /// <summary>
        /// 表示<see cref="ChannelA"/>的依赖属性
        /// </summary>
        internal static readonly DependencyProperty ChannelAProperty =
            DependencyProperty.Register("ChannelA", typeof(int), typeof(ColorPicker), new PropertyMetadata(0xFF, OnChannelAChanged));

        private static void OnChannelAChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPicker picker)
            {
                var v = (int)e.NewValue;
                if (v < 0)
                    v = 0;
                else if (v > 0xFF)
                    v = 0xFF;

                picker._sliderOpacity.Value = v;
            }
        }


        /// <summary>
        /// 红色
        /// </summary>
        internal int ChannelR
        {
            get { return (int)GetValue(ChannelRProperty); }
            set { SetValue(ChannelRProperty, value); }
        }
        /// <summary>
        /// 表示<see cref="ChannelR"/>的依赖属性
        /// </summary>
        internal static readonly DependencyProperty ChannelRProperty =
            DependencyProperty.Register("ChannelR", typeof(int), typeof(ColorPicker), new PropertyMetadata(0xFF, OnChanelRGBChanged));


        /// <summary>
        /// 绿色
        /// </summary>
        internal int ChannelG
        {
            get { return (int)GetValue(ChannelGProperty); }
            set { SetValue(ChannelGProperty, value); }
        }
        /// <summary>
        /// 表示<see cref="ChannelG"/>的依赖属性
        /// </summary>
        internal static readonly DependencyProperty ChannelGProperty =
            DependencyProperty.Register("ChannelG", typeof(int), typeof(ColorPicker), new PropertyMetadata(ValueBoxes.Inter0Box, OnChanelRGBChanged));


        /// <summary>
        /// 蓝色
        /// </summary>
        internal int ChannelB
        {
            get { return (int)GetValue(ChannelBProperty); }
            set { SetValue(ChannelBProperty, value); }
        }
        /// <summary>
        /// 表示<see cref="ChannelB"/>的依赖属性
        /// </summary>
        internal static readonly DependencyProperty ChannelBProperty =
            DependencyProperty.Register("ChannelB", typeof(int), typeof(ColorPicker), new PropertyMetadata(ValueBoxes.Inter0Box, OnChanelRGBChanged));

        private static void OnChanelRGBChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColorPicker picker)
            {
                if (!picker._surpressPropertyChanged)
                {
                    var color = Color.FromArgb((byte)picker.ChannelA, (byte)picker.ChannelR, (byte)picker.ChannelG, (byte)picker.ChannelB);
                    var hsl = HslColor.RGBToHSL(color);

                    if (!(color.R == color.G && color.R == color.B))
                        picker._sliderColor.Value = hsl.Hue;

                    picker._pickerTranslateTransform.X = picker._colorPanel.ActualWidth * (hsl.Saturation);
                    picker._pickerTranslateTransform.Y = picker._colorPanel.ActualHeight * (1 - hsl.Lightness);
                    picker._borderPicker.RenderTransform = picker._pickerTranslateTransform;

                    picker.UpdateRGBValues(color);
                    picker.UpdateHexColor(color);
                }
            }
        }


        public override void OnApplyTemplate()
        {
            if (_sliderOpacity != null)
            {
                _sliderOpacity.ValueChanged -= _sliderOpacity_ValueChanged;
            }

            if (_sliderColor != null)
            {
                _sliderColor.ValueChanged -= _sliderColor_ValueChanged;
            }

            base.OnApplyTemplate();

            _colorPanel = GetTemplateChild(ElementColorPanel) as Border;
            _borderDrag = GetTemplateChild(ElementBorderDrag) as Border;
            _borderPicker = GetTemplateChild(ElementBorderPicker) as Border;
            _sliderOpacity = GetTemplateChild(ElementSliderOpacity) as System.Windows.Controls.Slider;
            _sliderColor = GetTemplateChild(ElementSliderColor) as System.Windows.Controls.Slider;

            if (_borderDrag != null)
            {
                var behavior = new MouseDragElementBehavior();
                behavior.DragFinished += MouseDragElementBehavior_OnDragFinished;
                behavior.DragBegun += MouseDragElementBehavior_OnDragging;
                behavior.Dragging += MouseDragElementBehavior_OnDragging;
                behavior.ConstrainToParentBounds = true;
                var collection = Interaction.GetBehaviors(_borderDrag);
                collection.Add(behavior);
            }

            if (_borderPicker != null)
            {
                _pickerTranslateTransform = new TranslateTransform();
                _borderPicker.RenderTransform = _pickerTranslateTransform;
                _pickerTranslateTransform.X = ColorPanelWidth;
                _pickerTranslateTransform.Y = 0;
            }

            if (_sliderOpacity != null)
            {
                _sliderOpacity.ValueChanged += _sliderOpacity_ValueChanged;
            }

            if (_sliderColor != null)
            {
                _sliderColor.ValueChanged += _sliderColor_ValueChanged;
            }
        }

        private void MouseDragElementBehavior_OnDragFinished(object sender, MouseEventArgs e) => _borderDrag.RenderTransform = new MatrixTransform();

        private void MouseDragElementBehavior_OnDragging(object sender, MouseEventArgs e)
        {
            var p = e.GetPosition(_colorPanel);
            if (p.X >= 0 && p.X <= ColorPanelWidth && p.Y >= 0 && p.Y <= ColorPanelHeight)
            {
                _isOnDragging = true;
                UpdateColorWhenDrag(p);
                _isOnDragging = false;
            }
        }

        /// <summary>
        /// 拖动时更新颜色
        /// </summary>
        private void UpdateColorWhenDrag(Point p)
        {
            var matrix = _borderPicker.RenderTransform.Value;

            if (p.X < 0)
            {
                p.X = 0;
            }
            else if (p.X > ColorPanelWidth)
            {
                p.X = ColorPanelWidth;
            }

            if (p.Y < 0)
            {
                p.Y = 0;
            }
            else if (p.Y > ColorPanelHeight)
            {
                p.Y = ColorPanelHeight;
            }

            if (_isNeedUpdatePicker)
            {
                _borderPicker.RenderTransform = new MatrixTransform(matrix.M11, matrix.M12, matrix.M21, matrix.M22, p.X, p.Y);
            }

            //var pointColor = GetPixelColor();
            //var color = Color.FromArgb((byte)_sliderOpacity.Value, (byte)pointColor.R, (byte)pointColor.G, (byte)pointColor.B);
            //SelectedBrush = new SolidColorBrush(color);
            //SelectedBrushWithoutOpacity = new SolidColorBrush(pointColor);

            _pickerCurrentPosition.X = p.X / _colorPanel.ActualWidth;
            _pickerCurrentPosition.Y = p.Y / _colorPanel.ActualHeight;

            HslColor hsv = new HslColor(_sliderColor.Value, _pickerCurrentPosition.X, 1 - _pickerCurrentPosition.Y);
            var currentColor = HslColor.HSLToRGB(hsv);
            SelectedBrushWithoutOpacity = new SolidColorBrush(currentColor);
            currentColor.A = (byte)ChannelA;
            SelectedBrush = new SolidColorBrush(currentColor);

            UpdateRGBValues(currentColor);
            UpdateHexColor(currentColor);
        }

        private void _sliderOpacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var color = SelectedBrush.Color;
            SelectedBrush = new SolidColorBrush(Color.FromArgb((byte)_sliderOpacity.Value, color.R, color.G, color.B));
        }

        private void _sliderColor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var panelColor = HslColor.HSLToRGB(new HslColor(e.NewValue, 1, 1));
            ColorPanelBackground = new SolidColorBrush(panelColor);

            var actualColor = HslColor.HSLToRGB(new HslColor(e.NewValue, _pickerCurrentPosition.X, 1 - _pickerCurrentPosition.Y));
            SelectedBrush = new SolidColorBrush(Color.FromArgb((byte)_sliderOpacity.Value, actualColor.R, actualColor.G, actualColor.B));

            UpdateRGBValues(SelectedBrush.Color);
            UpdateHexColor(SelectedBrush.Color);
        }

        private void UpdateRGBValues(Color color)
        {
            _surpressPropertyChanged = true;

            ChannelA = color.A;
            ChannelR = color.R;
            ChannelG = color.G;
            ChannelB = color.B;

            _surpressPropertyChanged = false;
        }

        private void UpdateHexColor(Color color)
        {
            HexColor = $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        [Description("颜色选择事件")]
        public event RoutedPropertyChangedEventHandler<Brush> SelectedBrushChanged
        {
            add { this.AddHandler(SelectedBrushChangedEvent, value); }
            remove { this.RemoveHandler(SelectedBrushChangedEvent, value); }
        }
        /// <summary>
        /// <see cref="SelectedColorChangedEvent"/>标识颜色选择事件
        /// </summary>
        public static readonly RoutedEvent SelectedBrushChangedEvent =
            EventManager.RegisterRoutedEvent("SelectedBrushChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Brush>), typeof(ColorPicker));

        protected virtual void OnSelectedColorChanged(Brush oldValue, Brush newValue)
        {
            RoutedPropertyChangedEventArgs<Brush> arg = new RoutedPropertyChangedEventArgs<Brush>(oldValue, newValue, SelectedBrushChangedEvent);
            this.RaiseEvent(arg);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Window.GetWindow(this)?.Close();
                    }));
                }

                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        ~ColorPicker()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
