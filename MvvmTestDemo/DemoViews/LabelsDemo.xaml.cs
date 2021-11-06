using CookPopularControl.Controls;
using CookPopularCSharpToolkit.Windows;
using PropertyChanged;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// LabelDemo.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class LabelsDemo : UserControl
    {
        public List<GroupLableItem> GroupLists { get; set; }
        public List<GroupLableItem> GroupHeaderLists { get; set; }
        public List<GroupLableItem> GroupImageLists { get; set; }
        public List<GroupLableItem> GroupIconLists { get; set; }

        private List<string> Letters = new List<string> { "A", "B", "C", "D", "E", "F", "G" };

        public LabelsDemo()
        {
            InitializeComponent();
            this.DataContext = this;

            GroupLists = new List<GroupLableItem>();
            for (int i = 0; i < 7; i++)
            {
                GroupLists.Add(new GroupLableItem { Content = $"写代码的厨子{i + 1}" });
            }

            GroupHeaderLists = new List<GroupLableItem>();
            for (int i = 0; i < 7; i++)
            {
                GroupHeaderLists.Add(new GroupLableItem { Header = Letters[i], Content = $"写代码的厨子{i + 1}" });
            }

            GroupImageLists = new List<GroupLableItem>();
            for (int i = 0; i < 7; i++)
            {
                GroupImageLists.Add(new GroupLableItem { Header = "pack://application:,,,/MvvmTestDemo;component/Resources/Gif/tomcat.jpg", Content = $"写代码的厨子{i + 1}" });
            }

            GroupIconLists = new List<GroupLableItem>();
            for (int i = 0; i < 7; i++)
            {
                GroupIconLists.Add(new GroupLableItem { Header = ResourceHelper.GetResource<Geometry>("StarGeometry"), Content = $"写代码的厨子{i + 1}" });
            }
        }

        private void GroupLabel_ItemClosed(object sender, object e)
        {

        }
    }
}
