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
        //private Dictionary<string, UserControl> AllControlsDemo = new();
        private static HashSet<UserControl> AllControlsDemo = new();

        static ClassFactory()
        {
            //AllControlsDemo.Add(typeof(AdornerDemo).Name, new AdornerDemo());
            //AllControlsDemo.Add(typeof(AnimationDemo).Name, new AnimationDemo());
            //AllControlsDemo.Add(typeof(AnimationPathDemo).Name, new AnimationPathDemo());
            //AllControlsDemo.Add(typeof(ButtonDemo).Name, new ButtonDemo());
            //AllControlsDemo.Add(typeof(CheckBoxDemo).Name, new CheckBoxDemo());
            //AllControlsDemo.Add(typeof(ComboBoxDemo).Name, new ComboBoxDemo());
            //AllControlsDemo.Add(typeof(DataGridDemo).Name, new DataGridDemo());
            //AllControlsDemo.Add(typeof(DateDemo).Name, new DateDemo());
            //AllControlsDemo.Add(typeof(DialogBoxDemo).Name, new DialogBoxDemo());
            //AllControlsDemo.Add(typeof(ExpanderDemo).Name, new ExpanderDemo());
            //AllControlsDemo.Add(typeof(FieldsDemo).Name, new FieldsDemo());
            //AllControlsDemo.Add(typeof(GridDemo).Name, new GridDemo());
            //AllControlsDemo.Add(typeof(GroupBoxDemo).Name, new GroupBoxDemo());
            //AllControlsDemo.Add(typeof(ListsDemo).Name, new ListsDemo());
            //AllControlsDemo.Add(typeof(LoadingDemo).Name, new LoadingDemo());
            //AllControlsDemo.Add(typeof(MenuDemo).Name, new MenuDemo());
            //AllControlsDemo.Add(typeof(MessageDialogDemo).Name, new MessageDialogDemo());
            //AllControlsDemo.Add(typeof(PasswordBoxDemo).Name, new PasswordBoxDemo());
            //AllControlsDemo.Add(typeof(PopupDemo).Name, new PopupDemo());
            //AllControlsDemo.Add(typeof(ProgressBarDemo).Name, new ProgressBarDemo());
            //AllControlsDemo.Add(typeof(QRCodeControlDemo).Name, new QRCodeControlDemo());
            //AllControlsDemo.Add(typeof(RadioButtonDemo).Name, new RadioButtonDemo());
            //AllControlsDemo.Add(typeof(ScrollViewerDemo).Name, new ScrollViewerDemo());
            //AllControlsDemo.Add(typeof(SliderDemo).Name, new SliderDemo());
            //AllControlsDemo.Add(typeof(SwiperDemo).Name, new SwiperDemo());
            //AllControlsDemo.Add(typeof(TabControlDemo).Name, new TabControlDemo());

            AllControlsDemo.Add(new AdornerDemo());
            AllControlsDemo.Add(new AnimationDemo());
            AllControlsDemo.Add(new AnimationPathDemo());
            AllControlsDemo.Add(new ButtonDemo());
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
            AllControlsDemo.Add(new MenuDemo());
            AllControlsDemo.Add(new MessageDialogDemo());
            AllControlsDemo.Add(new PasswordBoxDemo());
            AllControlsDemo.Add(new PopupDemo());
            AllControlsDemo.Add(new ProgressBarDemo());
            AllControlsDemo.Add(new QRCodeControlDemo());
            AllControlsDemo.Add(new RadioButtonDemo());
            AllControlsDemo.Add(new ScrollViewerDemo());
            AllControlsDemo.Add(new SliderDemo());
            AllControlsDemo.Add(new SwiperDemo());
            AllControlsDemo.Add(new TabControlDemo());
        }

        public static UserControl GetSpecificClass(int index)
        {
            return AllControlsDemo.ElementAt(index);
        }
    }
}
