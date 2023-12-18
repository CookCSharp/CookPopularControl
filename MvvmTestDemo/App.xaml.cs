using CookPopularCSharpToolkit.Communal;
using CookPopularCSharpToolkit.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace MvvmTestDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : ApplicationBase
    {
        private readonly HookFactory hookFactory = new HookFactory();
        private KeyboardWatcher keyboardWatcher;
        private MouseWatcher mouseWatcher;
        private ClipboardWatcher clipboardWatcher;

        public static readonly ReadOnlyCollection<Color> DemoColors =
            "#E2602D,#1E94C0,#B7596B,#FF9C00,#93C6B9,#70634D,#FDCE4E,#759C00"
            .Split(',')
            .Select(cs => (Color)ColorConverter.ConvertFromString(cs))
            .ToReadOnlyCollection();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            //SplashScreen ss = new SplashScreen("Resources/Gif/cook.gif");
            //ss.Show(true, true);

            var s = SystemColors.ControlColor;
            //DynamicGeneratorDll.ILCreateSumAndSaveAsDll();
            //DynamicGeneratorDll.ILCreateHexToColorAndSaveAsDll();

            //TestHook();

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        private void TestHook()
        {
            keyboardWatcher = hookFactory.GetKeyboardWatcher();
            keyboardWatcher.Start();
            keyboardWatcher.OnKeyInput += (s, e) =>
            {
                //Debug.WriteLine("Key {0} event of key {1}", (KeyEvent)e.EventType, e.Key);
            };

            mouseWatcher = hookFactory.GetMouseWatcher();
            mouseWatcher.Start();
            mouseWatcher.OnMouseInput += (s, e) =>
            {
                //Debug.WriteLine("Mouse event {0} at point {1},{2}", e.MessageType.ToString(), e.Point.X, e.Point.Y);
            };

            clipboardWatcher = hookFactory.GetClipboardWatcher();
            clipboardWatcher.Start();
            clipboardWatcher.OnClipboardContentChanged += (s, e) =>
            {
                Debug.WriteLine("Clipboard updated with data '{0}' of format {1}", e.Data, e.DataFormat.ToString());
            };
        }
        
        protected override void OnExit(ExitEventArgs e)
        {
            keyboardWatcher.Stop();
            mouseWatcher.Stop();
            clipboardWatcher.Stop();

            hookFactory.Dispose();

            base.OnExit(e);
        }
    }

    public enum KeyEvent
    {
        Down = 0,
        Up = 1
    }
}
