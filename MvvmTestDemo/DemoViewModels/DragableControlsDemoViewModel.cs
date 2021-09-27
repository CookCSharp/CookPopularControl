using CookPopularControl.Communal.Data;
using CookPopularControl.Controls.Dragables;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：DragableControlsViewModel
 * Author： Chance_写代码的厨子
 * Create Time：2021-09-03 17:06:01
 */
namespace MvvmTestDemo.DemoViewModels
{
    public class DragableControlsDemoViewModel : BindableBase
    {
        public IEnumerable<int> Data { get; }

        public ICommand AddCommand { get; }

        public DragableControlsDemoViewModel()
        {
            Data = new ObservableCollection<int>
            {
                1,
                2,
                3
            };

            AddCommand = new DelegateCommand<DragableItemsControl>(dic => Add(dic));
        }

        private void Add(DragableItemsControl dragablzItemsControl)
        {
            var newItem = Data.Max() + 1;
            dragablzItemsControl.AddToSource(newItem, AddLocationHint.Last);

            //your source collection will have the new item added (but sort is mnever applied, for that use dragablzItemsControl.PositionMonitor, example also in the XAML).
            Debug.Assert(Data.Contains(newItem));
        }
    }
}
