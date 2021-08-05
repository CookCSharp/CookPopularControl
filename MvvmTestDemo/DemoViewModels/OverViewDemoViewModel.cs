using CookPopularControl.Tools.Extensions.Values;
using MvvmTestDemo.Commumal;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：OverViewDemoViewModel
 * Author： Chance_写代码的厨子
 * Create Time：2021-07-30 17:02:09
 */
namespace MvvmTestDemo.DemoViewModels
{
    public class OverViewDemoViewModel : ViewModelBase
    {
        public string ButtonContent { get; set; } = "开始";
        public bool IsButtonEnabled { get; set; }

        private double _value;
        public double Value
        {
            get => _value;
            set
            {
                SetProperty(ref _value, value, () => 
                {
                    if (_value.BetweenMinMax(0, 100))
                        IsButtonEnabled = false;
                    else
                        IsButtonEnabled = true;
                });
            }
        }

        public string TextContent { get; set; }
        public string NumericValue { get; set; }
        public string PasswordContent { get; set; }

        public ObservableCollection<object> Lists { get; set; } = new ObservableCollection<object>() { "111", "222", "333" };

        public ObservableCollection<TreeViewDemoClass> TreeViewTests { get; set; }


        public ICommand ProgressButtonCommand { get; set; }


        public OverViewDemoViewModel()
        {
            InitCommand();
        }

        private void InitCommand()
        {
            ProgressButtonCommand = new DelegateCommand(OnProgressButton);

            TreeViewTests = new ObservableCollection<TreeViewDemoClass>();
            for (int i = 1; i < 6; i++)
            {
                TreeViewTests.Add(new TreeViewDemoClass
                {
                    Header = $"Chance{i}",
                    Level = 1,
                });
            }


            for (int i = 1; i < 6; i++)
            {
                TreeViewTests[i - 1].Children = new ObservableCollection<TreeViewDemoClass>();
                for (int j = 1; j < 6; j++)
                {
                    TreeViewTests[i - 1].Children.Add(new TreeViewDemoClass
                    {
                        Header = $"Chance{j}",
                        Level = 2,
                    });
                }
            }

            for (int i = 1; i < 6; i++)
            {
                for (int j = 1; j < 6; j++)
                {
                    TreeViewTests[i - 1].Children[j - 1].Children = new ObservableCollection<TreeViewDemoClass>();
                    for (int h = 1; h < 6; h++)
                    {
                        TreeViewTests[i - 1].Children[j - 1].Children.Add(new TreeViewDemoClass
                        {
                            Header = $"Chance{h}",
                            Level = 3,
                        });
                    }
                }
            }
        }


        private async void OnProgressButton()
        {
            await Task.Run(() =>
            {
                for (int i = 1; i <= 100; i++)
                {
                    Value = i;
                    System.Threading.Thread.Sleep(10);
                }

                ButtonContent = "成功";
                //System.Threading.Thread.Sleep(500);
                //Value = 0;
                //ButtonContent = "开始!";
            });
        }

        public DelegateCommand<RoutedPropertyChangedEventArgs<object>> TreeViewSelectedItemCommand => new DelegateCommand<RoutedPropertyChangedEventArgs<object>>(OnTreeViewSelectedItemChanged);
        public DelegateCommand TreeViewDoubleClickCommand => new DelegateCommand(OnTreeViewDoubleClick);
        public DelegateCommand TreeViewItemExpandedCommand => new DelegateCommand(OnTreeViewItemExpanded);
        public DelegateCommand<RoutedEventArgs> TreeViewItemSelectedItemCommand => new DelegateCommand<RoutedEventArgs>(OnTreeViewItemSelectedItemChanged);


        private void OnTreeViewSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = e.Source as TreeView;
            var item = treeView.SelectedItem;
            var treeViewItem = treeView.ItemContainerGenerator.ContainerFromItem(treeView.Items.CurrentItem) as TreeViewItem;

            System.Diagnostics.Debug.WriteLine("111");
        }

        private void OnTreeViewDoubleClick()
        {
            System.Diagnostics.Debug.WriteLine("222");
        }

        private void OnTreeViewItemExpanded()
        {
       
        }

        private void OnTreeViewItemSelectedItemChanged(RoutedEventArgs e)
        {

        }
    }

    public class TreeViewDemoClass
    {
        public string Header { get; set; }

        public int Level { get; set; }

        public ObservableCollection<TreeViewDemoClass> Children { get; set; }
    }
}
