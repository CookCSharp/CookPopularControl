using CookPopularCSharpToolkit.Communal;
using Prism.Commands;
using System;
using System.Collections.Generic;
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

namespace MvvmTestDemo.UserControls
{
    /// <summary>
    /// HomePageView.xaml 的交互逻辑
    /// </summary>
    public partial class HomePageView : UserControl
    {
        public HomePageView()
        {
            InitializeComponent();
        }

        public ICommand SourceCodeHyperlinkCommand => new DelegateCommand<Uri>(uri => GotoSourceCodeHyperlink(uri));

        private void GotoSourceCodeHyperlink(Uri uri)
        {
            uri.AbsoluteUri.ToOpenLink();
        }
    }
}
