using System.Windows;


/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：$Do something$ 
 * Author： Chance_写代码的厨子
 * Create Time：2021-02-19 16:05:08
 */
namespace CookPopularCSharpToolkit.Communal
{
    /// <summary>
    /// 装箱后的值类型（用于提高效率）
    /// </summary>
    public static class ValueBoxes
    {
        public static readonly object VisibleBox = Visibility.Visible;
        public static readonly object HiddenBox = Visibility.Hidden;
        public static readonly object CollapsedBox = Visibility.Collapsed;
        public static object Box(this Visibility visibility)
        {
            object v;
            switch (visibility)
            {
                case Visibility.Visible:
                    v = VisibleBox;
                    break;
                case Visibility.Hidden:
                    v = HiddenBox;
                    break;
                case Visibility.Collapsed:
                    v = CollapsedBox;
                    break;
                default:
                    v = default(Visibility);
                    break;
            }

            return v;
        }

        public static readonly object TrueBox = true;
        public static readonly object FalseBox = false;
        public static readonly object NullBox = null;
        public static object BooleanBox(this bool value) => value ? TrueBox : FalseBox;
        public static object BooleanNullBox(this bool? value)
        {
            if (value.HasValue)
            {
                return value.Value ? TrueBox : FalseBox;
            }
            else
            {
                return NullBox;
            }
        }

        public static readonly object InterMinus1Box = -1;
        public static readonly object Inter0Box = 0;
        public static readonly object Inter5Box = 5;
        public static readonly object Inter10Box = 10;
        public static readonly object Inter15Box = 15;
        public static readonly object Inter30Box = 30;

        public static readonly object Double0Box = 0.0;
        public static readonly object Double1Box = 1.0;
        public static readonly object Double3Box = 3.0;
        public static readonly object Double5Box = 5.0;
        public static readonly object Double10Box = 10.0;
        public static readonly object Double20Box = 20.0;
        public static readonly object Double30Box = 30.0;
        public static readonly object Double50Box = 50.0;
        public static readonly object Double200Box = 200.0;

        public static readonly object CornerRadius0Box = new CornerRadius(0);
        public static readonly object CornerRadius2Box = new CornerRadius(2);
        public static readonly object CornerRadius10Box = new CornerRadius(10);
        public static readonly object MarginRight10Box = new Thickness(0, 0, 10, 0);
    }
}
