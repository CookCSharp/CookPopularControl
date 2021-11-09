using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：NumericUpDown
 * Author： Chance_写代码的厨子
 * Create Time：2021-05-12 17:25:14
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 表示显示数值的 Windows 数字显示框（也称作 up-down 控件）。
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [ComVisible(true)]
    [DefaultBindingProperty("Value")]
    [DefaultEvent("ValueChanged")]
    [DefaultProperty("Value")]
    [TemplatePart(Name = ElementTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = ElementPlusButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementSubtractButton, Type = typeof(Button))]
    public class NumericUpDown : Control
    {
        private const string ElementTextBox = "PART_TextBox";
        private const string ElementPlusButton = "PART_PlusButton";
        private const string ElementSubtractButton = "PART_SubtractButton";
        private TextBox _numericTextBox;
        private Button _plusButton;
        private Button _subtractButton;
        private double _currentValue;

        public static readonly ICommand SubtractCommand = new RoutedCommand(nameof(SubtractCommand), typeof(NumericUpDown));
        public static readonly ICommand PlusCommand = new RoutedCommand(nameof(PlusCommand), typeof(NumericUpDown));
        public static readonly ICommand ClearCommand = new RoutedCommand(nameof(ClearCommand), typeof(NumericUpDown));


        /// <summary>
        /// 当前值
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="Value"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(ValueBoxes.Double0Box, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, OnCoerceValueChanged), IsInRangeOfDouble);

        private static object OnCoerceValueChanged(DependencyObject d, object baseValue)
        {
            if (d is NumericUpDown numeric)
            {
                double minValue = numeric.Minimum;
                double value = (double)baseValue;
                if (value < minValue)
                {
                    numeric.Value = minValue;
                    numeric.SetNumericText();
                    return minValue;
                }
                double maxValue = numeric.Maximum;
                if (value > maxValue)
                {
                    numeric.Value = maxValue;
                    numeric.SetNumericText();
                    return maxValue;
                }
            }

            return baseValue;
        }
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumericUpDown numeric)
            {
                if (numeric.IsLoaded)
                    numeric.SetUpDownButtonEnabled(numeric);

                numeric.SetNumericText();
                numeric.OnValueChanged((double)e.OldValue, (double)e.NewValue);
                numeric._currentValue = (double)e.NewValue;
            }
        }

        private void SetNumericText()
        {
            if (_numericTextBox != null) 
            {
                var text = string.IsNullOrWhiteSpace(ValueFormat) ? Value.ToString() : Value.ToString(ValueFormat);
                _numericTextBox.Text = text;
                _numericTextBox.Select(_numericTextBox.Text.Length, 0);
            }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="Maximum"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(NumericUpDown),
                new PropertyMetadata(double.MaxValue, OnMaximumChanged, OnCoerceMaximumChanged), IsInRangeOfDouble);

        private static object OnCoerceMaximumChanged(DependencyObject d, object baseValue)
        {
            if (d is NumericUpDown numeric)
            {
                return (double)baseValue < numeric.Minimum ? numeric.Minimum : baseValue;
            }

            return baseValue;
        }
        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumericUpDown numeric)
            {
                numeric.CoerceValue(MinimumProperty);
                numeric.CoerceValue(ValueProperty);
            }
        }


        /// <summary>
        /// 最小值
        /// </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="Minimum"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(NumericUpDown),
                new PropertyMetadata(double.MinValue, OnMinimumChanged, OnCoerceMinimumChanged), IsInRangeOfDouble);

        private static object OnCoerceMinimumChanged(DependencyObject d, object baseValue)
        {
            if (d is NumericUpDown numeric)
            {
                return (double)baseValue > numeric.Maximum ? numeric.Maximum : baseValue;
            }

            return baseValue;
        }
        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumericUpDown numeric)
            {
                numeric.CoerceValue(MaximumProperty);
                numeric.CoerceValue(ValueProperty);
            }
        }


        /// <summary>
        /// 获取或设置单击向上或向下按钮时，数字显示框（也称作 up-down 控件）递增或递减的值
        /// </summary>
        public double Increment
        {
            get { return (double)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="Increment"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register("Increment", typeof(double), typeof(NumericUpDown), new PropertyMetadata(ValueBoxes.Double1Box));


        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="IsReadOnly"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(ValueBoxes.FalseBox));


        /// <summary>
        /// 表示要显示数字的格式
        /// </summary>
        public string ValueFormat
        {
            get { return (string)GetValue(ValueFormatProperty); }
            set { SetValue(ValueFormatProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="ValueFormat"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ValueFormatProperty =
            DependencyProperty.Register("ValueFormat", typeof(string), typeof(NumericUpDown), new PropertyMetadata(default(string)));


        /// <summary>
        /// 是否显示UpDownButton
        /// </summary>
        public bool IsShowUpDownButton
        {
            get { return (bool)GetValue(IsShowUpDownButtonProperty); }
            set { SetValue(IsShowUpDownButtonProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 标识<see cref="IsShowUpDownButton"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsShowUpDownButtonProperty =
            DependencyProperty.Register("IsShowUpDownButton", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(ValueBoxes.TrueBox));



        [Description("Value值更改时发生")]
        public event RoutedPropertyChangedEventHandler<double> ValueChanged
        {
            add { this.AddHandler(ValueChangedEvent, value); }
            remove { this.RemoveHandler(ValueChangedEvent, value); }
        }
        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(NumericUpDown));

        protected virtual void OnValueChanged(double oldValue, double newValue)
        {
            RoutedPropertyChangedEventArgs<double> arg = new RoutedPropertyChangedEventArgs<double>(oldValue, newValue, ValueChangedEvent);
            this.RaiseEvent(arg);
        }


        public NumericUpDown()
        {
            CommandBindings.Add(new CommandBinding(PlusCommand, (s, e) =>
            {
                if (IsReadOnly) return;

                Value += Increment;
            }));
            CommandBindings.Add(new CommandBinding(SubtractCommand, (s, e) =>
            {
                if (IsReadOnly) return;

                Value -= Increment;
            }));
            CommandBindings.Add(new CommandBinding(ClearCommand, (s, e) =>
            {
                if (IsReadOnly) return;

                SetCurrentValue(ValueProperty, ValueBoxes.Double0Box);
            }));

            Loaded += (s, e) => OnApplyTemplate();
        }

        public override void OnApplyTemplate()
        {
            if (_numericTextBox != null)
            {
                TextCompositionManager.RemovePreviewTextInputHandler(_numericTextBox, (s, e) => UpdateValue());
                _numericTextBox.TextChanged -= (s, e) => UpdateValue();
                _numericTextBox.PreviewKeyDown -= NumericTextBox_PreviewKeyDown;
                _numericTextBox.LostFocus -= NumericTextBox_LostFocus;
            }

            _numericTextBox = GetTemplateChild(ElementTextBox) as TextBox;
            _plusButton = GetTemplateChild(ElementPlusButton) as Button;
            _subtractButton = GetTemplateChild(ElementSubtractButton) as Button;

            SetUpDownButtonEnabled(this);

            if (_numericTextBox != null)
            {
                _numericTextBox.SetBinding(TextBoxBase.SelectionBrushProperty, new Binding(TextBoxBase.SelectionBrushProperty.Name) { Source = _numericTextBox });
#if !NET46 && !NET461
                _numericTextBox.SetBinding(TextBoxBase.SelectionTextBrushProperty, new Binding(TextBoxBase.SelectionTextBrushProperty.Name) { Source = _numericTextBox });
#endif
                _numericTextBox.SetBinding(TextBoxBase.SelectionOpacityProperty, new Binding(TextBoxBase.SelectionOpacityProperty.Name) { Source = _numericTextBox });
                _numericTextBox.SetBinding(TextBoxBase.CaretBrushProperty, new Binding(TextBoxBase.CaretBrushProperty.Name) { Source = _numericTextBox });

                TextCompositionManager.AddPreviewTextInputHandler(_numericTextBox, (s, e) => UpdateValue());
                _numericTextBox.TextChanged += (s, e) => UpdateValue();
                _numericTextBox.PreviewKeyDown += NumericTextBox_PreviewKeyDown;
                _numericTextBox.LostFocus += NumericTextBox_LostFocus;
                _numericTextBox.Text = Value.ToString();
            }

            base.OnApplyTemplate();
        }

        private void SetUpDownButtonEnabled(NumericUpDown numeric)
        {
            if (numeric._plusButton == null || numeric._subtractButton == null)
                return;
            if (numeric.Value.Equals(numeric.Maximum))
                numeric._plusButton.Foreground = ResourceHelper.GetResource<Brush>("UnEnabledBrush");
            else
                numeric._plusButton.Foreground = NumericUpDownAssistant.GetUpDownButtonBrush(numeric);
            if (numeric.Value.Equals(numeric.Minimum))
                numeric._subtractButton.Foreground = ResourceHelper.GetResource<Brush>("UnEnabledBrush");
            else
                numeric._subtractButton.Foreground = NumericUpDownAssistant.GetUpDownButtonBrush(numeric);
        }

        private void UpdateValue()
        {
            if (string.IsNullOrWhiteSpace(_numericTextBox.Text))
            {
                Value = 0;
            }
            else if (double.TryParse(_numericTextBox.Text, out double v))
            {
                Value = v;
            }
            else if (_numericTextBox.Text.StartsWith("-") && _numericTextBox.Text.Length == 1)
            {

            }
            else if ((_numericTextBox.Text.StartsWith("-") && Regex.Matches(_numericTextBox.Text, "-").Count == 1)
                    && _numericTextBox.Text.Length > 1 && RegularPatterns.Default.IsMatchRegularPattern(_numericTextBox.Text, InputTextType.Digital))
            {
                double.TryParse(_numericTextBox.Text, out double v1);
                Value = v1;
            }
            else
            {
                Value = _currentValue;
                CoerceValue(ValueProperty);
                SetNumericText();
            }
        }


        //Up、Down键控制Value的值
        private void NumericTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (IsReadOnly) return;

            if (e.Key == Key.Up)
                Value += Increment;
            else if (e.Key == Key.Down)
                Value -= Increment;
        }

        //失去焦点时重新赋值
        private void NumericTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _numericTextBox.Text = Value.ToString();
        }

        //使得输入框获取焦点
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (_numericTextBox != null)
            {
                _numericTextBox.Focus();
                _numericTextBox.Select(_numericTextBox.Text.Length, 0);
            }
        }

        //鼠标滚轮改变Value的值
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (_numericTextBox.IsFocused && !IsReadOnly)
            {
                Value += e.Delta > 0 ? Increment : -Increment;
                e.Handled = true;
            }
        }

        //判断<see cref="Value"/>改变时的值是否在<see cref="double"/>的范围
        private static bool IsInRangeOfDouble(object value)
        {
            double v = (double)value;
            return !(double.IsNaN(v) || double.IsInfinity(v));
        }
    }
}
