using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ViewModelBase
 * Author： Chance_写代码的厨子
 * Create Time：2021-07-30 09:19:43
 */
namespace MvvmTestDemo.Commumal
{
    /// <summary>
    /// ViewModel的基类
    /// </summary>
    public class ViewModelBase : BindableBase
    {
        public ICommand ViewLoadedCommand { get; set; }

        public ICommand ViewUnLoadedCommand { get; set; }

        public ICommand WindowClosingCommand { get; set; }

        public ICommand WindowClosedCommand { get; set; }
    }
}
