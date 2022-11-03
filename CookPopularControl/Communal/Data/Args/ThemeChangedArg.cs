/*
 * Description：ThemeChangedArg 
 * Author： Chance.Zheng
 * Create Time: 2022-11-01 14:33:37
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2020-2022 All Rights Reserved.
 */


using CookPopularControl.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CookPopularControl.Communal.Data
{
    public delegate void ThemeEventHandler<TDictionary>(object sender, ThemeChangedArg<TDictionary> e);

    public class ThemeChangedArg<TDictionary> : RoutedEventArgs
    {
        public TDictionary ThemeDictionary { get; private set; }

        public ThemeChangedArg(TDictionary dictionary) : base()
        {
            ThemeDictionary = dictionary;
        }
    }
}
