using CookPopularControl.Communal.ViewModel;
using System;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DefaultNewItemFactory
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-13 10:02:48
 */
namespace CookPopularControl.Controls.Dragables
{
    internal class DefaultNewItemFactory
    {
        public static Func<HeaderedItemViewModel> DefaultItemFactory
        {
            get
            {
                return () =>
                {
                    var dateTime = DateTime.Now;

                    return new HeaderedItemViewModel()
                    {
                        Header = dateTime.ToString("yyyy"),
                        Content = dateTime.ToString("R")
                    };
                };
            }
        }
    }
}
