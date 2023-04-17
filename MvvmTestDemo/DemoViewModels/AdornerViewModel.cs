/*
 * Description：AdornerViewModel 
 * Author： Chance.Zheng
 * Create Time: 2023-03-08 16:24:31
 * .Net Version: 6.0
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2023 All Rights Reserved.
 */


using CookPopularControl.Controls;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MvvmTestDemo.DemoViewModels
{
    public class AdornerViewModel : BindableBase, IDialogResultable<string>
    {
        public string Content1 { get; set; } = "Chance1111";
        public string Message { get; set; }
        public string Content2 { get; set; } = "CloseDialogBox222";

        public string Result { get; set; }

        public Action CloseAction { get; set; }
    }
}
