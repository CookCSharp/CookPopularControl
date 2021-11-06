using CookPopularCSharpToolkit.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;


namespace CookPopularControl.Controls.Windows.Printers
{
    /// <summary>
    /// PrintWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PrintPreviewWindow : Window, INotifyPropertyChanged
    {
        private int _currentPage;
        private int _totalPages;
        private bool _isLoadingPage;

        public int CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }

        public int TotalPages
        {
            get => _totalPages;
            set => SetProperty(ref _totalPages, value);
        }

        public bool IsLoadingPage
        {
            get => _isLoadingPage;
            set => SetProperty(ref _isLoadingPage, value);
        }

        private readonly Uri _xpsUri;



        public static readonly ICommand PrintCommand = new RoutedCommand(nameof(PrintCommand), typeof(PrintPreviewWindow));
        public static readonly ICommand CancelCommand = new RoutedCommand(nameof(CancelCommand), typeof(PrintPreviewWindow));
        public static readonly ICommand GoToFirstPageCommand = new RoutedCommand(nameof(GoToFirstPageCommand), typeof(PrintPreviewWindow));
        public static readonly ICommand PreviousPageCommand = new RoutedCommand(nameof(PreviousPageCommand), typeof(PrintPreviewWindow));
        public static readonly ICommand NextPageCommand = new RoutedCommand(nameof(NextPageCommand), typeof(PrintPreviewWindow));
        public static readonly ICommand GoToLastPageCommand = new RoutedCommand(nameof(GoToLastPageCommand), typeof(PrintPreviewWindow));

        public PrintPreviewWindow()
        {
            InitializeComponent();

            _xpsUri = new Uri($"memorystream://{Guid.NewGuid()}.xps");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitPageInfo();
            AddCommands();
            //LoadPreviewXPS(@"C:\Users\Chance\Desktop\761033.pdf");
            //LoadPreviewXPS(@"C:\Users\Chance\Desktop\新建文本文档 (2).txt");

            LoadFile();
        }

        private void LoadFile()
        {
            //OpenFileDialog dialog = new OpenFileDialog();
            //dialog.Filter = "XPS 文档(*.xps)|*.xps";
            //if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    XpsDocument doc = new XpsDocument(dialog.FileName, System.IO.FileAccess.Read);
            //    documentPreviewer.Document = doc.GetFixedDocumentSequence();
            //}
            //XpsDocument doc = new XpsDocument("‪C:\\Users\\Chance\\Desktop\\1.xps", FileAccess.Read);
            //documentPreviewer.Document = doc.GetFixedDocumentSequence();
        }

        private void AddCommands()
        {
            CommandManager.RegisterClassCommandBinding(typeof(FrameworkElement), new CommandBinding(ApplicationCommands.Print, null, (s, e) => e.CanExecute = true));
            CommandManager.RegisterClassCommandBinding(typeof(FrameworkElement), new CommandBinding(ApplicationCommands.Copy, null, (s, e) => e.CanExecute = true));
            CommandManager.RegisterClassCommandBinding(typeof(PrintPreviewWindow), new CommandBinding(PrintCommand, (s, e) => OnPrint()));
            CommandManager.RegisterClassCommandBinding(typeof(PrintPreviewWindow), new CommandBinding(CancelCommand, (s, e) => Close()));
            CommandManager.RegisterClassCommandBinding(typeof(PrintPreviewWindow), new CommandBinding(GoToFirstPageCommand, (s, e) => documentPreviewer.FirstPage()));
            CommandManager.RegisterClassCommandBinding(typeof(PrintPreviewWindow), new CommandBinding(PreviousPageCommand, (s, e) => documentPreviewer.PreviousPage()));
            CommandManager.RegisterClassCommandBinding(typeof(PrintPreviewWindow), new CommandBinding(NextPageCommand, (s, e) => documentPreviewer.NextPage()));
            CommandManager.RegisterClassCommandBinding(typeof(PrintPreviewWindow), new CommandBinding(GoToLastPageCommand, (s, e) => documentPreviewer.LastPage()));
        }

        private void OnPrint()
        {
            DispatcherHelper.DoEvents();
            PrintDocument();
        }

        private void PrintDocument()
        {
            //documentPreviewer.Print();
        }

        private void InitPageInfo()
        {
            CurrentPage = GetCurrentPage();
            TotalPages = documentPreviewer.PageCount;
        }

        private int GetCurrentPage()
        {
            int page;
            var scrollViewer = (ScrollViewer)documentPreviewer.Template.FindName("PART_ContentHost", documentPreviewer);
            if (documentPreviewer.MaxPagesAcross == 1)
                page = (int)(scrollViewer.VerticalOffset / (scrollViewer.ExtentHeight / documentPreviewer.PageCount)) + 1;
            else
                page = ((int)(scrollViewer.VerticalOffset / (scrollViewer.ExtentHeight / Math.Ceiling((double)documentPreviewer.PageCount / 2))) + 1) * 2 - 1;

            return page < 1 ? 1 : (page > documentPreviewer.PageCount ? documentPreviewer.PageCount : page);
        }




        /// <summary>
        /// 制作XPS文档和加载文档预览
        /// </summary>
        private void LoadPreviewXPS(string filePath, IDocumentPaginatorSource document = null)
        {
            //if (document == null)
            //    document = new FixedDocument();

            //PackageStore.RemovePackage(_xpsUri);
            //var buffer = filePath.ReadFileInSafe();
            //using var stream = new MemoryStream(buffer);
            //using var package = Package.Open(stream, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            //PackageStore.AddPackage(_xpsUri, package);
            //using var xpsDoc = new XpsDocument(package, CompressionOption.Fast, _xpsUri.AbsoluteUri) { Uri = _xpsUri };
            //XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDoc);
            //writer.Write(document.DocumentPaginator);

            //documentPreviewer.Document = xpsDoc.GetFixedDocumentSequence();

            //xpsDoc.Close();
        }












        private void ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void DocumentPreviewer_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void SetProperty<T>(ref T item, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(item, value))
            {
                item = value;
                OnPropertyChanged(propertyName);
            }
        }
    }
}
