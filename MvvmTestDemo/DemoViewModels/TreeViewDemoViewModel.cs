using MvvmTestDemo.DemoModels;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：TreeViewDemoViewModel
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-10 09:26:12
 */
namespace MvvmTestDemo.DemoViewModels
{
    public class TreeViewDemoViewModel : BindableBase
    {
        public ObservableCollection<TreeViewModelStandard> TreeViewDemoItems { get; set; }

        public DelegateCommand<RoutedPropertyChangedEventArgs<object>> TreeViewSelectedItemCommand => new DelegateCommand<RoutedPropertyChangedEventArgs<object>>(OnTreeViewSelectedItemChanged);
        public DelegateCommand TreeViewDoubleClickCommand => new DelegateCommand(OnTreeViewDoubleClick);
        public DelegateCommand TreeViewItemExpandedCommand => new DelegateCommand(OnTreeViewItemExpanded);
        public DelegateCommand<RoutedEventArgs> TreeViewItemSelectedCommand => new DelegateCommand<RoutedEventArgs>(OnTreeViewItemSelectedChanged);

        public TreeViewDemoViewModel()
        {
            InitData();
        }

        private void InitData()
        {
            var models = new List<TreeViewModelStandard>();
            for (int i = 0; i < 5; i++)
            {
                models.Add(new TreeViewModelStandard { Header = $"Chance{i + 1}", Level = 1, ParentLevel = 0, IsParent = true });
                models.Add(new TreeViewModelStandard { Header = $"Chance{i + 1}", Level = 2, ParentLevel = 1, IsParent = false });
                models.Add(new TreeViewModelStandard { Header = $"Chance{i + 1}", Level = 3, ParentLevel = 2, IsParent = false });
            }

            TreeViewDemoItems = new ObservableCollection<TreeViewModelStandard>();
            foreach (var model in models)
            {
                model.Children = new ObservableCollection<TreeViewModelStandard>();
                model.Children.AddRange(FindChild(model));

                if (model.ParentLevel == 0) //判断是否为根节点
                {
                    TreeViewDemoItems.Add(model); 
                }
            }

            IEnumerable<TreeViewModelStandard> FindChild(TreeViewModelStandard node)
            {
                foreach (var model in models)
                {
                    if(model.ParentLevel == node.Level)
                        yield return model;
                }

                yield break;
            }
        }

        private void InitData1()
        {
            //TreeViewDemoItems = new ObservableCollection<TreeViewModel>();
            //for (int i = 1; i < 6; i++)
            //{
            //    TreeViewDemoItems.Add(new TreeViewModel
            //    {
            //        Header = $"Chance{i}",
            //        HeaderIndex = i - 1,
            //        Level = 1,
            //        Root = null,
            //    }) ;
            //}


            //for (int i = 1; i < 6; i++)
            //{
            //    TreeViewDemoItems[i - 1].Children = new ObservableCollection<TreeViewModel>();
            //    for (int j = 1; j < 6; j++)
            //    {
            //        TreeViewDemoItems[i - 1].Children.Add(new TreeViewModel
            //        {
            //            Header = $"Chance{j}",
            //            HeaderIndex = j - 1,
            //            Level = 2,
            //            Root = TreeViewDemoItems[i - 1],
            //        });
            //    }
            //}

            //for (int i = 1; i < 6; i++)
            //{
            //    for (int j = 1; j < 6; j++)
            //    {
            //        TreeViewDemoItems[i - 1].Children[j - 1].Children = new ObservableCollection<TreeViewModel>();
            //        for (int h = 1; h < 6; h++)
            //        {
            //            TreeViewDemoItems[i - 1].Children[j - 1].Children.Add(new TreeViewModel
            //            {
            //                Header = $"Chance{h}",
            //                HeaderIndex = h - 1,
            //                Level = 3,
            //                Root = TreeViewDemoItems[i - 1].Children[j - 1],
            //            });
            //        }
            //    }
            //}
        }

        private void OnTreeViewSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = e.Source as TreeView;
            var item = treeView.SelectedItem;
            var treeViewItem = treeView.ItemContainerGenerator.ContainerFromItem(treeView.Items.CurrentItem) as TreeViewItem;

            System.Diagnostics.Debug.WriteLine("SelectedItem");
        }

        private void OnTreeViewDoubleClick()
        {
            System.Diagnostics.Debug.WriteLine("DoubleClick");
        }

        private void OnTreeViewItemExpanded()
        {
            System.Diagnostics.Debug.WriteLine("Expanded");
        }

        private void OnTreeViewItemSelectedChanged(RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ItemSelected");
        }
    }
}
