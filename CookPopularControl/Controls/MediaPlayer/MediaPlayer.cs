using CookPopularControl.Tools.Boxes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：MediaPlayer
 * Author： Chance_写代码的厨子
 * Create Time：2021-04-27 19:03:54
 */
namespace CookPopularControl.Controls
{
    /// <summary>
    /// 媒体播放器
    /// </summary>
    public class MediaPlayer : Control
    {
        static MediaPlayer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MediaPlayer), new FrameworkPropertyMetadata(typeof(MediaPlayer)));
        }


        /// <summary>
        /// 当前媒体资源路径
        /// </summary>
        public Uri CurrentUri
        {
            get { return (Uri)GetValue(CurrentUriProperty); }
            set { SetValue(CurrentUriProperty, value); }
        }
        public static readonly DependencyProperty CurrentUriProperty =
            DependencyProperty.Register("CurrentUri", typeof(Uri), typeof(MediaPlayer),
                new PropertyMetadata(default(Uri), OnCurrentUriChanged));

        private static void OnCurrentUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var media = d as MediaPlayer;
            if (media != null)
            {
                media.CurrentUri = e.NewValue as Uri;
            }
        }


        /// <summary>
        /// 媒体资源集合
        /// </summary>
        public IEnumerable<Uri> ItemSource
        {
            get { return (IEnumerable<Uri>)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }
        /// <summary>
        /// 标识<see cref="ItemSource"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register("ItemSource", typeof(IEnumerable<Uri>), typeof(MediaPlayer), new PropertyMetadata());


        /// <summary>
        /// 是否循环播放
        /// </summary>
        /// <remarks>指Next时资源内循环，单个资源循环播放</remarks>
        public bool IsCyclePlay
        {
            get { return (bool)GetValue(IsCyclePlayProperty); }
            set { SetValue(IsCyclePlayProperty, ValueBoxes.BooleanBox(value)); }
        }
        /// <summary>
        /// 标识<see cref="IsCyclePlay"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty IsCyclePlayProperty =
            DependencyProperty.Register("IsCyclePlay", typeof(bool), typeof(MediaPlayer), new PropertyMetadata(ValueBoxes.FalseBox));


        ///// <summary>
        ///// 媒体播放器当前状态
        ///// </summary>
        //[Description("媒体播放器当前状态")]
        //public MediaState MediaState
        //{
        //    get { return (MediaState)GetValue(MediaStateProperty); }
        //    set { SetValue(MediaStateProperty, value); }
        //}
        ///// <summary>
        ///// 标识<see cref="MediaState"/>的依赖属性
        ///// </summary>
        //public static readonly DependencyProperty MediaStateProperty =
        //    DependencyProperty.Register("MediaState", typeof(MediaState), typeof(MediaPlayer), new PropertyMetadata(MediaState.Manual, OnMediaStateValueChanged));

        //private static void OnMediaStateValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var media = d as MediaPlayer;
        //    if (media != null)
        //    {
        //        media.OnMediaStateChanged((MediaState)e.OldValue, (MediaState)e.NewValue);
        //    }
        //}


        ///// <summary>
        ///// 媒体播放状态更改通知事件
        ///// </summary>
        //[Description("播放状态更改时发生")]
        //public event RoutedEventHandler MediaStateChanged
        //{
        //    add { this.AddHandler(MediaStateChangedEvent, value); }
        //    remove { this.RemoveHandler(MediaStateChangedEvent, value); }
        //}
        //public static readonly RoutedEvent MediaStateChangedEvent =
        //    EventManager.RegisterRoutedEvent("MediaStateChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MediaPlayer));

        //protected virtual void OnMediaStateChanged(MediaState oldValue, MediaState newValue)
        //{
        //    RoutedPropertyChangedEventArgs<MediaState> arg = new RoutedPropertyChangedEventArgs<MediaState>(oldValue, newValue, MediaStateChangedEvent);
        //    this.RaiseEvent(arg);
        //}
    }

    ///// <summary>
    ///// 播放器状态
    ///// </summary>
    //public enum MediaPlayerState
    //{
    //    Last = 0, //上一集
    //    Next = 1, //下一集
    //    Pause = 2, //停止
    //    Play = 3, //播放
    //    Restart = 4, //重放
    //}
}
