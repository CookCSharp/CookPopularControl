using CookPopularControl.Controls;
using CookPopularControl.Tools.Helpers;
using CookPopularControl.Tools.Windows.Tasks;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：TimePicker
 * Author： Chance_写代码的厨子
 * Create Time：2021-07-13 15:14:04
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 时钟
    /// </summary>
    [TemplatePart(Name = Ticks, Type = typeof(SimpleGrid))]
    [TemplatePart(Name = ElementUniformGrid, Type = typeof(UniformGrid))]
    [TemplatePart(Name = Am, Type = typeof(RadioButton))]
    [TemplatePart(Name = Pm, Type = typeof(RadioButton))]
    public class Clock : Control
    {
        private const string Ticks = "PART_Ticks";
        private const string ElementUniformGrid = "PART_UniformGrid";
        private const string Am = "PART_Am";
        private const string Pm = "PART_Pm";
        private const double Radian = 180 / Math.PI; //弧度
        private const int HourDegree = 30; //360/12=30
        private const int MinuteSecondDegree = 6; //30/5=6
        private const int ClockLineLength = 70; //(150 - 5 * 2) / 2 = 70，时钟的长度为150，厚度为5，故得出标准长度为70
        private static readonly Brush ThemeBrush = ResourceHelper.GetResource<Brush>("PrimaryThemeBrush");
        private static readonly Brush AssistantBrush = ResourceHelper.GetResource<Brush>("AssistantThemeBrush");
        private static readonly Brush UnabledBrush = ResourceHelper.GetResource<Brush>("UnEnabledBrush");

        public static readonly ICommand ResetCommand = new RoutedCommand(nameof(ResetCommand), typeof(Clock));
        public static readonly ICommand ConfirmCommand = new RoutedCommand(nameof(ConfirmCommand), typeof(Clock));

        private SimpleGrid _ticksGrid; //钟表盘
        private RadioButton _amRadioButton; //上午
        private RadioButton _pmRadioButton; //下午

        private DispatcherTimer _secondTimer; //秒针定时器
        private StackPanel _tickHourPanel;
        private StackPanel _tickMinuteSecondPanel;
        private Ellipse _ellipse;
        private Rectangle _recHour;
        private Rectangle _recMinute;
        private Rectangle _recSecond;
        private int _hourValue;
        private int _minuteValue;
        private int _secondValue;
        private bool _isRecHourPressed;
        private bool _isRecMinutePressed;
        //private bool _isRecSecondPressed; //不能调整秒针
        private RotateTransform _hourRotate;
        private RotateTransform _minuteRotate;
        private RotateTransform _secondRotate;


        /// <summary>
        /// 当前时间
        /// </summary>
        internal string CurrentTime
        {
            get { return (string)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="CurrentTime"/>的依赖属性
        /// </summary>
        internal static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(string), typeof(Clock), new PropertyMetadata(DateTime.Now.ToString("HH:mm:ss")));

        public Clock()
        {
            CommandBindings.Add(new CommandBinding(ResetCommand, (s, e) => InitClockDial()));
        }

        public override void OnApplyTemplate()
        {
            if (_amRadioButton != null)
                _amRadioButton.Click -= (s, e) => UpdateCurrentValue(0);
            if (_pmRadioButton != null)
                _pmRadioButton.Click -= (s, e) => UpdateCurrentValue(1);

            base.OnApplyTemplate();

            _ticksGrid = GetTemplateChild(Ticks) as SimpleGrid;
            _amRadioButton = GetTemplateChild(Am) as RadioButton;
            _pmRadioButton = GetTemplateChild(Pm) as RadioButton;
            _amRadioButton.Click += (s, e) => UpdateCurrentValue(0);
            _pmRadioButton.Click += (s, e) => UpdateCurrentValue(1);

            Loaded += Clock_Loaded;
            Unloaded += TimePicker_Unloaded;
        }

        private void UpdateCurrentValue(int index)
        {
            int.TryParse(CurrentTime.Substring(0, 2), out int hourValue);
            if (index.Equals(0))
            {
                _hourValue = hourValue - 12;
            }
            else
            {
                _hourValue = hourValue + 12;
            }

            CurrentTime = _hourValue.ToString("00") + CurrentTime.Substring(2);
        }

        private void Clock_Loaded(object sender, RoutedEventArgs e)
        {
            //添加小时刻度
            for (int i = 0; i < 12; i++)
            {
                _tickHourPanel = new StackPanel();
                _tickHourPanel.RenderTransformOrigin = new Point(0.5, 0.5);
                RotateTransform rtf = new RotateTransform() { Angle = HourDegree * i };
                _tickHourPanel.RenderTransform = rtf;

                var rec1 = new Rectangle() { Width = 3, Height = 10, Fill = ThemeBrush };
                var rec2 = new Rectangle() { Width = 3, Height = 60, Fill = Brushes.Transparent };

                _tickHourPanel.Children.Add(rec1);
                _tickHourPanel.Children.Add(rec2);

                _ticksGrid.Children.Add(_tickHourPanel);
            }

            //添加分钟、秒刻度
            for (int j = 0; j < 60; j++)
            {
                _tickMinuteSecondPanel = new StackPanel();
                _tickMinuteSecondPanel.RenderTransformOrigin = new Point(0.5, 0.5);
                RotateTransform rtf = new RotateTransform() { Angle = MinuteSecondDegree * j };
                _tickMinuteSecondPanel.RenderTransform = rtf;

                var rec1 = new Rectangle() { Width = 2, Height = 10, Fill = ThemeBrush };
                var rec2 = new Rectangle() { Width = 2, Height = 60, Fill = Brushes.Transparent };

                _tickMinuteSecondPanel.Children.Add(rec1);
                _tickMinuteSecondPanel.Children.Add(rec2);

                _ticksGrid.Children.Add(_tickMinuteSecondPanel);
            }

            //添加时针，分针，秒针
            _ellipse = new Ellipse() { Width = 12, Height = 12, Fill = AssistantBrush };
            _recHour = new Rectangle() { Width = 4, Height = 40, Fill = AssistantBrush, VerticalAlignment = VerticalAlignment.Top, RenderTransformOrigin = new Point(0.5, 1), Margin = new Thickness(0, 30, 0, 0), ToolTip = "时针" };
            _recMinute = new Rectangle() { Width = 3, Height = 55, Fill = AssistantBrush, VerticalAlignment = VerticalAlignment.Top, RenderTransformOrigin = new Point(0.5, 1), Margin = new Thickness(0, 15, 0, 0), ToolTip = "分针" };
            _recSecond = new Rectangle() { Width = 2, Height = 70, Fill = AssistantBrush, VerticalAlignment = VerticalAlignment.Top, RenderTransformOrigin = new Point(0.5, 1), IsEnabled = false, ToolTip = "秒针" };
            _ticksGrid.Children.Add(_ellipse);
            _ticksGrid.Children.Add(_recHour);
            _ticksGrid.Children.Add(_recMinute);
            _ticksGrid.Children.Add(_recSecond);

            //初始化钟盘数据
            InitClockDial();

            //注册事件
            _ticksGrid.MouseMove += _ticksGrid_MouseMove;
            _ticksGrid.MouseLeave += _ticksGrid_MouseLeave;
            _recHour.MouseMove += new MouseEventHandler(RecHourMouseMoveHandler);
            _recMinute.MouseMove += new MouseEventHandler(RecMinuteMouseMoveHandler);
            //_recSecond.MouseMove += new MouseEventHandler(RecSecondMouseMoveHandler);
        }

        private void TimePicker_Unloaded(object sender, RoutedEventArgs e)
        {
            Loaded -= Clock_Loaded;
            Unloaded -= TimePicker_Unloaded;
            _ticksGrid.MouseMove -= _ticksGrid_MouseMove;
            _ticksGrid.MouseLeave -= _ticksGrid_MouseLeave;
            _recHour.MouseMove -= new MouseEventHandler(RecHourMouseMoveHandler);
            _recMinute.MouseMove -= new MouseEventHandler(RecMinuteMouseMoveHandler);
            //_recSecond.MouseMove -= new MouseEventHandler(RecSecondMouseMoveHandler);
        }

        /// <summary>
        /// 初始化钟盘数据
        /// </summary>
        private void InitClockDial()
        {
            if (DateTime.Now.Hour > 12)
                _pmRadioButton.IsChecked = true;
            else
                _amRadioButton.IsChecked = true;

            _hourValue = DateTime.Now.Hour;
            _minuteValue = DateTime.Now.Minute;
            _secondValue = DateTime.Now.Second;

            var _secondAngle = _secondValue * MinuteSecondDegree;
            var _minuteAngle = (_minuteValue + _secondValue / 60D) * MinuteSecondDegree;
            var _hourAngle = (_hourValue + _minuteValue / 60D + _secondValue / 3600D) * HourDegree;

            _secondRotate = new RotateTransform() { Angle = _secondAngle };
            _minuteRotate = new RotateTransform() { Angle = _minuteAngle };
            _hourRotate = new RotateTransform() { Angle = _hourAngle };

            _recSecond.RenderTransform = _secondRotate;
            _recMinute.RenderTransform = _minuteRotate;
            _recHour.RenderTransform = _hourRotate;

            if (_secondTimer == null)
            {
                _secondTimer = new DispatcherTimer();
                _secondTimer.Interval = TimeSpan.FromSeconds(1);
                _secondTimer.Tick += async (s, e) =>
                {
                    _secondValue += 1;
                    CalculateCurrentTime();

                    await Task.Run(() =>
                    {
                        SynchronizationWithAsync.AppInvoke(() =>
                        {
                            _secondRotate.Angle = _secondValue * MinuteSecondDegree;
                            _minuteRotate.Angle = (_minuteValue + _secondValue / 60D) * MinuteSecondDegree;
                            _hourRotate.Angle = (_hourValue + _minuteValue / 60D + _secondValue / 3600D) * HourDegree;
                        });
                    });
                };
                _secondTimer.IsEnabled = true;
            }
        }

        private void _ticksGrid_MouseMove(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(sender as IInputElement);
            var angle = GetRadian(point) * Radian;
            var rotate = new RotateTransform() { Angle = angle };

            if (_isRecHourPressed)
            {
                if (_pmRadioButton.IsChecked!.Value)
                    angle = angle == 0 ? 0 : angle + 360;

                _ellipse.Fill = _recHour.Fill;
                _recHour.RenderTransform = rotate;
                _hourValue = (int)(angle / HourDegree);
                _minuteValue = (int)((angle % HourDegree) / HourDegree * 60);
                _minuteRotate.Angle = (angle % HourDegree) / HourDegree * 360;

                System.Diagnostics.Debug.WriteLine("分钟角度:" + _minuteRotate.Angle);
            }
            else if (_isRecMinutePressed)
            {
                _ellipse.Fill = _recMinute.Fill;
                _recMinute.RenderTransform = rotate;
                _minuteValue = (int)(angle / MinuteSecondDegree);
                //_hourRotate.Angle = (_hourValue % 12 + _minuteValue / 60D) * HourDegree;
            }
            //else if (_isRecSecondPressed)
            //{
            //    _ellipse.Fill = _recSecond.Fill;
            //    _recSecond.RenderTransform = rotate;
            //    _secondAngle = angle;
            //}

            CalculateCurrentTime();

            e.Handled = true;
        }

        private void _ticksGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            _isRecHourPressed = false;
            _isRecMinutePressed = false;

            _recHour.IsEnabled = true;
            _recHour.Fill = AssistantBrush;
            _recMinute.IsEnabled = true;
            _recMinute.Fill = AssistantBrush;
        }

        private void RecHourMouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _isRecHourPressed = true;
                _isRecMinutePressed = false;
                //_isRecSecondPressed = false;

                _recMinute.IsEnabled = false;
                _recMinute.Fill = UnabledBrush;
            }
            else
            {
                _isRecHourPressed = false;

                _recMinute.IsEnabled = true;
                _recMinute.Fill = AssistantBrush;
            }
        }

        private void RecMinuteMouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _isRecHourPressed = false;
                _isRecMinutePressed = true;
                //_isRecSecondPressed = false;

                _recHour.IsEnabled = false;
                _recHour.Fill = UnabledBrush;
            }
            else
            {
                _isRecMinutePressed = false;

                _recHour.IsEnabled = true;
                _recHour.Fill = AssistantBrush;
            }
        }

        private void RecSecondMouseMoveHandler(object sender, MouseEventArgs e)
        {
            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    _isRecHourPressed = false;
            //    _isRecMinutePressed = false;
            //    _isRecSecondPressed = true;
            //}
            //else
            //    _isRecSecondPressed = false;
        }

        /// <summary>
        /// 由鼠标点得出指针转到哪个象限(以中心为原点，按照数学中ⅠⅡⅢⅣ)来确定位置，从而得到旋转弧度
        /// </summary>
        /// <param name="point"></param>
        /// <returns>旋转弧度</returns>
        private double GetRadian(Point point)
        {
            var x = point.X - ClockLineLength;
            var y = ClockLineLength - point.Y;
            var alpha = y / x;

            if ((x > 0 && y > 0) || (x > 0 && y < 0))      //第一、四象限
                return Math.PI / 2D - Math.Atan(alpha);
            else if ((x < 0 && y > 0) || (x < 0 && y < 0)) //第二、三象限
                return Math.PI * 3 / 2D - Math.Atan(alpha);
            else if (x > 0 && y == 0)
                return Math.PI / 2D;
            else if (x == 0 && y > 0)
                return 0;
            else if (x < 0 && y == 0)
                return -Math.PI / 2D;
            else if (x == 0 && y < 0)
                return Math.PI;
            else if (x == 0 && y == 0)
                return Math.PI / 2D;

            return default;
        }

        /// <summary>
        /// 计算调整后的时间
        /// </summary>
        private void CalculateCurrentTime()
        {
            if (_secondValue >= 60)
            {
                _secondValue = 0;
                _minuteValue += 1;
            }
            if (_minuteValue >= 60)
            {
                _minuteValue = 0;
                _hourValue += 1;
            }
            if (_hourValue >= 0 && _hourValue < 12)
            {
                _amRadioButton.IsChecked = true;
            }
            else if (_hourValue >= 12 && _hourValue < 24)
            {
                _pmRadioButton.IsChecked = true;
            }
            else if (_hourValue == 24)
            {
                _hourValue = 0;
            }

            CurrentTime = _hourValue.ToString("00") + ":" + _minuteValue.ToString("00") + ":" + _secondValue.ToString("00");
        }
    }
}
