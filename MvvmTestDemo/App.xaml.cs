using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace MvvmTestDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly ReadOnlyCollection<Color> DemoColors =
            "#E2602D,#1E94C0,#B7596B,#FF9C00,#93C6B9,#70634D,#FDCE4E,#759C00"
            .Split(',')
            .Select(cs => (Color)ColorConverter.ConvertFromString(cs))
            .ToReadOnlyCollection();


        protected override void OnStartup(StartupEventArgs e)
        {
            //SplashScreen ss = new SplashScreen("Resources/Gif/cook.gif");
            //ss.Show(true, true);

            base.OnStartup(e);

            //DynamicGeneratorDll.ILCreateSumAndSaveAsDll();
            //DynamicGeneratorDll.ILCreateHexToColorAndSaveAsDll();

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}
