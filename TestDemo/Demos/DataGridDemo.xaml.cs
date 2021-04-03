using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PropertyChanged;

namespace TestDemo.Demos
{
    /// <summary>
    /// DataGridDemo.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class DataGridDemo : UserControl
    {
        public ObservableCollection<Person> Workers { get; set; }

        public DataGridDemo()
        {
            InitializeComponent();

            this.Loaded += DataGridDemo_Loaded;
        }

        private void DataGridDemo_Loaded(object sender, RoutedEventArgs e)
        {
            Workers = new ObservableCollection<Person>();
            Workers.Add(new Person { Name = "张三", Gender = "男", Age = 18, Job = "软件开发", Department = "肿瘤事业部" });
            Workers.Add(new Person { Name = "李四", Gender = "女", Age = 32, Job = "技术支持", Department = "心脏事业部" });
            Workers.Add(new Person { Name = "王五", Gender = "未知", Age = 22, Job = "实施", Department = "心电事业部" });
            Workers.Add(new Person { Name = "赵六", Gender = "男", Age = 38, Job = "数据挖掘", Department = "卒中事业部" });
            Workers.Add(new Person { Name = "朱七", Gender = "男", Age = 15, Job = "测试", Department = "急救事业部" });
            Workers.Add(new Person { Name = "牛八", Gender = "女", Age = 68, Job = "UI设计", Department = "试剂事业一部" });
            Workers.Add(new Person { Name = "周九", Gender = "男", Age = 45, Job = "机械设计", Department = "试剂事业二部" });
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Job { get; set; }
        public string Department { get; set; }
    }
}
