using CookPopularControl.Controls;
using CookPopularCSharpToolkit.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;


/*
 * Description：TreeViewItemMarginConverter 
 * Author： Chance.Zheng
 * Create Time: 2022-06-21 16:14:51
 * .Net Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2020-2022 All Rights Reserved.
 */
namespace CookPopularControl.Communal
{
    [MarkupExtensionReturnType(typeof(Thickness))]
    public class TreeViewItemMarginConverter : MarkupExtensionBase, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var left = 0.0;
            double defautlMargin = 0.0;
            UIElement element = (TreeViewItem)value;
            if(element != null)
            {
                var treeview = element.GetVisualAncestors().OfType<TreeView>().FirstOrDefault();
                if(treeview != null)
                {
                    defautlMargin = TreeViewAssistant.GetExpandButtonSize(treeview) + 3;
                    if (TreeViewAssistant.GetIsShowHeaderIcon(treeview))
                        defautlMargin *= 2; 
                }
            }
            while (element != null && element.GetType() != typeof(TreeView))
            {
                element = (UIElement)VisualTreeHelper.GetParent(element);
                if (element is TreeViewItem)
                    left += defautlMargin;
            }
            return new Thickness(left, 0, 0, 0);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
