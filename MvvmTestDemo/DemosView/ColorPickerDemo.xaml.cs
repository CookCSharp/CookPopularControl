﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MvvmTestDemo.DemosView
{
    /// <summary>
    /// ColorPicker.xaml 的交互逻辑
    /// </summary>
    public partial class ColorPickerDemo : UserControl
    {
        public ColorPickerDemo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 将blend的8位颜色值转为color
        /// </summary>
        /// <param name="colorName"></param>
        /// <returns></returns>
        public Color ToColor(string colorName)
        {
            if (colorName.StartsWith("#"))
                colorName = colorName.Replace("#", string.Empty);
            int v = int.Parse(colorName, System.Globalization.NumberStyles.HexNumber);
            return new Color()
            {
                A = Convert.ToByte((v >> 24) & 255),
                R = Convert.ToByte((v >> 16) & 255),
                G = Convert.ToByte((v >> 8) & 255),
                B = Convert.ToByte((v >> 0) & 255)
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog dialog = new System.Windows.Forms.ColorDialog();
            dialog.AnyColor = true;
            dialog.ShowDialog();
        }
    }
}
