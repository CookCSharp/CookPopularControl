using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：HorizontalOrganiser
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-11 17:19:10
 */
namespace CookPopularControl.Controls.Dragables
{
    public class HorizontalOrganiser : StackOrganiser
    {
        public HorizontalOrganiser() : base(Orientation.Horizontal)
        { }

        public HorizontalOrganiser(double itemOffset) : base(Orientation.Horizontal, itemOffset)
        { }
    }
}
