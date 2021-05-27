using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PropertyChanged;

namespace MvvmTestDemo.DemosView
{
    /// <summary>
    /// DateDemo.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class DateDemo : UserControl
    {
        public DateTime FutureValidatingDate { get; set; } = DateTime.Now.Date;

        public DateDemo()
        {
            InitializeComponent();

            this.DataContext = this;
            FutureDatePicker.BlackoutDates.AddDatesInPast();
        }
    }
}
