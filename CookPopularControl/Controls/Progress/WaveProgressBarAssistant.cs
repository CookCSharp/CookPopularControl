using CookPopularCSharpToolkit.Communal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：WaveProgressBarAssistant
 * Author： Chance_写代码的厨子
 * Create Time：2021-03-15 17:41:15
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 提供WaveProgressBar的附加属性类
    /// </summary>
    public class WaveProgressBarAssistant
    {
        private const string WaveGrid = "PART_Wave";
        private const string WaveRectangle = "PART_Rectangle";



        internal static bool GetIsStartWave(DependencyObject obj) => (bool)obj.GetValue(IsStartWaveProperty);
        internal static void SetIsStartWave(DependencyObject obj, bool value) => obj.SetValue(IsStartWaveProperty, ValueBoxes.BooleanBox(value));
        /// <summary>
        /// <see cref="IsStartWaveProperty"/>开启波纹
        /// </summary>
        /// <remarks>必须设置True，默认为False</remarks>
        internal static readonly DependencyProperty IsStartWaveProperty =
            DependencyProperty.RegisterAttached("IsStartWave", typeof(bool), typeof(WaveProgressBarAssistant), new PropertyMetadata(ValueBoxes.FalseBox, OnStartWave));

        private static void OnStartWave(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProgressBar wave)
            {
                wave.Loaded += delegate
                {
                    var waveGrid = wave.Template.FindName(WaveGrid, wave) as System.Windows.Controls.Grid;
                    if (waveGrid == null) return;
                    waveGrid.Width = wave.Width + 2 * GetWaveStrokeThickness(wave);
                    waveGrid.Height = wave.Width;

                    var waveRectangle = wave.Template.FindName(WaveRectangle, wave) as Rectangle;
                    waveRectangle.Margin = new Thickness(0, -GetWaveStrokeThickness(wave), 0, 0);
                };
            }
        }


        public static double GetWaveStrokeThickness(DependencyObject obj) => (double)obj.GetValue(WaveStrokeThicknessProperty);
        public static void SetWaveStrokeThickness(DependencyObject obj, double value) => obj.SetValue(WaveStrokeThicknessProperty, value);
        /// <summary>
        /// <see cref="WaveStrokeThicknessProperty"/>波纹厚度
        /// </summary>
        public static readonly DependencyProperty WaveStrokeThicknessProperty =
            DependencyProperty.RegisterAttached("WaveStrokeThickness", typeof(double), typeof(WaveProgressBarAssistant), new PropertyMetadata(ValueBoxes.Double1Box));


        public static Brush GetWaveStroke(DependencyObject obj) => (Brush)obj.GetValue(WaveStrokeProperty);
        public static void SetWaveStroke(DependencyObject obj, Brush value) => obj.SetValue(WaveStrokeProperty, value);
        /// <summary>
        /// <see cref="WaveStrokeProperty"/>波纹绘制的颜色
        /// </summary>
        public static readonly DependencyProperty WaveStrokeProperty =
            DependencyProperty.RegisterAttached("WaveStroke", typeof(Brush), typeof(WaveProgressBarAssistant), new PropertyMetadata(default(Brush)));


        public static double GetWaveHeight(DependencyObject obj) => (double)obj.GetValue(WaveHeightProperty);
        public static void SetWaveHeight(DependencyObject obj, double value) => obj.SetValue(WaveHeightProperty, value);
        /// <summary>
        /// <see cref="WaveHeightProperty"/>表示波纹的高度
        /// </summary>
        public static readonly DependencyProperty WaveHeightProperty =
            DependencyProperty.RegisterAttached("WaveHeight", typeof(double), typeof(WaveProgressBarAssistant), new PropertyMetadata(ValueBoxes.Double30Box));
    }
}
