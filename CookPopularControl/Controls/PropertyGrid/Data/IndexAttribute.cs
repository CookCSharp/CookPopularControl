using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Description：IndexAttribute 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-01-10 19:49:13
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */
namespace CookPopularControl.Controls
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class IndexAttribute : Attribute
    {
        public int SelectedIndex { get; set; }

        public IndexAttribute(int selectedIndex)
        {
            SelectedIndex = selectedIndex;
        }
    }
}
