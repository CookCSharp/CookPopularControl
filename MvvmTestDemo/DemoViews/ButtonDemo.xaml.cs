﻿using CookPopularCSharpToolkit.Communal;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// ButtonDemo.xaml 的交互逻辑
    /// </summary>
    //[AddINotifyPropertyChangedInterface]
    public partial class ButtonDemo : UserControl, INotifyPropertyChanged
    {
        public ButtonDemo()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                for (int i = 1; i <= 100; i++)
                {
                    Value = i;
                    System.Threading.Thread.Sleep(10);
                }

                ButtonContent = "成功!";
                //System.Threading.Thread.Sleep(500);
                //Value = 0;
                //ButtonContent = "开始!";
            });
        }

        private string _buttonContent = "开始";
        public string ButtonContent
        {
            get { return _buttonContent; }
            set { _buttonContent = value; OnPropertyChanged(); }
        }


        private double _value;
        public double Value
        {
            get => _value;
            set
            {
                if (_value == value) return;
                _value = value;
                if (_value.BetweenMinMax(0, 100))
                    IsButtonEnabled = false;
                else
                    IsButtonEnabled = true;
                OnPropertyChanged();
            }
        }


        private bool _isEnabled = true;
        public bool IsButtonEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled == value) return;
                _isEnabled = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
