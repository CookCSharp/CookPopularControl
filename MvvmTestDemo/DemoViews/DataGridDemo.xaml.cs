using DocumentFormat.OpenXml.Spreadsheet;
using PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// DataGridDemo.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class DataGridDemo : UserControl
    {
        public ObservableCollection<Person> Workers { get; set; }
        public static IEnumerable<string> Movies = new[] { "僵尸叔叔", "警察故事", "速度与激情", "侏罗纪公园" };
        public ObservableCollection<dynamic> TestDatas { get; set; }

        public DataGridDemo()
        {
            InitializeComponent();

            this.Loaded += DataGridDemo_Loaded;
        }

        private void DataGridDemo_Loaded(object sender, RoutedEventArgs e)
        {
            Workers = new ObservableCollection<Person>();
            Workers.Add(new Person { Name = "张三", Gender = "男", Age = 18, IsWorkingOnIT = true, FavoriteMovie = "僵尸叔叔", Job = "软件开发", Department = "肿瘤事业部" });
            Workers.Add(new Person { Name = "李四", Gender = "女", Age = 32, IsWorkingOnIT = false, FavoriteMovie = "警察故事", Job = "技术支持", Department = "心脏事业部" });
            Workers.Add(new Person { Name = "王五", Gender = "未知", Age = 22, IsWorkingOnIT = false, FavoriteMovie = "僵尸叔叔", Job = "实施", Department = "心电事业部" });
            Workers.Add(new Person { Name = "赵六", Gender = "男", Age = 38, IsWorkingOnIT = true, FavoriteMovie = "侏罗纪公园", Job = "数据挖掘", Department = "卒中事业部" });
            Workers.Add(new Person { Name = "朱七", Gender = "男", Age = 15, IsWorkingOnIT = true, FavoriteMovie = "速度与激情", Job = "测试", Department = "急救事业部" });
            //Workers.Add(new Person { Name = "牛八", Gender = "女", Age = 68, IsWorkingOnIT = false, FavoriteMovie = "侏罗纪公园", Job = "UI设计", Department = "试剂事业一部" });
            //Workers.Add(new Person { Name = "周九", Gender = "男", Age = 45, IsWorkingOnIT = false, FavoriteMovie = "速度与激情", Job = "机械设计", Department = "试剂事业二部" });

            //var dpDescriptor = DependencyPropertyDescriptor.FromProperty(DataGrid.ItemsSourceProperty, typeof(DataGrid));
            //dpDescriptor.AddValueChanged(TestDataGrid, (s, e) =>
            //{
            //    if (TestDataGrid.ItemsSource is ObservableCollection<Person> dataSource && dataSource?.Count > 0)
            //    {
            //        for (int i = 0; i < dataSource.Count; i++)
            //        {
            //            if (dataSource[i] is Person person)
            //            {
            //                //待会实现
            //            }
            //            else
            //            {
            //                this.TestDataGrid.Columns.Add(new DataGridTextColumn() { Header = "时间", Binding = new Binding("[" + i.ToString() + "]") { StringFormat = "yyyy/MM/dd HH:mm:ss" } });
            //            }
            //        }
            //    }
            //});

            TestDatas = new ObservableCollection<dynamic>();
            TestDatas.Add(new Person { Name = "张三", Gender = "男", Age = 18, IsWorkingOnIT = true, FavoriteMovie = "僵尸叔叔", Job = "软件开发", Department = "肿瘤事业部" });

            List<PP> lss = new List<PP>();
            List<KK> kks = new List<KK>();
            for (int i = 0; i < 5; i++)
            {
                kks.Add(new KK { Key = i.ToString(), Value = i.ToString() });
            }
            lss.Add(new PP { Name = "First", Gender = "Man", KKs = new ObservableCollection<KK>(kks) });

            TestDataGrid.Columns.Add(new DataGridTextColumn() { Header = "姓名", Binding = new Binding("Name") });
            TestDataGrid.Columns.Add(new DataGridTextColumn() { Header = "性别", Binding = new Binding("Gender") });
            for (int i = 0; i < 5; i++)
            {
                TestDataGrid.Columns.Add(new DataGridTextColumn() { Header = $"Column{kks[i].Key}", Binding = new Binding("Value") });
            }

            TestDataGrid.ItemsSource = lss;
            //ObservableCollection<dynamic> dd = new ObservableCollection<dynamic>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Workers.Add(new Person { Name = "郑十", Gender = "女", Age = 60, IsWorkingOnIT = false, FavoriteMovie = "僵尸叔叔", Job = "机械设计", Department = "试剂事业三部" });
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public bool IsWorkingOnIT { get; set; }
        public string FavoriteMovie { get; set; }
        public string Job { get; set; }
        public string Department { get; set; }
        public OrderStatus Status { get; set; }
    }

    public enum OrderStatus { None, New, Processing, Shipped, Received };

    public class PP
    {
        public string Name { get; set; }
        public string Gender { get; set; }

        public ObservableCollection<KK> KKs { get; set; } = new ObservableCollection<KK>();
    }

    public class KK
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
