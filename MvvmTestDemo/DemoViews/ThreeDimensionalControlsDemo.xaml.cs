using CookPopularControl.Communal.Data.Enum;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MvvmTestDemo.DemoViews
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
