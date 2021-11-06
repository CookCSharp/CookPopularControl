using CookPopularCSharpToolkit.Communal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Threading;



/*
 * Copyright (c) 2021 All Rights Reserved.
 * Description：FlipTile3D
 * Author： Chance_写代码的厨子
 * Create Time：2021-08-10 14:22:31
 */
namespace CookPopularControl.Controls.ThreeDimensional
{
    /// <summary>
    /// 3D方形跳动面板
    /// </summary>
    public class FlipTile3D : WrapperElement<Viewport3D>
    {
        //private const int _xCount = 7, _yCount = 7;

        private const string _defaultImageSearchPattern = @"*.jpg | *.png";
        //private readonly ReadOnlyCollection<DiffuseMaterial> Materials = GetSamplePictures();
        private readonly DiffuseMaterial BackMaterial = new DiffuseMaterial();

        private FlipTile[] Tiles;
        private Point _lastMouse = new Point(double.NaN, double.NaN);
        private bool _isFlipped;


        /// <summary>
        /// X方向上的数量
        /// </summary>
        public int XCount
        {
            get { return (int)GetValue(XCountProperty); }
            set { SetValue(XCountProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="XCount"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty XCountProperty =
            DependencyProperty.Register("XCount", typeof(int), typeof(FlipTile3D), new PropertyMetadata(ValueBoxes.Inter10Box, OnValueChanged));


        /// <summary>
        /// X方向上的数量
        /// </summary>
        public int YCount
        {
            get { return (int)GetValue(YCountProperty); }
            set { SetValue(YCountProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="YCount"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty YCountProperty =
            DependencyProperty.Register("YCount", typeof(int), typeof(FlipTile3D), new PropertyMetadata(ValueBoxes.Inter10Box, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FlipTile3D ft3D)
            {
                ft3D.UpdateTileLayout();
            }
        }


        /// <summary>
        /// 块状的最大数量
        /// </summary>
        public int MaxImageCount
        {
            get { return (int)GetValue(MaxImageCountProperty); }
            set { SetValue(MaxImageCountProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="MaxImageCount"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty MaxImageCountProperty =
            DependencyProperty.Register("MaxImageCount", typeof(int), typeof(FlipTile3D), new PropertyMetadata(ValueBoxes.Inter30Box));


        public IEnumerable<DiffuseMaterial> ItemMaterials
        {
            get { return (IEnumerable<DiffuseMaterial>)GetValue(ItemMaterialsProperty); }
            set { SetValue(ItemMaterialsProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="ItemMaterials"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty ItemMaterialsProperty =
            DependencyProperty.Register("ItemMaterials", typeof(IEnumerable<DiffuseMaterial>), typeof(FlipTile3D), new PropertyMetadata(default, (s, e) =>
             {
                 //if (s is FlipTile3D ft3D)
                 //{
                 //    ft3D.UpdateTileLayout();
                 //}
             }));


        private ReadOnlyCollection<DiffuseMaterial> GetItemMaterials()
        {
            IList<string> files = GetPicturePaths().ToList();
            if (files.Count > 0)
            {
                return files.Select(file =>
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(file);
                    bitmapImage.DecodePixelWidth = 320;
                    bitmapImage.DecodePixelHeight = 240;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();

                    ImageBrush imageBrush = new ImageBrush(bitmapImage);
                    imageBrush.Stretch = Stretch.UniformToFill;
                    imageBrush.ViewportUnits = BrushMappingMode.Absolute;
                    imageBrush.Freeze();

                    return new DiffuseMaterial(imageBrush);
                }).ToReadOnlyCollection();
            }
            else
            {
                return (new Brush[] { Brushes.Red, Brushes.Orange, Brushes.Yellow, Brushes.Green, Brushes.Cyan, Brushes.Blue, Brushes.Purple })
                        .Select(brush => new DiffuseMaterial(brush))
                        .ToReadOnlyCollection();
            }
        }

        private IEnumerable<string> GetPicturePaths()
        {
            string[]? commandLineArgs = null;
            try
            {
                commandLineArgs = Environment.GetCommandLineArgs();
            }
            catch (NotSupportedException) { }
            catch (SecurityException) { } // In an xbap

            IEnumerable<string>? picturePaths = null;
            if (commandLineArgs != null)
            {
                picturePaths = commandLineArgs.
                    Select(arg => GetPicturePaths(arg))
                    .Where(paths => paths.Any())
                    .FirstOrDefault();
            }

            if (picturePaths == null)
            {
                picturePaths = DefaultPicturePaths
                    .Select(option => GetPicturePaths(option))
                    .Where(value => value.Any())
                    .FirstOrDefault();
            }

            return picturePaths.EmptyIfNull().Take(MaxImageCount);
        }

        private IEnumerable<string> GetPicturePaths(string sourceDirectory)
        {
            if (!string.IsNullOrEmpty(sourceDirectory))
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(sourceDirectory);
                    if (dir.Exists)
                    {
                        return dir.EnumerateFiles(_defaultImageSearchPattern, SearchOption.AllDirectories).Select(file => file.FullName);
                    }
                }
                catch (IOException) { }
                catch (ArgumentException) { }
                catch (SecurityException) { }
            }
            return Enumerable.Empty<string>();
        }

        private ReadOnlyCollection<string> DefaultPicturePaths
        {
            get
            {
                if (BrowserInteropHelper.IsBrowserHosted)
                {
                    return new string[0].ToReadOnlyCollection();
                }
                else
                {
                    return new string[]{@"C:\Users\Public\Pictures\Sample Pictures\",
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)}.ToReadOnlyCollection();
                }
            }
        }


        public FlipTile3D() : base(new Viewport3D())
        {
            if (ItemMaterials == null || ItemMaterials.Count() <= 0)
                ItemMaterials = GetItemMaterials();

            UpdateTileLayout();

            //尽量避免3D剪辑，很消耗性能
            WrappedElement.ClipToBounds = false;

            this.Loaded += delegate (object sender, RoutedEventArgs e)
            {
                CompositionTarget.Rendering += CompositionTarget_Rendering;
            };

            this.Unloaded += delegate (object sender, RoutedEventArgs e)
            {
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
            };
        }

        private void UpdateTileLayout()
        {
            WrappedElement.Children.Clear();
            Dispatcher.Invoke(new Action(Setup3D), DispatcherPriority.ApplicationIdle);
        }

        private void Setup3D()
        {
            PerspectiveCamera camera = new PerspectiveCamera(
                new Point3D(0, 0, 3.73), //Position
                new Vector3D(0, 0, -1), //LookDirection
                new Vector3D(0, 1, 0), //UpDirection
                30 //FOV
                );

            WrappedElement.Camera = camera;

            Model3DGroup everything = new Model3DGroup();

            Model3DGroup lights = new Model3DGroup();
            DirectionalLight whiteLight = new DirectionalLight(Colors.White, new Vector3D(0, 0, -1));
            lights.Children.Add(whiteLight);

            everything.Children.Add(lights);

            ModelVisual3D model = new ModelVisual3D();

            double tileSizeX = 2.0 / XCount;
            double startX = -((double)XCount) / 2 * tileSizeX + tileSizeX / 2;
            double startY = -((double)YCount) / 2 * tileSizeX + tileSizeX / 2;

            int index;
            Size tileTextureSize = new Size(1.0 / XCount, 1.0 / YCount);
            Tiles = new FlipTile[XCount * YCount];

            //从左向右（升序x），从下到上（升序y）
            for (int y = 0; y < YCount; y++)
            {
                for (int x = 0; x < XCount; x++)
                {
                    index = y * XCount + x;

                    Rect backTextureCoordinates = new Rect(x * tileTextureSize.Width,
                                                           // this will give you a headache. Exists since we are going
                                                           // from bottom bottomLeft of 3D space (negative Y is down),
                                                           // but texture coor are negative Y is up
                                                           1 - y * tileTextureSize.Height - tileTextureSize.Height,
                                                           tileTextureSize.Width, tileTextureSize.Height);

                    Tiles[index] = new FlipTile(GetMaterial(index),
                                                new Size(tileSizeX, tileSizeX),
                                                new Point(startX + x * tileSizeX, startY + y * tileSizeX),
                                                BackMaterial,
                                                backTextureCoordinates);

                    Tiles[index].Click += (sender, args) => TileClicked((FlipTile)sender);
                    WrappedElement.Children.Add(Tiles[index]);
                }
            }

            model.Content = everything;
            WrappedElement.Children.Add(model);

            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private DiffuseMaterial GetMaterial(int index)
        {
            //if (ItemMaterials != null)
            //    return ItemMaterials.ElementAt(index % ItemMaterials.Count());
            //else
            //    return default;

            return ItemMaterials.ElementAt(index % ItemMaterials.Count());
        }

        private void TileClicked(FlipTile tileData)
        {
            _isFlipped = !_isFlipped;
            if (_isFlipped)
            {
                BackMaterial.Brush = tileData.FrontBrush;
            }
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            bool keepTicking = false;
            if (Tiles[0] != null)
            {
                Vector mouseFixed = FixMouse(_lastMouse, this.RenderSize);
                for (int i = 0; i < Tiles.Length; i++)
                {
                    keepTicking = Tiles[i].TickData(mouseFixed, _isFlipped) || keepTicking;
                }
            }
            if (!keepTicking)
            {
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
            }
        }

        private static Vector FixMouse(Point mouse, Size size)
        {
            Debug.Assert(size.Width >= 0 && size.Height >= 0);
            double scale = Math.Max(size.Width, size.Height) / 2;

            // Translate y going down to y going up
            mouse.Y = -mouse.Y + size.Height;
            mouse.Y -= size.Height / 2;
            mouse.X -= size.Width / 2;

            Vector v = new Vector(mouse.X, mouse.Y);
            v /= scale;

            return v;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(this.RenderSize));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            UpdateLastMouse(e.GetPosition(this));
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            UpdateLastMouse(new Point(double.NaN, double.NaN));
        }

        private void UpdateLastMouse(Point point)
        {
            if (point != _lastMouse)
            {
                _lastMouse = point;
                CompositionTarget.Rendering += CompositionTarget_Rendering;
            }
        }
    }
}
