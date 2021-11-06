using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DispatcherVisual
 * Author： Chance_写代码的厨子
 * Create Time：2021-10-18 13:14:14
 */
namespace CookPopularCSharpToolkit.Windows
{
    /// <summary>
    /// 在另一个线程绘制元素，在跟主界面一样的线程渲染
    /// </summary>
    /// <remarks>
    /// 使用VisualTarget做到多个UI线程的绘制，
    /// WPF的渲染线程只有一个，多个UI线程无法让渲染的速度加快，
    /// 也就是说如果一个界面有很多的Visual那么渲染速度也不会因为添加UI线程用的时间比原来少
    /// </remarks>
    public class DispatcherVisual : UIElement
    {
        public DispatcherVisual()
        {
            var thread = new Thread(() =>
            {
                _visualTarget = new VisualTarget(_hostVisual);
                DrawingVisual drawingVisual = new DrawingVisual();
                var drawing = drawingVisual.RenderOpen();
                using (drawing)
                {
                    var text = new FormattedText("欢迎访问我的开源项目 https://gitee.com/ChanceZXY ",
                        CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                        new Typeface(new FontFamily("微软雅黑"), new FontStyle(), FontWeight.FromOpenTypeWeight(1),
                            FontStretch.FromOpenTypeStretch(1)), 20, Brushes.DarkSlateBlue);

                    drawing.DrawText(text, new Point(100, 100));
                }

                var containerVisual = new ContainerVisual();

                containerVisual.Children.Add(drawingVisual);

                _visualTarget.RootVisual = containerVisual;

                System.Windows.Threading.Dispatcher.Run();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        protected override Visual GetVisualChild(int index)
        {
            return _hostVisual;
        }

        protected override int VisualChildrenCount => 1;

        private readonly HostVisual _hostVisual = new HostVisual();
        private VisualTarget _visualTarget;
    }
}
