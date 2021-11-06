using CookPopularCSharpToolkit.Communal;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ZXing;
using ZXing.QrCode;

namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// QRCodeControlDemo.xaml 的交互逻辑
    /// </summary>
    public partial class QRCodeControlDemo : UserControl
    {
        public QRCodeControlDemo()
        {
            InitializeComponent();

            CreateBarCode();
            ReadBarCode();
        }

        private void QRCodeControl_IsRefreshChanged(object sender, RoutedEventArgs e)
        {
            //(sender as QRCodeControl).IsShowRefreshIcon = false;            
        }

        private void CreateBarCode()
        {
            // 1.设置条形码规格

            QrCodeEncodingOptions encodeOption = new QrCodeEncodingOptions();
            encodeOption.CharacterSet = "UTF-8";
            encodeOption.DisableECI = true;// Extended Channel Interpretation (ECI) 主要用于特殊的字符集。并不是所有的扫描器都支持这种编码。
            /***
             * L - 约 7% 纠错能力。
             * M - 约 15% 纠错能力。
             * Q - 约 25% 纠错能力。
             * H - 约 30% 纠错能力。 
             */
            encodeOption.ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.H; // 纠错级别
            encodeOption.Height = 120; // 必须制定高度、宽度
            encodeOption.Width = 240;

            // 2.生成条形码图片并保存
            BarcodeWriter writer = new BarcodeWriter();
            writer.Options = encodeOption;
            writer.Format = BarcodeFormat.CODE_93;  // 这里可以设定条码的标准
            using (Bitmap bitmap = writer.Write("Chance123")) // 生成图片
            {
                MemoryStream ms = new MemoryStream();
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                }
                // 3.读取保存的图片
                string filePath = AppDomain.CurrentDomain.BaseDirectory + "BarCode.png";
                bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                BarCodeImage.Source = ImageBitmapExtension.ToImageSource(bitmap);
            }
        }

        private void ReadBarCode()
        {
            // create a barcode reader instance
            IBarcodeReader reader = new BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            // load a bitmap
            using (var barcodeBitmap = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "BarCode.png"))
            {
#if NET461
                // detect and decode the barcode inside the bitmap
                var result = reader.Decode(barcodeBitmap);
                // do something with the result
                if (result != null)
                {
                    CodeTypeText.Text = result.BarcodeFormat.ToString();
                    CodeContentText.Text = result.Text;
                }
#endif
            }
        }
    }
}
