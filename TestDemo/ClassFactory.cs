using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TestDemo.Demos;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：ClassFactory
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-26 15:28:17
 */
namespace TestDemo
{
    public class ClassFactory
    {
        public static ClassFactory Intance => new ClassFactory();
        private static HashSet<UserControl> AllControlsDemo = new();

        static ClassFactory()
        {
            AllControlsDemo.Add(new AnimationPathDemo());
            AllControlsDemo.Add(new ButtonDemo());
            AllControlsDemo.Add(new CarouselViewDemo());
            AllControlsDemo.Add(new CheckBoxDemo());
            AllControlsDemo.Add(new ComboBoxDemo());
            AllControlsDemo.Add(new DataGridDemo());
            AllControlsDemo.Add(new DateDemo());
            AllControlsDemo.Add(new DialogBoxDemo());
            AllControlsDemo.Add(new ExpanderDemo());
            AllControlsDemo.Add(new FieldsDemo());
            AllControlsDemo.Add(new GridDemo());
            AllControlsDemo.Add(new GroupBoxDemo());
            AllControlsDemo.Add(new ListsDemo());
            AllControlsDemo.Add(new LoadingDemo());
            AllControlsDemo.Add(new MediaPlayerDemo());
            AllControlsDemo.Add(new MenuDemo());
            AllControlsDemo.Add(new MessageDialogDemo());
            AllControlsDemo.Add(new NotifyIconDemo());
            AllControlsDemo.Add(new PasswordBoxDemo());
            AllControlsDemo.Add(new PopupDemo());
            AllControlsDemo.Add(new ProgressBarDemo());
            AllControlsDemo.Add(new QRCodeControlDemo());
            AllControlsDemo.Add(new RadioButtonDemo());
            AllControlsDemo.Add(new ScrollViewerDemo());
            AllControlsDemo.Add(new SliderDemo());
            AllControlsDemo.Add(new SwiperDemo());
            AllControlsDemo.Add(new TabControlDemo());
            AllControlsDemo.Add(new ToggleButtonDemo());
        }

        public static UserControl GetSpecificClass(int index)
        {
            return AllControlsDemo.ElementAt(index);
        }
    }
}
