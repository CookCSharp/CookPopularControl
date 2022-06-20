using CookPopularCSharpToolkit.Communal;
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
 * Description：DivideLine 
 * Author： Chance.Zheng
 * Create Time: 2022-06-20 19:11:15
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2020-2022 All Rights Reserved.
 */
namespace CookPopularControl.Controls
{
    public class DivideLine : ContentControl
    {   
        public Brush Stroke
        {
            get => (Brush)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }
        /// <summary>
        /// 提供<see cref="Stroke"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =  
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(DivideLine), new PropertyMetadata(default(Brush)));


        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }
        /// <summary>
        /// 提供<see cref="StrokeThickness"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(DivideLine), new PropertyMetadata(ValueBoxes.Double1Box));



    }
}
