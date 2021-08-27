using CookPopularControl.Communal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DropZone
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 17:10:53
 */
namespace CookPopularControl.Controls.Dragables
{
    public class DropZone : System.Windows.Controls.Control
    {
        static DropZone()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropZone), new FrameworkPropertyMetadata(typeof(DropZone)));
        }

        public DropZoneLocation Location
        {
            get { return (DropZoneLocation)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }
        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register("Location", typeof(DropZoneLocation), typeof(DropZone), new PropertyMetadata(default(DropZoneLocation)));

        private static readonly DependencyPropertyKey IsOfferedPropertyKey =
            DependencyProperty.RegisterReadOnly("IsOffered", typeof(bool), typeof(DropZone), new PropertyMetadata(default(bool)));
        public bool IsOffered
        {
            get { return (bool)GetValue(IsOfferedProperty); }
            internal set { SetValue(IsOfferedPropertyKey, value); }
        }
        public static readonly DependencyProperty IsOfferedProperty = IsOfferedPropertyKey.DependencyProperty;
    }
}
