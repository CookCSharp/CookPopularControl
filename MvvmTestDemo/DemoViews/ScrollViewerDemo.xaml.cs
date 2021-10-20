using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// ScrollViewerDemo.xaml 的交互逻辑
    /// </summary>
    public partial class ScrollViewerDemo : UserControl
    {
        public ScrollViewerDemo()
        {
            InitializeComponent();

            scrollViewer2.ScrollChanged += ScrollViewer2_ScrollChanged;
        }

        private void ScrollViewer2_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("VerticalOffset" + e.VerticalOffset);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //this.scrollViewer1.LineDown();//向下滚动一行
            //this.scrollViewer1.LineUp();//向上滚动一行
            //this.scrollViewer1.PageDown();//向下滚动一页
            //this.scrollViewer1.PageUp();//向上滚动一页
            //this.scrollViewer1.ScrollToEnd();//向下滚动到底部
            //this.scrollViewer1.ScrollToHome();//向上滚动到顶部
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            scrollViewer2.ScrollToHorizontalOffset(1500);
        }

        private void btn2_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var point = e.GetPosition(scrollViewer2);
            //System.Diagnostics.Debug.WriteLine(point.Y);
            //System.Diagnostics.Debug.WriteLine(canvas.ActualHeight);
            //System.Diagnostics.Debug.WriteLine(Window.GetWindow(this).ActualHeight);

            

            //var sb = scrollViewer2.Template.FindName("PART_VerticalScrollBar", scrollViewer2) as ScrollBar;
            //sb.SmallChange = 1E-19;


            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                //if (point.Y <= 768)
                //    scrollViewer2.ScrollToVerticalOffset(point.Y - 768);
                //else

                ScrollBar.LineDownCommand.Execute(null,scrollViewer2);
                //scrollViewer2.ScrollToHorizontalOffset(point.X - SystemParameters.WorkArea.Width + 20);

                //System.Diagnostics.Debug.WriteLine("ViewportHeight" + scrollViewer2.ViewportHeight);
                //System.Diagnostics.Debug.WriteLine("ScrollableHeight" + scrollViewer2.ScrollableHeight); //1248.8
                //System.Diagnostics.Debug.WriteLine("ExtentHeight" + scrollViewer2.ExtentHeight); //1248.8

                //System.Diagnostics.Debug.WriteLine("VerticalOffset" + scrollViewer2.VerticalOffset);
                //System.Diagnostics.Debug.WriteLine("ContentVerticalOffset" + scrollViewer2.ContentVerticalOffset);

                //Canvas.SetLeft(sender as Button, scr);
                //Canvas.SetTop(btn1, scrollViewer2.VerticalOffset);

                System.Diagnostics.Debug.WriteLine("*************");
            }
        }
    }
}
