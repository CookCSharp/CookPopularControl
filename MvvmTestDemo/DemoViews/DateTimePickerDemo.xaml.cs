using PropertyChanged;
using System;
using System.Windows.Controls;

namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// DateDemo.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class DateTimePickerDemo : UserControl
    {
        public DateTime FutureValidatingDate { get; set; } = DateTime.Now.Date;

        public DateTimePickerDemo()
        {
            InitializeComponent();

            this.DataContext = this;
            //FutureDatePicker.BlackoutDates.AddDatesInPast();
        }
    }
}
