using CookPopularControl.Communal.Data;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MvvmTestDemo.DemoViews
{
    /// <summary>
    /// OtherControlsDemo.xaml 的交互逻辑
    /// </summary>
    public partial class OtherControlsDemo : UserControl
    {
        private int index = 1;
        private List<AlertorState> alertorStates = new List<AlertorState>() { AlertorState.Normal, AlertorState.Success, AlertorState.Warning, AlertorState.Error, AlertorState.Fatal };

        public OtherControlsDemo()
        {
            InitializeComponent();

            MeasureModel(RootGeometryContainer);
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        private void StateExchange_Click(object sender, RoutedEventArgs e)
        {
            if (index >= alertorStates.Count)
                index = 0;
            alertor.CurrentState = alertorStates[index];
            index++;
        }

        private void StartOrStop_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as ToggleButton;
            alertor.IsCancelAlarm = btn.IsChecked.Value;
            if (btn.IsChecked.Value)
                btn.Content = "开始";
            else
                btn.Content = "停止";
        }

        private void SearchBar_StartSearch(object sender, RoutedPropertySingleEventArgs<string> e)
        {
            //StreamResourceInfo sri = Application.GetResourceStream(new Uri("/Resources/Cursors/myCursor.cur", UriKind.Relative));
            //(sender as SearchBar).Cursor = new Cursor(sri.Stream);
            //(sender as SearchBar).Cursor = CursorHelper.ConvertToCursor(customCursor, new Point(0.5, 0.5));
        }

        private void SearchBar_ContentChanged(object sender, TextChangedEventArgs e)
        {

        }


        private Point3D _point3DCenter;
        public void MeasureModel(ModelVisual3D model)
        {
            var rect3D = Rect3D.Empty;
            UnionRect(model, ref rect3D);

            _point3DCenter = new Point3D((rect3D.X + rect3D.SizeX / 2), (rect3D.Y + rect3D.SizeY / 2), (rect3D.Z + rect3D.SizeZ / 2));
            double radius = (_point3DCenter - rect3D.Location).Length;
            Point3D position = _point3DCenter;
            position.Z += radius * 2;
            position.X = position.Z;
            camera.Position = position;
            camera.LookDirection = _point3DCenter - position;
            camera.NearPlaneDistance = radius / 100;
            camera.FarPlaneDistance = radius * 100;
        }

        private void UnionRect(ModelVisual3D model, ref Rect3D rect3D)
        {
            for (int i = 0; i < model.Children.Count; i++)
            {
                var child = model.Children[i] as ModelVisual3D;
                UnionRect(child, ref rect3D);
            }
            if (model.Content != null)
                rect3D.Union(model.Content.Bounds);
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            YawWithModelCenter(false, 4);
            //YawWithDefaultCenter(false, 2);           
        }

        public void YawWithModelCenter(bool leftRight, double angleDeltaFactor)
        {
            var axis = new AxisAngleRotation3D(camera.UpDirection, leftRight ? angleDeltaFactor : -angleDeltaFactor);
            var rt3D = new RotateTransform3D(axis) { CenterX = _point3DCenter.X, CenterY = _point3DCenter.Y, CenterZ = _point3DCenter.Z };
            Matrix3D matrix3D = rt3D.Value;
            Point3D point3D = camera.Position;
            Point3D position = matrix3D.Transform(point3D);
            camera.Position = position;
            camera.LookDirection = camera.LookDirection = _point3DCenter - position;
        }

        public void YawWithDefaultCenter(bool leftRight, double angleDeltaFactor)
        {
            var axis = new AxisAngleRotation3D(camera.UpDirection, leftRight ? angleDeltaFactor : -angleDeltaFactor);
            var rt3D = new RotateTransform3D(axis);
            Matrix3D matrix3D = rt3D.Value;
            Point3D point3D = camera.Position;
            Point3D position = matrix3D.Transform(point3D);
            camera.Position = position;
            camera.LookDirection = camera.LookDirection = _point3DCenter - position;
        }
    }
}
