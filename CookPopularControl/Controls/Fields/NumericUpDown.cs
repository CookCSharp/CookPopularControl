using CookPopularControl.Tools.Boxes;
using CookPopularControl.Tools.Helpers;
using System.ComponentModel;
using System.Runtime.InteropServices;
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
    [TemplatePart(Name = ElementPlusButton, Type = typeof(System.Windows.Controls.Button))]
    [TemplatePart(Name = ElementSubtractButton, Type = typeof(System.Windows.Controls.Button))]
    public class NumericUpDown : Control
    {
        private const string ElementTextBox = "PART_TextBox";
        private const string ElementPlusButton = "PART_PlusButton";
        private const string ElementSubtractButton = "PART_SubtractButton";

        private TextBox numericTextBox;
        private System.Windows.Controls.Button plusButton;
        private System.Windows.Controls.Button subtractButton;
        private bool canUpdateValue; //输入的值满足要求时为false，不满足要求时重新赋值=CurrentValue

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
                var minValue = numeric.Minimum;
                var value = (double)baseValue;
                if (value < minValue)
                {
                    numeric.Value = minValue;
                    numeric.canUpdateValue = true;
                    numeric.SetNumericText();
                    return minValue;
                }
                var maxValue = numeric.Maximum;
                if (value > maxValue)
                {
                    numeric.Value = maxValue;
                    numeric.canUpdateValue = true;
                }

                numeric.SetNumericText();
                return value > maxValue ? maxValue : value;
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
            }
        }

        private void SetNumericText()
        {
            if (numericTextBox != null && canUpdateValue)
            {
                numericTextBox.Text = CurrentValue;
                numericTextBox.Select(numericTextBox.Text.Length, 0);
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


        private string CurrentValue => string.IsNullOrWhiteSpace(ValueFormat) ? Value.ToString() : Value.ToString(ValueFormat);

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
            if (numericTextBox != null)
            {
                TextCompositionManager.RemovePreviewTextInputHandler(numericTextBox, (s, e) => UpdateValue());
                numericTextBox.TextChanged -= (s, e) => UpdateValue();
                numericTextBox.PreviewKeyDown -= NumericTextBox_PreviewKeyDown;
                numericTextBox.LostFocus -= NumericTextBox_LostFocus;
            }

            numericTextBox = GetTemplateChild(ElementTextBox) as TextBox;
            plusButton = GetTemplateChild(ElementPlusButton) as System.Windows.Controls.Button;
            subtractButton = GetTemplateChild(ElementSubtractButton) as System.Windows.Controls.Button;

            SetUpDownButtonEnabled(this);

            if (numericTextBox != null)
            {
                numericTextBox.SetBinding(TextBoxBase.SelectionBrushProperty, new Binding(TextBoxBase.SelectionBrushProperty.Name) { Source = numericTextBox });
#if !NET46 && !NET461
                numericTextBox.SetBinding(TextBoxBase.SelectionTextBrushProperty, new Binding(TextBoxBase.SelectionTextBrushProperty.Name) { Source = numericTextBox });
#endif
                numericTextBox.SetBinding(TextBoxBase.SelectionOpacityProperty, new Binding(TextBoxBase.SelectionOpacityProperty.Name) { Source = numericTextBox });
                numericTextBox.SetBinding(TextBoxBase.CaretBrushProperty, new Binding(TextBoxBase.CaretBrushProperty.Name) { Source = numericTextBox });

                TextCompositionManager.AddPreviewTextInputHandler(numericTextBox, (s, e) => UpdateValue());
                numericTextBox.TextChanged += (s, e) => UpdateValue();
                numericTextBox.PreviewKeyDown += NumericTextBox_PreviewKeyDown;
                numericTextBox.LostFocus += NumericTextBox_LostFocus;
                numericTextBox.Text = CurrentValue;
            }

            base.OnApplyTemplate();
        }

        private void SetUpDownButtonEnabled(NumericUpDown numeric)
        {
            if (numeric.plusButton == null || numeric.subtractButton == null)
                return;
            if (numeric.Value.Equals(numeric.Maximum))
                numeric.plusButton.Foreground = ResourceHelper.GetResource<Brush>("UnEnabledBrush");
            else
                numeric.plusButton.Foreground = NumericUpDownAssistant.GetUpDownButtonBrush(numeric);
            if (numeric.Value.Equals(numeric.Minimum))
                numeric.subtractButton.Foreground = ResourceHelper.GetResource<Brush>("UnEnabledBrush");
            else
                numeric.subtractButton.Foreground = NumericUpDownAssistant.GetUpDownButtonBrush(numeric);
        }

        private void UpdateValue()
        {
            if (string.IsNullOrWhiteSpace(numericTextBox.Text))
            {
                canUpdateValue = false;
                Value = 0;
                canUpdateValue = true;
            }
            else if (double.TryParse(numericTextBox.Text, out double v))
            {
                canUpdateValue = false;
                Value = v;
                canUpdateValue = true;
            }
            else
            {
                canUpdateValue = true;
                CoerceValue(ValueProperty);
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
            numericTextBox.Text = CurrentValue;
        }

        //使得输入框获取焦点
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (numericTextBox != null)
            {
                numericTextBox.Focus();
                numericTextBox.Select(numericTextBox.Text.Length, 0);
            }
        }

        //鼠标滚轮改变Value的值
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (numericTextBox.IsFocused && !IsReadOnly)
            {
                Value += e.Delta > 0 ? Increment : -Increment;
                e.Handled = true;
            }
        }

        //判断<see cref="Value"/>改变时的值是否在<see cref="double"/>的范围
        private static bool IsInRangeOfDouble(object value)
        {
            var v = (double)value;
            return !(double.IsNaN(v) || double.IsInfinity(v));
        }
    }
}
