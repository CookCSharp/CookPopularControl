﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：TreeViewModel
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-10 09:28:59
 */
namespace MvvmTestDemo.DemoModels
{
    public class TreeViewModel
    {
        public string Header { get; set; }

        public int Level { get; set; }

        public ObservableCollection<TreeViewModel> Children { get; set; }
    }
}