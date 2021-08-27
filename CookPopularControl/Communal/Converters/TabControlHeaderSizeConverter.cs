using CookPopularControl.Tools.Extensions.Markup;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using CookPopularControl.Controls.Dragables;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：TabControlHeaderSizeConverter
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 14:50:39
 */
namespace CookPopularControl.Communal.Converters
{
    /// <summary>
    /// <see cref="TabControl"/>标题面板大小转换器
    /// </summary>
    [MarkupExtensionReturnType(typeof(double))]
    public class TabControlHeaderSizeConverter : MarkupExtensionBase, IMultiValueConverter
    {
        public Orientation Orientation { get; set; }

        /// <summary>
        /// 第一个值通常是父控件可用的大小 
        /// 第二个值来自于<see cref="DragableItemsControl.ItemsPresenterWidthProperty"/>
        /// 额外的值应该是相邻子项大小(宽度或高度)，这将影响(减少)可用大小
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) throw new ArgumentNullException("values");

            if (values.Length < 2) return Binding.DoNothing;

            var val = values
                .Skip(2)
                .OfType<double>()
                .Where(d => !double.IsInfinity(d) && !double.IsNaN(d))
                .Aggregate(values.OfType<double>().First(), (current, diminish) => current - diminish);

            var maxWidth = values.Take(2).OfType<double>().Min();

            return Math.Min(Math.Max(val, 0), maxWidth);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
