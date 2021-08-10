using CookPopularControl.Tools.Extensions.Values;
using MvvmTestDemo.Commumal;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：OverViewDemoViewModel
 * Author： Chance_写代码的厨子
 * Create Time：2021-07-30 17:02:09
 */
namespace MvvmTestDemo.DemoViewModels
{
    public class OverViewDemoViewModel : ViewModelBase
    {
        public string ButtonContent { get; set; } = "开始";
        public bool IsButtonEnabled { get; set; }

        private double _value;
        public double Value
        {
            get => _value;
            set
            {
                SetProperty(ref _value, value, () => 
                {
                    if (_value.BetweenMinMax(0, 100))
                        IsButtonEnabled = false;
                    else
                        IsButtonEnabled = true;
                });
            }
        }

        public string TextContent { get; set; }
        public string NumericValue { get; set; }
        public string PasswordContent { get; set; }

        public ObservableCollection<object> Lists { get; set; } = new ObservableCollection<object>() { "111", "222", "333" };


        public ICommand ProgressButtonCommand { get; set; }


        public OverViewDemoViewModel()
        {
            InitCommand();
        }

        private void InitCommand()
        {
            ProgressButtonCommand = new DelegateCommand(OnProgressButton);
        }


        private async void OnProgressButton()
        {
            await Task.Run(() =>
            {
                for (int i = 1; i <= 100; i++)
                {
                    Value = i;
                    System.Threading.Thread.Sleep(10);
                }

                ButtonContent = "成功";
                //System.Threading.Thread.Sleep(500);
                //Value = 0;
                //ButtonContent = "开始!";
            });
        }
    }
}
