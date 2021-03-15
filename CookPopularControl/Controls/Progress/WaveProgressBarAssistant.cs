using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
namespace CookPopularControl.Controls.Progress
{
    /// <summary>
    /// 提供WaveProgressBar的附加属性类
    /// </summary>
    public class WaveProgressBarAssistant
    {
        private const string WaveGrid = "PART_Wave";
        private const string WaveRectangle = "PART_Rectangle";

        public static double GetWaveStrokeThickness(DependencyObject obj) => (double)obj.GetValue(WaveStrokeThicknessProperty);
        public static void SetWaveStrokeThickness(DependencyObject obj, double value) => obj.SetValue(WaveStrokeThicknessProperty, value);
        /// <summary>
        /// 波纹厚度
        /// </summary>
        public static readonly DependencyProperty WaveStrokeThicknessProperty =
            DependencyProperty.RegisterAttached("WaveStrokeThickness", typeof(double), typeof(WaveProgressBarAssistant), 
                new PropertyMetadata(ValueBoxes.Double1Box,new PropertyChangedCallback(OnWaveStrokeThicknessValueChanged)));

        private static void OnWaveStrokeThicknessValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wave = d as ProgressBar;
            wave.Loaded += delegate { WaveLoaded(wave); };     
        }
        private static void WaveLoaded(ProgressBar wave)
        {
            if (wave != null && wave.IsLoaded)
            {
                var waveGrid = wave.Template.FindName(WaveGrid, wave) as Grid;
                waveGrid.Width = wave.Width + 2 * GetWaveStrokeThickness(wave);
                waveGrid.Height = wave.Width;

                var waveRectangle = wave.Template.FindName(WaveRectangle, wave) as Rectangle;
                waveRectangle.Margin = new Thickness(0, -GetWaveStrokeThickness(wave), 0, 0);
            }
        }

        public static Brush GetWaveStroke(DependencyObject obj) => (Brush)obj.GetValue(WaveStrokeProperty);
        public static void SetWaveStroke(DependencyObject obj, Brush value) => obj.SetValue(WaveStrokeProperty, value);
        /// <summary>
        /// 波纹绘制的颜色
        /// </summary>
        public static readonly DependencyProperty WaveStrokeProperty =
            DependencyProperty.RegisterAttached("WaveStroke", typeof(Brush), typeof(WaveProgressBarAssistant), new PropertyMetadata(default(Brush)));

        
        public static double GetWaveHeight(DependencyObject obj) => (double)obj.GetValue(WaveHeightProperty);
        public static void SetWaveHeight(DependencyObject obj, double value) => obj.SetValue(WaveHeightProperty, value);
        /// <summary>
        /// 表示波纹的高度
        /// </summary>
        public static readonly DependencyProperty WaveHeightProperty =
            DependencyProperty.RegisterAttached("WaveHeight", typeof(double), typeof(WaveProgressBarAssistant), new PropertyMetadata(ValueBoxes.Double30Box));
    }
}
