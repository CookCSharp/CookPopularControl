using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：WrapperElement_
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-10 14:18:23
 */
namespace CookPopularControl.Controls
{
    public class WrapperElement<TElement> : FrameworkElement where TElement : UIElement
    {
        private readonly TElement _element;
        protected TElement WrappedElement => _element;


        protected WrapperElement(TElement element)
        {
            //Contract.Requires<ArgumentNullException>(element != null);

            _element = element;

            AddVisualChild(_element);
        }

        protected override int VisualChildrenCount => 1;


        protected override Visual GetVisualChild(int index)
        {
            Debug.Assert(index == 0);

            return _element;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            _element.Measure(availableSize);
            return _element.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _element.Arrange(new Rect(finalSize));
            return _element.RenderSize;
        }
    }
}
