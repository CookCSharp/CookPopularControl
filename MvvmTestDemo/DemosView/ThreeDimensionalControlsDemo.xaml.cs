using CookPopularControl.Communal.Data.Enum;
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
using System.Windows.Threading;

namespace MvvmTestDemo.DemosView
{
    /// <summary>
    /// _3DControlsDemo.xaml 的交互逻辑
    /// </summary>
    public partial class ThreeDimensionalControlsDemo : UserControl
    {
        private int stateIndex = 0;

        public ThreeDimensionalControlsDemo()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            OnAlertor3DAutoChanged(3, typeof(AlertorState).GetEnumValues());
        }

        private void OnAlertor3DAutoChanged(int sleep, Array array)
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3),
                IsEnabled = true,
            };
            timer.Tick += (s, e) =>
            {               
                alertor3D.State = (AlertorState)array.GetValue(stateIndex);
                stateIndex++;
                if (stateIndex >= array.Length)
                    stateIndex = 0;
            };
            timer.Start();
        }
    }
}
