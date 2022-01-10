using CookPopularCSharpToolkit.Communal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;



/*
 * Description：ExpanderAssistant 
 * Author： Chance(a cook of write code)
 * Company: CookCSharp
 * Create Time：2022-01-08 14:54:54
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) CookCSharp 2022 All Rights Reserved.
 */
namespace CookPopularControl.Controls
{
    public class ExpanderAssistant
    {
        public static double GetExpanderHeaderWidth(DependencyObject obj) => (double)obj.GetValue(ExpanderHeaderWidthProperty);
        public static void SetExpanderHeaderWidth(DependencyObject obj, double value) => obj.SetValue(ExpanderHeaderWidthProperty, value);
        /// <summary>
        /// 表示当<see cref="ExpandDirection.Left"/>或者<see cref="ExpandDirection.Right"/>时<see cref="Expander"/>的标头宽度附加属性
        /// </summary>
        public static readonly DependencyProperty ExpanderHeaderWidthProperty =
            DependencyProperty.RegisterAttached("ExpanderHeaderWidth", typeof(double), typeof(ExpanderAssistant), new PropertyMetadata(ValueBoxes.Double30Box));


        public static double GetExpanderHeaderHeight(DependencyObject obj) => (double)obj.GetValue(ExpanderHeaderHeightProperty);
        public static void SetExpanderHeaderHeight(DependencyObject obj, double value) => obj.SetValue(ExpanderHeaderHeightProperty, value);
        /// <summary>
        /// 表示当<see cref="ExpandDirection.Down"/>或者<see cref="ExpandDirection.Up"/>时<see cref="Expander"/>的标头高度附加属性
        /// </summary>
        public static readonly DependencyProperty ExpanderHeaderHeightProperty =
            DependencyProperty.RegisterAttached("ExpanderHeaderHeight", typeof(double), typeof(ExpanderAssistant), new PropertyMetadata(ValueBoxes.Double30Box));


        public static Brush GetExpanderHeaderBackground(DependencyObject obj) => (Brush)obj.GetValue(ExpanderHeaderBackgroundProperty);
        public static void SetExpanderHeaderBackground(DependencyObject obj, Brush value) => obj.SetValue(ExpanderHeaderBackgroundProperty, value);
        /// <summary>
        /// 表示<see cref="HeaderedContentControl.Header"/>的背景色附加属性
        /// </summary>
        public static readonly DependencyProperty ExpanderHeaderBackgroundProperty =
            DependencyProperty.RegisterAttached("ExpanderHeaderBackground", typeof(Brush), typeof(ExpanderAssistant), new PropertyMetadata(default(Brush)));


        public static Geometry GetExpandedGeometry(DependencyObject obj) => (Geometry)obj.GetValue(ExpandedGeometryProperty);
        public static void SetExpandedGeometry(DependencyObject obj, Geometry value) => obj.SetValue(ExpandedGeometryProperty, value);
        /// <summary>
        /// 当<see cref="Expander.IsExpanded"/>为<c>True</c>时图标附加属性
        /// </summary>
        public static readonly DependencyProperty ExpandedGeometryProperty =
            DependencyProperty.RegisterAttached("ExpandedGeometry", typeof(Geometry), typeof(ExpanderAssistant), new PropertyMetadata(default(Geometry)));


        public static Geometry GetCollapsedGeometry(DependencyObject obj) => (Geometry)obj.GetValue(CollapsedGeometryProperty);
        public static void SetCollapsedGeometry(DependencyObject obj, Geometry value) => obj.SetValue(CollapsedGeometryProperty, value);
        /// <summary>
        /// 当<see cref="Expander.IsExpanded"/>为<c>False</c>时图标附加属性
        /// </summary>
        public static readonly DependencyProperty CollapsedGeometryProperty =
            DependencyProperty.RegisterAttached("CollapsedGeometry", typeof(Geometry), typeof(ExpanderAssistant), new PropertyMetadata(default(Geometry)));
    }
}
