using CookPopularControl.Controls;
using CookPopularCSharpToolkit.Communal;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：AnimationDemoViewModel
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-05 17:42:05
 */
namespace MvvmTestDemo.DemoViewModels
{
    public class AnimationDemoViewModel : BindableBase
    {
        public UIThreadCollection<SolidColorBrush> TilePanelColors { get; set; }
        public AnimationTilePanel TilePanel { get; set; } = new AnimationTilePanel();

        public AnimationDemoViewModel()
        {
            TilePanelColors = UIThreadCollection<SolidColorBrush>.Create(App.DemoColors.Select(c => c.ToBrushFromColor()).ToList(), 48, 0, 96);
        }
    }
}
