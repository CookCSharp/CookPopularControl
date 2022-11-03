using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace MvvmTestDemo.UserControls
{
    /// <summary>
    /// AnimationDemo.xaml 的交互逻辑
    /// </summary>
    public partial class Animation : UserControl
    {
        public Animation()
        {
            InitializeComponent();
        }
    }

    public class ConverterY : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return -(double)value / 100D;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
