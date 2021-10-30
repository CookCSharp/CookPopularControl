using CookPopularControl.Communal.Data.Enum;
using CookPopularControl.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CookPopularControl.Controls.ThreeDimensional
{
    /// <summary>
    /// 3D警报器
    /// </summary>
    public partial class Alertor3D : UserControl
    {
        private Point3D _point3DCenter;
        private static readonly List<string> colors = new List<string>() { ResourceHelper.GetResource<Color>("PrimaryThemeColor").ToString(), "#32AA32", "#FFA500", "#FF0000", "#500000" };


        public Alertor3D()
        {
            InitializeComponent();

            MeasureModel(RootGeometryContainer);
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

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
                UnionRect(child!, ref rect3D);
            }
            if (model.Content != null)
                rect3D.Union(model.Content.Bounds);
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            YawWithModelCenter(false, 4);
            //YawWithDefaultCenter(false, 4);           
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

        /// <summary>
        /// 警报器的颜色
        /// </summary>
        internal Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="Color"/>的依赖属性
        /// </summary>
        internal static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(Alertor3D), new PropertyMetadata(ResourceHelper.GetResource<Color>("PrimaryThemeColor")));


        /// <summary>
        /// 当前警报器的状态
        /// </summary>
        public AlertorState State
        {
            get { return (AlertorState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }
        /// <summary>
        /// 提供<see cref="State"/>的依赖属性
        /// </summary>
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(AlertorState), typeof(Alertor3D), new PropertyMetadata(default(AlertorState), OnStatePropertyChanged));

        private static void OnStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Alertor3D alertor)
            {
                alertor.Color = (Color)ColorConverter.ConvertFromString(colors[(int)e.NewValue]);
            }
        }
    }
}
