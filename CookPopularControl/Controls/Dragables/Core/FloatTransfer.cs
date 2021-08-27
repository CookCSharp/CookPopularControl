using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：FloatTransfer
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 17:13:31
 */
namespace CookPopularControl.Controls.Dragables.Core
{
    internal class FloatTransfer
    {
        private readonly double _width;
        private readonly double _height;
        private readonly object _content;

        public FloatTransfer(double width, double height, object content)
        {
            if (content == null) throw new ArgumentNullException("content");

            _width = width;
            _height = height;
            _content = content;
        }

        public static FloatTransfer TakeSnapshot(DragableItem dragableItem, DragableTabControl sourceTabControl)
        {
            if (dragableItem == null) throw new ArgumentNullException("dragablzItem");

            return new FloatTransfer(sourceTabControl.ActualWidth, sourceTabControl.ActualHeight, dragableItem.UnderlyingContent ?? dragableItem.Content ?? dragableItem);
        }

        [Obsolete]
        //TODO width and height transfer obsolete
        public double Width
        {
            get { return _width; }
        }

        [Obsolete]
        //TODO width and height transfer obsolete
        public double Height
        {
            get { return _height; }
        }

        public object Content
        {
            get { return _content; }
        }
    }
}
