using CookPopularControl.Communal.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DefaultNewItemFactory
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-13 10:02:48
 */
namespace CookPopularControl.Controls.Dragables.Core
{
    public class DefaultNewItemFactory
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
