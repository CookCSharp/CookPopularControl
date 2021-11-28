using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;



/*
 * Description：AsyncObservableCollection<T> 
 * Author： Chance(a cook of write code)
 * Company: NCATest
 * Create Time：2021-11-25 11:11:23
 * .NET Version: 4.6
 * CLR Version: 4.0.30319.42000
 * Copyright (c) NCATest 2021 All Rights Reserved.
 */
namespace CookPopularCSharpToolkit.Communal
{
    /// <summary>
    /// 线程安全的集合，将ObservableCollection替换掉即可
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        //获取当前线程的SynchronizationContext对象
        private SynchronizationContext _synchronizationContext = SynchronizationContext.Current;
        public AsyncObservableCollection() { }
        public AsyncObservableCollection(IEnumerable<T> list) : base(list) { }
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                //如果操作发生在同一个线程中，不需要进行跨线程执行         
                RaiseCollectionChanged(e);
            }
            else
            {
                //如果不是发生在同一个线程中
                //准确说来，这里是在一个非UI线程中，需要进行UI的更新所进行的操作         
                _synchronizationContext.Post(RaiseCollectionChanged, e);
            }
        }
        private void RaiseCollectionChanged(object param)
        {
            // 执行         
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                // Execute the PropertyChanged event on the current thread             
                RaisePropertyChanged(e);
            }
            else
            {
                // Post the PropertyChanged event on the creator thread             
                _synchronizationContext.Post(RaisePropertyChanged, e);
            }
        }
        private void RaisePropertyChanged(object param)
        {
            // We are in the creator thread, call the base implementation directly         
            base.OnPropertyChanged((PropertyChangedEventArgs)param);
        }
    }

    /// <summary>
    /// 线程安全的集合，将ObservableCollection替换掉即可
    /// </summary>
    public class SaveThreadObservableCollection<T> : ObservableCollection<T>
    {
        public override event NotifyCollectionChangedEventHandler CollectionChanged;
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler CollectionChanged = this.CollectionChanged;
            if (CollectionChanged != null)
            {
                foreach (NotifyCollectionChangedEventHandler nh in CollectionChanged.GetInvocationList())
                {
                    DispatcherObject dispObj = nh.Target as DispatcherObject;
                    if (dispObj != null)
                    {
                        Dispatcher dispatcher = dispObj.Dispatcher;
                        if (dispatcher != null && !dispatcher.CheckAccess())
                        {
                            dispatcher.BeginInvoke((Action)(() => nh.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))), DispatcherPriority.DataBind);
                            continue;
                        }
                    }
                    nh.Invoke(this, e);
                }
            }
        }
    }
}
