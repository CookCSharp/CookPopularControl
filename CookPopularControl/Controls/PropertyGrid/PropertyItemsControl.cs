using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;



/*
 * Description：PropertyItemsControl 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-01-07 14:48:08
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */
namespace CookPopularControl.Controls
{
    public class PropertyItemsControl : ListBox
    {
        protected override bool IsItemItsOwnContainerOverride(object item) => item is PropertyItem;

        public PropertyItemsControl()
        {
            VirtualizingPanel.SetIsVirtualizingWhenGrouping(this, true);
            VirtualizingPanel.SetScrollUnit(this, ScrollUnit.Pixel);

            this.Loaded += PropertyItemsControl_Loaded;
        }

        private void PropertyItemsControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.Items.Count > 0)
                SelectedIndex = 0;
        }
    }
}
